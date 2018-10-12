using System.Collections.Generic;

namespace BaseApp.Identity.ViewModels
{
    public class AddNewRoleViewModel
    {
        public string RoleName { get; set; }
        
        public List<ActionList> Actions { get; set; }
    }

    public class ActionList
    {
        public string ActionName { get; set; }
        
        public string ControllerName { get; set; }

    }
}