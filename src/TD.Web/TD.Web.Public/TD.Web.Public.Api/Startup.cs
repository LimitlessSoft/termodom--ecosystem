using Lamar;
using LSCore.Contracts.Interfaces;
using LSCore.Framework;
using LSCore.Repository;
using Lamar.Scanning.Conventions;
using System.Reflection;
using TD.Web.Common.Contracts;
using TD.Web.Common.Repository;
using FluentValidation;

namespace TD.Web.Public.Api
{
    public class Startup : LSCoreBaseApiStartup, ILSCoreMigratable
    {
        private const string ProjectName = "TD.Web.Public";

        public Startup()
            : base(ProjectName,
            addAuthentication: true,
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

            ConfigurationRoot.ConfigureNpgsqlDatabase<WebDbContext, Startup>(services, Constants.DbName);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            AdditionalScanOptions = (s) =>
            {
                s.AssembliesAndExecutablesFromApplicationBaseDirectory((Assembly x) => x.GetName().Name!.StartsWith("TD.Web.Common"));
            };
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