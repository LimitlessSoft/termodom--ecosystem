using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    ///
    /// </summary>
    [ApiController]
    public class RobaUMagacinuController : Controller
    {
        private readonly ILogger<RobaUMagacinuController> _logger;

        /// <summary>
        /// Default contructor
        /// </summary>
        /// <param name="logger"></param>
        public RobaUMagacinuController(ILogger<RobaUMagacinuController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Vraca dictionary objekata roba u magacinu za datu godinu.
        /// Ukoliko godinaBaze nije prosledjena, gledace se trenutna godina.
        /// </summary>
        /// <returns></returns>
        /// <param name="godinaBaze">Godina baze nad kojom ce se izvrsiti akcija. Ukoliko nije prosledjena, akcija ce se vrsiti nad trenutnom godinom</param>
        [HttpGet]
        [Tags("/Komercijalno/RobaUMagacinu")]
        [Route("/Komercijalno/RobaUMagacinu/Dictionary")]
        public Task<IActionResult> Dictionary(int? godinaBaze, int? bazaID, int[]? magacinId)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[bazaID ?? DB.Settings.MainMagacinKomercijalno, godinaBaze ?? DateTime.Now.Year]))
                    {
                        con.Open();

                        List<string> whereParameters = new List<string>();

                        if (magacinId != null && magacinId.Length > 0)
                            whereParameters.Add($"MAGACINID in ({string.Join(", ", magacinId)})");

                        return Json(DB.Komercijalno.RobaUMagacinuManager.Dictionary(con, whereParameters));
                    }
                }
                catch(Exception ex)
                {
                    _logger.LogError(ex.Message);
                    Debug.Log(ex.Message);
                    return StatusCode(500);
                }
            });
        }
    }
}
