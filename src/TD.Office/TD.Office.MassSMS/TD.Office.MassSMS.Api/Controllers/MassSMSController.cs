using Microsoft.AspNetCore.Mvc;
using TD.Office.MassSMS.Contracts.Interfaces;
using TD.Office.MassSMS.Contracts.Requests;

namespace TD.Office.MassSMS.Api.Controllers;

public class MassSMSController(IMassSMSManager manager) : ControllerBase
{
	[HttpPost]
	[Route("/queue")]
	public IActionResult Queue([FromBody] QueueSmsRequest request)
	{
		manager.Queue(request);
		return Ok();
	}

	[HttpGet]
	[Route("/status")]
	public IActionResult GetStatus() => Ok(manager.GetCurrentStatus());

	[HttpGet]
	[Route("/queue-count")]
	public IActionResult GetQueueCount() => Ok(manager.GetQueueCount());
}
