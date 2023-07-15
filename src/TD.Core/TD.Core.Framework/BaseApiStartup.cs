using FluentValidation;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace TD.Core.Framework
{
    public class BaseApiStartup : BaseStartup
    {
        public BaseApiStartup(string projectName) : base(projectName)
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddHttpLogging(logging =>
            {
                logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.HttpLoggingFields.All;
                logging.RequestBodyLogLimit = 4096;
                logging.ResponseBodyLogLimit = 4096;
            });
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            base.Configure(applicationBuilder, serviceProvider);

            applicationBuilder.UseHttpLogging();

            applicationBuilder.UseRouting();

            applicationBuilder.UseSwagger();
            applicationBuilder.UseSwaggerUI();

            applicationBuilder.UseHttpsRedirection();

            applicationBuilder.UseAuthorization();

            applicationBuilder.UseEndpoints((routes) =>
            {
                routes.MapControllers();
            });
        }

        public void ConfigureValidatorsIoC(ServiceRegistry services)
        {
            Domain.Validators.Constants.Container = new Container(services);
        }
    }
}
