using Microsoft.Extensions.Configuration;
using System.Net.NetworkInformation;

namespace TD.Web.Cronjobs.OrderStatusUpdater.Helpers
{
    public static class StringHelper
    {
        public static string CreateConnectionString(IConfigurationRoot configurationRoot)
        {
            var server = configurationRoot["POSTGRES_HOST"];
            var port = configurationRoot["POSTGRES_PORT"];
            var userId = configurationRoot["POSTGRES_USER"];
            var password = configurationRoot["POSTGRES_PASSWORD"];
            var db = configurationRoot["POSTGRES_DATABASE_NAME"];
            return $"Server={server};Port={port};Userid={userId};Password={password};Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database={db};Include Error Detail=true;";
        }
    }
}
