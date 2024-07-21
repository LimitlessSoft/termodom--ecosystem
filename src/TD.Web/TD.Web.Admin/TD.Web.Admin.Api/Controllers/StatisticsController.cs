using Microsoft.AspNetCore.Authorization;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Statistics;
using TD.Web.Admin.Contracts.Dtos.Statistics;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers;

[Authorize]
[ApiController]
[Permissions(Permission.Access)]
public class StatisticsController (IStatisticsManager statisticsManager) : ControllerBase
{
    [HttpGet]
    [Route("/products-statistics")]
    public ProductsStatisticsDto GetProductsStatistics([FromQuery] ProductsStatisticsRequest request) =>
        statisticsManager.GetProductsStatistics(request);
        
    [HttpGet]
    [Route("/search-phrases-statistics")]
    public SearchPhrasesStatisticsDto GetSearchPhrasesStatistics([FromQuery] SearchPhrasesStatisticsRequest request) =>
        statisticsManager.GetSearchPhrasesStatistics(request);
}