using Lamar;
using LSCore.Framework;
using LSCore.Repository;
using TD.Web.Common.Contracts;
using TD.Web.Common.Repository;
using LSCore.Contracts.Interfaces;
using TD.Web.Admin.Domain.Middlewares;
using LSCore.Contracts.SettingsModels;

namespace TD.Web.Admin.Api
{
    public class Startup : LSCoreBaseApiStartup, ILSCoreMigratable
    {
        public Startup()
            : base(Constants.ProjectName,
            addAuthentication: true,
            useCustomAuthorizationPolicy: true)
        {
            AfterAuthenticationMiddleware = (appBuilder) =>
            {
                return appBuilder.UseMiddleware<LastSeenMiddleware>();
            };
        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);

            services.AddAuthorization(options =>
            {
                options.AddPolicy("TestPolicy",
                    policy => policy.RequireClaim("TestPolicyPermission"));
            });

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
            services.For<LSCoreMinioSettings>().Use(
                new LSCoreMinioSettings()
                {
                    BucketBase = ProjectName.ToLower(),
                    Host = ConfigurationRoot["MINIO_HOST"],
                    AccessKey = ConfigurationRoot["MINIO_ACCESS_KEY"],
                    SecretKey = ConfigurationRoot["MINIO_SECRET_KEY"],
                    Port = ConfigurationRoot["MINIO_PORT"]
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