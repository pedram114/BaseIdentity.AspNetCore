namespace BaseApp.Identity
{
    public class AppSettingConfigs
    {
        public Authentication Authentication { get; set; }
       
    }
    
    
    
    public class Authentication
    {

        public LinkedIn LinkedIn { get; set; }
    }
    public class LinkedIn
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}