using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using LimitlessSoft.NET5.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Termodom.API;
using Termodom.Attributes;
using Termodom.Models;

namespace Termodom.Controllers
{
	[DefinisaniKorisnik]
	public class PorudzbinaController : Controller
	{
		public static Config<int> ValidnostPorudzbine = Config<int>.Get("CENE_IZ_PORUDZBINE_VAZE");
		private static Dictionary<int, string> Radnje = new Dictionary<int, string>() // TODO: Ovo vec postoji negde samo opsirnije
		{
			{ 12, "0698801506" },
			{ 13, "0618801513" },
			{ 28, "0618801528" },
			{ 26, "0618801526" },
			{ 17, "0618801517" },
			{ 15, "0618801515" },
			{ 16, "0618801516" },
			{ 18, "0618801518" },
			{ 19, "0618801519" },
			{ 21, "0618801521" },
			{ 20, "0618801520" },
			{ 22, "0618801522" },
			{ 23, "0618801523" },
			{ 25, "0618801525" },
			{ 27, "0618801527" }
		};

		[HttpPost]
		[Authorization("administrator")]
		[Route("/Porudzbina/Validnost/Set")]
		public IActionResult SetValidnostPorudzbine(int validnost)
		{
			if (validnost <= 0)
				return StatusCode(500, "Vrednost ne sme biti manja od nule");

			ValidnostPorudzbine.Value = validnost;
			ValidnostPorudzbine.Update();
			return StatusCode(200, "Uspesno ste promenili validnost porudzbine");
		}

		[Route("/Porudzbina/{hash}")]
		public async Task<IActionResult> Index(string hash)
		{
			return await Task.Run<IActionResult>(() =>
			{
				try
				{
					int porudzbinaID = Convert.ToInt32(hash);

					Korisnik korisnik = HttpContext.GetKorisnik();

					Porudzbina p = Porudzbina.Get(porudzbinaID);

					if (p == null)
						return p.NotFound();

					if (korisnik == null || korisnik.Tip != 1 && korisnik.ID != p.UserID)
						return Json("Nema pravo");

					return View("Index", p);
				}
				catch (Exception)
				{
					Porudzbina p = Porudzbina.GetHash(hash);

					if (p == null)
						return p.NotFound();

					return View("Index", p);
				}
			});
		}

		//[Route("/Porudzbina/{porudzbina}/Profaktura")]
		//public async Task<IActionResult> Profaktura(int porudzbina)
		//{
		//    return await Task.Run<IActionResult>(() =>
		//    {
		//        Porudzbina p = Porudzbina.Get(porudzbina);

		//        if (p == null)
		//            return p.NotFound();

		//        Korisnik FakturaUser = Korisnik.List()
		//                            .Where(t => t.ID == p.UserID)
		//                            .FirstOrDefault();

		//        Task<List<Proizvod>> proizvodi = Proizvod.ListAsync();

		//        PdfDocument document = new();
		//        document.Info.Title = "Termodom profaktura";

		//        PdfPage page = document.AddPage();
		//        XGraphics gfx = XGraphics.FromPdfPage(page);

		//        XFont h1 = new("Verdana", 20, XFontStyle.Bold);
		//        XFont h2 = new("Verdana", 16, XFontStyle.Regular);
		//        XFont h3 = new("Verdana", 10, XFontStyle.Regular);
		//        XFont h4 = new("Verdana", 6, XFontStyle.Regular);

		//        double ukupanPDV = 0;
		//        double ukupnoSAPDV = 0;
		//        double linePadding = 5;
		//        double currLine = 0;
		//        double leftMargin = 10;
		//        currLine += h1.Size + linePadding;

