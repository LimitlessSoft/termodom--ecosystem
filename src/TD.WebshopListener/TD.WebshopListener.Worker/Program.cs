using Lamar;
using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using TD.WebshopListener.Contracts.ConfigurationOptions;
using TD.WebshopListener.Contracts.IManagers;
using TD.WebshopListener.Contracts.Managers;

namespace TD.WebshopListener.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = Host.CreateDefaultBuilder(args)
                .UseLamar()
                .ConfigureContainer<ServiceRegistry>((context, services) =>
                {
                    services.AddHostedService<Worker>();
                    services.For<IWebApiRequestManager>().Use(new WebApiRequestManager());

                    services.Scan(s =>
                    {
                        s.TheCallingAssembly();
                        s.WithDefaultConventions();
                    });
                })
                .Build();

            host.Run();
        }
    }
}