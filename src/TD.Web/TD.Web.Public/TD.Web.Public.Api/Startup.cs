using Lamar;
using LSCore.Contracts.Interfaces;
using LSCore.Contracts.SettingsModels;
using LSCore.Framework;
using LSCore.Repository;
using TD.Web.Common.Contracts;
using TD.Web.Common.Repository;

namespace TD.Web.Public.Api
{
    public class Startup : LSCoreBaseApiStartup, ILSCoreMigratable
    {
        public Startup()
            : base(Constants.ProjectName,
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

            ConfigurationRoot.ConfigureNpgsqlDatabase<WebDbContext, Startup>(services);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            services.For<LSCoreMinioSettings>().Use(
                new LSCoreMinioSettings()
                {
                    BucketBase = String.Format("{0}{1}{2}", ConfigurationRoot["DEPLOY_ENV"], Constants.DefaultMinioBucketSeparator, ProjectName.ToLower()),
                    Host = ConfigurationRoot["MINIO_HOST"]!,
                    AccessKey = ConfigurationRoot["MINIO_ACCESS_KEY"]!,
                    SecretKey = ConfigurationRoot["MINIO_SECRET_KEY"]!,
                    Port = ConfigurationRoot["MINIO_PORT"]!
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