		//        if (FakturaUser != null && FakturaUser.PPID != null &&  FakturaUser.PPID != 0 && !string.IsNullOrEmpty(Models.PoslovniPartner.GetName((int)FakturaUser.PPID)))
		//        {
		//            Models.PoslovniPartner Partner = Models.PoslovniPartner.GetPartner((int)FakturaUser.PPID);
		//            gfx.DrawString("Kupac: " + Partner.Naziv, h2, XBrushes.Black, document.Pages[0].Width - 300, currLine);
		//            gfx.DrawString("          " + Partner.Adresa, h2, XBrushes.Black, document.Pages[0].Width - 300, currLine + 20);
		//            gfx.DrawString("PIB: " + Partner.PIB, h2, XBrushes.Black, document.Pages[0].Width - 300, currLine + 40);
		//        }
		//        if (p.MagacinID == 27 || p.MagacinID == 25 || p.MagacinID == 21)
		//        {
		//            gfx.DrawString("TERMOSHOP D.O.O", h1, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("Batajnicki drum 6a", h4, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("PIB: 109321256", h3, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("Ziro racun: 220-137292-78", h3, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("MB:  21161349", h3, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("Web profaktura: " + p.ID, h3, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("Datum: " + DateTime.UtcNow.ToString("dd.MM.yyyy."), h3, XBrushes.Black, leftMargin, currLine);

		//        }
		//        else
		//        {
		//            gfx.DrawString("TERMODOM D.O.O.", h1, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("Zrenjaninski Put 84g", h4, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("PIB: 100005295", h3, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("Ziro racun: 160-8923-79", h3, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("MB: 07814143", h3, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("Web profaktura: " + p.ID, h3, XBrushes.Black, leftMargin, currLine);

		//            currLine += h3.Size + linePadding;
		//            gfx.DrawString("Datum: " + DateTime.UtcNow.ToString("dd.MM.yyyy."), h3, XBrushes.Black, leftMargin, currLine);
		//        }
		//        double OsnovicaZaPdv = 0;
		//        p.Stavke.ForEach(t => OsnovicaZaPdv += ((t.Kolicina * t.VPCena) / (1 + (proizvodi.Result.Where(v => v.ID == t.RobaID).FirstOrDefault().PDV / 100))));
		//        currLine += 30;
		//        double currentX = 20;
		//        for (int i = -1; i < p.Stavke.Count + 1; i++)
		//        {
		//            if (i == -1)
		//            {
		//                currentX += 20;
		//                gfx.DrawString("Naziv artikla", h3, XBrushes.Black, currentX, currLine);
		//                currentX += 140;
		//                gfx.DrawString("Kol. JM", h3, XBrushes.Black, currentX, currLine);
		//                currentX += 70;
		//                gfx.DrawString("Cena", h3, XBrushes.Black, currentX, currLine);
		//                gfx.DrawString("(sa PDV)", h3, XBrushes.Black, currentX, currLine + 10);
		//                currentX += 70;
		//                gfx.DrawString("Osnovica za", h3, XBrushes.Black, currentX, currLine);
		//                gfx.DrawString("PDV", h3, XBrushes.Black, currentX, currLine + 10);
		//                currentX += 70;
		//                gfx.DrawString("Stopa", h3, XBrushes.Black, currentX, currLine);
		//                gfx.DrawString("PDV", h3, XBrushes.Black, currentX, currLine + 10);
		//                currentX += 70;
		//                gfx.DrawString("PDV", h3, XBrushes.Black, currentX, currLine);
		//                currentX += 40;
		//                gfx.DrawString("Ukupna naknada", h3, XBrushes.Black, currentX, currLine);
		//                XPen line = new(XColors.Black, 2);
		//                currLine += 15;
		//                gfx.DrawLine(line, 20, currLine, document.Pages[0].Width - 50, currLine);
		//                currentX = 20;

