using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using LSCore.Repository;

namespace TD.Office.Common.Repository
{
    public static class ServicesExtensions
    {
        public static void RegisterDatabase(this IServiceCollection serviceCollection, IConfigurationRoot configurationRoot)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            serviceCollection.AddEntityFrameworkNpgsql()
                .AddDbContext<OfficeDbContext>((services, options) =>
                {
                    options.ConfigureDbContext(configurationRoot, "TD.Office.Common.DbMigrations")
                        .UseInternalServiceProvider(services);
                });
        }
    }
}