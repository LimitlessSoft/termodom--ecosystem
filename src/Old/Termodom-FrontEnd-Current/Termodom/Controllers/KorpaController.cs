using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Termodom.Attributes;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Termodom.Models;
using System;

namespace Termodom.Controllers
{
    [DefinisaniKorisnik]
    public class KorpaController : Controller
    {
        [Route("/Korpa")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        [Route("/api/Korpa/Dodaj")] // TODO: Bad Route /api "ODRADIO"
        public async Task<IActionResult> DodajProizvod(int id, double kolicina)
        {
            return await Task.Run<IActionResult>(() =>
            {
                Proizvod p = Proizvod.Get(id);
                if (p.KupovinaSamoUTransportnomPakovanju == 1)
                {
                    decimal ProveraKolicine = (decimal)kolicina / (decimal)p.TransportnoPakovanje;
                    decimal k = ProveraKolicine % 1;
                    if (k > (decimal)0.001)
                        return StatusCode(400, "Uneta kolicina nije dobra");
                }

                if(!Program.Korpe[Request.GetKorpa().Identifikator].DodajStavku(id, kolicina))
                    return StatusCode(404, "Greska prilikom dodavanja u korpu"); 

                return StatusCode(200, "Uspesno ste dodali proizvod u korpu");
            });
        }
        [HttpPost]
        [Route("/api/Korpa/Ukloni")] 
        public async Task<IActionResult> UkloniProizvod(int id)
        {
            return await Task.Run(() =>
            {
                if (!Program.Korpe[Request.GetKorpa().Identifikator].UkloniStavku(id))
                    return StatusCode(404, "Ne postoji proizvod");

                return StatusCode(200, "Uspesno ste uklonili proizvod iz korpe");
            });
        }
        [HttpPost]
        [Route("/api/Korpa/ZakljuciPorudzbinu")]
        public async Task<IActionResult> ZakljuciPorudzbinu(string komentar, int magacin, int nacinUplate, string imePrezime, string mobilni, string adresa)
        {
            return await Task.Run<IActionResult>(() =>
            {
                Task<List<Proizvod>> proizvodi = Proizvod.ListAsync();

                double usteda = 0;
                Korpa korpa = Request.GetKorpa();
                Cenovnik cenovnik = HttpContext.GetCenovnik();
                if (Request.GetTipKupca() == Enums.TipKupca.Jednokratni)
                {
                    if (korpa.ListaStavki().Count == 0)
                        return StatusCode(400, "Korpa ne sadrzi proizvode"); 
                    if (!korpa.ValidacijaJednokratne(imePrezime, mobilni))
                        return StatusCode(400, "Niste popunili sva polja"); 
                    if (magacin == -5 && string.IsNullOrEmpty(adresa))
                        return StatusCode(400, "Niste popunili adresu isporuke"); 

                    foreach (var stavka in korpa.ListaStavki()) 
                    {
                        double cena = cenovnik[stavka.RobaID].Cena.VPCena;
                        Proizvod p = proizvodi.Result.FirstOrDefault(t => t.RobaID == stavka.RobaID); 
                        usteda += ((p.ProdajnaCena - cena) * stavka.Kolicina) * (1 + (p.PDV / 100));
                    }
                    string hash = korpa.ZakljucajJednokratnuKupovinu(komentar, magacin, nacinUplate, imePrezime, mobilni, adresa, usteda);

                    #region TODO
                    // TODO: Porudzbina.Get
                    List<Porudzbina> list = Porudzbina.List();
                    Porudzbina p1 = list.Where(t => t.Hash == hash).FirstOrDefault();
                    #endregion

                    SMS.AdministratorSendSMS("Nova WEB Porudzbina je zakljucena. ID: " + p1.ID, -1);
                    Request.ClearKorpa();

                    return StatusCode(200, Hash(p1.ID.ToString()));
                }
                else if (Request.GetTipKupca() == Enums.TipKupca.Profi)
                {
                    Korisnik korinsik = HttpContext.GetKorisnik();
                    if (korpa.ListaStavki().Count == 0)
                        return StatusCode(400, "Korpa ne sadrzi proizvode"); 
                    if (magacin == -5 && string.IsNullOrEmpty(adresa))
                        return StatusCode(400, "Niste popunili adresu isporuke"); 

                    foreach (var stavka in korpa.ListaStavki())
                    {
                        double cena = cenovnik[stavka.RobaID].Cena.VPCena;
                        Proizvod p = proizvodi.Result.Where(t => t.RobaID == stavka.RobaID).FirstOrDefault();
                        usteda += ((p.ProdajnaCena - cena) * stavka.Kolicina) * (1 + (p.PDV / 100));
                    }

                    string hash = Request.GetKorpa().ZakljucajProfiPorudzbinu(korinsik, komentar, magacin, nacinUplate, usteda, adresa);

                    Porudzbina p1 = Porudzbina.List().Where(t => t.Hash == hash).FirstOrDefault(); // TODO: Get

                    SMS.AdministratorSendSMS("Nova WEB Porudzbina je zakljucena. ID: " + p1.ID, -1);

                    Request.ClearKorpa();
                    return StatusCode(200, Hash(p1.ID.ToString()));
                }
                else
                {
                    return StatusCode(500, "Doslo je do greske, obratite se administratoru");
                }
            });
        }

        [HttpPost]
        [Route("/api/Korpa/Kolicina/Set")]
        public async Task<IActionResult> apiSetKolicina(int robaID, double kolicina)
        {
            return await Task.Run<IActionResult>(() =>
            {
                try
                {
                    Proizvod p = Proizvod.Get(robaID);
                    if (p.KupovinaSamoUTransportnomPakovanju == 1)
                    {
                        double helper = (kolicina / p.TransportnoPakovanje);
                        if (helper % 1 != 0)
                            return StatusCode(400, "Niste uneli ispravnu kolicinu");
                    }
                    Program.Korpe[Request.GetKorpa().Identifikator][robaID].Kolicina = kolicina;
                    return StatusCode(200);
                }
                catch
                {
                    return StatusCode(500, "Doslo je do greske. Obratite se administratoru!");
                }
            });
        }

        private static string Hash(string value)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] res = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in res)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
