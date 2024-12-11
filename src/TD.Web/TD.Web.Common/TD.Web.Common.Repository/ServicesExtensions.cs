using System.Reflection;
using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TD.Web.Common.Repository;

public static class ServicesExtensions
{
    public static void RegisterDatabase(
        this IServiceCollection serviceCollection,
        IConfigurationRoot configurationRoot
    )
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        serviceCollection
            .AddEntityFrameworkNpgsql()
            .AddDbContext<WebDbContext>(
                (serviceProvider, options) =>
                {
                    options.ConfigureDbContext(configurationRoot, "TD.Web.Common.DbMigrations");
                }
            );
    }
}
