using Microsoft.AspNetCore.Mvc;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.IManagers;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.TDOffice.Api.Controllers;

[ApiController]
public class DokumentTagIzvodController(IDokumentTagIzvodManager dokumentTagIzvodManager)
	: Controller
{
	[HttpGet]
	[Route("/dokument-tag-izvodi")]
	public List<DokumentTagIzvodGetDto> GetMultiple(
		[FromQuery] DokumentTagIzvodGetMultipleRequest request
	) => dokumentTagIzvodManager.GetMultiple(request);

	/// <summary>
	/// Insert ili update nad entitetom DokumentTagIzvod.
	/// Ukoliko se radi Update, BrojDokumentaIzvoda ce biti ignorisan iz requesta (nece biti azuiraran),
	/// u suprotnom novi entitet ce imati vrednost iz request.BrojDokumentaIzvoda
	/// </summary>
	/// <param name="request"></param>
	/// <returns></returns>
	[HttpPut]
	[Route("/dokument-tag-izvodi")]
	public DokumentTagIzvodGetDto Put([FromBody] DokumentTagizvodPutRequest request) =>
		dokumentTagIzvodManager.Save(request);
}
