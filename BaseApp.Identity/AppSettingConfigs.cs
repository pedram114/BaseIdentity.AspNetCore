namespace BaseApp.Identity
{
    public class AppSettingConfigs
    {
        public Authentication Authentication { get; set; }
        public string SecretKey { set; get; }
    }
    
    
    
    // ReSharper disable once ClassNeverInstantiated.Global
    public class Authentication
    {

        public LinkedIn LinkedIn { get; set; }
        public GitHub GitHub { get; set; }
    }
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LinkedIn
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class GitHub
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}