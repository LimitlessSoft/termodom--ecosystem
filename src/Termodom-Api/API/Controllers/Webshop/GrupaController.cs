using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Webshop
{
    /// <summary>
    /// Koristi se za upravljanje grupama
    /// </summary>
    [ApiController]
    [RequireBearer]
    public class GrupaController : ControllerBase
    {
        /// <summary>
        /// Vraca Grupu.
        /// Rezultat moze varirati zavisno od parametra (id ili name).
        /// Moze biti prosledjen samo jedan parametar po request-u (id ili name)
        /// Jedan parametar mora briti prosledjen
        /// </summary>
        /// <param name="id"></param>
        /// <param name="naziv"></param>
        /// <returns></returns>
        /// <response code="200">Grupa je pronadjena i vracen</response>
        /// <response code="204">Grupa sa zadatim uslovom ne postoji</response>
        [HttpGet]
        [Route("/Webshop/Grupa/Get")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Grupa" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> Get(int id, string naziv)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (id == 0 && string.IsNullOrWhiteSpace(naziv))
                    return StatusCode(400, "Barem jedan parametar mora biti prosledjen (id ili naziv)");

                if (id > 0 && !string.IsNullOrWhiteSpace(naziv))
                    return StatusCode(400, "Prosledili ste oba parametra sto nije validno! Prosledite ili id ili naziv!");

                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();

                        string command = "SELECT ID, NAZIV, DISPLAY_INDEX FROM GRUPA WHERE ";
                        command += id > 0 ? "ID = @PAR" : "NAZIV = @PAR";

                        using (MySqlCommand cmd = new MySqlCommand(command, con))
                        {
                            cmd.Parameters.AddWithValue("@PAR", id > 0 ? id : naziv);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                if (dr.Read())
                                    return StatusCode(200, new Models.Webshop.Grupa()
                                    {
                                        ID = Convert.ToInt32(dr["ID"]),
                                        Naziv = dr["NAZIV"].ToString(),
                                        DisplayIndex = Convert.ToInt32(dr["DISPLAY_INDEX"])
                                    });
                        }
                    }
                    return StatusCode(204);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Kreira novu grupu
        /// </summary>
        /// <param name="naziv">Min length: 1, Max length: 64</param>
        /// <param name="displayIndex"></param>
        /// <returns></returns>
        /// <response code="201">Grupa uspesno kreirana</response>
        /// <response code="400">Neispravan parametar</response>
        [HttpPost]
        [Route("/Webshop/Grupa/Insert")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Grupa" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Insert([Required] string naziv, int displayIndex)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (string.IsNullOrWhiteSpace(naziv) || naziv.Length < 1 || naziv.Length > 64)
                    return StatusCode(400);

                try
                {
                    using(MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using(MySqlCommand cmd = new MySqlCommand("INSERT INTO GRUPA (NAZIV, DISPLAY_INDEX) VALUES (@NAZIV, @DISPLAY_INDEX)", con))
                        {
                            cmd.Parameters.AddWithValue("@NAZIV", naziv);
                            cmd.Parameters.AddWithValue("@DISPLAY_INDEX", displayIndex);

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
        /// Vraca listu svih grupa
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Vraca listu grupa</response>
        [HttpGet]
        [Route("/Webshop/Grupa/List")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Grupa" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> List()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT ID, NAZIV, DISPLAY_INDEX FROM GRUPA", con))
                        {
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                List<Models.Webshop.Grupa> list = new List<Models.Webshop.Grupa>();

                                while (dr.Read())
                                    list.Add(new Models.Webshop.Grupa() { ID = Convert.ToInt32(dr["ID"]), Naziv = dr["NAZIV"].ToString(), DisplayIndex = Convert.ToInt32(dr["DISPLAY_INDEX"]) });

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
