using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Models.Magacin;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Swashbuckle.AspNetCore.Annotations;

namespace API.Controllers.Magacin
{
    /// <summary>
    /// Koristi se za kontrolisanje dokumenata
    /// </summary>
    [ApiController]
    [RequireBearer]
    public class DokumentController : ControllerBase
    {
        private object ___insertLock = new object();

        /// <summary>
        /// Vraca dokument zadate vrste i broja
        /// </summary>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        /// <returns></returns>
        /// <response code="200">Dokument je uspesno pronadjen i vracen</response>
        /// <response code="204">Dokument date vrste sa datim brojem nije pronadjen</response>
        /// <response code="400">vrDok ili brDok nisu ispravni</response>
        [HttpGet]
        [RequireBearer]
        [Route("/Magacin/Dokument/Get")]
        [SwaggerOperation(Tags = new[] { "/Magacin/Dokument" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Get([Required] int vrDok, [Required] int brDok)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (vrDok == 0 || brDok == 0)
                    return StatusCode(400);
                try
                {
                    using(MySqlConnection con = new MySqlConnection(Program.ConnectionStringMagacin))
                    {
                        con.Open();

                        using (MySqlCommand cmd = new MySqlCommand("SELECT VRDOK, BRDOK, DATUM, KORISNIKID FROM DOKUMENT WHERE VRDOK = @V AND BRDOK = @B", con))
                        {
                            cmd.Parameters.AddWithValue("@V", vrDok);
                            cmd.Parameters.AddWithValue("@B", brDok);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                if (dr.Read())
                                {
                                    return StatusCode(200, new Dokument()
                                    {
                                        VrDok = Convert.ToInt32(dr["VRDOK"]),
                                        BrDok = Convert.ToInt32(dr["BRDOK"]),
                                        Datum = Convert.ToDateTime(dr["DATUM"]),
                                        KorisnikID = Convert.ToInt32(dr["KORISNIKID"])
                                    });
                                }
                            }
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
        /// Vraca listu svih dokumenata.
        /// Rezultat se moze filtrirati ako se proslede parametri.
        /// </summary>
        /// <param name="vrDok"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireBearer]
        [Route("/Magacin/Dokument/List")]
        [SwaggerOperation(Tags = new[] { "/Magacin/Dokument" })]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public Task<IActionResult> List(int? vrDok)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringMagacin))
                    {
                        con.Open();

                        List<Dokument> list = new List<Dokument>();
                        string command = "SELECT VRDOK, BRDOK, DATUM, KORISNIKID FROM DOKUMENT";

                        if (vrDok != null)
                            command += " WHERE VRDOK = @VRDOK";

                        using (MySqlCommand cmd = new MySqlCommand(command, con))
                        {
                            if (vrDok != null)
                                cmd.Parameters.AddWithValue("@VRDOK", (int)vrDok);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                            {
                                while (dr.Read())
                                {
                                    list.Add(new Dokument()
                                    {
                                        VrDok = Convert.ToInt32(dr["VRDOK"]),
                                        BrDok = Convert.ToInt32(dr["BRDOK"]),
                                        Datum = Convert.ToDateTime(dr["DATUM"]),
                                        KorisnikID = Convert.ToInt32(dr["KORISNIKID"])
                                    });
                                }
                            }
                        }

                        return StatusCode(200, list);
                    }
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        
        /// <summary>
        /// Kreira novi dokument i vraca broj novog dokumenta
        /// </summary>
        /// <param name="vrDok"></param>
        /// <param name="korisnikID"></param>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/Magacin/Dokument/Insert")]
        [SwaggerOperation(Tags = new[] { "/Magacin/Dokument" })]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public Task<IActionResult> Insert([Required] int vrDok, [Required] int korisnikID)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringMagacin))
                    {
                        con.Open();

                        using (MySqlCommand cmd = new MySqlCommand("INSERT INTO DOKUMENT (VRDOK, BRDOK, DATUM, KORISNIKID) VALUES (@VRDOK, @BRDOK, @DATUM, @KORISNIKID)", con))
                        {
                            lock (___insertLock)
                            {
                                int newBrDok = _maxBrDok(vrDok);

                                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                                cmd.Parameters.AddWithValue("@BRDOK", 1);
                                cmd.Parameters.AddWithValue("@DATUM", DateTime.Now);
                                cmd.Parameters.AddWithValue("@KORISNIKID", korisnikID);

                                cmd.ExecuteNonQuery();
                                return StatusCode(201, newBrDok);
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
        /// Vraca maximalni brdok za dati vrdok
        /// </summary>
        /// <param name="vrDok"></param>
        /// <returns></returns>
        [HttpGet]
        [RequireBearer]
        [Route("/Magacin/Dokument/BrDok/Max")]
        [SwaggerOperation(Tags = new[] { "/Magacin/Dokument" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> BrDokMax([Required] int vrDok)
        {
            return Task.Run<IActionResult>(() =>
            {
                return StatusCode(200, _maxBrDok(vrDok));
            });
        }

        // ==============================================
        // ==============================================
        // ==============================================

        /// <summary>
        /// Vraca maximalni broj dokumenta za dati vrDok
        /// </summary>
        /// <param name="vrDok"></param>
        /// <returns></returns>
        private int _maxBrDok(int vrDok)
        {
            using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringMagacin))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT MAX(BRDOK) DOKUMENT WHERE VRDOK = @V", con))
                {
                    cmd.Parameters.AddWithValue("@V", vrDok);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return Convert.ToInt32(dr[0]);
                        }
                    }
                }
            }

            return 0;
        }
    }
}
