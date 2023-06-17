using FluentValidation;
using Lamar;
using Microsoft.EntityFrameworkCore;
using TD.Core.Framework;
using TD.Komercijalno.Repository;

namespace TD.Komercijalno.Api
{
    public class Startup : BaseApiStartup
    {
        public Startup() : base("TD.Komercijalno")
        {

        }

        public override void ConfigureServices(IServiceCollection services)
        {
            base.ConfigureServices(services);
        }

        public override void ConfigureContainer(ServiceRegistry services)
        {
            base.ConfigureContainer(services);
            services.AddEntityFrameworkFirebird()
                .AddDbContext<KomercijalnoDbContext>((serviceProvider, options) =>
                {
                    options.UseFirebird(ConfigurationRoot.GetSection("ConnectionStrings")["Komercijalno"]);
                });
        }

        public override void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {
            base.Configure(applicationBuilder, serviceProvider);
        }
    }
}