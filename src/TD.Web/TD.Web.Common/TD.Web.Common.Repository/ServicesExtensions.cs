using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using LSCore.Repository;
using System.Reflection;

namespace TD.Web.Common.Repository;

public static class ServicesExtensions
{
    public static void RegisterDatabase(this IServiceCollection serviceCollection, IConfigurationRoot configurationRoot)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        serviceCollection.AddEntityFrameworkNpgsql()
            .AddDbContext<WebDbContext>((serviceProvider, options) =>
            {
                options.ConfigureDbContext(configurationRoot, Assembly.GetCallingAssembly().GetName().Name!);
            });
    }
}