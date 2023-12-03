using LSCore.Contracts.Http;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Dtos.Cities;
using TD.Web.Common.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Api.Controllers
{
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly ICityManager _cityManager;
        
        public CitiesController(ICityManager cityManager)
        {
            _cityManager = cityManager;
        }

        [HttpGet]
        [Route("/cities")]
        public LSCoreListResponse<CityDto> GetMultiple() =>
            _cityManager.GetMultiple();
    }
}
