using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using LimitlessSoft.NET5.Authorization;
using Microsoft.AspNetCore.Mvc;
using Termodom.Models;

namespace Termodom.Controllers.KontrolnaTabla
{
	// TODO: Izbaciti iz foldera
	[Authorization("administrator", "staff")]
	public class KontrolnaTablaController : Controller
	{
		#region Views
		[Route("/KontrolnaTabla")]
		public IActionResult Index()
		{
			return View();
		}

		[Route("/KontrolnaTabla/Korisnici")]
		public IActionResult Korisnici()
		{
			return View();
		}

		/// <summary>
		/// Admin view za pregled proizvoda
		/// </summary>
		/// <returns></returns>
		[Route("/KontrolnaTabla/Proizvodi")]
		public async Task<IActionResult> Proizvodi()
		{
			return await Task.Run(() =>
			{
				return View("/Views/KontrolnaTabla/Proizvodi.cshtml");
			});
		}

		[Route("/KontrolnaTabla/MasovniSMS")]
		public IActionResult MasovniSMS()
		{
			return View();
		}

		[Route("/KontrolnaTabla/Porudzbine")]
		public IActionResult Porudzbine()
		{
			return View();
		}

		[Route("/KontrolnaTabla/Proizvod/Novi")]
		public IActionResult NoviProizvod()
		{
			Proizvod p = new Proizvod();
			return View("/Views/Proizvod/Uredi.cshtml", new Proizvod());
		}
		#endregion

		#region Paritals
		[HttpGet]
		[Route("/p/KontrolnaTabla/Porudzbina/GetSkorasnje")]
		public async Task<IActionResult> GetSkorasnje()
		{
			return await Task.Run(() =>
			{
				Task<List<Porudzbina>> porudzbineTask = Porudzbina.ListAsync();
				Task<List<Korisnik>> korisniciTask = Korisnik.ListAsync();
				Task<List<Porudzbina.Stavka>> stavkeTask = Porudzbina.Stavka.ListAsync();

				Task.WaitAll(porudzbineTask, korisniciTask);

				List<Porudzbina> porudzbine = porudzbineTask
					.Result.Where(x =>
						Math.Abs((x.Datum.ToUniversalTime() - DateTime.UtcNow).TotalDays) <= 3
					)
					.OrderByDescending(x => x.Datum)
					.ToList();
				List<Korisnik> korisnici = korisniciTask.Result;
				List<Porudzbina.Stavka> stavke = stavkeTask.Result;

				foreach (Porudzbina p in porudzbine)
					p.UcitajStavke(stavke);

				return View(
					"/Views/Porudzbina/_pPrikaz_Mali.cshtml",
					new Tuple<List<Porudzbina>, List<Korisnik>>(porudzbine, korisnici)
				);
			});
		}

		// Ovo je partial, ruta treba biti /p/...
		// Get asocira da cu dobiti porudzbinu, a ja dobijam partial sa porudzbinama
		// Predlog imena: /p/KontrolnaTabla/Porudzbina/Prikaz/Mali
		// Ovo treba da stoji u KontrolnaTablaController-u
		// Ipak bi bolje bilo da se zove /p/KontrolnaTabla/Porudzbina/Prikaz/{tipPrikaza} pa zavisno od tipa prikaza vraca partial _pPrikaz_Mali, _pPrikaz_Srednji ili _pPrikaz_Veliki
		// Takodje parametri trebaju biti tako da se preko ovog get-a mogu filtrirati porudzbine i po vremenu, korisniku i ostalim parametrima (naravno ako su dati)
		[HttpGet]
		[Route("/p/KontrolnaTabla/Porudzbina/Prikaz/Mali")]
		public async Task<IActionResult> Filter(int? tip, int strana)
		{
			return await Task.Run(() =>
			{
				List<Porudzbina> list = Porudzbina.List();

				if (tip != null)
					list = list.Where(t => t.Status == (PorudzbinaStatus)tip).ToList();

				return View(
					"/Views/KontrolnaTabla/_pPrikaz_Mali.cshtml",
					list.OrderByDescending(x => x.ID).Skip(strana * 15).Take(15).ToList()
				);
			});
		}

		[Route("/p/KontrolnaTabla/GetSaobracaj")]
		public async Task<IActionResult> GetSaobracaj()
		{
			return await Task.Run(() =>
			{
				return View("/Views/KontrolnaTabla/_pSaobracaj.cshtml");
			});
		}

		[Route("/p/KontrolnaTabla/GetSlavljenici")]
		public async Task<IActionResult> GetSlavljenici(int narednihXDana)
		{
			return await Task.Run(() =>
			{
				List<Korisnik> slavljenici = new List<Korisnik>();

				DateTime compareTo = new DateTime(4, DateTime.Now.Month, DateTime.Now.Day).AddDays(
					narednihXDana
				);

				foreach (Korisnik k in Korisnik.List())
				{
					DateTime rodjenje = new DateTime(4, k.DatumRodjenja.Month, k.DatumRodjenja.Day);

					int dayss = compareTo.Date.DayOfYear - rodjenje.Date.DayOfYear;

					if (dayss >= 0 && dayss <= narednihXDana)
						slavljenici.Add(k);
				}
				return View(
					"/Views/KontrolnaTabla/_pSlavljenici.cshtml",
					new Tuple<int, List<Korisnik>>(narednihXDana, slavljenici)
				);
			});
		}
		#endregion

