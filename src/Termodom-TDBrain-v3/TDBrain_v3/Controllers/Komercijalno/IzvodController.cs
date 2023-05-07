using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using TDBrain_v3.Exceptions;

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
        public Task<IActionResult> Collection([FromQuery][Required]int bazaId, [FromQuery][Required]int godinaBaze, [FromQuery]string? pozNaBroj)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    List<string> whereParameters = new List<string>();

                    if(!string.IsNullOrWhiteSpace(pozNaBroj))
                        whereParameters.Add($"POZNABROJ = '{pozNaBroj}'");

                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[bazaId, godinaBaze]))
                    {
                        con.Open();
                        return Json(DB.Komercijalno.IzvodManager.Dictionary(con, whereParameters));
                    }
                }
                catch(Exception ex)
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
        /// <param name="tekuciRacun"></param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Izvod")]
        [Route("/Komercijalno/Izvod/Duguje/Sum")]
        public Task<IActionResult> DugujeSum([FromQuery][Required]int bazaId, [FromQuery][Required]int godinaBaze, [FromQuery][Required]string tekuciRacun)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[bazaId, godinaBaze]))
                    {
                        con.Open();
                        return Json(DB.Komercijalno.IzvodManager.DugujeSum(con, tekuciRacun));
                    }
                }
                catch(PathToDatabaseNotFoundException ex)
                {
                    Debug.Log(ex.Message);
                    return StatusCode(400, ex.Message);
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
        /// <param name="tekuciRacun"></param>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Izvod")]
        [Route("/Komercijalno/Izvod/Potrazuje/Sum")]
        public Task<IActionResult> PotrazujeSum([FromQuery][Required] int bazaId, [FromQuery][Required] int godinaBaze, [FromQuery][Required] string tekuciRacun)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[bazaId, godinaBaze]))
                    {
                        con.Open();
                        return Json(DB.Komercijalno.IzvodManager.PotrazujeSum(con, tekuciRacun));
                    }
                }
                catch (Exception ex)
                {
                    Debug.Log(ex.Message);
                    return StatusCode(500);
                }
            });
        }
    }
}
