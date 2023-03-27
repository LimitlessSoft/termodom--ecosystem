using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Koristi se za upravljanje robom
    /// </summary>
    [RequireBearer]
    [ApiController]
    public class RobaController : ControllerBase
    {
        private static object ___insertLock = new object();
        /// <summary>
        /// Azurira podatke robe po datom id-u
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("api/Roba/Update")]
        [SwaggerOperation(Tags = new[] { "api/Roba" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Update([Required] Models.Roba roba)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (string.IsNullOrWhiteSpace(roba.KatBr) ||
                    string.IsNullOrWhiteSpace(roba.Naziv) ||
                    string.IsNullOrWhiteSpace(roba.JM))
                    return StatusCode(400);
                try
                {
                    using(MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        using(MySqlCommand cmd = new MySqlCommand("UPDATE ROBA SET KATBR = @KATBR, NAZIV = @NAZIV, JM = @JM WHERE ROBAID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@KATBR", roba.KatBr);
                            cmd.Parameters.AddWithValue("@NAZIV", roba.Naziv);
                            cmd.Parameters.AddWithValue("@JM", roba.JM);
                            cmd.Parameters.AddWithValue("@ID", roba.ID);

                            cmd.ExecuteNonQuery();

                            return StatusCode(200);
                        }
                    }
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Insert robe
        /// </summary>
        /// <param name="naziv"></param>
        /// <param name="katBr"></param>
        /// <param name="jm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/api/Roba/Insert")]
        [SwaggerOperation(Tags = new[] {"api/Roba"})]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Insert(string naziv, string katBr, string jm)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (string.IsNullOrWhiteSpace(naziv))
                    return StatusCode(400, "Morate proslediti naziv proizvoda!");
                if (naziv.Length < 3)
                    return StatusCode(400, "Naziv je prekratak!");
                if (naziv.Length > 64)
                    return StatusCode(400, "Naziv moze imati najvise 64 karaktera!");
                if(string.IsNullOrWhiteSpace(katBr))
                    return StatusCode(400, "Morate proslediti Kataloski Broj proizvoda!");
                if (katBr.Length > 64)
                    return StatusCode(400, "Kataloski broj moze imati najvise 64 karaktera!");
                if (string.IsNullOrWhiteSpace(jm))
                    return StatusCode(400, "Morate proslediti Jedinicu Mere proizvoda!");
                if (jm.Length > 16)
                    return StatusCode(400, "Jedinica mere moze imati najvise 16 karaktera!");

                int robaID = 0;
                try
                {
                    lock (___insertLock)
                    {
                        using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                        {
                            con.Open();
                            using (MySqlCommand cmd = new MySqlCommand("SELECT MAX(ROBAID) FROM ROBA", con))
                            {
                                using (MySqlDataReader dt = cmd.ExecuteReader())
                                    if (dt.Read())
                                        robaID = Convert.ToInt32(dt[0]) + 1;
                            }
                            using (MySqlCommand cmd = new MySqlCommand("INSERT INTO ROBA(ROBAID, KATBR, NAZIV, JM) VALUES (@ROBAID, @KATBR, @NAZIV, @JM)", con))
                            {
                                cmd.Parameters.AddWithValue("@ROBAID", robaID);
                                cmd.Parameters.AddWithValue("@NAZIV", naziv);
                                cmd.Parameters.AddWithValue("@KATBR", katBr);
                                cmd.Parameters.AddWithValue("@JM", jm);
                                cmd.ExecuteNonQuery();
                                return StatusCode(201, robaID);
                            }
                        }
                    }
                }
                catch(Exception)
                {
                    return StatusCode(500);
                }
               
            });
        }
        /// <summary>
        /// Vraca podatke robe po zadatom ID-u
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Roba/Get")]
        [SwaggerOperation(Tags = new[] { "api/Roba" })]
        [Consumes(MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Get([Required] int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (id <= 0)
                    return StatusCode(400);

                try
                {
                    using(MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        using(MySqlCommand cmd = new MySqlCommand("SELECT ROBAID, KATBR, NAZIV, JM FROM ROBA WHERE ROBAID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                if (dr.Read())
                                    return StatusCode(200, new Models.Roba()
                                    {
                                        ID = Convert.ToInt32(dr["ROBAID"]),
                                        KatBr = dr["KATBR"].ToString(),
                                        Naziv = dr["NAZIV"].ToString(),
                                        JM = dr["JM"].ToString()
                                    });

                            return StatusCode(204);
                        }
                    }
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Vraca listu sve robe
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("api/Roba/List")]
        [SwaggerOperation(Tags = new[] { "api/Roba" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> List()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    List<Models.Roba> list = new List<Models.Roba>();
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT ROBAID, KATBR, NAZIV, JM FROM ROBA", con))
                        {
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                while (dr.Read())
                                    list.Add(new Models.Roba()
                                    {
                                        ID = Convert.ToInt32(dr["ROBAID"]),
                                        KatBr = dr["KATBR"].ToString(),
                                        Naziv = dr["NAZIV"].ToString(),
                                        JM = dr["JM"].ToString()
                                    });
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
    }
}
