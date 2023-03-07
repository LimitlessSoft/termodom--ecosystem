using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TDBrain_v3.Managers.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class TekuciRacunController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/TekuciRacun")]
        [Route("/Komercijalno/TekuciRacun/List")]
        public Task<IActionResult> List([FromQuery][Required] int bazaId, [FromQuery][Required] int godinaBaze)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[bazaId, godinaBaze]))
                    {
                        con.Open();
                        return Json(TekuciRacunManager.List(con));
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.ToString());
                    return StatusCode(500);
                }
            });
        }
    }
}
