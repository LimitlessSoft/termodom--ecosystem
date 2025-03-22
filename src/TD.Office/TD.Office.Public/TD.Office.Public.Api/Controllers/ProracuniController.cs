using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Proracuni;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access, Permission.ProracuniRead)]
public class ProracuniController(IProracunManager proracunManager) : ControllerBase
{
	[HttpGet]
	[Route("/proracuni")]
	public IActionResult GetMultiple([FromQuery] ProracuniGetMultipleRequest request) =>
		Ok(proracunManager.GetMultiple(request));

	[HttpPost]
	[Route("/proracuni")]
	public IActionResult Create([FromBody] ProracuniCreateRequest request)
	{
		proracunManager.Create(request);
		return Ok();
	}

	[HttpGet]
	[Route("/proracuni/{Id}")]
	public IActionResult GetSingle([FromRoute] LSCoreIdRequest request) =>
		Ok(proracunManager.GetSingle(request));

	[HttpPut]
	[Route("/proracuni/{Id}/state")]
	public IActionResult PutState(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] ProracuniPutStateRequest request
	)
	{
		request.Id = idRequest.Id;
		proracunManager.PutState(request);
		return Ok();
	}

	[HttpPut]
	[Route("/proracuni/{Id}/ppid")]
	public IActionResult PutPPID(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] ProracuniPutPPIDRequest request
	)
	{
		request.Id = idRequest.Id;
		proracunManager.PutPPID(request);
		return Ok();
	}

	[HttpPut]
	[Route("/proracuni/{Id}/nuid")]
	public IActionResult PutNUID(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] ProracuniPutNUIDRequest request
	)
	{
		request.Id = idRequest.Id;
		proracunManager.PutNUID(request);
		return Ok();
	}

	[HttpPost]
	[Route("/proracuni/{Id}/items")]
	public async Task<IActionResult> AddItem(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] ProracuniAddItemRequest request
	)
	{
		request.Id = idRequest.Id;
		return Ok(await proracunManager.AddItemAsync(request));
	}

	[HttpDelete]
	[Route("/proracuni/{ProracunId}/items/{Id}")]
	public IActionResult DeleteItem([FromRoute] LSCoreIdRequest request)
	{
		proracunManager.DeleteItem(request);
		return Ok();
	}

	[HttpPut]
	[Route("/proracuni/{Id}/items/{StavkaId}/kolicina")]
	public IActionResult PutItemKolicina(
		[FromRoute] long Id,
		[FromRoute] long StavkaId,
		[FromBody] ProracuniPutItemKolicinaRequest request
	)
	{
		request.Id = Id;
		request.StavkaId = StavkaId;
		proracunManager.PutItemKolicina(request);
		return Ok();
	}

	[HttpPut]
	[Route("/proracuni/{Id}/items/{StavkaId}/rabat")]
	public IActionResult PutItemRabat(
		[FromRoute] long Id,
		[FromRoute] long StavkaId,
		[FromBody] ProracuniPutItemRabatRequest request
	)
	{
		request.Id = Id;
		request.StavkaId = StavkaId;
		proracunManager.PutItemRabat(request);
		return Ok();
	}

	[HttpPost]
	[Route("/proracuni/{Id}/forward-to-komercijalno")]
	public async Task<IActionResult> ForwardToKomercijalno([FromRoute] LSCoreIdRequest request) =>
		Ok(await proracunManager.ForwardToKomercijalnoAsync(request));
}
