using Microsoft.AspNetCore.Authorization;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Cities;
using TD.Web.Common.Contracts.Dtos.Cities;
using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Attributes;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Permissions(Permission.Access)]
    public class CitiesController : ControllerBase
    {
        private readonly ICityManager _cityManager;
        
        public CitiesController(ICityManager cityManager)
        {
            _cityManager = cityManager;
        }

        [HttpGet]
        [Route("/cities")]
        public List<CityDto> GetMultiple([FromQuery] GetMultipleCitiesRequest request) =>
            _cityManager.GetMultiple(request);
    }
}