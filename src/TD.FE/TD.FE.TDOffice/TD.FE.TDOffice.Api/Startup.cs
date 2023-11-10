using Lamar;
using TD.Core.Framework;

namespace TD.FE.TDOffice.Api
{
    public class Startup : LSCoreBaseApiStartup
    {
        public Startup() : base("TD.FE.TDOffice", false)
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
        }
    }
}