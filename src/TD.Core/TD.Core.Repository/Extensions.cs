using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TD.Core.Repository
{
    public static class Extensions
    {
        public static void ConfigureNpgsqlDatabase<TDbContext>(this IConfigurationRoot configurationRoot, IServiceCollection services)
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
