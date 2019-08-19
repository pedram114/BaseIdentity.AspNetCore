using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using BaseApp.Identity.Auth;
using BaseApp.Identity.Model;
using BaseApp.Identity.Services.Interfaces;
using BaseApp.Identity.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BaseApp.Identity.Services
{
    public class UserService : IUserService
    {
        
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _appDbContext;

        
        public UserService(
            UserManager<ApplicationUser> userManager
            ,IJwtFactory jwtfactory
            ,IMapper mapper
            ,ApplicationDbContext appDbContext
            ,RoleManager<ApplicationRole> roleManager)
        {
            _userManager = userManager;
            _jwtFactory = jwtfactory;
            _mapper = mapper;
            _appDbContext = appDbContext;
            _roleManager = roleManager;
        }
        
        
        public async Task<ClaimsIdentity> GetClaimsIdentityTaskAsync(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                return await Task.FromResult(_jwtFactory.GenerateClaimsIdentity(userName, userToVerify.Id));
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }


        public async Task<List<AddNewRoleViewModel>> GettAllRoles()
        {
            var data = await _roleManager.Roles.Include(r=>r.Actions).ToListAsync();
            var retdata= data.Select(r => new AddNewRoleViewModel()
            {
                Actions = r.Actions?.Select(a=>new Actions()
                {
                     ActionName = a.ActionName,
                     ControllerName = a.ControlName
                }).ToList(),
                RoleName = r.Name


            }).ToList();
            return retdata;
        }


        public async Task<IdentityResult> CreateNewUserTaskAsync(RegisterViewModel newUser)
        {
            var userIdentity = _mapper.Map<ApplicationUser>(newUser);
            userIdentity.UserName = userIdentity.Email;
            var result = await _userManager.CreateAsync(userIdentity, newUser.Password);
            return result;
        }
        
        
        public async Task<IEnumerable<ShowUsersViewModel>> GetUsersTaskAsync(string userid=null)
        {
            var users = new List<ApplicationUser>();

            if (userid == null)
            {  
                users = await _appDbContext.Users.Include(d=>d.ExternalData).ToListAsync();
            }
            else
            {
                users.Add(await _appDbContext.Users.Include(d=>d.ExternalData).FirstOrDefaultAsync(user => user.Id == userid));
            }
            var returnModel = 
                _mapper.Map<List<ApplicationUser>, List<ShowUsersViewModel>>(users);
            return returnModel;
        }


        public async Task<IdentityResult> AddNewRoleTaskAsync(AddNewRoleViewModel model)
        {
            var newRole=new ApplicationRole();
            _mapper.Map(model, newRole);  
            var returndata=await _roleManager.CreateAsync(newRole);
            return returndata;
        }
        
        
        public async Task<IdentityResult> AddUserToRolesTaskAsync(string userName,List<string> roleName)
        {
            var user =await _userManager.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == userName.Normalize());
            var returndata = await _userManager.AddToRolesAsync(user, roleName);
            return returndata;
        }


        public async Task<bool> UserHasPermitToAction(string userName,string actionName,string controllerName)
        {
            var action=await _appDbContext.AccessActions.Include(a=>a.ApplicationRole).
                Where(a => a.ActionNameNormalized == actionName.Normalize() && 
                           (a.ControllerNameNormalized ==controllerName.Normalize() || a.ControllerNameNormalized==controllerName.Normalize() + "Controller"))
                .ToListAsync();
            var roles = action?.Select(r=>r.ApplicationRole);
            if (roles == null)
                return false;
            var user =await _userManager.FindByNameAsync(userName);
           
            foreach (var role in roles)
            {
                var result = await _userManager.IsInRoleAsync(user, role.Name);
                if (result)
                {
                    return true;
                }
            }

            return false;
            
           

        }

        public async Task<IdentityResult> AddActionToRoleTaskAsync(AddNewRoleViewModel model)
        {
            var role = await _roleManager.Roles.Include(r=>r.Actions).FirstOrDefaultAsync(r => r.Name == model.RoleName);
            if(role.Actions==null)
                role.Actions=new List<AccessAction>();
            foreach (var action in model.Actions)
            {
                if (!role.Actions.Any(a=>a.ActionNameNormalized==action.ActionName.Normalize() && a.ControllerNameNormalized==action.ControllerName.Normalize()))
                {
                    role.Actions.Add(new AccessAction()
                    {
                         ActionName = action.ActionName,
                          ActionNameNormalized = action.ActionName.Normalize(),
                          ControlName = action.ControllerName,
                           ControllerNameNormalized = action.ControllerName.Normalize(),
                            
                        
                    });
                }
            }

            return await _roleManager.UpdateAsync(role);
        }
    }
}