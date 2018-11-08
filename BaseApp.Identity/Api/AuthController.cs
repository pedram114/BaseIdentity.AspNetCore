using System.Threading.Tasks;
using AutoMapper;
using BaseApp.Identity.Auth;
using BaseApp.Identity.Helpers;
using BaseApp.Identity.Model;
using BaseApp.Identity.Services.Interfaces;
using BaseApp.Identity.SwaggerExamples;
using BaseApp.Identity.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Examples;

namespace BaseApp.Identity.Api
{

    [Route("api/[controller]")]
    public class AuthController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        private readonly ApplicationDbContext _appDbContext;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;


        public AuthController(UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions, ApplicationDbContext appDbContext, IMapper mapper,
            IUserService userService)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
            _appDbContext = appDbContext;
            _userService = userService;
            _mapper = mapper;
        }

        // POST api/auth/login
        [Route("login")]
        [HttpPost]
        [SwaggerRequestExample(typeof(CredentialsViewModel),typeof(CredentialsViewModelExample))]

        public async Task<IActionResult> Login([FromBody] CredentialsViewModel credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await _userService.GetClaimsIdentityTaskAsync(credentials.UserName, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.",
                    ModelState));
            }            
            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtOptions,
                new JsonSerializerSettings {Formatting = Formatting.Indented});
            
            return new OkObjectResult(jwt);
        }

        
        
        // POST api/auth/externalLogin
        [Route("externallogin")]
        [HttpGet]
        public IActionResult ExternalLogin(string returnUrl = "/")
        {
            return Challenge(new AuthenticationProperties() { RedirectUri = returnUrl });
        }

        
        // POST api/accounts
        [Route("register")]
        [HttpPost]
        [SwaggerRequestExample(typeof(RegisterViewModel),typeof(RegisterViewModelExample))]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result= await _userService.CreateNewUserTaskAsync(model);
            if (!result.Succeeded)
                return new BadRequestObjectResult(result.Errors);
            return new OkObjectResult("Account created");
        }

    }
}
