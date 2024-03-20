namespace KFU.Common
{
    public class ApplicationStaticContentSetting
    {
        public static ApplicationStaticContent Config;
        public static void Init(IConfiguration configuration)
        {
            Config = configuration.GetSection("portal").Get<ApplicationStaticContent>();
        }
    }
}
