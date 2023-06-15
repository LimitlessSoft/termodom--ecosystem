using Lamar.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TD.Core.Framework.Extensions
{
    public static class StartupExtensions
    {
        public static void CreateTDBuilder<TStartup>(string[] args)
            where TStartup : class
        {
            Host.CreateDefaultBuilder(args)
            .UseLamar()
            .ConfigureLogging(x =>
            {
                x.ClearProviders();
                x.AddConsole();
                x.AddDebug();
            })
            .ConfigureWebHostDefaults((webBuilder) =>
            {
                webBuilder.UseStartup<TStartup>();
            })
            .Build()
            .Run();
        }
        public static void ConfigureDatabase<TDbContext>(this IConfigurationRoot configurationRoot, IServiceCollection services)
            where TDbContext : DbContext
        {
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<TDbContext>((servicesProvider, options) =>
                {
                    options.ConfigureDbContext(configurationRoot)
                        .UseInternalServiceProvider(servicesProvider);
                });
        }

        public static DbContextOptionsBuilder ConfigureDbContext(this DbContextOptionsBuilder dbContextOptionsBuilder, IConfigurationRoot configurationRoot)
        {
            var connection = "Server=192.168.0.3;Port=65002;Userid=termodom;Password=Plivanje123$;Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database=termodom";
            return dbContextOptionsBuilder.UseNpgsql(connection, x =>
            {
                x.MigrationsHistoryTable("migrations_history");
                x.MigrationsAssembly("TD.DbMigrations.App");
            });
        }
    }
}
