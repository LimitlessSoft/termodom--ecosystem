using Lamar;
using LSCore.Framework;
using LSCore.Repository;
using TD.Web.Common.Contracts;
using TD.Web.Common.Repository;
using LSCore.Contracts.Interfaces;
using LSCore.Contracts.SettingsModels;
using TD.Web.Common.Contracts.Helpers;
using TD.Web.Admin.Api.Middlewares;

namespace TD.Web.Admin.Api
{
    public class Startup : LSCoreBaseApiStartup, ILSCoreMigratable
    {
        public Startup()
            : base(Constants.ProjectName,
            addAuthentication: false,
            useCustomAuthorizationPolicy: true)
        {
            // AfterAuthenticationMiddleware = (appBuilder) =>
            // {
            //     return appBuilder.UseMiddleware<LastSeenMiddleware>();
            // };
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
            services.For<LSCoreMinioSettings>().Use(
                new LSCoreMinioSettings()
                {
                    BucketBase = GeneralHelpers.GenerateBucketName(ConfigurationRoot["DEPLOY_ENV"]!),
                    Host = ConfigurationRoot["MINIO_HOST"]!,
                    AccessKey = ConfigurationRoot["MINIO_ACCESS_KEY"]!,
                    SecretKey = ConfigurationRoot["MINIO_SECRET_KEY"]!,
                    Port = ConfigurationRoot["MINIO_PORT"]!
                });
            
            services.For<LSCoreApiKeysSettings>().Use(new LSCoreApiKeysSettings()
            {
                ApiKeys = new List<string>()
                {
                    "2v738br3t89abtv8079yfc9q324yr7n7qw089rcft3y2w978"
                }
            });

            base.ConfigureContainer(services);
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            applicationBuilder.UseCors("default");
            
            applicationBuilder.UseHttpLogging();

            applicationBuilder.UseRouting();

            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI();

            applicationBuilder.UseAuthentication();
            
            applicationBuilder.UseMiddleware<WebAdminAuthorizationMiddleware>();

            applicationBuilder.UseEndpoints((routes) =>
            {
                routes.MapControllers();
            });
        }
    }
}