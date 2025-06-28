using Microsoft.AspNetCore.Mvc;
using TD.Office.PregledIUplataPazara.Contracts.Interfaces;
using TD.Office.PregledIUplataPazara.Contracts.Requests;

namespace TD.Office.PregledIUplataPazara.Api.Controllers;

public class PregledIUplataPazaraController(IPregledIUplataPazaraManager manager) : ControllerBase
{
	[HttpGet]
	[Route("/pregled-i-uplata-pazara")]
	public async Task<IActionResult> GetPregledIUplataPazara(
		[FromQuery] GetPregledIUplataPazaraRequest request
	) => Ok(await manager.GetAsync(request));

	[HttpGet]
	[Route("/pregled-i-uplata-pazara/neispravne-stavke-izvoda")]
	public async Task<IActionResult> GetNeispravneStavkeIzvoda() =>
		Ok(await manager.GetNeispravneStavkeIzvoda());
}
