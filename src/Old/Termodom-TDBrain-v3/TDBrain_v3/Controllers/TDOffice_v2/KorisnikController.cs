using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.TDOffice_v2;
using TDBrain_v3.RequestBodies.TDOffice;

namespace TDBrain_v3.Controllers.TDOffice_v2
{
	/// <summary>
	///
	/// </summary>
	[ApiController]
	public class KorisnikController : Controller
	{
		/// <summary>
		/// Vraca dictionary sa beleskama za prosledjenog korisnika
		/// </summary>
		/// <param name="korisnikId"></param>
		/// <returns></returns>
		[HttpGet]
		[Tags("/TDOffice/Korisnik/Beleska")]
		[Route("/TDOffice/Korisnik/Beleska/Dictionary")]
		public IActionResult BeleskaDictionary([FromQuery] [Required] int korisnikId)
		{
			return Json(KorisnikManager.BeleskeDictionary(korisnikId));
		}

		/// <summary>
		/// Cuva / kreira belesku korisnika.
		/// Kreira belesku ukoliko se ne prosledi Id u suprotnom azurira belesku sa ID-em
		/// </summary>
		/// <param name="body"></param>
		/// <returns>Ukoliko kreira belesku vraca ID nove beleske</returns>
		[HttpPut]
		[Tags("/TDOffice/Korisnik/Beleska")]
		[Route("/TDOffice/Korisnik/Beleska/Save")]
		public IActionResult BeleskaSave([FromBody] KorisnikBeleskaSaveRequestBody body)
		{
			if (body.Id == null)
				return StatusCode(201, KorisnikManager.BeleskaInsert(body));

			return Json(KorisnikManager.BeleskaUpdate(body));
		}

		/// <summary>
		/// Insertuje novog korisnika u TDOffice
		/// </summary>
		/// <returns></returns>
		[HttpPut]
		[Tags("/TDOffice/Korisnik")]
		[Route("/TDOffice/Korisnik/Insert")]
		public IActionResult Insert([FromForm] KorisnikInsertRequestBody body)
		{
			return Json(KorisnikManager.Create(body));
		}
	}
}
