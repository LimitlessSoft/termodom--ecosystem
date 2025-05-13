using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Dtos.Namene;
using TD.Komercijalno.Contracts.IManagers;

namespace TD.Komercijalno.Api.Controllers
{
	[ApiController]
	public class NameneController(INamenaManager namenaManager) : Controller
	{
		[HttpGet]
		[Route("/namene")]
		public List<NamenaDto> GetMultiple() => namenaManager.GetMultiple();
	}
}
