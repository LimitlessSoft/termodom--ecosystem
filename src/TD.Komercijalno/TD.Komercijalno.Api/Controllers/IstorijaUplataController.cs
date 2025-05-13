using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.IManagers;
using TD.Komercijalno.Contracts.Requests.IstorijaUplata;

namespace TD.Komercijalno.Api.Controllers;

[ApiController]
public class IstorijaUplataController(IIstorijaUplataManager istorijaUplataManager) : ControllerBase
{
	[HttpGet]
	[Route("/istorija-uplata")]
	public IActionResult GetMultiple([FromQuery] IstorijaUplataGetMultipleRequest request) =>
		Ok(istorijaUplataManager.GetMultiple(request));
}
