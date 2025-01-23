using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace TD.Office.Common.Repository
{
    public static class ServicesExtensions
    {
        public static void RegisterDatabase(this IServiceCollection serviceCollection, IConfigurationRoot configurationRoot)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            serviceCollection.AddEntityFrameworkNpgsql()
                .AddDbContext<OfficeDbContext>();
        }
    }
}