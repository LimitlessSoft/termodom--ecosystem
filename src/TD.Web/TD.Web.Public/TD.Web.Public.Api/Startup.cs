using Lamar;
using TD.Core.Domain.Managers;
using TD.Core.Framework;
using TD.Core.Repository;
using TD.Core.Contracts.Interfaces;
using TD.Web.Common.Repository;

namespace TD.Web.Public.Api
{
    public class Startup : BaseApiStartup, IMigratable
    {
        private const string ProjectName = "TD.Web.Public";

        public Startup()
            : base(ProjectName,
            addAuthentication: false,
            useCustomAuthorizationPolicy: false)
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
            ConfigurationRoot.ConfigureNpgsqlDatabase<WebDbContext, Startup>(services);
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