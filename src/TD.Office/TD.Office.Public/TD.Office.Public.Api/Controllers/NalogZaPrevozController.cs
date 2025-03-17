using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.NalogZaPrevoz;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[ApiController]
public class NalogZaPrevozController(INalogZaPrevozManager nalogZaPrevozManager) : ControllerBase
{
	[HttpGet]
	[Route("/nalog-za-prevoz")]
	public IActionResult GetMultiple([FromQuery] GetMultipleNalogZaPrevozRequest request) =>
		Ok(nalogZaPrevozManager.GetMultiple(request));

	[HttpGet]
	[Route("/nalog-za-prevoz/{Id}")]
	public IActionResult GetSingle([FromRoute] LSCoreIdRequest request) =>
		Ok(nalogZaPrevozManager.GetSingle(request));

	[HttpPut]
	[Route("/nalog-za-prevoz")]
	public IActionResult SaveNalogZaPrevoz([FromBody] SaveNalogZaPrevozRequest request)
	{
		nalogZaPrevozManager.SaveNalogZaPrevoz(request);
		return Ok();
	}

	[HttpGet]
	[Route("/nalog-za-prevoz-referentni-dokument")]
	public async Task<IActionResult> GetReferentniDokument(
		[FromQuery] GetReferentniDokumentNalogZaPrevozRequest request
	) => Ok(await nalogZaPrevozManager.GetReferentniDokumentAsync(request));
}
