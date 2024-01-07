using Lamar;
using TD.Office.Common.Repository;
using TD.Office.Common.Contracts;
using LSCore.Framework;
using LSCore.Contracts.Interfaces;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;

namespace TD.Office.Public.Api
{
    public class Startup : LSCoreBaseApiStartup, ILSCoreMigratable
    {
        public Startup()
            : base(Constants.ProjectName, false, false)
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

            ConfigurationRoot.ConfigureNpgsqlDatabase<OfficeDbContext, Startup>(services);
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