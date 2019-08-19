using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using BaseApp.Identity.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BaseApp.Identity.Services
{
    public class BaseIdentityUserManager : UserManager<ApplicationUser>
    {
//        private IRoleStore<ApplicationUser> _store;
//        private IEnumerable<IRoleValidator<ApplicationUser>> _roleValidators;
//        private ILookupNormalizer _keyNormalizer;
//        private IdentityErrorDescriber _errors;
//        private ILogger<RoleManager<ApplicationUser>> _logger;
        
       
        
      
        public BaseIdentityUserManager(IUserStore<ApplicationUser> store,
            IOptions<IdentityOptions> optionsAccessor, 
            IPasswordHasher<ApplicationUser> passwordHasher, 
            IEnumerable<IUserValidator<ApplicationUser>> userValidators, 
            IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators, 
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
            IServiceProvider services, ILogger<UserManager<ApplicationUser>> logger) : 
            base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }


        public async Task<bool> IsInRoleAsync(ApplicationUser user, string[] rolenames)
        {
            foreach (var role in rolenames)
            {
                var result = await IsInRoleAsync(user, role);
                if (result)
                {
                    return true;
                }
            }

            return false;

        }

    }
}