using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;
using TD.Komercijalno.Contracts.IManagers;

namespace TD.Komercijalno.Api.Controllers
{
	[ApiController]
	public class NaciniPlacanjaController(INacinPlacanjaManager nacinPlacanjaManager) : Controller
	{
		[HttpGet]
		[Route("/nacini-placanja")]
		public List<NacinPlacanjaDto> GetMultiple() => nacinPlacanjaManager.GetMultiple();
	}
}
