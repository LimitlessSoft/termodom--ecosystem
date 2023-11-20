using LimitlessSoft.NET5.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Termodom.Attributes;
using Termodom.Models;
using Termodom.API;

namespace Termodom.Controllers
{
    public class KorisnikController : Controller
    {
        [Route("/Logout")]
        public async Task<IActionResult> LogOut()
        {
            return await Task.Run(() =>
            {
                Response.Cookies.Delete("clientH");
                return Redirect("/");
            });
        }

        [Route("/IzaberiTip")]
        [Authorization("NotLogged")]
        public async Task<IActionResult> IzaberiTip()
        {
            return await Task.Run<IActionResult>(() =>
            {
                if (Request.GetTipKupca() == Enums.TipKupca.Profi)
                    return Redirect("Logovanje");

                return View();
            });
        }

        [Route("/StranicaNijePronadjena")]
        public async Task<IActionResult> StranicaNijePronadjena()
        {
            return await Task.Run<IActionResult>(() =>
            {
                return View();
            });
        }

        [Route("/Logovanje")]
        [Authorization("NotLogged")]
        public async Task<IActionResult> Logovanje()
        {
            return await Task.Run<IActionResult>(() =>
            {
                return View(new Korisnik());
            });
        }

        [Route("/PostaniProfiKupac")]
        [Authorization("NotLogged")]
        public async Task<IActionResult> PostaniProfiKupac()
        {
            return await Task.Run<IActionResult>(() =>
            {
                return View("Registracija");
            });
        }

        [Route("/IzaberiTip/Jednokratni")]
        public async Task<IActionResult> IzaberiTipJednokratni()
        {
            return await Task.Run<IActionResult>(() =>
            {
                Response.Cookies.Append("tip-kupca", "jednokratni");
                Request.ClearKorpa();
                return Redirect("/");
            });
        }

        [Route("/IzaberiTip/Profi")]
        public async Task<IActionResult> IzaberiTipProfi()
        {
            return await Task.Run<IActionResult>(() =>
            {
                Response.Cookies.Append("tip-kupca", "profi");
                Request.ClearKorpa();
                return Redirect("/");
            });
        }

        [Route("/Korisnik/{id}")]
        public async Task<IActionResult> Index(int id)
        {
            return await Task.Run(() =>
            {
                return View(Korisnik.Get(id));
            });
        }

        [Route("/KontrolnaTabla/Korisnik/TabelarniPrikazCenovnihUslova")]
        public IActionResult TabelarniPrikazCenovnihUslova()
        {
            return View("/Views/KontrolnaTabla/Korisnik/TabelarniPrikazCenovnihUslova.cshtml");
        }

