using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TD.Komercijalno.Repository;

public static class ServicesExtensions
{
	public static void RegisterDatabase(
		this IServiceCollection serviceCollection,
		IConfigurationRoot configurationRoot
	)
	{
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
		serviceCollection
			.AddEntityFrameworkFirebird()
			.AddDbContext<KomercijalnoDbContext>(
				(serviceProvider, options) =>
				{
					options
						.UseFirebird(configurationRoot["ConnectionString_Komercijalno"])
						.UseInternalServiceProvider(serviceProvider);
				}
			);
	}
}
