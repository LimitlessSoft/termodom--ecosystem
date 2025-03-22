using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Office.InterneOtpremnice.Contracts.Enums;
using TD.Office.InterneOtpremnice.Contracts.Requests;
using TD.Office.Public.Contracts.Interfaces;

namespace TD.Office.Public.Api.Controllers;

public class InterneOtpremniceController(IInterneOtpremniceManager interneOtpremniceManager)
	: ControllerBase
{
	[HttpGet]
	[LSCoreAuth]
	[Route("/interne-otpremnice")]
	public async Task<IActionResult> GetMultiple([FromQuery] GetMultipleRequest request) =>
		Ok(await interneOtpremniceManager.GetMultipleAsync(request));

	[HttpPost]
	[LSCoreAuth]
	[Route("/interne-otpremnice")]
	public async Task<IActionResult> Create([FromBody] InterneOtpremniceCreateRequest request) =>
		Ok(await interneOtpremniceManager.CreateAsync(request));

	[HttpGet]
	[Route("/interne-otpremnice/{Id}")]
	public async Task<IActionResult> Get([FromRoute] LSCoreIdRequest request) =>
		Ok(await interneOtpremniceManager.GetAsync(request));

	[HttpPut]
	[Route("/interne-otpremnice/{Id}/items")]
	public async Task<IActionResult> SaveItem(
		[FromBody] InterneOtpremniceItemCreateRequest request
	) => Ok(await interneOtpremniceManager.SaveItemAsync(request));

	[HttpDelete]
	[Route("/interne-otpremnice/{Id}/items/{ItemId}")]
	public async Task<IActionResult> DeleteItem(
		[FromRoute] InterneOtpremniceItemDeleteRequest request
	)
	{
		await interneOtpremniceManager.DeleteItemAsync(request);
		return Ok();
	}

	[HttpPost]
	[Route("/interne-otpremnice/{Id}/state/{State}")]
	public async Task<IActionResult> ChangeState(
		[FromRoute] LSCoreIdRequest request,
		[FromRoute] InternaOtpremnicaStatus state
	)
	{
		await interneOtpremniceManager.ChangeStateAsync(request, state);
		return Ok();
	}

	[HttpPost]
	[Route("/interne-otpremnice/{Id}/forward-to-komercijalno")]
	public async Task<IActionResult> ForwardToKomercijalno([FromRoute] LSCoreIdRequest request) =>
		Ok(await interneOtpremniceManager.ForwardToKomercijalno(request));
}
