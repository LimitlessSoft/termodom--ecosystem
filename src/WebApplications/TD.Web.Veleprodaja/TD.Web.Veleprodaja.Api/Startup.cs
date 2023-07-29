using Lamar;
using LS.MinIO.Contracts;
using LS.MinIO.Contracts.Contracts;
using LS.MinIO.Domain.Managers;
using TD.Core.Framework;
using TD.Web.Veleprodaja.Repository;

namespace TD.Web.Veleprodaja.Api
{
    public class Startup : BaseApiStartup
    {
        public Startup() : base("TD.Web.Veleprodaja", true)
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
            ConfigurationRoot.ConfigureNpgsqlDatabase<VeleprodajaDbContext>(services);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);

#if DEBUG
            var minIOSettings = new MinIOSettings()
            {
                AccessKey = ConfigurationRoot["MinIO:AccessKey"],
                SecretKey = ConfigurationRoot["MinIO:SecretKey"],
                Endpoint = ConfigurationRoot["MinIO:Endpoint"],
            };
#else
            var minIOSettings = new MinIOSettings()
            {
                AccessKey = Environment.GetEnvironmentVariable("MINIO_ACCESS_KEY"),
                SecretKey = Environment.GetEnvironmentVariable("MINIO_SECRET_KEY"),
                Endpoint = Environment.GetEnvironmentVariable("MINIO_ENDPOINT"),
            };
#endif
            services.For<IMinIOManager>().Use(new MinIOManager(minIOSettings));
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