		//            }
		//            else if (i < p.Stavke.Count)
		//            {
		//                var product = proizvodi.Result.Where(t => t.ID == p.Stavke[i].RobaID).FirstOrDefault();
		//                double pdv = (p.Stavke[i].VPCena * p.Stavke[i].Kolicina) * (1 + (product.PDV / 100));
		//                double osnovicaZaPdvArtikal = (p.Stavke[i].VPCena / (1 + (product.PDV / 100)));
		//                ukupanPDV += pdv - (p.Stavke[i].VPCena * p.Stavke[i].Kolicina);
		//                ukupnoSAPDV += pdv;
		//                currLine += 20;
		//                //Naziv
		//                gfx.DrawString(product.Naziv, h4, XBrushes.Black, currentX, currLine);
		//                currentX += 160 + h4.Size;
		//                //Kolicina
		//                gfx.DrawString(p.Stavke[i].Kolicina + " " + product.JM, h4, XBrushes.Black, currentX, currLine);
		//                currentX += 65 + h4.Size;
		//                //Cena sa pdv
		//                gfx.DrawString((p.Stavke[i].VPCena * (1 + (product.PDV / 100))).ToString("#,##0.00"), h4, XBrushes.Black, currentX, currLine);
		//                currentX += 70 + h4.Size;
		//                //Osnovica za pdv
		//                gfx.DrawString(p.Stavke[i].VPCena.ToString("#,##0.00"), h4, XBrushes.Black, currentX, currLine);
		//                currentX += 65 + h4.Size;
		//                //PDv Stopa
		//                gfx.DrawString(product.PDV + "%", h4, XBrushes.Black, currentX, currLine);
		//                currentX += 55 + h4.Size;
		//                //Ukupan pdv PDV
		//                gfx.DrawString((pdv - (p.Stavke[i].VPCena * p.Stavke[i].Kolicina)).ToString("#,##0.00"), h4, XBrushes.Black, currentX, currLine);
		//                currentX += 40 + h4.Size;
		//                //Ukupna
		//                gfx.DrawString(pdv.ToString("#,##0.00"), h4, XBrushes.Black, currentX, currLine);
		//                currLine += 10;
		//                currentX = 20;
		//            }
		//            else
		//            {
		//                XPen line = new(XColors.Black, 2);
		//                gfx.DrawLine(line, 20, currLine, document.Pages[0].Width - 50, currLine);
		//                currLine += 20;
		//                currentX = document.Pages[0].Width - 250;
		//                gfx.DrawString("Vrednost: " + ukupnoSAPDV.ToString("#,##0.00"), h3, XBrushes.Black, currentX, currLine);
		//                currLine += 40;
		//                gfx.DrawString("Osnovica za PDV: " + p.VPVrednost().ToString("#,##0.00"), h3, XBrushes.Black, currentX, currLine);
		//                currLine += 20;
		//                gfx.DrawString("PDV: " + ukupanPDV.ToString("#,##0.00"), h3, XBrushes.Black, currentX, currLine);
		//                currLine += 20;
		//                gfx.DrawString("Iznos sa PDV: " + ukupnoSAPDV.ToString("#,##0.00"), h3, XBrushes.Black, currentX, currLine);

		//            }


		//        }

		//        MemoryStream ms = new();
		//        document.Save(ms);
		//        return File(ms, "application/pdf");

		//    });
		//}

		[Route("/MojePorudzbine")]
		public async Task<IActionResult> MojePorudzbine() // TODO: Uh... Koja je razlika izmedju MojePorudzbine i Korisnikove Porudzbine
		{
			return await Task.Run(() =>
			{
				Korisnik k = HttpContext.GetKorisnik();
				return View(k.ID);
			});
		}

		[Route("/KorisnikovePorudzbine/{id}")]
		public async Task<IActionResult> KorisnikovePorudzbine(int id) // TODO: Uh... Koja je razlika izmedju MojePorudzbine i Korisnikove Porudzbine
		{
			return await Task.Run(() =>
			{
				return View("/Views/Porudzbina/MojePorudzbine.cshtml", id);
			});
		}

		[Route("/Porudzbina/NovaStavkaPopup")]
		public async Task<IActionResult> NovaStavkaPopup()
		{
			return await Task.Run(() =>
			{
				return View("/Views/Porudzbina/NovaStavkaPopup.cshtml");
			});
		}

		[Route("/PratiPorudzbinu")]
		public IActionResult PratiPorudzbinu()
		{
			return View();
		}

		[Route("/PronadjiPrekoTelefona/{mobilni}")]
		public async Task<IActionResult> PronadjiPorudzbinu(string mobilni) // TODO: Izmena na kraju
		{
			return await Task.Run(() =>
			{
				return View("/Views/Porudzbina/PorudzbinePerekoTelefona.cshtml", mobilni);
			});
		}

