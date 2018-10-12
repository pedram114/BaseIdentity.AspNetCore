using System.Collections.Generic;

namespace BaseApp.Identity.ViewModels
{
    public class UserInRolesViewModel
    {
        public string UserName { get; set; }
        public List<string> RoleNames { get; set; }
    }
}