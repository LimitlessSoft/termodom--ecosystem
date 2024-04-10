using Microsoft.Extensions.Configuration;

namespace TD.Web.Cronjobs.Common.Framework
{
    public class BaseCronjobStartup
    {
        protected IConfigurationRoot Configuration;

        public BaseCronjobStartup()
        {
            Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();
        }
    }
}
