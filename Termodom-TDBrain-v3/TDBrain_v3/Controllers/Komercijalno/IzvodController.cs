using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class IzvodController : Controller
    {
        private ILogger<IzvodController> _logger { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public IzvodController(ILogger<IzvodController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Izvod")]
        [Route("/Komercijalno/Izvod/Dictionary")]
        public Task<IActionResult> Collection([FromQuery][Required]int bazaId, [FromQuery][Required]int godinaBaze)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[bazaId, godinaBaze]))
                    {
                        con.Open();
                        return Json(DB.Komercijalno.IzvodManager.Dictionary(con));
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
