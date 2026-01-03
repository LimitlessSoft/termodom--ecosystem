using LSCore.Auth.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.MassSMS.Client;
using TD.Office.MassSMS.Contracts.Requests;
using TD.Office.Public.Contracts.Interfaces.IManagers;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[Permissions(Permission.Access, Permission.MasovniSMSRead)]
public class MassSMSController(
	MassSMSApiClient client,
	ITDWebAdminApiManager webAdminApiManager,
	ITDKomercijalnoApiManager komercijalnoApiManager
) : ControllerBase
{
	[HttpPost]
	[Route("/mass-sms/invoke-sending")]
	public async Task<IActionResult> InvokeSendingAsync()
	{
		await client.InvokeSendingAsync();
		return Ok();
	}

	[HttpGet]
	[Route("/mass-sms/status")]
	public async Task<IActionResult> GetCurrentStatusAsync() =>
		Ok(await client.GetCurrentStatusAsync());

	[HttpGet]
	[Route("/mass-sms/queue")]
	public async Task<IActionResult> GetQueueAsync() => Ok(await client.GetQueueAsync());

	[HttpGet]
	[Route("/mass-sms/queue-count")]
	public async Task<IActionResult> GetQueueCountAsync() => Ok(await client.GetQueueCountAsync());

	[HttpDelete]
	[Route("/mass-sms/clear-queue")]
	public async Task<IActionResult> ClearQueueAsync()
	{
		await client.ClearQueueAsync();
		return Ok();
	}

	[HttpPost]
	[Route("/mass-sms/prepare-phone-numbers-from-public-web")]
	public async Task<IActionResult> PreparePhoneNumbersFromPublicWebAsync()
	{
		var numbers = await webAdminApiManager.GetPhoneNumbers();
		await client.MassQueueAsync(
			new MassQueueSmsRequest { Message = "Termodom", PhoneNumbers = numbers }
		);
		return Ok();
	}

	[HttpPost]
	[Route("/mass-sms/prepare-phone-numbers-from-komercijalno")]
	public async Task<IActionResult> PreparePhoneNumbersFromKomercijalnoAsync()
	{
		var partners = await komercijalnoApiManager.GetPartnersAsync(
			new PartneriGetMultipleRequest() { PageSize = int.MaxValue, ImaMobilni = true }
		);
		await client.MassQueueAsync(
			new MassQueueSmsRequest
			{
				Message = "Termodom",
				PhoneNumbers = partners.Payload!.Select(x => x.Mobilni).ToList()!
			}
		);
		return Ok();
	}

	[HttpDelete]
	[Route("/mass-sms/clear-duplicates")]
	public async Task<IActionResult> ClearDuplicatesAsync()
	{
		await client.ClearDuplicatesAsync();
		return Ok();
	}

	[HttpPut]
	[Route("/mass-sms/text")]
	public async Task<IActionResult> SetTextAsync([FromBody] SetTextRequest request)
	{
		await client.SetTextAsync(request);
		return Ok();
	}

	[HttpGet]
	[Route("/mass-sms/{number}/is-blacklisted")]
	public async Task<IActionResult> IsBlacklistedAsync([FromRoute] string number) =>
		Ok(await client.IsBlacklistedAsync(number));

	[HttpPost("/mass-sms/{number}/blacklist")]
	public async Task<IActionResult> BlacklistAsync([FromRoute] string number)
	{
		await client.Blacklist(number);
		return Ok();
	}

	[HttpDelete("/mass-sms/clear-blacklisted")]
	public async Task<IActionResult> ClearBlacklistedAsync()
	{
		await client.ClearBlacklistedAsync();
		return Ok();
	}
}
