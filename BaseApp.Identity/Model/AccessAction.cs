using System;

namespace BaseApp.Identity.Model
{
    public class AccessAction : BaseEntity
    {
        public string ControlName { get; set; }
        public string ActionName { get; set; }
        public string ActionNameNormalized { get; set; }
        public string ControllerNameNormalized { get; set; }
        public virtual ApplicationRole ApplicationRole { set; get; } 
    }
}