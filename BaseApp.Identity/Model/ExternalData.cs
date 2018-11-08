using System.ComponentModel.DataAnnotations.Schema;
using BaseApp.Identity.ViewModels;

namespace BaseApp.Identity.Model
{
    public  class ExternalData
    {
        public int Id { get; set; }
        [ForeignKey("ApplicationUser")]
        public virtual ApplicationUser Identity { get; set; }  // navigation property
        public string Location { get; set; }
        public string Locale { get; set; }
        public Gender Gender { get; set; }
    }
}