using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Admin.Contracts.Dtos.Statistics;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.Statistics;

namespace TD.Web.Admin.Api.Controllers
{
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsManager _statisticsManager;
        
        public StatisticsController(IStatisticsManager statisticsManager)
        {
            _statisticsManager = statisticsManager;
        }
        
        [HttpGet]
        [Route("/products-statistics")]
        public LSCoreResponse<ProductsStatisticsDto> GetProductsStatistics([FromQuery] ProductsStatisticsRequest request) =>
            _statisticsManager.GetProductsStatistics(request);
        
        [HttpGet]
        [Route("/search-phrases-statistics")]
        public LSCoreResponse<SearchPhrasesStatisticsDto> GetSearchPhrasesStatistics([FromQuery] SearchPhrasesStatisticsRequest request) =>
            _statisticsManager.GetSearchPhrasesStatistics(request);
    }
}