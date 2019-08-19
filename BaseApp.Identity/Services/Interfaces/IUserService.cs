using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using BaseApp.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace BaseApp.Identity.Services.Interfaces
{
    public interface IUserService
    {
        Task<ClaimsIdentity> GetClaimsIdentityTaskAsync(string userName, string password);
        Task<IdentityResult> CreateNewUserTaskAsync(RegisterViewModel newUser);
        Task<IEnumerable<ShowUsersViewModel>> GetUsersTaskAsync(string userid = null);
        Task<IdentityResult> AddNewRoleTaskAsync(AddNewRoleViewModel model);
        Task<IdentityResult> AddActionToRoleTaskAsync(AddNewRoleViewModel model);
        Task<IdentityResult> AddUserToRolesTaskAsync(string userId, List<string> roleName);
        Task<bool> UserHasPermitToAction(string userId, string actionName, string controllerName);
        Task<List<AddNewRoleViewModel>> GettAllRoles();

    }
}