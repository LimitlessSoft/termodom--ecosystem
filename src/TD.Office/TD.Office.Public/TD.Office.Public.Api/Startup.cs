using Lamar;
using TD.Core.Contracts.Interfaces;
using TD.Core.Framework;
using TD.Office.Common.Repository;
using TD.Core.Repository;
using TD.Office.Common.Contracts;

namespace TD.Office.Public.Api
{
    public class Startup : BaseApiStartup, IMigratable
    {
        private const string ProjectName = "TD.Office.Public";

        public Startup()
            : base(ProjectName, false, false)
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
            ConfigurationRoot.ConfigureNpgsqlDatabase<OfficeDbContext, Startup>(services, Constants.DbName);
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