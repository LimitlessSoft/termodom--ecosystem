using Microsoft.AspNetCore.Mvc;
using TD.Web.Common.Contracts.Dtos.Cities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Requests.Cities;

namespace TD.Web.Public.Api.Controllers;

[ApiController]
public class CitiesController(ICityManager cityManager) : ControllerBase
{
	[HttpGet]
	[Route("/cities")]
	public List<CityDto> GetMultiple([FromQuery] GetMultipleCitiesRequest request) =>
		cityManager.GetMultiple(request);
}
