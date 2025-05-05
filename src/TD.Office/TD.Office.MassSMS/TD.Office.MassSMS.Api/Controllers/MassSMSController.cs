using Microsoft.AspNetCore.Mvc;
using TD.Office.MassSMS.Contracts.Interfaces;
using TD.Office.MassSMS.Contracts.Requests;

namespace TD.Office.MassSMS.Api.Controllers;

public class MassSMSController(IMassSMSManager manager) : ControllerBase
{
	[HttpPost]
	[Route("/mass-sms/invoke-sending")]
	public IActionResult InvokeSending()
	{
		manager.InvokeSending();
		return Ok();
	}

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

	[HttpDelete("/mass-sms/clear-duplicates")]
	public IActionResult ClearDuplicates()
	{
		manager.ClearDuplicates();
		return Ok();
	}

	[HttpDelete("/mass-sms/clear-blacklisted")]
	public IActionResult ClearBlacklisted()
	{
		manager.ClearBlacklisted();
		return Ok();
	}

	[HttpGet("/mass-sms/{number}/is-blacklisted")]
	public IActionResult IsBlacklisted([FromRoute] string number) =>
		Ok(manager.IsBlacklisted(number));

	[HttpPost("/mass-sms/{number}/blacklist")]
	public IActionResult Blacklist([FromRoute] string number)
	{
		manager.Blacklist(number);
		return Ok();
	}

	[HttpPut]
	[Route("/mass-sms/text")]
	public IActionResult SetText([FromBody] SetTextRequest request)
	{
		manager.SetText(request);
		return Ok();
	}
}
