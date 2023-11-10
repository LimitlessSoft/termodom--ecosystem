using Lamar;
using TD.Core.Framework;
using Microsoft.EntityFrameworkCore;
using TD.TDOffice.Repository;

namespace TD.TDOffice.Api
{
    public class Startup : LSCoreBaseApiStartup
    {
        public Startup() : base("TD.TDOffice", false)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
            services.AddEntityFrameworkFirebird()
                .AddDbContext<TDOfficeDbContext>((serviceProvider, options) =>
                {
#if DEBUG
                    options.UseFirebird(ConfigurationRoot.GetSection("ConnectionStrings")["TDOffice"]);
#else
                    options.UseFirebird(Environment.GetEnvironmentVariable("ConnectionString_TDOffice"));
#endif
                });
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            base.Configure(applicationBuilder, serviceProvider);

            var logger = serviceProvider.GetService<ILogger<Startup>>();
            logger.LogInformation("Application started!");
#if DEBUG
            logger.LogInformation("Connection string: " + ConfigurationRoot.GetSection("ConnectionStrings")["TDOffice"]);
#else
            logger.LogInformation("Connection string: " + Environment.GetEnvironmentVariable("ConnectionString_TDOffice"));
#endif
        }
    }
}