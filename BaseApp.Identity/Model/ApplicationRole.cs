using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace BaseApp.Identity.Model
{
    public class ApplicationRole : IdentityRole
    {
        public virtual IList<AccessAction> Actions { set; get; }
        
    }
}