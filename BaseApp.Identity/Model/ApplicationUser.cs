using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BaseApp.Identity.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public long? FacebookId { get; set; }
        public long? LinkedInId { get; set; }
        public string PictureUrl { get; set; }
        public ExternalData ExternalData{set; get; }
       
    }

    
}