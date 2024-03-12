using Microsoft.Extensions.Configuration;

namespace GreenDrive
{
    public static class ConfigurationHelper
    {
        private static IConfiguration _configuration;

        public static void Initialize(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static string GetConfigValue(string key)
        {
            return _configuration[key];
        }
    }
}
