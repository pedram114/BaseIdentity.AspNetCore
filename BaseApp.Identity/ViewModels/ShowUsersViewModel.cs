namespace BaseApp.Identity.ViewModels
{
    public class ShowUsersViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        
        public long? FacebookId { get; set; }
        public long? LinkedInId { get; set; }
        
        public string PictureUrl { get; set; }
        public string Id { get; set; }
        
        public string Location { get; set; }
        public string Locale { get; set; }
        public string Gender { get; set; }
        
        public string PhoneNumber { set; get; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }
}