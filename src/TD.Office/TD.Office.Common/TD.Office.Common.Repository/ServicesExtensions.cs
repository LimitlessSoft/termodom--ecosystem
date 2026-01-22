using Microsoft.Extensions.DependencyInjection;
using TD.Office.Common.Contracts.IRepositories;

namespace TD.Office.Common.Repository;

public static class ServicesExtensions
{
	public static void RegisterDatabase(this IServiceCollection serviceCollection)
	{
		serviceCollection
			.AddEntityFrameworkNpgsql()
			.AddDbContext<OfficeDbContext>(
				(serviceProvider, options) =>
				{
					options.UseInternalServiceProvider(serviceProvider);
				}
			);
		serviceCollection.AddSingleton<IOfficeDbContextFactory, OfficeDbContextFactory>();
	}
}
