using Microsoft.Extensions.DependencyInjection;
using TD.Web.Common.Contracts.Interfaces;

namespace TD.Web.Common.Repository;

public static class ServicesExtensions
{
	public static void RegisterDatabase(this IServiceCollection serviceCollection)
	{
		serviceCollection
			.AddEntityFrameworkNpgsql()
			.AddDbContext<WebDbContext>(
				(serviceProvider, options) =>
				{
					options.UseInternalServiceProvider(serviceProvider);
				}
			);
		serviceCollection.AddScoped<IWebDbContext>(sp => sp.GetRequiredService<WebDbContext>());
	}
}
