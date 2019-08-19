using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using BaseApp.Identity.Auth;
using BaseApp.Identity.Model;
using BaseApp.Identity.Services.Interfaces;
using BaseApp.Identity.SwaggerExamples;
using BaseApp.Identity.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Examples;

namespace BaseApp.Identity.Api
{
//    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RoleManagerController : Microsoft.AspNetCore.Mvc.Controller
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

        public async Task<IActionResult> AddNewRole([FromBody] AddNewRoleViewModel newrole)
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

        public async Task<IActionResult> AddUserToRoles([FromBody] UserInRolesViewModel model)
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

        [Route("addactiontorole")]
        [HttpPost]
        public async Task<IActionResult> AddActionToRole([FromBody] AddNewRoleViewModel model)
        {
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var response= await _userService.AddActionToRoleTaskAsync(model);
            if(!response.Succeeded)
                return new BadRequestObjectResult(response.Errors);
            return new OkObjectResult("Actions added to the selected role");
        }

        [Route("getallactions")]     
        [HttpGet]
        public async Task<List<Actions>> GetAllActios()
        {
            var result = new List<Actions>();
            var controllers = Assembly.GetExecutingAssembly().GetTypes().
                Where(type => typeof(Microsoft.AspNetCore.Mvc.Controller).IsAssignableFrom(type)).ToList();
            foreach (var controller in controllers)
            {
                var methods = controller.GetMethods
                    (BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly);
                result.AddRange(methods.Select(x => new Actions()
                {
                    ActionName = x.Name,
                    ControllerName = controller.Name

                }).ToList());
            }

            return result;
        }
        
        
        [Route("getallroles")]
        [HttpGet]
        public async Task<List<AddNewRoleViewModel>> GetAllRoles()
        {
            var result = await _userService.GettAllRoles();
            return result;
        }
    }
}