using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Statistics;

namespace TD.Web.Public.Domain.Managers;

public class StatisticsManager(
	ILogger<StatisticsManager> logger,
	IStatisticsItemRepository repository
) : IStatisticsManager
{
	private readonly ILogger<StatisticsManager> _logger = logger;

	public Task LogAsync(ProductViewCountRequest request) =>
		Task.Run(() =>
		{
			repository.Insert(
				new StatisticsItemEntity
				{
					Type = StatisticType.ProductViewCount,
					Value = request.ProductId.ToString()
				}
			);
		});

	public Task LogAsync(ProductSearchKeywordRequest request) =>
		Task.Run(() =>
		{
			Log(request);
		});

	public void Log(ProductSearchKeywordRequest request)
	{
		repository.Insert(
			new StatisticsItemEntity
			{
				Type = StatisticType.SearchPhrase,
				Value = request.SearchPhrase.ToLower()
			}
		);
	}
}
