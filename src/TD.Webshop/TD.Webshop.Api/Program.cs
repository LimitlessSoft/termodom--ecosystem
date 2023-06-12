using Lamar;
using Lamar.Microsoft.DependencyInjection;

namespace TD.Webshop.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Host.CreateDefaultBuilder(args)
                .UseLamar()
                .ConfigureLogging(x =>
                {
                    x.ClearProviders();
                    x.AddConsole();
                    x.AddDebug();
                })
                .ConfigureContainer<ServiceRegistry>((context, services) =>
                {
                    
                })
                .ConfigureWebHostDefaults((webBuilder) =>
                {
                    webBuilder.UseStartup<Startup>();
                })
                .Build()
                .Run();
        }
    }
}