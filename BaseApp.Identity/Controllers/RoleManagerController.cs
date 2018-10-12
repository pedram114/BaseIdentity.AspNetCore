using System.Threading.Tasks;
using AutoMapper;
using BaseApp.Identity.Auth;
using BaseApp.Identity.Helpers;
using BaseApp.Identity.Model;
using BaseApp.Identity.Services.Interfaces;
using BaseApp.Identity.SwaggerExamples;
using BaseApp.Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Examples;

namespace BaseApp.Identity.Controllers
{
//    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]/[action]")]
    public class RoleManagerController : Controller
    {
     
        
        private readonly IUserService _userService;


        public RoleManagerController(UserManager<ApplicationUser> userManager, IJwtFactory jwtFactory,
            IOptions<JwtIssuerOptions> jwtOptions, ApplicationDbContext appDbContext, IMapper mapper,
            IUserService userService)
        {
            
            _userService = userService;

        }
        
        
        
        
        // POST api/auth/login
        [Route("addnewrole")]
        [HttpPost]
        [SwaggerRequestExample(typeof(AddNewRoleViewModel),typeof(AddNewRoleViewModelExample))]

        public async Task<IActionResult> Post([FromBody] AddNewRoleViewModel newrole)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resp=await _userService.AddNewRoleTaskAsync(newrole);
            if(!resp.Succeeded)
                return new BadRequestObjectResult(resp.Errors);
            return new OkObjectResult("New Role Created.");
        }
        
        
        
        [Route("addusertoroles")]
        [HttpPost]
        [SwaggerRequestExample(typeof(UserInRolesViewModel),typeof(UserInRolesViewModelExample))]

        public async Task<IActionResult> Post([FromBody] UserInRolesViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resp=await _userService.AddUserToRolesTaskAsync(model.UserName,model.RoleNames);
            if(!resp.Succeeded)
                return new BadRequestObjectResult(resp.Errors);
            return new OkObjectResult("New Role Created.");
        }

    }
}