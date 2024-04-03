using TD.Web.Public.Contracts.Requests.Statistics;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Public.Domain.Managers
{
    public class StatisticsManager : LSCoreBaseManager<StatisticsManager>, IStatisticsManager
    {
        private readonly ILogger<StatisticsManager> _logger;
        
        public StatisticsManager(ILogger<StatisticsManager> logger, DbContextOptions dbContextOptions)
            : base(logger, new WebDbContext(dbContextOptions))
        {
            _logger = logger;
        }

        public Task LogAsync(ProductViewCountRequest request) =>
            Task.Run(() =>
            {
                var insertResponse = Insert(new StatisticsItemEntity
                {
                    Type = StatisticType.ProductViewCount,
                    Value = request.ProductId.ToString()
                });
                if(insertResponse.NotOk)
                    _logger.LogError(JsonConvert.SerializeObject(insertResponse));
            });

        public Task LogAsync(ProductSearchKeywordRequest request) =>
            Task.Run(() =>
            {
                var insertResponse = Insert(new StatisticsItemEntity
                {
                    Type = StatisticType.SearchPhrase,
                    Value = request.SearchPhrase.ToLower()
                });
                if(insertResponse.NotOk)
                    _logger.LogError(JsonConvert.SerializeObject(insertResponse));
            });
    }
}