		#region Partials
		[HttpPost]
		[Route("/p/Porudzbina/Stavke/Administrator/Item/Get")] // TODO: Zbunjujuca ruta. Moramo videti ovo ali nista hitno
		[Consumes(MediaTypeNames.Application.Json)]
		public async Task<IActionResult> pStavkeAdministratorItemGet(
			[FromBody] pStavkeAdministratorItemGetDTO itemGetDTO
		)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina.Stavka stavka = Porudzbina.Stavka.Get(itemGetDTO.StavkaID);
				DTO.Views.pStavkeAdministratorItemDTO pStavkeAdministratoraItemDTO =
					new DTO.Views.pStavkeAdministratorItemDTO(
						itemGetDTO.Porudzbina,
						itemGetDTO.Proizvod,
						stavka,
						itemGetDTO.IronCenovnik,
						itemGetDTO.SilverCenovnik,
						itemGetDTO.GoldCenovnik,
						itemGetDTO.PlatinumCenovnik
					);
				return View(
					"/Views/Porudzbina/_pStavke_Administrator_Item.cshtml",
					pStavkeAdministratoraItemDTO
				);
			});
		}

		[HttpGet]
		[Route("/p/Porudzbina/Get/Mali")]
		public async Task<IActionResult> pGetMali(int? korisnikID)
		{
			return await Task.Run(() =>
			{
				Task<List<Porudzbina>> porudzbineTask = Porudzbina.ListAsync();
				Task<List<Korisnik>> korisniciTask = Korisnik.ListAsync();
				Task<List<Porudzbina.Stavka>> stavkeTask = Porudzbina.Stavka.ListAsync();

				Task.WaitAll(porudzbineTask, korisniciTask);

				List<Porudzbina> porudzbine = porudzbineTask.Result;
				List<Korisnik> korisnici = korisniciTask.Result;
				List<Porudzbina.Stavka> stavke = stavkeTask.Result;

				if (korisnikID != null)
					porudzbine.RemoveAll(x => x.UserID != korisnikID);

				foreach (Porudzbina p in porudzbine)
					p.UcitajStavke(stavke);

				porudzbine.Sort((x, y) => y.Datum.CompareTo(x.Datum));

				return View(
					"/Views/Porudzbina/_pPrikaz_Mali.cshtml",
					new Tuple<List<Porudzbina>, List<Korisnik>>(porudzbine, korisnici)
				);
			});
		}

		[HttpGet]
		[Route("/p/Porudzbina/Get/PronadjiPrekoMobilnog")]
		public async Task<IActionResult> pPrekoTelefonaGetMaliP(string mobilni)
		{
			return await Task.Run(() =>
			{
				Task<List<Porudzbina>> porudzbineTask = Porudzbina.ListAsync();
				Task<List<Porudzbina.Stavka>> stavkeTask = Porudzbina.Stavka.ListAsync();

				List<Porudzbina> porudzbine = porudzbineTask
					.Result.Where(t => t.KontaktMobilni == mobilni)
					.ToList();
				List<Porudzbina.Stavka> stavke = stavkeTask.Result;

				foreach (Porudzbina p in porudzbine)
					p.UcitajStavke(stavke);

				porudzbine.Sort((x, y) => y.Datum.CompareTo(x.Datum));
				return View(
					"/Views/Porudzbina/_pPrikaz_Mali.cshtml",
					new Tuple<List<Porudzbina>, List<Korisnik>>(porudzbine, null)
				);
			});
		}

		#endregion
		#region API
		[HttpPost]
		[Authorization("Administrator")]
		[Route("/api/Porudzbina/Status/Set")]
		public async Task<IActionResult> SetStatus(int porudzbina, int status)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina p = Porudzbina.Get(porudzbina);

				if (p == null)
					return p.NotFound();

				p.Status = (PorudzbinaStatus)status;
				try
				{
					p.Update();
				}
				catch (APIRequestTimeoutException)
				{
					return StatusCode(500);
				}
				catch (APIRequestInternalServerErrorException)
				{
					return StatusCode(500);
				}
				catch (Exception)
				{
					return StatusCode(500);
				}

				return StatusCode(200);
			});
		}

		[HttpPost]
		[Authorization("Administrator")]
		[Route("/api/Porudzbina/BrDokKom/Set")]
		public async Task<IActionResult> SetBrDokKom(int porudzbina, int tip = 0)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina p = Porudzbina.Get(porudzbina);

				if (p == null)
					return p.NotFound();

				if (p.BrDokKom != 0)
					return StatusCode(400, "Porudzbina je vec pretrvorena u proracun");

				string akc = tip == 0 ? "PretvoriUProracun" : "PretvoriUPonudu";

				if (Models.AKC.Get($"{akc}|" + p.ID) != null)
					return StatusCode(400, "Akcija je vec pokrenuta, sacekajte");

				if (p.MagacinID == -5)
					return StatusCode(400, "Nije izabran validan magacin");

				Models.AKC.Insert($"{akc}|" + p.ID, HttpContext.GetKorisnik().ID);
				return StatusCode(200, "Uspesno ste zadali zadatak za pretvaranje porudzbine");
			});
		}

		[HttpPost]
		[Authorization("Administrator")]
		[Route("/api/Porudzbina/BrDokKom/Remove")]
		public async Task<IActionResult> DeleteBrDokKom(int porudzbina)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina p = Porudzbina.Get(porudzbina);
				if (p == null)
					return p.NotFound();
				if (p.BrDokKom == 0)
					return StatusCode(400, "Porudzbina nije pretvorena u proracun");

				p.BrDokKom = 0;
				p.Update();
				return StatusCode(200, "Uspesno ste razvezali porudzbinu od proracuna");
			});
		}

		[HttpPost]
		[Authorization("Administrator")]
		[Route("/api/Porudzbina/ReferentObrade/Set")]
		public async Task<IActionResult> SetReferentObrade(int porudzbina, int korisnikID)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina p = Porudzbina.Get(porudzbina);

				if (korisnikID <= 0)
					return StatusCode(500, "Doslo je do greske!");

				if (p == null)
					return p.NotFound();

				p.ReferentObrade = korisnikID;
				p.Update();

				return StatusCode(200, "Uspesno preuzeto na obradu");
			});
		}

		[HttpPost]
		[Authorization("Administrator")]
		[Route("/api/Porudzbina/Stavka/Ukloni")]
		public async Task<IActionResult> UkloniStavku(int porudzbina, int stavka)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina p = Porudzbina.Get(porudzbina);
				if (p == null)
					return StatusCode(400, "Porudzbina nije pronadjena");

				p.UcitajStavke(Porudzbina.Stavka.List(p.ID));
				if (p.Stavke == null || !p.Stavke.Where(t => t.ItemID == stavka).Any())
					return StatusCode(400, "Stavka nije pronadjena");

				Porudzbina.Stavka.Remove(stavka);
				return StatusCode(200);
			});
		}

		[HttpPost]
		[Route("/api/Porudzbina/Stavka/Add")]
		[Authorization("Administrator")]
		public async Task<IActionResult> DodajStavku(int porudzbinaID, int robaID, double kolicina) // TODO: StavkaAdd(...)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Task<Porudzbina> porudzbina = Porudzbina.GetAsync(porudzbinaID);
				Task<Proizvod> proizvod = Proizvod.GetAsync(robaID);

				if (porudzbina.Result == null)
					return porudzbina.Result.NotFound();

				if (
					proizvod.Result.KupovinaSamoUTransportnomPakovanju == 1
					&& kolicina % proizvod.Result.TransportnoPakovanje != 0
				)
					return StatusCode(400, "Neispravna kolicina");

				if (porudzbina.Result.JeProfi())
				{
					Task<Korisnik> korisnik = Korisnik.GetAsync(porudzbina.Result.UserID);
					Task<Korisnik.Cenovnik> cenovnik = korisnik.Result.GetCenovnikAsync();
					Cenovnik.Artikal artikal = cenovnik.Result[robaID];

					double rabat =
						((artikal.Cena.VPCena / proizvod.Result.ProdajnaCena) - 1) * (-100);

					try
					{
						int newID = Porudzbina.Stavka.Insert(
							porudzbina.Result.ID,
							proizvod.Result.RobaID,
							kolicina,
							artikal.Cena.VPCena,
							rabat
						);

						if (newID > 0)
							return StatusCode(201, newID);

						return StatusCode(500);
					}
					catch (APIRequestTimeoutException)
					{
						return StatusCode(408);
					}
					catch (APIRequestInternalServerErrorException)
					{
						return StatusCode(500);
					}
					catch (APIBadRequestException ex)
					{
						return StatusCode(400, ex.Message);
					}
					catch (Exception ex)
					{
						return StatusCode(500);
					}
				}
				else
				{
					Cenovnik cenovnik = Models.JednokratnaKupovina.CenovnikPoPorudzbini(
						porudzbina.Result
					);
					Cenovnik.Artikal artikal = cenovnik[robaID];
					double rabat =
						((artikal.Cena.VPCena / proizvod.Result.ProdajnaCena) - 1) * (100);

					try
					{
						int newID = Porudzbina.Stavka.Insert(
							porudzbina.Result.ID,
							proizvod.Result.RobaID,
							kolicina,
							artikal.Cena.VPCena,
							rabat
						);

						if (newID > 0)
							return StatusCode(201, newID);

						return StatusCode(500);
					}
					catch (APIRequestTimeoutException)
					{
						return StatusCode(408);
					}
					catch (APIRequestInternalServerErrorException)
					{
						return StatusCode(500);
					}
					catch (APIBadRequestException ex)
					{
						return StatusCode(400, ex.Message);
					}
					catch
					{
						return StatusCode(500);
					}
				}
			});
		}

		[HttpPost]
		[Authorization("Administrator")]
		[Route("/api/Porudzbina/Stavka/Kolicina/Set")]
		public async Task<IActionResult> StavkaKolicinaSet(
			int porudzbina,
			int stavka,
			string kolicina
		)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina p = Porudzbina.Get(porudzbina);

				if (p == null)
					return StatusCode(500, "Doslo je do greske");

				Porudzbina.Stavka s = p.Stavke.FirstOrDefault(t => t.ItemID == stavka);

				if (s == null)
					return StatusCode(500, "Doslo je do greske");

				if (kolicina.Contains(","))
					kolicina = kolicina.Replace(",", ".");

				s.Kolicina = Convert.ToDouble(kolicina);
				s.Update();

				return StatusCode(200);
			});
		}

		[HttpPost]
		[Authorization("Administrator")]
		[Route("/api/Porudzbina/MestoPreuzimanja/Set")]
		public async Task<IActionResult> MestoPreuzimanjaSet(int id, int mesto)
		{
			return await Task.Run(() =>
			{
				Porudzbina p = Porudzbina.Get(id);
				if (p == null)
					return StatusCode(400, "Porudzbina nije pronadjena");

				p.MagacinID = mesto;
				p.Update();
				return StatusCode(200, "Uspesno ste izmenili mesto preuzimanja");
			});
		}

		[HttpPost]
		[Authorization("Administrator")]
		[Route("/api/Porudzbina/NacinPlacanja/Set")]
		public async Task<IActionResult> NacinPlacanjaSet(int porudzbina, int nacinPlacanja)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina p = Porudzbina.Get(porudzbina);
				if (p == null)
					return StatusCode(400, "Porudzbina nije pronadjena");

				p.NacinUplate = (PorudzbinaNacinUplate)nacinPlacanja;
				p.Update();
				return StatusCode(200, "Uspesno te promenili nacin placanja");
			});
		}

		[HttpPost]
		[Authorization("Administrator")]
		[Route("/api/Porudzbina/InterniKomentar/Set")]
		public async Task<IActionResult> InterniKomentarSet(string komentar, int porudzbina)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina p = Porudzbina.Get(porudzbina);
				if (p == null)
					return StatusCode(400, "Porudzbina nije pronadjena");

				p.KomercijalnoInterniKomentar = komentar;
				p.Update();
				return StatusCode(200, "Uspesno ste dodali interni komentar");
			});
		}

		[HttpPost]
		[Authorization("Administrator")]
		[Route("/api/Porudzbina/Kometar/Set")]
		public async Task<IActionResult> KomentarSet(string komentar, int porudzbina)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina p = Porudzbina.Get(porudzbina);
				if (p == null)
					return StatusCode(400, "Porudzbina nije pronadjena");

				p.KomercijalnoKomentar = komentar;
				p.Update();
				return StatusCode(200, "Uspesno ste dodali  komentar");
			});
		}

		[Authorization("Administrator")]
		[Route("/api/Porudzbina/ObavestiRanju")]
		public async Task<IActionResult> ObavestiRadnju(int id)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina p = Porudzbina.Get(id);

				if (p.BrDokKom == 0)
					return StatusCode(400, "Porudzbina nije pretvorena u proracuna");

				string mobilni = Radnje[p.MagacinID];
				string poruka =
					"Web porudzbina "
					+ id
					+ " koja pripada vasem magacinu je obradjena i povezana sa proracunom "
					+ p.BrDokKom
					+ ". www.termodom.rs";
				SMS.SendSMS(mobilni, poruka, HttpContext.GetKorisnik().ID);

				return StatusCode(200, "Poruka je poslata");
			});
		}

		[Authorization("Administrator")]
		[Route("/api/Porudzbina/ObavestiKorisnika")]
		public async Task<IActionResult> ObavestiKorisnika(int id)
		{
			return await Task.Run<IActionResult>(() =>
			{
				Porudzbina p = Porudzbina.Get(id);

				if (p.Status != PorudzbinaStatus.CekaUplatu)
					return StatusCode(400, "Porudzbina jos uvek nije na obradi");

				Korisnik korisnik = Korisnik.Get(p.UserID);

				string mobilni = korisnik == null ? p.KontaktMobilni : korisnik.Mobilni;
				if (string.IsNullOrWhiteSpace(mobilni))
					return StatusCode(400, "Neispravan mobilni: " + mobilni);

				string poruka =
					"WEB Porudzbina "
					+ id
					+ " je obradjena, novi status je: CEKA UPLATU - www.termodom.rs";
				SMS.SendSMS(mobilni, poruka, HttpContext.GetKorisnik().ID);

				return StatusCode(200, "Poruka je poslata");
			});
		}

		//[Route("/api/Porudzbina/PosaljiSmsKorisniku")]
		//public async Task<IActionResult> PosaljiSmsKorisniku(int id, string poruka)
		//{
		//    return await Task.Run<IActionResult>(() =>
		//    {
		//        Porudzbina p = Porudzbina.Get(id);
		//        if (p == null)
		//            p = Porudzbina.Get(id);


		//        string mobilni = p.KontaktMobilni;
		//        SMS.SendSMS(mobilni, poruka, HttpContext.GetKorisnik().ID);

		//        return StatusCode(200, "Poruka je poslata");
		//    });
		//}
		#endregion

		[HttpGet]
		[Authorization("Administrator")]
		[Route("/Porudzbina/Dodaj/NovuStavku/{porudzbina}")] // TOOD: Ovo nije za api region..... Ruta zeznuta
		public IActionResult PromeniKolicinu(int porudzbina) // TODO: Razmisliti o nazivu
		{
			return View("/Views/Porudzbina/_pDodavanje_Stavke.cshtml", porudzbina);
		}

		#region DTO
		public class pStavkeAdministratorItemGetDTO // TOOD: Wrong Namespace
		{
			public int StavkaID { get; set; }
			public Porudzbina Porudzbina { get; set; }
			public Proizvod Proizvod { get; set; }
			public Cenovnik IronCenovnik { get; set; }
			public Cenovnik SilverCenovnik { get; set; }
			public Cenovnik GoldCenovnik { get; set; }
			public Cenovnik PlatinumCenovnik { get; set; }

			public pStavkeAdministratorItemGetDTO() { }

			public pStavkeAdministratorItemGetDTO(
				int stavkaID,
				Porudzbina porudzbina,
				Proizvod proizvod,
				Cenovnik ironCenovnik,
				Cenovnik silverCenovnik,
				Cenovnik goldCenovnik,
				Cenovnik platinumCenovnik
			)
			{
				this.StavkaID = stavkaID;
				this.Porudzbina = porudzbina;
				this.Proizvod = proizvod;
				this.IronCenovnik = ironCenovnik;
				this.SilverCenovnik = silverCenovnik;
				this.GoldCenovnik = goldCenovnik;
				this.PlatinumCenovnik = platinumCenovnik;
			}
		}
		#endregion
	}
}
