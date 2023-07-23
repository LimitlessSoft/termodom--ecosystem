using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TD.Web.Veleprodaja.Repository
{
    public static class Extensions
    {
        public static void ConfigureNpgsqlDatabase<TDbContext>(this IConfigurationRoot configurationRoot, IServiceCollection services)
        {

            services.AddEntityFrameworkNpgsql()
                .AddDbContext<VeleprodajaDbContext>((services, options) =>
                {
                    options.ConfigureDbContext(configurationRoot).UseInternalServiceProvider(services);
                });
        }

        public static DbContextOptionsBuilder ConfigureDbContext(this DbContextOptionsBuilder dbContextOptionsBuilder, IConfigurationRoot configurationRoot)
        {
            var postgresHost = configurationRoot.GetSection("POSTGRES")["HOST"];
            var postgresPort = configurationRoot.GetSection("POSTGRES")["PORT"];
            var postgresPassword = configurationRoot.GetSection("POSTGRES")["PASSWORD"];
            var connection = $"Server={postgresHost};Port={postgresPort};Userid=postgres;Password={postgresPassword};Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database=Web_Veleprodaja";
            return dbContextOptionsBuilder.UseNpgsql(connection, x =>
            {
                x.MigrationsHistoryTable("migrations_history");
                x.MigrationsAssembly("TD.Web.Veleprodaja.Api");
            });
        }
    }
}
