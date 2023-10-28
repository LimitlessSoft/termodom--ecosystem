using Lamar;
using TD.Core.Domain.Managers;
using TD.Core.Framework;
using TD.Web.Admin.Domain.Middlewares;
using TD.Core.Repository;
using TD.Core.Contracts.Interfaces;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Api
{
    public class Startup : BaseApiStartup, IMigratable
    {
        private const string ProjectName = "TD.Web.Admin";

        public Startup()
            : base(ProjectName,
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
            ConfigurationRoot.ConfigureNpgsqlDatabase<WebDbContext, Startup>(services);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
#if DEBUG
            services.For<MinioManager>().Use(
                new MinioManager(ProjectName, ConfigurationRoot["minio:host"], ConfigurationRoot["minio:access_key"],
                ConfigurationRoot["minio:secret_key"], ConfigurationRoot["minio:port"]));
#else
            services.For<MinioManager>().Use(
                new MinioManager(ProjectName,
                Environment.GetEnvironmentVariable("MINIO_HOST"),
                Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY"),
                Environment.GetEnvironmentVariable("MINIO_SECRET_KEY"),
                Environment.GetEnvironmentVariable("MINIO_PORT")));
#endif
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