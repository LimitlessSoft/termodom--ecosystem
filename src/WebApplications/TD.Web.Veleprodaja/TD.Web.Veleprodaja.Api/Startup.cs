using Lamar;
using Microsoft.EntityFrameworkCore;
using TD.Core.Framework;
using TD.Core.Repository;
using TD.Web.Veleprodaja.Repository;

namespace TD.Web.Veleprodaja.Api
{
    public class Startup : BaseApiStartup
    {
        public Startup() : base("TD.Web.Veleprodaja")
        {

        }
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            ConfigurationRoot.ConfigureNpgsqlDatabase<VeleprodajaDbContext>(services);
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