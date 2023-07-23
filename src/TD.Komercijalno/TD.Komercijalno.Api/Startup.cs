using Lamar;
using Microsoft.EntityFrameworkCore;
using TD.Core.Framework;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Api
{
    public class Startup : BaseApiStartup
    {
        public Startup() : base("TD.Komercijalno")
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            base.Configure(applicationBuilder, serviceProvider);

            var logger = serviceProvider.GetService<ILogger<Startup>>();
            logger.LogInformation("Application started!");
#if DEBUG
            logger.LogInformation("Connection string: " + ConfigurationRoot.GetSection("ConnectionStrings")["Komercijalno"]);
#else
            logger.LogInformation("Connection string: " + Environment.GetEnvironmentVariable("ConnectionString_Komercijalno"));
#endif
        }
    }
}