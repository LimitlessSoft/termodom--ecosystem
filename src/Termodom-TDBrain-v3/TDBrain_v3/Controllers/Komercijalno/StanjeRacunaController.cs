using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TDBrain_v3.Managers.Komercijalno;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class StanjeRacunaController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bazaId"></param>
        /// <param name="godinaBaze"></param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/StanjeRacuna")]
        [Route("/Komercijalno/StanjeRacuna/Get")]
        public Task<IActionResult> Get([FromQuery][Required] int bazaId, [FromQuery][Required] int godinaBaze, [FromQuery][Required] string tekuciRacun)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[bazaId, godinaBaze]))
                    {
                        con.Open();
                        StanjeRacuna stanjeRacuna = StanjeRacunaManager.Get(con, tekuciRacun);

                        if (stanjeRacuna == null)
                            return StatusCode(204);

                        return Json(stanjeRacuna);
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bazaId"></param>
        /// <param name="godinaBaze"></param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/StanjeRacuna")]
        [Route("/Komercijalno/StanjeRacuna/Dictionary")]
        public Task<IActionResult> Dictionary([FromQuery][Required]int bazaId, [FromQuery][Required]int godinaBaze)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[bazaId, godinaBaze]))
                    {
                        con.Open();
                        return Json(StanjeRacunaManager.Dictionary(con));
                    }
                }
                catch(Exception ex)
                {
                    Debug.Log(ex.Message);
                    return StatusCode(500);
                }
            });
        }
    }
}
