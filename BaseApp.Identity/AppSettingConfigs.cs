namespace BaseApp.Identity
{
    public class AppSettingConfigs
    {
        public Authentication Authentication { get; set; }
        public string SecretKey { set; get; }
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