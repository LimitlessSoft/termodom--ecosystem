using Microsoft.AspNetCore.Mvc;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Popisi;

namespace TD.Office.Public.Api.Controllers;

public class PopisiController(IPopisManager popisManager) : ControllerBase
{
	[HttpGet]
	[Route("/popisi")]
	public IActionResult GetMultiple([FromQuery] GetPopisiRequest request) =>
		Ok(popisManager.GetMultiple(request));

	[HttpGet]
	[Route("/popisi/{Id}")]
	public IActionResult GetById([FromRoute] long Id) => Ok(popisManager.GetById(Id));

	[HttpPost]
	[Route("/popisi")]
	public async Task<IActionResult> CreatePopis([FromBody] CreatePopisiRequest request) =>
		Ok(await popisManager.CreateAsync(request));

	[HttpPost]
	[Route("/popisi/{Id}/storniraj")]
	public async Task<IActionResult> StornirajPopis([FromRoute] long Id) =>
		Ok(await popisManager.StornirajPopisAsync(Id));

	[HttpPut]
	[Route("/popisi/{Id}/status")]
	public async Task<IActionResult> SetStatus(
		[FromRoute] long Id,
		[FromBody] PopisSetStatusRequest request
	)
	{
		request.Id = Id;
		await popisManager.SetStatusAsync(request);
		return Ok();
	}

	[HttpPost]
	[Route("/popisi/{Id}/items")]
	public async Task<IActionResult> AddItemToPopis(
		[FromRoute] long Id,
		[FromBody] PopisAddItemRequest request
	)
	{
		request.PopisId = Id;
		return Ok(await popisManager.AddItemToPopisAsync(request));
	}

	[HttpDelete]
	[Route("/popisi/{Id}/items/{itemId}")]
	public IActionResult RemoveItemFromPopis([FromRoute] long Id, [FromRoute] long itemId)
	{
		popisManager.RemoveItemFromPopisAsync(Id, itemId);
		return Ok();
	}

	[HttpPut]
	[Route("/popisi/{Id}/items{itemId}/popisana-kolicina")]
	public async Task<IActionResult> UpdatePopisanaKolicina(
		[FromRoute] long Id,
		[FromRoute] long itemId,
		[FromBody] double popisanaKolicina
	)
	{
		await popisManager.UpdatePopisanaKolicinaAsync(Id, itemId, popisanaKolicina);
		return Ok();
	}

	[HttpPut]
	[Route("/popisi/{Id}/items{itemId}/narucena-kolicina")]
	public async Task<IActionResult> UpdateNarucenaKolicina(
		[FromRoute] long Id,
		[FromRoute] long itemId,
		[FromBody] double narucenaKolicina
	)
	{
		await popisManager.UpdateNarucenaKolicinaAsync(Id, itemId, narucenaKolicina);
		return Ok();
	}

	[HttpPost]
	[Route("/popisi/{Id}/masovno-dodavanje")]
	public async Task<IActionResult> PopisMasovnoDodavanjeStavki(
		[FromRoute] long Id,
		[FromBody] PopisMasovnoDodavanjeStavkiRequest request
	)
	{
		await popisManager.MasovnoDodavanjeStavkiAsync(Id, request);
		return Ok();
	}
}
