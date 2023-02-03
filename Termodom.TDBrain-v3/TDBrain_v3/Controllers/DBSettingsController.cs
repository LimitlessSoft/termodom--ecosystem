using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TDBrain_v3.DB.Komercijalno;

namespace TDBrain_v3.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class DBSettingsController : Controller
    {
        /// <summary>
        /// Vraca ime servera gde se nalazi Firebird server
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/DBSettings")]
        [Route("/dbsettings/firebird/servername/get")]
        public Task<IActionResult> FirebirdServerGet()
        {
            return Task.Run<IActionResult>(() =>
            {
                return Content(DB.Settings.ServerName);
            });
        }
        /// <summary>
        /// Azurira ime servera gde se nalazi Firebird
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("/DBSettings")]
        [Route("/dbsettings/servername/set")]
        public IActionResult ServerNameSet([FromForm][Required] string serverName)
        {
            try
            {
                DB.Settings.ServerName = serverName;
                Settings.Save();
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                ex.Log();
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Vraca sifru Firebird servera
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/DBSettings")]
        [Route("/dbsettings/firebird/password/get")]
        public Task<IActionResult> FirebirdPasswordGet()
        {
            return Task.Run<IActionResult>(() =>
            {
                return Json(DB.Settings.Password);
            });
        }
        /// <summary>
        /// Azurira sifru firebird servera
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("/DBSettings")]
        [Route("/dbsettings/password/set")]
        public IActionResult PasswordSet([FromForm][Required] string password)
        {
            try
            {
                DB.Settings.Password = password;
                Settings.Save();
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                ex.Log();
                return StatusCode(500);
            }
        }
        /// <summary>
        /// Vraca putanju do komercijalno config baze
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/DBSettings")]
        [Route("/dbsettings/komercijalno/config/path/get")]
        public Task<IActionResult> KomercijalnoConfigPathGet()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(DB.Settings.ConnectionStringKomercijalnoConfig.Path()))
                        return StatusCode(204);
                    else
#pragma warning disable CS8604 // Possible null reference argument.
                        return Content(DB.Settings.ConnectionStringKomercijalnoConfig.Path());
#pragma warning restore CS8604 // Possible null reference argument.
                }
                catch (Exception ex)
                {
                    ex.Log();
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Azurira putanju do komercijalno config baze
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("/DBSettings")]
        [Route("/dbsettings/komercijalno/config/path/set")]
        public IActionResult KomercijalnoConfigPathSet([FromForm][Required] string path)
        {
            try
            {
                DB.Settings.ConnectionStringKomercijalnoConfig.SetPath(path);
                Settings.Save();
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                ex.Log();
                return StatusCode(500);
            }
        }
        
        /// <summary>
        /// Vraca putanju do TDOffice_v2 baze
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/DBSettings")]
        [Route("/dbsettings/tdoffice_v2/path/get")]
        public Task<IActionResult> TDOffice_v2PathGet()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(DB.Settings.ConnectionStringTDOffice_v2.Path()))
                        return StatusCode(204);
                    else
#pragma warning disable CS8604 // Possible null reference argument.
                        return Content(DB.Settings.ConnectionStringTDOffice_v2.Path());
#pragma warning restore CS8604 // Possible null reference argument.
                }
                catch (Exception ex)
                {
                    ex.Log();
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Azurira putanju do TDOffice_v2 baze
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("/DBSettings")]
        [Route("/dbsettings/tdoffice_v2/path/set")]
        public IActionResult TDOffice_v2PathSet([FromForm][Required] string path)
        {
            try
            {
                DB.Settings.ConnectionStringTDOffice_v2.SetPath(path);
                Settings.Save();
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                ex.Log();
                return StatusCode(500);
            }
        }

        /// <summary>
        /// Vraca informacije o putanjama do baza komercijalnog poslovanja
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/DBSettings")]
        [Route("/dbsettings/baza/komercijalno/list")]
        public Task<IActionResult> BazaKomercijalnoList()
        {
            return Task.Run<IActionResult>(() =>
            {
                return Json(DB.Settings.ConnectionStringKomercijalno.ToPutanjeDoBazaDTO());
            });
        }
        /// <summary>
        /// Dodaje ili azurira (PK: magacinid, godina) putanju do baze
        /// </summary>
        /// <param name="magacinId"></param>
        /// <param name="godina"></param>
        /// <param name="putanja"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("/DBSettings")]
        [Route("/dbsettings/baza/komercijalno/addorupdate")]
        public Task<IActionResult> BazaKomercijalnoAdd([FromForm][Required] int magacinId, [FromForm][Required] int godina, [FromForm][Required] string putanja)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    DB.Settings.ConnectionStringKomercijalno.AddOrUpdate(magacinId, godina, putanja);
                    Settings.Save();
                    return StatusCode(200);
                }
                catch(Exception ex)
                {
                    ex.Log();
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Brise putanju do baze za dati magacin i godinu
        /// </summary>
        /// <param name="magacinId"></param>
        /// <param name="godina"></param>
        /// <returns></returns>
        [HttpPost]
        [Tags("/DBSettings")]
        [Route("/dbsettings/baza/komercijalno/delete")]
        public Task<IActionResult> BazaKomercijalnoDelete ([FromForm] int magacinId, [FromForm] int godina)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    DB.Settings.ConnectionStringKomercijalno.Remove(magacinId, godina);
                    Settings.Save();
                    return StatusCode(200);
                }
                catch(Exception ex)
                {
                    ex.Log();
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Brise sve putanje do baza komercijalnog poslovanja
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Tags("/DBSettings")]
        [Route("/dbsettings/baza/komercijalno/deleteall")]
        public Task<IActionResult> BazaKomercijalnoDeleteAll()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    DB.Settings.ConnectionStringKomercijalno.RemoveAll();
                    Settings.Save();
                    return StatusCode(200);
                }
                catch (Exception ex)
                {
                    ex.Log();
                    return StatusCode(500);
                }
            });
        }

    }
}
