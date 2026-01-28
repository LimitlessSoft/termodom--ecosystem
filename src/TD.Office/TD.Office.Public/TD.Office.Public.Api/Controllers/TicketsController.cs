using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Ticket;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access, Permission.TicketsRead)]
public class TicketsController(ITicketManager ticketManager) : ControllerBase
{
	[HttpGet]
	[Route("/tickets")]
	public IActionResult GetMultiple([FromQuery] GetMultipleTicketsRequest request) =>
		Ok(ticketManager.GetMultiple(request));

	[HttpGet]
	[Route("/tickets/{Id}")]
	public IActionResult GetSingle([FromRoute] LSCoreIdRequest request) =>
		Ok(ticketManager.GetSingle(request));

	[HttpGet]
	[Route("/tickets/recently-solved")]
	public IActionResult GetRecentlySolved() =>
		Ok(ticketManager.GetRecentlySolved());

	[HttpGet]
	[Route("/tickets/in-progress")]
	public IActionResult GetInProgress() =>
		Ok(ticketManager.GetInProgress());

	[HttpPut]
	[Route("/tickets")]
	[Permissions(Permission.TicketsCreateBug)]
	public IActionResult Save([FromBody] SaveTicketRequest request)
	{
		ticketManager.Save(request);
		return Ok();
	}

	[HttpDelete]
	[Route("/tickets/{id}")]
	[Permissions(Permission.TicketsCreateBug)]
	public IActionResult Delete([FromRoute] long id)
	{
		ticketManager.Delete(id);
		return Ok();
	}

	[HttpPut]
	[Route("/tickets/{id}/priority")]
	[Permissions(Permission.TicketsManagePriority)]
	public IActionResult UpdatePriority([FromRoute] long id, [FromBody] UpdateTicketPriorityRequest request)
	{
		ticketManager.UpdatePriority(id, request);
		return Ok();
	}

	[HttpPut]
	[Route("/tickets/{id}/status")]
	[Permissions(Permission.TicketsManageStatus)]
	public IActionResult UpdateStatus([FromRoute] long id, [FromBody] UpdateTicketStatusRequest request)
	{
		ticketManager.UpdateStatus(id, request);
		return Ok();
	}

	[HttpPut]
	[Route("/tickets/{id}/developer-notes")]
	[Permissions(Permission.TicketsDeveloperNotes)]
	public IActionResult UpdateDeveloperNotes([FromRoute] long id, [FromBody] UpdateDeveloperNotesRequest request)
	{
		ticketManager.UpdateDeveloperNotes(id, request);
		return Ok();
	}
}
