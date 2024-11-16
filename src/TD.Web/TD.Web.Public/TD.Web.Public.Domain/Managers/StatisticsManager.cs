using LSCore.Contracts;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.Statistics;

namespace TD.Web.Public.Domain.Managers;

public class StatisticsManager(
    ILogger<StatisticsManager> logger,
    WebDbContext dbContext,
    LSCoreContextUser contextUser
) : LSCoreManagerBase<StatisticsManager>(logger, dbContext, contextUser), IStatisticsManager
{
    private readonly ILogger<StatisticsManager> _logger = logger;

    public Task LogAsync(ProductViewCountRequest request) =>
        Task.Run(() =>
        {
            Insert(
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
        Insert(
            new StatisticsItemEntity
            {
                Type = StatisticType.SearchPhrase,
                Value = request.SearchPhrase.ToLower()
            }
        );
    }
}
