using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TD.Office.Common.Repository;

public static class ServicesExtensions
{
    public static void RegisterDatabase(
        this IServiceCollection serviceCollection,
        IConfigurationRoot configurationRoot
    )
    {
        serviceCollection
            .AddEntityFrameworkNpgsql()
            .AddDbContext<OfficeDbContext>(options =>
            {
                options.UseInternalServiceProvider(serviceCollection.BuildServiceProvider());
            });
    }
}
