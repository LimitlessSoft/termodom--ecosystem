using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TD.Core.Contracts.Interfaces;

namespace TD.Core.Repository
{
    public static class Extensions
    {
        public static EntityTypeBuilder<TEntity> AddMap<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, IEntityMap<TEntity> map)
            where TEntity : class
        {
            return map.Map(entityTypeBuilder);
        }

        public static void ConfigureNpgsqlDatabase<TDbContext, TStartup>(this IConfigurationRoot configurationRoot, IServiceCollection services, string dbName) where TDbContext : DbContext
            where TStartup : IMigratable
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<TDbContext>((services, options) =>
                {
                    options.ConfigureDbContext(configurationRoot, typeof(TStartup).Namespace, dbName).UseInternalServiceProvider(services);
                });
        }

        public static DbContextOptionsBuilder ConfigureDbContext(this DbContextOptionsBuilder dbContextOptionsBuilder, IConfigurationRoot configurationRoot, string migrationAssembly, string dbName)
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
            var connection = $"Server={postgresHost};Port={postgresPort};Userid=postgres;Password={postgresPassword};Pooling=false;MinPoolSize=1;MaxPoolSize=20;Timeout=15;Database={dbName};Include Error Detail=true;";

            return dbContextOptionsBuilder.UseNpgsql(connection, x =>
            {
                x.MigrationsHistoryTable("migrations_history");
                x.MigrationsAssembly(migrationAssembly);
            });
        }
    }
}
