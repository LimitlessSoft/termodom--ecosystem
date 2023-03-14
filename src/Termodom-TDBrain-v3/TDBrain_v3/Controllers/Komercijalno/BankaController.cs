using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using TDBrain_v3.Managers.Komercijalno;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class BankaController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Tags("/Komercijalno/Banka")]
        [Route("/Komercijalno/Banka/Dictionary")]
        public Task<IActionResult> Dictionary()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[DB.Settings.MainMagacinKomercijalno, DateTime.Now.Year]))
                    {
                        con.Open();
                        return Json(BankaManager.Dictionary(con));
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
