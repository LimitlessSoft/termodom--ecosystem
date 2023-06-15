using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace TD.Core.Framework
{
    public class BaseStartup : IBaseStartup
    {
        public IConfigurationRoot ConfigurationRoot { get; set; }

        public BaseStartup()
        {
            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json", true);

            ConfigurationRoot = builder.Build();
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider => ConfigurationRoot);
        }

        public virtual void ConfigureContainer(ServiceRegistry services)
        {
            services.Scan(s =>
            {
                s.TheCallingAssembly();
                s.WithDefaultConventions();
            });
            services.AddAuthorization();
        }

        public virtual void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {

        }
    }
}
