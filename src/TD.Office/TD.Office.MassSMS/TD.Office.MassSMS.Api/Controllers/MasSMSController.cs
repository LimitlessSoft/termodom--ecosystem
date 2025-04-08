using Microsoft.AspNetCore.Mvc;
using TD.Office.MassSMS.Contracts.Interfaces;

namespace TD.Office.MassSMS.Api.Controllers;

public class MasSMSController(IMassSMSManager manager) : ControllerBase
{
	[HttpGet]
	[Route("/ping")]
	public IActionResult Ping()
	{
		manager.InvokeSending();
		return Ok("Pong");
	}
}
