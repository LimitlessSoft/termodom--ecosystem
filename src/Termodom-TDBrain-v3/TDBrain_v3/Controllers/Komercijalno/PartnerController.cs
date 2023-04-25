using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;
using TDBrain_v3.Managers.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class PartnerController : Controller
    {
        /// <summary>
        /// Insertuje partnera u sve baze komercijalnog poslovanja.
        /// </summary>
        /// <param name="naziv">Naziv Partnera</param>
        /// <param name="adresa">Adresa partnera</param>
        /// <param name="posta">Postanski broj partnera</param>
        /// <param name="telefon">Fiksni telefon partnera</param>
        /// <param name="mobilni">Mobilni telefon partnera</param>
        /// <param name="fax">Fax partnera</param>
        /// <param name="email">Email partnera</param>
        /// <param name="kontakt">Ime kontakt osobe</param>
        /// <param name="pib">Poreski Identifikacioni Broj</param>
        /// <param name="kategorija">Kategorija Komercijalno Poslovanje</param>
        /// <param name="aktivan">Aktivna (1 = aktivan, 0 = neaktivan)</param>
        /// <param name="mestoID">Mesto ID partnera (komercijalno)</param>
        /// <param name="mbroj">Maticni Broj partnera</param>
        /// <param name="refID">ID Korisnika komercijalnog poslovanja koji je referent ovog partnera</param>
        /// <param name="pdvo">Da li je u PDV obavezi (1 = jeste, 0 = nije)</param>
        /// <param name="nazivZaStampu">Naziv koji se vidi na stampi</param>
        /// <param name="zapID">ID Korisnika komercijalnog poslovanja koji je vlasnik ovog objekta partnera</param>
        /// <param name="valuta">Osnovna valuta parntera (default: DIN)</param>
        /// <param name="dozvoljeniMinus">Vrednost dozovljenog minusa (default: 0)</param>
        /// <param name="imaUgovor">(default: 0)</param>
        /// <param name="vrstaCenovnika">(default: 0)</param>
        /// <param name="opstinaID">(default: null)</param>
        /// <param name="drzavljanstvoID">(default: 0)</param>
        /// <param name="zanimanjeID">(default: 0)</param>
        /// <param name="webStatus">(default: 0)</param>
        /// <param name="gppID">(default: 0)</param>
        /// <param name="ceneOdGrupe">(default: 0)</param>
        /// <param name="vpCID">(defult: 1)</param>
        /// <param name="procpc">(default: 0)</param>
        /// <returns>Vraca PPID novokreiranog partnera</returns>
        [HttpPost]
        [Tags("/Komercijalno/Partner")]
        [Route("/Komercijalno/Partner/Insert")]
        [SwaggerResponse(201, "Partner uspesno kreiran!")]
        [SwaggerResponse(400, "Neki od parametara nije uredu!")]
        [SwaggerResponse(409, "Konflikt u ID-evima!")]
        public Task<IActionResult> Insert(
            [FromForm][Required] string naziv,
            [FromForm][Required] string adresa,
            [FromForm][Required] string posta,
            [FromForm][Required] string mobilni,
            [FromForm][Required] string email,
            [FromForm][Required] string kontakt,
            [FromForm][Required] string pib,
            [FromForm][Required] Int64 kategorija,
            [FromForm][Required] string mestoID,
            [FromForm][Required] string mbroj,
            [FromForm][Required] int refID,
            [FromForm][Required] string nazivZaStampu,
            [FromForm][Required] int zapID,
            [FromForm] int pdvo = 1,
            [FromForm] string valuta = "DIN",
            [FromForm] int imaUgovor = 0,
            [FromForm] int vrstaCenovnika = 0,
            [FromForm] int? opstinaID = null,
            [FromForm] int zanimanjeID = 0,
            [FromForm] int webStatus = 0,
            [FromForm] int gppID = 0,
            [FromForm] int vpCID = 1,
            [FromForm] double procpc = 0,
            [FromForm] string? telefon = null,
            [FromForm] string? fax = null,
            [FromForm] double dozvoljeniMinus = 0,
            [FromForm] int ceneOdGrupe = 0,
            [FromForm] int aktivan = 1,
            [FromForm] int drzavljanstvoID = 0)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(naziv))
                        return StatusCode(400, "Morate proslediti parametar 'naziv'!");

                    if (naziv.Length > 50)
                        return StatusCode(400, "Parametar 'naziv' ne sme imati vise od 50 karaktera!");

                    int noviID = DB.Komercijalno.PartnerManager.Insert(naziv, adresa, posta, telefon, mobilni, fax, email, kontakt, pib, kategorija, aktivan, mestoID, mbroj,
                        opstinaID, drzavljanstvoID, refID, pdvo, nazivZaStampu, valuta, dozvoljeniMinus, imaUgovor, vrstaCenovnika, drzavljanstvoID, zanimanjeID,
                        webStatus, gppID, ceneOdGrupe, vpCID, procpc, zapID);

                    return StatusCode(201, noviID);
                }
                catch(DB.Komercijalno.Exceptions.PartnerInsertKonfliktIDevaException ex)
                {
                    ex.Log();
                    return StatusCode(409, "Konflikt, neujednaceni ID-evi u bazama!");
                }
                catch(Exception ex)
                {
                    ex.Log();
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Vraca objekat partnera na osnovu prosledjenog PPID-a ili PIB-a.
        /// U trenutku moze biti prosledjen samo jedan uslov pretrage (ppid ili pib)
        /// </summary>
        /// <param name="ppid">Unikatni identifikator partnera</param>
        /// <param name="pib">Poreski Identifikacioni Broj partnera</param>
        /// <param name="godina">Godina baze nad kojom se radi. Ukoliko se ne prosledi, radi nad trenutnom godinom</param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Partner")]
        [SwaggerResponse(200)]
        [SwaggerResponse(204, "Partner sa datim PPID nije pronadjen")]
        [SwaggerResponse(400, "Neki od parametara nije dobar!")]
        [Route("/Komercijalno/Partner/Get")]
        public Task<IActionResult> Get([FromQuery] int? ppid, [FromQuery] string? pib, [FromQuery] int? godina)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (ppid == null && pib == null)
                        return StatusCode(400, "Morate proslediti barem 1 uslov pretrage (pib ili ppid)");

                    if(ppid != null && pib != null)
                        return StatusCode(400, "Morate proslediti samo 1 uslov pretrage (pib ili ppid)");

                    if (godina == null)
                        godina = DateTime.Now.Year;

                    var where = new List<string>();
                    where.Add(ppid != null ? $"PPID = {ppid}" : $"PIB = '{pib}'");

                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[DB.Settings.MainMagacinKomercijalno, (int)godina]))
                    {
                        con.Open();
                        var partner = PartnerManager.Dictionary(con, where);

                        if (partner == null || partner.Count == 0)
                            return StatusCode(204);

                        return Json(partner);
                    }
                }
                catch(Exception ex)
                {
                    ex.Log();
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Vraca dictionary parntera komecijalnog
        /// </summary>
        /// <param name="godina">Godina baze iz koje se izvlace partneri. Ukoliko se ne prosledi uzece partnere trenutne godine.</param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Partner")]
        [Route("/Komercijalno/Partner/Dict")]
        public Task<IActionResult> Dict([FromQuery] int? godina)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (godina == null)
                        godina = DateTime.Now.Year;

                    return Json(DB.Komercijalno.PartnerManager.Dict(50, (int)godina));
                }
                catch(Exception ex)
                {
                    Debug.Log(ex.ToString());
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Vraca listu parntera komecijalnog
        /// </summary>
        /// <param name="godina">Godina baze iz koje se izvlace partneri. Ukoliko se ne prosledi uzece partnere trenutne godine.</param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Partner")]
        [Route("/Komercijalno/Partner/List")]
        public Task<IActionResult> List([FromQuery] int? godina)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (godina == null)
                        godina = DateTime.Now.Year;

                    var dict = DB.Komercijalno.PartnerManager.Dict(50, (int)godina);

                    return Json(dict.Values);
                }
                catch(Exception ex)
                {
                    Debug.Log(ex.ToString());
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="godinaBaze"></param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Partner")]
        [Route("/Komercijalno/Partner/Dictionary")]
        public Task<IActionResult> Dictionary([FromQuery]int? godinaBaze)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[DB.Settings.MainMagacinKomercijalno, godinaBaze ?? DateTime.Now.Year]))
                    {
                        con.Open();
                        return Json(TDBrain_v3.Managers.Komercijalno.PartnerManager.Dictionary(con));
                    }
                }
                catch(Exception ex)
                {
                    Debug.Log(ex.ToString());
                    return StatusCode(500);
                }
            });
        }
    }
}
