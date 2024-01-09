using Lamar;
using LSCore.Framework;
using Microsoft.EntityFrameworkCore;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Api
{
    public class Startup : LSCoreBaseApiStartup
    {
        public Startup() : base("TD.Komercijalno", false)
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
                .AddDbContext<KomercijalnoDbContext>((serviceProvider, options) =>
                {
                    options.UseFirebird(ConfigurationRoot["ConnectionString_Komercijalno"]);
                });
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            base.Configure(applicationBuilder, serviceProvider);

            var logger = serviceProvider.GetService<ILogger<Startup>>();
            logger.LogInformation("Application started!");
            logger.LogInformation("Connection string: " + ConfigurationRoot["ConnectionString_Komercijalno"]);
        }
    }
}