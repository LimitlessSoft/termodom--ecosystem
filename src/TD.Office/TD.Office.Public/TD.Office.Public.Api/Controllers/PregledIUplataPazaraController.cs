using Microsoft.AspNetCore.Mvc;
using TD.Office.PregledIUplataPazara.Client;
using TD.Office.PregledIUplataPazara.Contracts.Requests;

namespace TD.Office.Public.Api.Controllers;

public class PregledIUplataPazaraController(TDOfficePregledIUplataPazaraClient client)
	: ControllerBase
{
	[HttpGet("/pregled-i-uplata-pazara")]
	public async Task<IActionResult> GetMultiple(
		[FromQuery] GetPregledIUplataPazaraRequest request
	) => Ok(await client.GetMultiple(request));

	[HttpGet("/pregled-i-uplata-pazara/neispravne-stavke-izvoda")]
	public async Task<IActionResult> GetNeispravneStavkeIzvoda() =>
		Ok(await client.GetNeispravneStavkeIzvoda());
}
