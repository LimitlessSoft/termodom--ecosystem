using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Odsustvo;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access, Permission.KalendarAktivnostiRead)]
public class OdsustvoController(IOdsustvoManager odsustvoManager) : ControllerBase
{
	[HttpGet]
	[Route("/odsustvo/calendar")]
	public IActionResult GetCalendar([FromQuery] GetOdsustvoCalendarRequest request) =>
		Ok(odsustvoManager.GetCalendar(request));

	[HttpGet]
	[Route("/odsustvo/year-list")]
	[Permissions(Permission.KalendarAktivnostiEditAll)]
	public IActionResult GetYearList([FromQuery] GetOdsustvoYearListRequest request) =>
		Ok(odsustvoManager.GetYearList(request));

	[HttpGet]
	[Route("/odsustvo/{Id}")]
	public IActionResult GetSingle([FromRoute] LSCoreIdRequest request) =>
		Ok(odsustvoManager.GetSingle(request));

	[HttpPut]
	[Route("/odsustvo")]
	[Permissions(Permission.KalendarAktivnostiWrite)]
	public IActionResult Save([FromBody] SaveOdsustvoRequest request)
	{
		odsustvoManager.Save(request);
		return Ok();
	}

	[HttpDelete]
	[Route("/odsustvo/{id}")]
	[Permissions(Permission.KalendarAktivnostiDelete)]
	public IActionResult Delete([FromRoute] long id)
	{
		odsustvoManager.Delete(id);
		return Ok();
	}

	[HttpPut]
	[Route("/odsustvo/{id}/approve")]
	[Permissions(Permission.KalendarAktivnostiApprove)]
	public IActionResult Approve([FromRoute] long id)
	{
		odsustvoManager.Approve(id);
		return Ok();
	}

	[HttpPut]
	[Route("/odsustvo/{id}/realizovano")]
	[Permissions(Permission.KalendarAktivnostiWrite)]
	public IActionResult UpdateRealizovano([FromRoute] long id, [FromBody] UpdateRealizovanoRequest request)
	{
		odsustvoManager.UpdateRealizovano(id, request);
		return Ok();
	}
}
