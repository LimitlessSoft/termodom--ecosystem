using LSCore.Contracts.Extensions;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Statistics;
using TD.Web.Admin.Contracts.Dtos.Statistics;
using Microsoft.Extensions.Logging;
using LSCore.Domain.Managers;
using LSCore.Contracts.Http;
using LSCore.Domain.Validators;
using TD.Web.Admin.Contracts;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class StatisticsManager : LSCoreBaseManager<StatisticsManager>, IStatisticsManager
    {
        public StatisticsManager(ILogger<StatisticsManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse<ProductsStatisticsDto> GetProductsStatistics(ProductsStatisticsRequest request)
        {
            var response = new LSCoreResponse<ProductsStatisticsDto>();
            if (request.IsRequestInvalid(response))
                return response;

            var qProducts = Queryable<ProductEntity>();
            response.Merge(qProducts);
            if (response.NotOk)
                return response;
            
            var products = qProducts.Payload!.ToList();
            
            var productsViewsStatisticsItemsQueryResponse = Queryable<StatisticsItemEntity>()
                .LSCoreFilters(x => x.IsActive
                                    && x.CreatedAt >= request.DateFromUtc
                                    && x.CreatedAt <= request.DateToUtc
                                    && x.Type == StatisticType.ProductViewCount);
            response.Merge(productsViewsStatisticsItemsQueryResponse);
            if (response.NotOk)
                return response;
            
            var productsViewsStatisticsItems = productsViewsStatisticsItemsQueryResponse.Payload!.ToList();
            var productsViewsStatistics = new ProductsStatisticsViewsDto();
            foreach (var val in productsViewsStatisticsItems.Select(x => x.Value).Distinct())
            {
                if (!int.TryParse(val, out var productId))
                    continue;
                
                var product = products.FirstOrDefault(x => x.Id == productId);
                if (product == null)
                    continue;
                
                productsViewsStatistics.Items.Add(new ProductsStatisticsViewsItemDto()
                {
                    ProductId = productId,
                    Views = productsViewsStatisticsItems.Count(x => x.Value == val),
                    Name = product.Name
                });
            }

            response.Payload = new ProductsStatisticsDto()
            {
                Views = productsViewsStatistics
            };
            return response;
        }

        public LSCoreResponse<SearchPhrasesStatisticsDto> GetSearchPhrasesStatistics(SearchPhrasesStatisticsRequest request)
        {
            var response = new LSCoreResponse<SearchPhrasesStatisticsDto>();
            if (request.IsRequestInvalid(response))
                return response;

            response.Payload = new SearchPhrasesStatisticsDto();

            var q = Queryable<StatisticsItemEntity>()
                .LSCoreFilters(x => x.IsActive
                    && x.CreatedAt >= request.DateFromUtc
                    && x.CreatedAt <= request.DateToUtc
                    && x.Type == StatisticType.SearchPhrase);
            
            response.Merge(q);
            if (response.NotOk)
                return response;
            
            var searchPhrasesStatisticsItems = q.Payload!.ToList();

            var words = searchPhrasesStatisticsItems
                    .Select(x => x.Value!.ToLower().Trim().Split(' '))
                    .SelectMany(x => x)
                    .Distinct();
            foreach (var word in words.Where(x => !Constants.SearchPhrasesStatisticsExclude.Contains(x.ToLower().Trim())))
            {
                var phrasesContainingWord = searchPhrasesStatisticsItems.Where(x => x.Value!.ToLower().Contains(word)).ToList();
                
                response.Payload.Items.Add(new SearchPhrasesItemStatisticsDto()
                {
                    SearchedTimesCount = phrasesContainingWord.Count,
                    Keyword = word,
                    Phrases = phrasesContainingWord.Select(x => x.Value).ToList()!
                });
            }
            
            return response;
        }
    }
}