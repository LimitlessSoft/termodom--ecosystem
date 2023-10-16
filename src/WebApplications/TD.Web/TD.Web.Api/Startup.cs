using Lamar;
using TD.Core.Domain.Managers;
using Microsoft.AspNetCore.Builder;
using TD.Core.Framework;
using TD.Web.Domain.Middlewares;
using TD.Web.Repository;

namespace TD.Web.Api
{
    public class Startup : BaseApiStartup
    {
        private const string ProjectName = "TD.Web";

        public Startup()
            : base(ProjectName,
            addAuthentication: true,
            useCustomAuthorizationPolicy: true)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

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
            ConfigurationRoot.ConfigureNpgsqlDatabase<WebDbContext>(services);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
            services.For<MinioManager>().Use(
                new MinioManager(ProjectName, ConfigurationRoot["minio:host"], ConfigurationRoot["minio:access_key"],
                ConfigurationRoot["minio:secret_key"], ConfigurationRoot["minio:port"]));
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