using Microsoft.AspNetCore.Mvc;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Popisi;

namespace TD.Office.Public.Api.Controllers;

public class PopisiController(IPopisManager popisManager) : ControllerBase
{
	[HttpGet]
	[Route("/popisi")]
	public IActionResult GetMultiple(GetPopisiRequest request) =>
		Ok(popisManager.GetMultiple(request));

	[HttpGet]
	[Route("/popisi/{Id}")]
	public IActionResult GetById(long Id) => Ok(popisManager.GetById(Id));

	[HttpPost]
	[Route("/popisi")]
	public IActionResult CreatePopis(CreatePopisiRequest request) =>
		Ok(popisManager.Create(request));

	[HttpPost]
	[Route("/popisi/{Id}/storniraj")]
	public IActionResult StornirajPopis(long Id) => Ok(popisManager.StornirajPopis(Id));

	[HttpPut]
	[Route("/popisi/{Id}/status")]
	public IActionResult SetStatus(long Id, [FromBody] PopisSetStatusRequest request)
	{
		request.Id = Id;
		popisManager.SetStatus(request);
		return Ok();
	}

	[HttpPost]
	[Route("/popisi/{Id}/items")]
	public IActionResult AddItemToPopis(long Id, [FromBody] PopisAddItemRequest request)
	{
		request.PopisId = Id;
		return Ok(popisManager.AddItemToPopis(request));
	}

	[HttpDelete]
	[Route("/popisi/{Id}/items/{itemId}")]
	public IActionResult RemoveItemFromPopis(long Id, long itemId)
	{
		popisManager.RemoveItemFromPopis(Id, itemId);
		return Ok();
	}

	[HttpPut]
	[Route("/popisi/{Id}/items{itemId}/popisana-kolicina")]
	public IActionResult UpdatePopisanaKolicina(
		long Id,
		long itemId,
		[FromBody] double popisanaKolicina
	)
	{
		popisManager.UpdatePopisanaKolicina(Id, itemId, popisanaKolicina);
		return Ok();
	}

	[HttpPut]
	[Route("/popisi/{Id}/items{itemId}/narucena-kolicina")]
	public IActionResult UpdateNarucenaKolicina(
		long Id,
		long itemId,
		[FromBody] double narucenaKolicina
	)
	{
		popisManager.UpdateNarucenaKolicina(Id, itemId, narucenaKolicina);
		return Ok();
	}
}