		#region API

		[HttpGet]
		[Route("/api/KontrolnaTabla/PosaljiRodjendanskeSMSove")]
		public async Task<IActionResult> PosaljiRodjendanskeSMSove(int narednihXDana)
		{
			return await Task.Run(() =>
			{
				DateTime compareTo = new DateTime(4, DateTime.Now.Month, DateTime.Now.Day).AddDays(
					narednihXDana
				);

				List<Korisnik> slavljenici = new List<Korisnik>();
				Parallel.ForEach(
					Korisnik.List(),
					korinsik =>
					{
						DateTime rodjenje = new DateTime(
							4,
							korinsik.DatumRodjenja.Month,
							korinsik.DatumRodjenja.Day
						);

						int dayss = compareTo.Date.DayOfYear - rodjenje.Date.DayOfYear;

						if (dayss >= 0 && dayss <= narednihXDana)
							slavljenici.Add(korinsik);
					}
				);

				foreach (var slavljenik in slavljenici)
				{
					try
					{
						if (!slavljenik.PoslatRodjendanskiSMS)
						{
							SMS.SendSMS(
								slavljenik.Mobilni,
								"Po TERMODOM podacima uskoro je tvoj rodjendan. SRECNO!!!"
									+ "U to ime poklanjamo ti rodjendanski bonus 50% na alat. www.termodom.rs",
								this.HttpContext.GetKorisnik().ID
							);
							slavljenik.PoslatRodjendanskiSMS = true;
							slavljenik.Update();
						}
					}
					catch { }
				}

				return StatusCode(200);
				Parallel.ForEach(
					slavljenici,
					slavljenik =>
					{
						try
						{
							if (!slavljenik.PoslatRodjendanskiSMS)
							{
								SMS.SendSMS(
									slavljenik.Mobilni,
									"Po TERMODOM podacima uskoro je tvoj rodjendan. SRECNO!!! U to ime poklanjamo ti rodjendanski bonus 50% na alat. www.termodom.rs",
									this.HttpContext.GetKorisnik().ID
								);
								slavljenik.PoslatRodjendanskiSMS = true;
								slavljenik.Update();
							}
						}
						catch { }
					}
				);
				return StatusCode(200);
			});
		}

		[HttpGet]
		[Route("/api/KontrolnaTabla/PosaljiMasovniSMS")]
		public async Task<IActionResult> PosaljiMasovniSMS(string text, int kome)
		{
			return await Task.Run(() =>
			{
				List<string> brojevi = new List<string>();
				List<Porudzbina> porudzbine = new List<Porudzbina>();
				switch (kome)
				{
					case 1:
						brojevi.AddRange(Korisnik.List().Select(x => x.Mobilni));
						break;
					case 2:
						brojevi.AddRange(
							Korisnik.List().Where(x => x.PrimaObavestenja).Select(x => x.Mobilni)
						);
						break;
					case 8:
						porudzbine = Porudzbina.List();
						brojevi.AddRange(
							Korisnik
								.List()
								.Where(x => porudzbine.Count(y => y.UserID == x.ID) == 0)
								.Select(x => x.Mobilni)
						);
						break;
					case 7:
						porudzbine = Porudzbina.List();
						brojevi.AddRange(
							Korisnik
								.List()
								.Where(x =>
									porudzbine
										.Where(y => y.Datum < DateTime.Now.AddDays(-30))
										.Count(y => y.UserID == x.ID) == 0
								)
								.Select(x => x.Mobilni)
						);
						break;
					case 4:
						porudzbine = Porudzbina.List();
						brojevi.AddRange(
							Korisnik
								.List()
								.Where(x =>
									porudzbine
										.Where(y => y.Status == PorudzbinaStatus.Preuzeto)
										.Count(y => y.UserID == x.ID) == 0
								)
								.Select(x => x.Mobilni)
						);
						break;
					case 3:
						brojevi.AddRange(
							Korisnik
								.List()
								.Where(x => x.PoslednjiPutVidjen < DateTime.Now.AddDays(-30))
								.Select(x => x.Mobilni)
						);
						break;
					case 5:
						porudzbine = Porudzbina.List();
						brojevi.AddRange(
							Korisnik
								.List()
								.Where(x =>
									(DateTime)x.PoslednjiPutVidjen < DateTime.Now.AddDays(-30)
									|| porudzbine.Count(y => y.Datum < DateTime.Now.AddDays(-30))
										== 0
								)
								.Select(x => x.Mobilni)
						);
						break;
					case 6:
						brojevi.AddRange(
							Korisnik
								.List()
								.Where(x =>
									x.PoslednjiPutVidjen == null
									|| x.DatumOdobrenja != null
										&& ((DateTime)x.PoslednjiPutVidjen).AddDays(-1)
											< x.DatumOdobrenja
								)
								.Select(x => x.Mobilni)
						);
						break;
					default:
						return StatusCode(400);
				}

				SMSController.PosaljiSMS(text, brojevi.ToArray(), -1);
				return Ok();
			});
		#endregion
		}
	}
}
