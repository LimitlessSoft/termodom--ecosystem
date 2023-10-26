using FluentValidation;
using Lamar;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TD.Core.Contracts;
using TD.Core.Contracts.Interfaces;
using TD.Core.Domain;

namespace TD.Core.Framework
{
    public class BaseStartup : IBaseStartup
    {
        public static IContainer? Container { get; set; }
        public IConfigurationRoot ConfigurationRoot { get; set; }
        public string ProjectName { get; set; }

        public BaseStartup(string projectName)
        {
            ProjectName = projectName;

            var builder = new ConfigurationBuilder();

            builder.AddJsonFile("appsettings.json", true);

            ConfigurationRoot = builder.Build();
        }

        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(provider => ConfigurationRoot);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public virtual void ConfigureContainer(ServiceRegistry services)
        {
            services.Scan(s =>
            {
                s.AssembliesAndExecutablesFromApplicationBaseDirectory(x =>
                    x.GetName().Name.StartsWith(ProjectName) ||
                    x.GetName().Name.StartsWith("TD.Core")
                );
                s.TheCallingAssembly();
                s.WithDefaultConventions();
                s.ConnectImplementationsToTypesClosing(typeof(IValidator<>));
                s.ConnectImplementationsToTypesClosing(typeof(IMap<,>));
                s.ConnectImplementationsToTypesClosing(typeof(IDtoMapper<,>));
            });

            ConfigureIoC(services);
        }

        public virtual void ConfigureIoC(ServiceRegistry services)
        {
            Domain.Constants.Container = new Container(services);
        }

        public virtual void Configure(IApplicationBuilder applicationBuilder, IServiceProvider serviceProvider)
        {

        }
    }
}