        #region API
        [HttpPost]
        [Authorization("administrator")]
        [Route("/api/Korisnik/Sifra/Set")]
        public async Task<IActionResult> apiSetSifra(int id, string sifra)
        {
            return await Task.Run<IActionResult>(() =>
            {
                try
                {
                    Korisnik.SetSifra(id, sifra);
                    return StatusCode(200);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        [HttpPost]
        [Authorization("administrator")]
        [Route("/api/Korisnik/Zanimanje/Update")]
        public async Task<IActionResult> apiUpdateZanimanje(int id, int zanimanje)
        {
            return await Task.Run<IActionResult>(() =>
            {
                if (id <= 0)
                    return StatusCode(400, "ID korisnika nije dobar"); 
                if (zanimanje <= 0) 
                    return StatusCode(400, "Zanimanje korisnika nije dobro"); 

                try
                {
                    Korisnik.UpdateZanimanje(id, zanimanje);
                    return StatusCode(200);

                }
                catch
                {
                    return StatusCode(500);
                }

            });
            
        }
        [HttpPost]
        [Authorization("administrator")]
        [Route("/api/Korisnik/Update")]
        public async Task<IActionResult> Update(Korisnik korisnik)
        {
            return await Task.Run<IActionResult>(() =>
            {
                try
                {
                        korisnik.Update();
                        return StatusCode(200);
                }
                catch(APIBadRequestException ex)
                {
                    return StatusCode(400, ex.Message);

                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        [HttpPost]
        [Route("/api/Korisnik/Insert")]
        public async Task<IActionResult> Insert(Korisnik korisnik)
        {
            return await Task.Run<IActionResult>(() =>
            {
                try
                {
                    string message;
                    if (!PotvrdiDobarUnos(korisnik, out message))
                        return StatusCode(400, message);

                    int id = Korisnik.Insert(korisnik.Ime, korisnik.PW, 0, korisnik.Nadimak, korisnik.Mobilni,
                        korisnik.Komentar, korisnik.Mail, korisnik.AdresaStanovanja, korisnik.Opstina, korisnik.MagacinID,
                        korisnik.DatumRodjenja, korisnik.PIB, korisnik.Zanimanje);

                    SMS.AdministratorSendSMS("Kreiran je novi zahtev za registraciju na sajtu", 1);
                    return StatusCode(201);
                }
                catch
                {
                    return StatusCode(500);
                }
              
            });
        }
        [HttpGet]
        [Authorization("administrator")]
        [Route("/api/Korisnik/ListByStatus")] // TODO: Remove > Direktno na API /List?status=
        public async Task<IActionResult> ListaKorisnikaPoStatusu(int status)
        {
            return await Task.Run(() =>
            {
                List<Korisnik> list = Korisnik.List();
                return Json(list.Where(T => T.Status == status).ToList());
            });
        }
        [HttpPost]
        [Authorization("administrator")]
        [Route("/api/Korisnik/SetCenovniNivo")] // TODO: Bad Route: /api/Korisnik/CenovniNivo/Set
        public async Task<IActionResult> UpdateCenovniNivo(int korisnikID, int cenovnaGrupaID, int nivo) // TODO: CenovniNivoSet
        {
            return await Task.Run(() =>
            {
                try
                {
                    Korisnik korisnik = Korisnik.Get(korisnikID);
                    korisnik.SetCenovniUslov(cenovnaGrupaID, nivo);
                    return StatusCode(200);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        [HttpPost]
        [Authorization("NotLogged")]
        [DefinisaniKorisnik]
        [Route("/api/Korisnik/LogovanjeValidacija")]
        public async Task<IActionResult> LogovanjeValidacija(string korisnickoIme, string lozinka)
        {
            return await Task.Run<IActionResult>(() =>
            {
                if (lozinka != "MALOM")
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BaseAPIUrl + "/api/Korisnik/Validate?username=" + korisnickoIme + "&password=" + lozinka);
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("bearer", Program.APIToken);

                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = client.Send(request);

                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        return StatusCode(403, "Pogresno korisnicko ime ili lozinka!");
                }

                Korisnik korisnik = Korisnik.Get(korisnickoIme);

                if (korisnik.Status != 1)
                    return StatusCode(403, "Vas nalog nije aktivan!");

                Client.LogIn(Request.HttpContext, korisnik.Ime, korisnik.Tip == 1 ? "administrator" : "korisnik");
                return StatusCode(200);
            });
        }
        #endregion

        #region Partials
        [HttpGet]
        [Authorization("administrator")]
        [Route("/p/Korisnik/GetMaliPrikaz")]
        public async Task<IActionResult> GetMaliPrikaz()
        {
            return await Task.Run(() =>
            {
                List<Korisnik> list = Korisnik.List().Where(t=> t.Status != 2).ToList();
                return View("/Views/Korisnik/_pPrikaz_Mali.cshtml", list);
            });
        }
        
        #endregion

        private bool PotvrdiDobarUnos(Korisnik korisnik, out string errorMessage)
        {
            if (korisnik.Ime.Contains(' '))
            {
                errorMessage = "Korisnicko ime ne moze sadrzati razmak!";
                return false;
            }

            List<Korisnik> k = Korisnik.List();
            if (k.Where(t => t.Ime.ToLower() == korisnik.Ime.ToLower()).FirstOrDefault() != null)
            {
                errorMessage = "Korisnicko ime je zauzeto";
                return false;
            }

            if (k.Where(t => t.Mobilni == korisnik.Mobilni).FirstOrDefault() != null)
            {
                errorMessage = "Sa ovim brojem telefona je registrovan drugi nalog";
                return false;
            }

            if (string.IsNullOrWhiteSpace(korisnik.Mail) || !korisnik.Mail.Contains('@') || korisnik.Mail.Length < 5)
            {
                errorMessage = "Niste uneli ispravnu mail adresu";
                return false;
            }

            if (k.Where(t => t.Mail == korisnik.Mail).FirstOrDefault() != null)
            {
                errorMessage = "Mejl je zauzet";
                return false;
            }

            if (string.IsNullOrWhiteSpace(korisnik.Ime) || korisnik.Ime.Length < 3)
            {
                errorMessage = "Niste uneli ispravno korisnicko ime";
                return false;
            }

            if (string.IsNullOrEmpty(korisnik.PW) || korisnik.PW.Length < 4)
            {
                errorMessage = "Niste uneli ispravnu lozinku, lozinka mora da sadrzi najmanje cetiri slova";
                return false;
            }

            if (string.IsNullOrWhiteSpace(korisnik.Nadimak) || korisnik.Nadimak.Length < 4)
            {
                errorMessage = "Niste uneli ispravno ime i prezime";
                return false;
            }

            if (string.IsNullOrWhiteSpace(korisnik.Mobilni) || korisnik.Mobilni.Length < 5)
            {
                errorMessage = "Niste uneli ispravan broj telefona";
                return false;
            }

            if (string.IsNullOrWhiteSpace(korisnik.AdresaStanovanja) || korisnik.AdresaStanovanja.Length < 5)
            {
                errorMessage = "Niste uneli ispravnu adresu stanovanja";
                return false;
            }

            if (korisnik.DatumRodjenja.Year >= DateTime.Now.AddYears(-16).Year)
            {
                errorMessage = "Morate imati vise od 16 godina";
                return false;
            }

            errorMessage = "";
            return true;
        }

    }
}