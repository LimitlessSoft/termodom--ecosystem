using TD.Web.Public.Contracts.Requests.Statistics;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Domain.Managers;

public class StatisticsManager (ILogger<StatisticsManager> logger)
    : LSCoreManagerBase<StatisticsManager>(logger), IStatisticsManager
{
    private readonly ILogger<StatisticsManager> _logger = logger;

    public Task LogAsync(ProductViewCountRequest request) =>
        Task.Run(() =>
        {
            Insert(new StatisticsItemEntity
            {
                Type = StatisticType.ProductViewCount,
                Value = request.ProductId.ToString()
            });
        });

    public Task LogAsync(ProductSearchKeywordRequest request) =>
        Task.Run(() =>
        {
            Insert(new StatisticsItemEntity
            {
                Type = StatisticType.SearchPhrase,
                Value = request.SearchPhrase.ToLower()
            });
        });
}