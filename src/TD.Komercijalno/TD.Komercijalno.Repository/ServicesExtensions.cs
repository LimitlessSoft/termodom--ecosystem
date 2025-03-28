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
			//.AddEntityFrameworkFirebird() // new version problem. Not sure what it does
			.AddDbContext<KomercijalnoDbContext>(
				(serviceProvider, options) =>
				{
					options
						.UseFirebird(configurationRoot["ConnectionString_Komercijalno"]);
						//.UseInternalServiceProvider(serviceProvider); // new version problem. Not sure why it doesn't allow it
				}
			);
	}
}
