using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TD.Web.Repository
{
    public static class Extensions
    {
        public static void ConfigureNpgsqlDatabase<TDbContext>(this IConfigurationRoot configurationRoot, IServiceCollection services)
        {

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<WebDbContext>((services, options) =>
                {
                    options.ConfigureDbContext(configurationRoot).UseInternalServiceProvider(services);
                });
        }

        public static DbContextOptionsBuilder ConfigureDbContext(this DbContextOptionsBuilder dbContextOptionsBuilder, IConfigurationRoot configurationRoot)
        {
#if DEBUG
            var postgresHost = configurationRoot.GetSection("POSTGRES")["HOST"];
            var postgresPort = configurationRoot.GetSection("POSTGRES")["PORT"];
            var postgresPassword = configurationRoot.GetSection("POSTGRES")["PASSWORD"];
#else
            var postgresHost = Environment.GetEnvironmentVariable("POSTGRES_HOST");
            var postgresPort = Environment.GetEnvironmentVariable("POSTGRES_PORT");
            var postgresPassword = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD");
#endif
            var connection = $"Server={postgresHost};Port={postgresPort};Userid=postgres;Password={postgresPassword};Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database=Web_Main";
            return dbContextOptionsBuilder.UseNpgsql(connection, x =>
            {
                x.MigrationsHistoryTable("migrations_history");
                x.MigrationsAssembly("TD.Web.Api");
            });
        }
    }
}
