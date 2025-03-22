using Microsoft.AspNetCore.Mvc;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Interfaces.IManagers;
using TD.Office.InterneOtpremnice.Contracts.Requests;

namespace TD.Office.InterneOtpremnice.Api.Controllers;

public class InterneOtpremniceController(IInterneOtpremniceManager manager) : ControllerBase
{
	[HttpPost]
	[Route("/interne-otpremnice")]
	public IActionResult Create([FromBody] InterneOtpremniceCreateRequest request) =>
		Ok(manager.Create(request));

	[HttpGet]
	[Route("/interne-otpremnice")]
	public async Task<IActionResult> GetMultiple([FromQuery] GetMultipleRequest request) =>
		Ok(await manager.GetMultipleAsync(request));

	[HttpGet]
	[Route("/interne-otpremnice/{Id}")]
	public async Task<IActionResult> Get([FromRoute] IdRequest request) =>
		Ok(await manager.GetAsync(request));

	[HttpPut]
	[Route("/interne-otpremnice/{Id}/items")]
	public IActionResult SaveItem([FromBody] InterneOtpremniceItemCreateRequest request) =>
		Ok(manager.SaveItem(request));

	[HttpDelete]
	[Route("/interne-otpremnice/{Id}/items/{ItemId}")]
	public IActionResult DeleteItem([FromRoute] InterneOtpremniceItemDeleteRequest request)
	{
		manager.DeleteItem(request);
		return Ok();
	}

	[HttpPost]
	[Route("/interne-otpremnice/{Id}/state/{State}")]
	public IActionResult ChangeState(
		[FromRoute] IdRequest request,
		[FromRoute] InternaOtpremnicaStatus state
	)
	{
		manager.ChangeState(request, state);
		return Ok();
	}

	[HttpPost]
	[Route("/interne-otpremnice/{Id}/forward-to-komercijalno")]
	public async Task<IActionResult> ForwardToKomercijalno([FromRoute] IdRequest request)
	{
		return Ok(await manager.ForwardToKomercijalnoAsync(request));
	}
}
