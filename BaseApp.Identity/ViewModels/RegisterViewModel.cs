using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;

namespace BaseApp.Identity.ViewModels
{
    public class RegisterViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Location { get; set; }
        public Gender Gender { get; set; }
    }
}