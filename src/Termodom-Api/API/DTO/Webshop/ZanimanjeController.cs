using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using Swashbuckle.AspNetCore.Annotations;

namespace API.DTO.Webshop
{ 
    /// <summary>
    /// Koristi se za upravljanjem zanimanje na webshopu
    /// </summary>
    [ApiController]
    [RequireBearer]
    public class ZanimanjeController : Controller
    {
        /// <summary>
        /// Vraca listu zanimanja
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/Webshop/Zanimanje/List")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Zanimanje" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> List()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {

                    List<Models.Webshop.Zanimanje> list = new List<Models.Webshop.Zanimanje>();
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT ID, NAZIV FROM ZANIMANJE", con))
                        {
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                            {
                                while (dt.Read())
                                    list.Add(new Models.Webshop.Zanimanje
                                    {
                                        ID = Convert.ToInt32(dt["ID"]),
                                        Naziv = dt["NAZIV"].ToString()
                                    });
                            }
                        }
                    }
                    return StatusCode(200, list);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Kreira novo zanimanje u bazi
        /// </summary>
        /// <param name="naziv"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Webshop/Zanimanje/Insert")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Zanimanje" })]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public Task<IActionResult> Insert(string naziv)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(naziv))
                        return StatusCode(400, "Morate da prosledite naziv");
                    if (naziv.Length < 4)
                        return StatusCode(400, "Naziv mora imati vise od tri slova");

                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("INSERT INTO ZANIMANJE(NAZIV) VALUES(@NAZIV)", con))
                        {
                            cmd.Parameters.AddWithValue("@NAZIV", naziv);
                            cmd.ExecuteNonQuery();
                        }
                        return StatusCode(201);
                    }
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Brise zanimanje u bazi, pod uslovom da ne postoji korisnik sa tim zanimanjem
        /// </summary>
        /// <param name="id">id zanimanja</param>
        /// <returns></returns>
        [HttpPost]
        [Route("/WEbshop/Zanimanje/Remove")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Zanimanje" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> Remove(int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(*) FROM KORISNIK WHERE ZANIMANJE = @ZANIMANJE", con))
                        {
                            cmd.Parameters.AddWithValue("@ZANIMANJE", id);
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                            {
                                if (dt.Read())
                                    if (Convert.ToInt32(dt[0]) > 0)
                                        return StatusCode(400, "Postoje korisnici za zanimanjem koje ste pokusali da izbrisete");
                            }
                        }
                        using (MySqlCommand cmd = new MySqlCommand("DELETE FROM ZANIMANJE WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.ExecuteNonQuery();
                        }
                        return StatusCode(200);
                    }
                }
                catch(Exception ex)
                {
                    return StatusCode(500);
                }
            });
        }
    }
}
