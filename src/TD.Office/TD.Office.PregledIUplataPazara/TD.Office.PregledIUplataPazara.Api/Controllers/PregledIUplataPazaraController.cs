using Microsoft.AspNetCore.Mvc;
using TD.Office.PregledIUplataPazara.Contracts.Interfaces;
using TD.Office.PregledIUplataPazara.Contracts.Requests;
using TD.Office.PregledIUplataPazara.Contracts.Responses;

namespace TD.Office.PregledIUplataPazara.Api.Controllers;

public class PregledIUplataPazaraController(
	IPregledIUplataPazaraManager manager,
	ILogger<PregledIUplataPazaraController> logger
) : ControllerBase
{
	[HttpGet]
	[Route("/pregled-i-uplata-pazara")]
	public async Task<IActionResult> GetPregledIUplataPazara(
		[FromQuery] GetPregledIUplataPazaraRequest request
	)
	{
		logger.LogInformation(
			"Starting request for PregledIUplataPazara with parameters: {@Request}",
			request
		);
		var res = await manager.GetAsync(request);
		return Ok(res);
	}
}
