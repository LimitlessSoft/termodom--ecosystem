using Microsoft.AspNetCore.Mvc;
using TD.Office.MassSMS.Contracts.Interfaces;
using TD.Office.MassSMS.Contracts.Requests;

namespace TD.Office.MassSMS.Api.Controllers;

public class MassSMSController(IMassSMSManager manager) : ControllerBase
{
	[HttpPost]
	[Route("/mass-sms/queue")]
	public IActionResult Queue([FromBody] QueueSmsRequest request)
	{
		manager.Queue(request);
		return Ok();
	}

	[HttpPost]
	[Route("/mass-sms/mass-queue")]
	public IActionResult MassQueue([FromBody] MassQueueSmsRequest request)
	{
		manager.MassQueue(request);
		return Ok();
	}

	[HttpGet]
	[Route("/mass-sms/queue")]
	public IActionResult GetQueue() => Ok(manager.GetQueue());

	[HttpGet]
	[Route("/mass-sms/status")]
	public IActionResult GetStatus() => Ok(manager.GetCurrentStatus());

	[HttpGet]
	[Route("/mass-sms/queue-count")]
	public IActionResult GetQueueCount() => Ok(manager.GetQueueCount());

	[HttpDelete]
	[Route("/mass-sms/clear-queue")]
	public IActionResult ClearQueue()
	{
		manager.ClearQueue();
		return Ok();
	}
}
