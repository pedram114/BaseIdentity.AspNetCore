using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BaseApp.Identity.Model;
using BaseApp.Identity.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;

namespace BaseApp.Identity
{
    public class ActionAuthorizationHadler : IAuthorizationHandler
    {
        
        private RoleManager<ApplicationRole> _appRole { set; get; }
        private readonly IUserService _userService;
        
        
        public ActionAuthorizationHadler(
            RoleManager<ApplicationRole> appRole
            ,IUserService userService
        )
        {
            _appRole = appRole;
            _userService = userService;

        }

        public Task HandleAsync(AuthorizationHandlerContext context)
        {

            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var filterContext = (FilterContext)context.Resource;
            var readdata = filterContext.HttpContext.GetRouteData();
            
            var actionName = readdata.Values["Action"].ToString();
            var controllerName = readdata.Values["Controller"].ToString();
            var user = context.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            if (_userService.UserHasPermitToAction(user, actionName, controllerName).Result)
                context.PendingRequirements.ToList().ForEach(context.Succeed);
            else
            {            
                context.Fail();
            }
               
            return Task.CompletedTask;
        }


    }
}