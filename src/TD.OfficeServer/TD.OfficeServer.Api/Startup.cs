using LSCore.Contracts.SettingsModels;
using LSCore.Contracts.Interfaces;
using TD.OfficeServer.Contracts;
using LSCore.Framework;
using Lamar;

namespace TD.OfficeServer.Api
{
    public class Startup : LSCoreBaseApiStartup, ILSCoreMigratable
    {
        public Startup()
            : base(Constants.ProjectName,
                addAuthentication: false,
                apiKeyAuthentication: true)
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
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
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

            base.Configure(applicationBuilder, serviceProvider);

            var logger = serviceProvider.GetService<ILogger<Startup>>();
            logger!.LogInformation("Application started!");
        }
    }
}