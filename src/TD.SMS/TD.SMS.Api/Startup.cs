using Lamar;
using Microsoft.EntityFrameworkCore;
using TD.Core.Framework;
using TD.SMS.Repository;

namespace TD.SMS.Api
{
    public class Startup : BaseApiStartup
    {
        public Startup() : base("TD.SMS", false)
        {
        }
        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<SMSDbContext>((services, options) =>
                {
#if DEBUG
                    options.UseNpgsql(ConfigurationRoot.GetSection("ConnectionString")["SMS"]);
#else
                    options.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionString_SMS"));
#endif
                });
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