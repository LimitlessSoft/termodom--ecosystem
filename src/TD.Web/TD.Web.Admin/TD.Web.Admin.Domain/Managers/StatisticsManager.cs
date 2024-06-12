using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Statistics;
using TD.Web.Admin.Contracts.Dtos.Statistics;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;
using Microsoft.Extensions.Logging;
using LSCore.Domain.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;
using TD.Web.Admin.Contracts;

namespace TD.Web.Admin.Domain.Managers;

public class StatisticsManager (ILogger<StatisticsManager> logger, WebDbContext dbContext)
    : LSCoreManagerBase<StatisticsManager>(logger, dbContext), IStatisticsManager
{
    public ProductsStatisticsDto GetProductsStatistics(ProductsStatisticsRequest request)
    {
        request.Validate();

        var products = Queryable<ProductEntity>();
            
        var productsViewsStatisticsItemsQuery = Queryable<StatisticsItemEntity>()
            .Where(x => x.IsActive
                                && x.CreatedAt >= request.DateFromUtc
                                && x.CreatedAt <= request.DateToUtc
                                && x.Type == StatisticType.ProductViewCount);
            
        var productsViewsStatisticsItems = productsViewsStatisticsItemsQuery!.ToList();
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

        return new ProductsStatisticsDto()
        {
            Views = productsViewsStatistics
        };
    }

    public SearchPhrasesStatisticsDto GetSearchPhrasesStatistics(SearchPhrasesStatisticsRequest request)
    {
        request.Validate();

        var dto = new SearchPhrasesStatisticsDto();

        var query = Queryable<StatisticsItemEntity>()
            .Where(x => x.IsActive
                                && x.CreatedAt >= request.DateFromUtc
                                && x.CreatedAt <= request.DateToUtc
                                && x.Type == StatisticType.SearchPhrase);
            
        var searchPhrasesStatisticsItems = query.ToList();

        var words = searchPhrasesStatisticsItems
            .Select(x => x.Value!.ToLower().Trim().Split(' '))
            .SelectMany(x => x)
            .Distinct();
        foreach (var word in words.Where(x => !Constants.SearchPhrasesStatisticsExclude.Contains(x.ToLower().Trim())))
        {
            var phrasesContainingWord = searchPhrasesStatisticsItems.Where(x => x.Value!.ToLower().Split(' ').Contains(word)).ToList();
                
            dto.Items.Add(new SearchPhrasesItemStatisticsDto()
            {
                SearchedTimesCount = phrasesContainingWord.Count,
                Keyword = word,
                Phrases = phrasesContainingWord.Select(x => x.Value).ToList()!
            });
        }
            
        return dto;
    }
}