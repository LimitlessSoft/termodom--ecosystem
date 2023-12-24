using Lamar;
using LSCore.Contracts.Interfaces;
using LSCore.Framework;
using LSCore.Repository;
using TD.Office.Common.Repository;
using TD.Quiz.Contracts;

namespace TD.Quiz.Api
{
    public class Startup : LSCoreBaseApiStartup, ILSCoreMigratable
    {
        private const string ProjectName = "TD.Quiz";

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
            ConfigurationRoot.ConfigureNpgsqlDatabase<TDQuizDbContext, Startup>(services);
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