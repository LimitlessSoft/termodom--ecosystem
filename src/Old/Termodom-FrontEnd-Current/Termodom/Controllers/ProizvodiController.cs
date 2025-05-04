using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using LimitlessSoft.NET5.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Termodom.Attributes;
using Termodom.Models;

namespace Termodom.Controllers
{
	public class ProizvodiController : Controller
	{
		private static Buffer<List<Proizvod>> _proizvodiBuffer = new Buffer<List<Proizvod>>(
			bProizvodiList,
			TimeSpan.FromSeconds(30)
		);
		private static Buffer<List<Podgrupa>> _podgrupeBuffer = new Buffer<List<Podgrupa>>(
			bPodgrupeList,
			TimeSpan.FromSeconds(30)
		);
		private static Buffer<List<Grupa>> _grupeBuffer = new Buffer<List<Grupa>>(
			bGrupeList,
			TimeSpan.FromSeconds(30)
		);

		private static List<Proizvod> bProizvodiList()
		{
			return Proizvod
				.ListAktivnih()
				.OrderByDescending(x => x.DisplayIndex)
				.ThenBy(x => x.RobaID)
				.ToList();
		}

		private static List<Podgrupa> bPodgrupeList()
		{
			return Podgrupa.List();
		}

		private static List<Grupa> bGrupeList()
		{
			return Grupa.List();
		}

		public const int BROJ_PROIZVODA_PO_STRANI = 100;

		/// <summary>
		/// TODO
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="term"></param>
		/// <returns></returns>
		public static bool SearchMatch(string sender, string term)
		{
			sender = sender.ToLower();
			term = term.ToLower();

			string[] termElements = term.Split(' ');

			int nMatch = 0;

			foreach (string element in termElements)
				if (sender.IndexOf(element) >= 0)
					nMatch++;

			return nMatch == termElements.Length;
		}

		[Route("/")]
		[Route("/Proizvodi")]
		[DefinisaniKorisnik]
		public async Task<IActionResult> Index()
		{
			return await Task.Run<IActionResult>(() =>
			{
				ViewData["Title"] = "Svi Proizvodi";
				Response.Cookies.Delete("Grupa");
				Response.Cookies.Delete("Podgrupa");

				ViewData["proizvodi-pretraga-unos"] = Request.Cookies["proizvodi-pretraga-unos"];

				// ovo radim jer unutar view-a ne radi lepo Request.Cookies[ime], vraca zastarelu vrednost
				ViewData["nProizvoda"] = Proizvod.ListAktivnih().Count;
				ViewData["Grupa"] = null;
				ViewData["Podgrupa"] = null;
				return View();
			});
		}

		[Route("/Proizvodi/Omiljeni")]
		[DefinisaniKorisnik]
		public IActionResult Omiljeni()
		{
			return View();
		}

		[Route("/Proizvodi/{Grupa}")]
		[DefinisaniKorisnik]
		public async Task<IActionResult> ProizvodiGrupa(string grupa)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Grupa g = Grupa
					.List()
					.Where(x => x.Naziv.ToLower() == grupa.ToLower())
					.FirstOrDefault(); // TODO: GET
				if (grupa != null)
				{
					ViewData["Title"] = grupa;
					Response.Cookies.Append("Grupa", grupa);
					Response.Cookies.Delete("Podgrupa");

					// ovo radim jer unutar view-a ne radi lepo Request.Cookies[ime], vraca zastarelu vrednost
					List<int> podgrupeID = Podgrupa
						.List()
						.Where(x => x.GrupaID == g.ID)
						.Select(x => x.ID)
						.ToList(); // TODO: GET
					ViewData["nProizvoda"] = Proizvod
						.ListAktivnih()
						.Where(x => podgrupeID.Contains(x.PodgrupaID))
						.Count(); // TODO: GetProizvodi().Count(Expression)
					ViewData["Grupa"] = grupa;
					ViewData["Podgrupa"] = null;
					return View("Index");
				}

