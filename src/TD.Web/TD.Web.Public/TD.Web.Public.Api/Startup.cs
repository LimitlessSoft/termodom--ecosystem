using Lamar;
using TD.Web.Common.Repository;
using TD.Web.Common.Contracts;
using LSCore.Contracts.Interfaces;
using LSCore.Framework;
using LSCore.Repository;
using FluentValidation;
using Lamar.Scanning.Conventions;
using System.Reflection;

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
            services.Scan(delegate (IAssemblyScanner s)
            {
                s.AssembliesAndExecutablesFromApplicationBaseDirectory((Assembly x) => x.GetName().Name!.StartsWith("TD.Web.Common"));
                s.WithDefaultConventions();
            });
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