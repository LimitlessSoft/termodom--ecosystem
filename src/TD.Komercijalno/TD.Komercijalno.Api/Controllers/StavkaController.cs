using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Stavke;

namespace TD.Komercijalno.Api.Controllers
{
	[ApiController]
	public class StavkaController(IStavkaManager stavkaManager) : Controller
	{
		[HttpPost]
		[Route("/stavke")]
		public StavkaDto Create([FromBody] StavkaCreateRequest request) =>
			stavkaManager.Create(request);

		[HttpPost]
		[Route("/stavke-optimized")]
		public List<StavkaDto> CreateOptimized([FromBody] StavkeCreateOptimizedRequest request) =>
			stavkaManager.CreateOptimized(request);

		[HttpGet]
		[Route("/stavke")]
		public List<StavkaDto> GetMultiple([FromQuery] StavkaGetMultipleRequest request) =>
			stavkaManager.GetMultiple(request);

		[HttpDelete]
		[Route("/stavke")]
		public IActionResult DeleteStavke([FromQuery] StavkeDeleteRequest request)
		{
			stavkaManager.DeleteStavke(request);
			return Ok();
		}

		[HttpGet]
		[Route("/stavke-by-robaid")]
		public IActionResult GetMultipleByRobaId([FromQuery] StavkeGetMultipleByRobaId request) =>
			Ok(stavkaManager.GetMultipleByRobaId(request));
	}
}