				return View("Error", "Grupa ne postoji!");
			});
		}

		[Route("/Proizvodi/{Grupa}/{Podgrupa}")]
		[DefinisaniKorisnik]
		public async Task<IActionResult> ProizvodiGrupaPodgrupa(string grupa, string podgrupa)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Grupa g = Grupa.Get(grupa); // TODO: Get
				if (grupa != null)
				{
					Podgrupa pg = Podgrupa
						.List()
						.Where(x => x.GrupaID == g.ID && x.Naziv == podgrupa)
						.FirstOrDefault(); // TODO: Get
					if (podgrupa != null)
					{
						ViewData["Title"] = grupa;
						Response.Cookies.Append("Grupa", grupa);
						Response.Cookies.Append("Podgrupa", podgrupa);

						// ovo radim jer unutar view-a ne radi lepo Request.Cookies[ime], vraca zastarelu vrednost
						ViewData["nProizvoda"] = Proizvod
							.ListAktivnih()
							.Where(x => x.PodgrupaID == pg.ID)
							.Count(); // TODO: .Count(Expression)
						ViewData["Grupa"] = grupa;
						ViewData["Podgrupa"] = podgrupa;
						return View("Index");
					}
				}

				return View("Error", "Podgrupa ne postoji!");
			});
		}

		[HttpPost]
		[Route("/p/Proizvodi/Get")]
		public async Task<IActionResult> pGet(int offset, int count, string searchKey)
		{
			return await Task.Run<IActionResult>(() =>
			{
				try
				{
					searchKey = string.IsNullOrWhiteSpace(searchKey) ? "" : searchKey.ToLower();
					List<Proizvod> proizvodi = _proizvodiBuffer.Get();
					proizvodi = proizvodi
						.Where(x =>
							!string.IsNullOrWhiteSpace(x.Naziv)
								&& x.Naziv.ToLower().Contains(searchKey)
							|| !string.IsNullOrWhiteSpace(x.KatBr)
								&& x.KatBr.ToLower().Contains(searchKey)
							|| !string.IsNullOrWhiteSpace(x.KratakOpis)
								&& x.KratakOpis.ToLower().Contains(searchKey)
							|| !string.IsNullOrWhiteSpace(x.PovezaniProizvodi)
								&& x.PovezaniProizvodi.ToLower().Contains(searchKey)
						)
						.Skip(offset)
						.Take(count)
						.ToList();

					if (proizvodi == null || proizvodi.Count == 0)
						return StatusCode(204);

					if (Request.Cookies["Grupa"] != null)
					{
						List<int> filterPodgrupe = new List<int>();

						try
						{
							if (Request.Cookies["Podgrupa"] != null)
							{
								filterPodgrupe.Add(
									_podgrupeBuffer
										.Get()
										.Where(x =>
											x.Naziv.ToLower()
											== Request.Cookies["Podgrupa"].ToString().ToLower()
										)
										.FirstOrDefault()
										.ID
								);
							}
							else
							{
								Grupa grupaProizvoda = _grupeBuffer
									.Get()
									.Where(x =>
										x.Naziv.ToLower()
										== Request.Cookies["Grupa"].ToString().ToLower()
									)
									.FirstOrDefault();
								filterPodgrupe.AddRange(
									_podgrupeBuffer
										.Get()
										.Where(x => x.GrupaID == grupaProizvoda.ID)
										.Select(x => x.ID)
								);
							}
						}
						catch (Exception ex)
						{
							Log.WriteAsync(
								new LogovanjeGreske()
								{
									Date = DateTime.UtcNow.ToString("dd-MM HH:mm:ss"),
									Messages = ex.Message,
								}
							);
						}
						proizvodi = proizvodi
							.Where(x => filterPodgrupe.Any(t => x.Podgrupe.Contains(t)))
							.ToList();
					}

					Korisnik korisnik = HttpContext.GetKorisnik();
					if (HttpContext.GetTipKupca() == Enums.TipKupca.Profi && korisnik == null)
						return PartialView();

					if (HttpContext.GetTipKupca() == Enums.TipKupca.NULL)
						return PartialView(
							"~/Views/Proizvod/Prikaz_Mali.cshtml",
							new Tuple<List<Proizvod>, Cenovnik>(
								proizvodi,
								Korisnik.Cenovnik.PlatinumCenovnik.Result
							)
						);

					// Brze je ucitavati iz buffera, ali ako buffer nije relativan i bude pokretao ponovo ucitavanje iz baze koje traje par minuta onda nije brze.
					Cenovnik cenovnik = korisnik == null ? null : korisnik.GetCenovnik();

					return PartialView(
						"~/Views/Proizvod/Prikaz_Mali.cshtml",
						new Tuple<List<Proizvod>, Cenovnik>(proizvodi, cenovnik)
					);
				}
				catch (Exception ex)
				{
					Log.WriteAsync(
						new LogovanjeGreske()
						{
							Date = DateTime.UtcNow.ToString("dd-MM HH:mm:ss"),
							Messages = ex.Message,
						}
					);
					return StatusCode(500);
				}
			});
		}

		[HttpGet]
		[DefinisaniKorisnik]
		[Route("/p/Proizvodi/GetPoPretrazi")]
		public async Task<IActionResult> GetPoPretrazi(string unos)
		{
			return await Task.Run<IActionResult>(() =>
			{
				if (string.IsNullOrWhiteSpace(unos))
					return StatusCode(400); // TODO: Response message

				unos = unos.ToLower();
				Response.Cookies.Append(
					"proizvodi-pretraga-unos",
					unos,
					new CookieOptions() { Expires = DateTime.Now.AddMinutes(30) }
				);
				List<Proizvod> proizvodi = Proizvod
					.ListAktivnih()
					.Where(x =>
						x.Aktivan == 1
						&& x.Parent == 0
						&& (
							SearchMatch(x.KatBr, unos)
							|| SearchMatch(x.KatBr, unos)
							|| SearchMatch(x.Naziv, unos)
							|| (
								!string.IsNullOrWhiteSpace(x.PovezaniProizvodi)
								&& SearchMatch(x.PovezaniProizvodi, unos)
							)
							|| SearchMatch(x.Rel, unos)
							|| SearchMatch(x.RobaID.ToString(), unos)
							|| (
								!string.IsNullOrWhiteSpace(x.KratakOpis)
								&& SearchMatch(x.KratakOpis, unos)
							)
							|| (
								!string.IsNullOrWhiteSpace(x.IstaknutiProizvodi)
								&& SearchMatch(x.IstaknutiProizvodi, unos)
							)
						)
					)
					.OrderByDescending(x => x.DisplayIndex)
					.ThenBy(x => x.Naziv)
					.ToList()
					.ToList();

				Korisnik korisnik = HttpContext.GetKorisnik();

				if (HttpContext.GetTipKupca() == Enums.TipKupca.Profi && korisnik == null)
					return PartialView();

				Cenovnik cenovnik = korisnik == null ? null : korisnik.GetCenovnik();

				return PartialView(
					"/Views/Proizvod/Prikaz_Mali.cshtml",
					new Tuple<List<Proizvod>, Cenovnik>(proizvodi, cenovnik)
				);
			});
		}

		[HttpGet]
		[DefinisaniKorisnik]
		[Route("/Proizvodi/Get/Povezani")] // TODO: /Proizvodi/GetPovezani  ---- ako gledamo prethodnu rutu --- fali /p/
		public async Task<IActionResult> GetPovezani(int trenutniProizvod, int count)
		{
			return await Task.Run<IActionResult>(() =>
			{
				List<Proizvod> proizvodi = Proizvod.ListAktivnih();
				List<Proizvod> toReturn = new List<Proizvod>();

				Proizvod p = null;
				while (toReturn.Count < count)
				{
					p = proizvodi[Random.Next(0, proizvodi.Count)];
					if (toReturn.Count(x => x.RobaID == p.RobaID) > 0)
						continue;

					toReturn.Add(p);
				}

				Korisnik korisnik = HttpContext.GetKorisnik();

				if (HttpContext.GetTipKupca() == Enums.TipKupca.Profi && korisnik == null)
					return PartialView();

				// Brze je ucitavati iz buffera, ali ako buffer nije relativan i bude pokretao ponovo ucitavanje iz baze koje traje par minuta onda nije brze.
				Cenovnik cenovnik = korisnik == null ? null : korisnik.GetCenovnik();

				return PartialView(
					"~/Views/Proizvod/Prikaz_Mali.cshtml",
					new Tuple<List<Proizvod>, Cenovnik>(toReturn, cenovnik)
				);
			});
		}

		[HttpGet]
		[Route("/p/Proizvodi/Get/Omiljeni")] // TODO: /pProizvodi/GetOmiljeni --- Ako gledamo prethodnu rutu
		public async Task<IActionResult> GetOmiljeni()
		{
			return await Task.Run<IActionResult>(() =>
			{
				Korisnik korisnik = HttpContext.GetKorisnik();
				//Postaviti kao atribut ovo
				if (
					korisnik == null
					|| HttpContext.GetTipKupca() != Enums.TipKupca.Profi && korisnik == null
				)
					return StatusCode(403);

				List<Porudzbina> porudzbineKorisnika = new List<Porudzbina>();
				List<Porudzbina.Stavka> sveStavke = new List<Porudzbina.Stavka>();
				porudzbineKorisnika = Porudzbina.ListByUserID(korisnik.ID);

				sveStavke = Porudzbina.Stavka.List();

				if (sveStavke == null || sveStavke.Count == 0)
					return StatusCode(204);

				List<Proizvod> proizvodi = Proizvod.ListAktivnih();
				Dictionary<int, int> omiljeniProizvodi = new Dictionary<int, int>();

				foreach (
					Porudzbina.Stavka s in sveStavke.Where(x =>
						porudzbineKorisnika.Count(y => y.ID == x.PorudzbinaID) > 0
					)
				)
					if (omiljeniProizvodi.ContainsKey(s.RobaID))
						omiljeniProizvodi[s.RobaID] += 1;
					else
						omiljeniProizvodi[s.RobaID] = 1;

				List<Proizvod> toGo = new List<Proizvod>();
				foreach (
					KeyValuePair<int, int> i in omiljeniProizvodi
						.OrderByDescending(x => x.Value)
						.Take(15)
				)
				{
					Proizvod p = proizvodi.Where(x => x.RobaID == i.Key).FirstOrDefault(); // .FirstOrDefault(Expression)

					if (p != null)
						toGo.Add(proizvodi.Where(x => x.RobaID == i.Key).FirstOrDefault()); // .FirstOrDefault(Expression)
				}

				Cenovnik cenovnik = korisnik == null ? null : korisnik.GetCenovnik();

				return PartialView(
					"~/Views/Proizvod/Prikaz_Mali.cshtml",
					new Tuple<List<Proizvod>, Cenovnik>(toGo, cenovnik)
				);
			});
		}

		[Route("/api/Proizvodi/RazlikaUCeni")]
		[Authorization("administrator", "staff")]
		public async Task<IActionResult> RazlikaUCeni()
		{
			return await Task.Run<IActionResult>(() =>
			{
				List<Proizvod> list = Proizvod
					.List()
					.Where(t => t.NabavnaCena > t.ProdajnaCena)
					.ToList(); // TODO: List(whereQuery)
				if (list.Count > 0)
					return StatusCode(200, list);

				return StatusCode(204);
			});
		}

		[HttpGet]
		[Route("/api/proizvodi/list")]
		public async Task<IActionResult> apiList()
		{
			return await Task.Run<IActionResult>(() =>
			{
				try
				{
					return StatusCode(200, Proizvod.ListAktivnih());
				}
				catch
				{
					return StatusCode(500);
				}
			});
		}

		public static string GetLastFilterPage(HttpRequest Request)
		{
			return Request.Cookies["lpf"];
		}

		public static void SetLastFilterPage(HttpResponse Response, string link)
		{
			Response.Cookies.Append("lpf", link);
		}
	}
}
