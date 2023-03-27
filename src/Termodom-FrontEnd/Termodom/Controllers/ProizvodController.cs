using System;
using System.Threading.Tasks;
using LimitlessSoft.NET5.Authorization;
using Microsoft.AspNetCore.Mvc;
using Termodom.Attributes;
using Termodom.Models;
using Termodom.API;

namespace Termodom.Controllers
{
    public class ProizvodController : Controller
    {
        #region Views
        [DefinisaniKorisnik]
        [Route("/Proizvod/{Rel}")]
        public async Task<IActionResult> Index(string Rel)
        {
            return await Task.Run<IActionResult>(() =>
            {
                Proizvod proizvod = Proizvod.Get(Rel);

                if (proizvod == null)
                    return View("Error", "Proizvod nije pronadjen!");

                Response.Cookies.Append("proizvodi-referrer", Request.Headers["Referer"].ToString());

                return View(proizvod);
            });
        }
        [Authorization("administrator", "staff")]
        [Route("/Proizvod/Uredi/{Rel}")]
        public async Task<IActionResult> Uredi(string Rel)
        {
            return await Task.Run(() =>
            {
                Proizvod p = Proizvod.Get(Rel);
                if (p == null)
                    return View("Error", "Proizvod nije pronadjen!");

                return View("Uredi", p);
            });
        }
        #endregion

        #region API
        [Authorization("administrator", "staff")]
        [Route("/api/Proizvod/Update")] // TODO: /api/Proizvod/Azuriraj ---- /api/Prozivodi/Update ------ /api/Proizvodi/PotvrdiIzmene
        public async Task<IActionResult> Update([FromBody]Proizvod proizvod) // TOOD: Azuriraj / Update
        {
            return await Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(proizvod.Naziv))
                        return StatusCode(400, "Niste dodali naziv proizvoda");
                    if (string.IsNullOrWhiteSpace(proizvod.KatBr))
                        return StatusCode(400, "Niste dodali kataloski broj");
                    if (proizvod.ProdajnaCena == 0)
                        return StatusCode(400, "Prodajna Cena nije validna!");
                    if (proizvod.NabavnaCena < 0)
                        return StatusCode(400, "Nabavna Cena nije validna");
                    if (string.IsNullOrWhiteSpace(proizvod.Slika))
                        return StatusCode(400, "Niste dodali sliku");
                    if(proizvod.PDV < 0)
                        return StatusCode(400, "Niste dodali PDV");
                    if(proizvod.DisplayIndex < 0)
                        return StatusCode(400, "Niste dodali display index");

                    if (proizvod.RobaID == 0)
                    {
                        string robaID = Roba.Insert(proizvod.KatBr, proizvod.Naziv, proizvod.JM);
                        proizvod.RobaID = Convert.ToInt32(robaID);
                        Proizvod.Insert(proizvod);
                        return StatusCode(200, "Uspesno ste dodali novi proizvod");
                    }
                    else 
                    {
                        Roba r = Roba.Get(proizvod.RobaID);
                        r.Naziv = proizvod.Naziv;
                        r.KatBr = proizvod.KatBr;
                        r.JM = proizvod.JM;
                        r.Update();
                        proizvod.Update();
                        return StatusCode(200, "Uspesno ste sacuvali promene");
                    }
                }
                catch(Exception ex)
                {
                    return StatusCode(500, ex.Message);
                }
            });
        }
        
        [Authorization("administrator", "staff")]
        [Route("/api/Proizvod/ID/Set")] 
        public async Task<IActionResult> apiIzmeniID(int oldID, int newID) 
        {
            return await Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (newID <= 0)
                        return StatusCode(400, "Novi id nije validan"); 
                    Proizvod novi = Proizvod.Get(newID);
                    if (novi != null)
                        return StatusCode(400, "ID proizvoda je zauzet"); 

                    Proizvod p = Proizvod.Get(oldID);
                    if (p == null)
                        return StatusCode(400, "Proizvod nije pronadjen");
                    p.UpdateID(newID);
                    return StatusCode(200);
                }
                catch(APIBadRequestException a)
                {
                    return StatusCode(400, a.Message);
                }
                catch (APIRequestTimeoutException)
                {
                    return StatusCode(504);
                }
                catch(APIResponseNoContentException)
                {
                    return StatusCode(204);
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }
                    
            });
        }
        #endregion
    }
}