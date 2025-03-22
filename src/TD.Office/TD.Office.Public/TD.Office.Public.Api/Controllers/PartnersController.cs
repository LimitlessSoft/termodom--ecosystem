using LSCore.Auth.Contracts;
using LSCore.Common.Contracts;
using Microsoft.AspNetCore.Mvc;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Office.Common.Contracts.Attributes;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Partneri;

namespace TD.Office.Public.Api.Controllers;

[LSCoreAuth]
[ApiController]
[Permissions(Permission.Access, Permission.PartneriRead)]
public class PartnersController(
	ITDKomercijalnoApiManager komercijalnoApiManager,
	IPartnerManager partnerManager
) : ControllerBase
{
	[HttpGet]
	[Route("/partners")]
	public async Task<IActionResult> GetPartners([FromQuery] PartneriGetMultipleRequest request) =>
		Ok(await komercijalnoApiManager.GetPartnersAsync(request));

	[HttpGet]
	[Route("/partners/{Id}")]
	public async Task<IActionResult> GetPartner([FromRoute] LSCoreIdRequest request) =>
		Ok(await komercijalnoApiManager.GetPartnerAsync(request));

	[HttpPost]
	[Route("/partners")]
	public async Task<IActionResult> CreatePartner([FromBody] PartneriCreateRequest request) =>
		Ok(await komercijalnoApiManager.CreatePartnerAsync(request));

	[HttpGet]
	[Route("/partners-mesta")]
	public async Task<IActionResult> GetPartnersMesta() =>
		Ok(await komercijalnoApiManager.GetPartnersMestaAsync());

	[HttpGet]
	[Route("/partners-kategorije")]
	public async Task<IActionResult> GetPartnersKategorije() =>
		Ok(await komercijalnoApiManager.GetPartnersKategorijeAsync());

	[HttpGet]
	[Route("/partners-recently-created")]
	[Permissions(Permission.PartneriSkoroKreirani)]
	public async Task<IActionResult> GetRecentlyCreatedPartners() =>
		Ok(await partnerManager.GetRecentlyCreatedPartnersAsync());

	[HttpGet]
	[Route("/partneri-po-godinama-komercijalno-finansijsko")]
	public IActionResult GetPartnersReportByYearsKomercijalnoFinansijsko() =>
		Ok(partnerManager.GetPartnersReportByYearsKomercijalnoFinansijsko());

	[HttpGet]
	[Route("/partneri-po-godinama-komercijalno-finansijsko-data")]
	public async Task<IActionResult> GetPartnersReportByYearsKomercijalnoFinansijskoDataAsync(
		[FromQuery] GetPartnersReportByYearsKomercijalnoFinansijskoRequest request
	) => Ok(await partnerManager.GetPartnersReportByYearsKomercijalnoFinansijskoDataAsync(request));

	[HttpPut]
	[Route("/partneri-po-godinama-komercijalno-finansijsko-data/{Id}/status")]
	public async Task<IActionResult> SaveKomercijalnoFinansijskoStatus(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] SaveKomercijalnoFinansijskoStatusRequest request
	)
	{
		request.PPID = Convert.ToInt16(idRequest.Id);
		return Ok(partnerManager.SaveKomercijalnoFinansijskoStatus(request));
	}

	[HttpPut]
	[Route("/partneri-po-godinama-komercijalno-finansijsko-data/{Id}/komentar")]
	public async Task<IActionResult> SaveKomercijalnoFinansijskoKomentar(
		[FromRoute] LSCoreIdRequest idRequest,
		[FromBody] SaveKomercijalnoFinansijskoCommentRequest request
	)
	{
		request.PPID = Convert.ToInt16(idRequest.Id);
		return Ok(partnerManager.SaveKomercijalnoFinansijskoKomentar(request));
	}

	[HttpGet]
	[Route("/partneri-analiza/{Id}")]
	[Permissions(Permission.PartnerAnalizaRead)]
	public async Task<IActionResult> GetPartnerAnalysis([FromRoute] LSCoreIdRequest request) =>
		Ok(await partnerManager.GetPartnerAnalysisAsync(request));

	[HttpGet]
	[Route("/company-types")]
	public IActionResult GetCompanyTypes() => Ok(partnerManager.GetCompanyTypes());
}
