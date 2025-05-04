using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.Magacini;

namespace TD.Komercijalno.Api.Controllers;

[ApiController]
public class MagaciniController(IMagacinManager magacinManager) : Controller
{
	[HttpGet]
	[Route("/magacini")]
	public List<MagacinDto> GetMultiple([FromQuery] MagaciniGetMultipleRequest request) =>
		magacinManager.GetMultiple(request);
}
