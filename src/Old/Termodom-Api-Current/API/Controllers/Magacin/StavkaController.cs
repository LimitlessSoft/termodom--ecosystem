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

namespace API.Controllers.Magacin
{
    /// <summary>
    /// Koristi se za upravljanje stavkama unutar dokumenata
    /// </summary>
    [ApiController]
    [RequireBearer]
    public class StavkaController : ControllerBase
    {
        /// <summary>
        /// Azurira podatke stavke u bazi na osnovu datog ID-a
        /// </summary>
        /// <param name="stavka"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Magacin/Stavka/Update")]
        [SwaggerOperation(Tags = new[] { "/Magacin/Stavka" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Update([Required] DTO.Magacin.StavkaUpdateDTO stavka)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (stavka == null ||
                    stavka.ID <= 0 ||
                    stavka.Kolicina <= 0)
                    return StatusCode(400, "ID i kolicina moraju biti veci od 0!");
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringMagacin))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand(@"UPDATE STAVKA SET
                        KOLICINA = @KOLICINA WHERE STAVKAID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", stavka.ID);
                            cmd.Parameters.AddWithValue("@KOLICINA", stavka.Kolicina);

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
        /// Kreira novu stavku
        /// </summary>
        /// <param name="stavka"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Magacin/Stavka/Insert")]
        [SwaggerOperation(Tags = new[] { "/Magacin/Stavka" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Insert([Required] DTO.Magacin.StavkaInsertDTO stavka)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (stavka.VrDok <= 0 ||
                    stavka.BrDok <= 0 ||
                    stavka.Kolicina <= 0 ||
                    stavka.RobaID <= 0)
                    return StatusCode(400, "Neispravan parametar. VRDOK, BRDOK, ROBAID i KOLICINA moraju biti prosledjeni i veci od 0!");
                try
                {
                    using(MySqlConnection con = new MySqlConnection(Program.ConnectionStringMagacin))
                    {
                        con.Open();
                        using(MySqlCommand cmd = new MySqlCommand("INSERT INTO STAVKA (ROBAID, VRDOK, BRDOK, KOLICINA) VALUES (@ROBAID, @VRDOK, @BRDOK, @KOLICINA)", con))
                        {
                            cmd.Parameters.AddWithValue("@ROBAID", stavka.RobaID);
                            cmd.Parameters.AddWithValue("@VRDOK", stavka.VrDok);
                            cmd.Parameters.AddWithValue("@BRDOK", stavka.BrDok);
                            cmd.Parameters.AddWithValue("@KOLICINA", stavka.Kolicina);

                            cmd.ExecuteNonQuery();

                            return StatusCode(201);
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
        /// Brise stavku po zadatom ID-u
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Magacin/Stavka/Delete")]
        [SwaggerOperation(Tags = new[] { "/Magacin/Stavka" })]
        [Consumes(MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Delete([Required] int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (id <= 0)
                    return StatusCode(400, "ID mora biti veci od 0!");
                try
                {
                    using(MySqlConnection con = new MySqlConnection(Program.ConnectionStringMagacin))
                    {
                        con.Open();

                        using(MySqlCommand cmd = new MySqlCommand("DELETE FROM STAVKA WHERE STAVKAID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);

                            cmd.ExecuteReader();

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
        /// Vraca objekat stavke iz baze na osnovu ID-a
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Magacin/Stavka/Get")]
        [SwaggerOperation(Tags = new[] { "/Magacin/Stavka" })]
        [Consumes(MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Get([Required] int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (id <= 0)
                    return StatusCode(400, "Morate proslediti ID stavke!");

                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringMagacin))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT STAVKAID, ROBAID, VRDOK, BRDOK, KOLICINA FROM STAVKA WHERE STAVKAID = @SID", con))
                        {
                            cmd.Parameters.AddWithValue("@SID", id);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    return StatusCode(200, new Models.Magacin.Stavka()
                                    {
                                        ID = Convert.ToInt16(dr["STAVKAID"]),
                                        RobaID = Convert.ToInt16(dr["ROBAID"]),
                                        VrDok = Convert.ToInt16(dr["VRDOK"]),
                                        BrDok = Convert.ToInt16(dr["BRDOK"]),
                                        Kolicina = Convert.ToDouble(dr["KOLICINA"]),
                                    });
                                }
                                else
                                {
                                    return StatusCode(204);
                                }
                            }
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
        /// Vraca listu stavki
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/Magacin/Stavka/List")]
        [SwaggerOperation(Tags = new[] { "/Magacin/Stavka" })]
        [Consumes(MediaTypeNames.Text.Plain)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> List(int? vrDok, int? brDok)
        {
            return Task.Run<IActionResult>(() =>
            {
                List<Models.Magacin.Stavka> list = new List<Models.Magacin.Stavka>();
                try
                {
                    if (vrDok != null && brDok == null ||
                        brDok == null && brDok != null)
                        return StatusCode(400, "Ukoliko prosledjujete vrdok ili brdok, morate proslediti i drugi parametar!");

                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringMagacin))
                    {
                        con.Open();
                        string cmdText = "SELECT STAVKAID, ROBAID, VRDOK, BRDOK, KOLICINA FROM STAVKA WHERE 1";

                        if (vrDok != null)
                            cmdText += " AND VRDOK = @VRDOK AND BRDOK = @BRDOK";

                        using (MySqlCommand cmd = new MySqlCommand(cmdText, con))
                        {
                            if (vrDok != null)
                            {
                                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                                cmd.Parameters.AddWithValue("@BRDOK", brDok);
                            }

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    list.Add(new Models.Magacin.Stavka()
                                    {
                                        ID = Convert.ToInt16(dr["STAVKAID"]),
                                        RobaID = Convert.ToInt16(dr["ROBAID"]),
                                        VrDok = Convert.ToInt16(dr["VRDOK"]),
                                        BrDok = Convert.ToInt16(dr["BRDOK"]),
                                        Kolicina = Convert.ToDouble(dr["KOLICINA"]),
                                    });
                                }
                                return StatusCode(200, list);
                            }
                        }
                    }
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
    }
}
