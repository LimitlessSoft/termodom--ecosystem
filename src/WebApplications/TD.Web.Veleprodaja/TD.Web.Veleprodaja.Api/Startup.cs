using Lamar;
using TD.Core.Framework;
using TD.Web.Veleprodaja.Repository;

namespace TD.Web.Veleprodaja.Api
{
    public class Startup : BaseApiStartup
    {
        public Startup() : base("TD.Web.Veleprodaja", true)
        {

        }
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddCors(options =>
            {
                options.AddPolicy("default", policy =>
                {
                    policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });
            ConfigurationRoot.ConfigureNpgsqlDatabase<VeleprodajaDbContext>(services);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            applicationBuilder.UseCors("default");
            base.Configure(applicationBuilder, serviceProvider);

            var logger = serviceProvider.GetService<ILogger<Startup>>();
            logger.LogInformation("Application started!");
        }
    }
}