using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System;

namespace TDBrain_v3.Controllers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class TimeKeeperController : Controller
    {
        /// <summary>
        /// Sluzi da azurira time keeper
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Tags("/Komercijalno/TimeKeeper")]
        [Route("/Komercijalno/TimeKeeper/Update")]
        public Task<IActionResult> Update()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    foreach (string connString in DB.Settings.ConnectionStringKomercijalno.GetConnectionStringsDistinct(DateTime.Now.Year))
                    {
                        using (FbConnection con = new FbConnection(connString))
                        {
                            con.Open();
                            using (FbCommand cmd = new FbCommand("UPDATE PARAMETRI SET VREDNOST = @DAT WHERE NAZIV = @NAZ", con))
                            {
                                cmd.Parameters.AddWithValue("@DAT", DateTime.Now.ToString("dd.MM.yyyy"));
                                cmd.Parameters.AddWithValue("@NAZ", "danas");

                                cmd.ExecuteNonQuery();
                            }
                        }
                    }
                    return StatusCode(200);
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
