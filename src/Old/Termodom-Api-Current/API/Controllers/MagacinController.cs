using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;
using API.DTO;
using System.Text.RegularExpressions;
using System.Net.Mime;
using System.ComponentModel.DataAnnotations;
using API.Controllers.Webshop;

namespace API.Controllers
{
    /// <summary>
    /// Koristi se za kontrolisanje magacina
    /// </summary>
    [ApiController]
    [RequireBearer]

    public class MagacinController : Controller
    {
        private Dictionary<string, int> MAX_CHARACTERS = new Dictionary<string, int>()
        {
            { "ADRESA", 64 },
            { "GRAD", 64 },
            { "NAZIV", 64 }
        };
        private Dictionary<string, int> MIN_CHARACTERS = new Dictionary<string, int>()
        {
            { "ADRESA", 4 },
            { "GRAD", 2 },
            { "NAZIV", 4 }
        };
        private string[] VALID_PARAMETERS = new string[7] { "ID", "ADRESA", "GRAD", "EMAIL", "KOORDINATE", "TELEFON", "NAZIV" };
        private static object _____insertLock { get; set; } = new object();

        /// <summary>
        /// Insertuje novi magacin
        /// </summary>
        /// <param name="magacin">DTO Objekat</param>
        /// <returns></returns>
        /// <response code="201">Kao response objekat vraca ID novokreiraong magacina</response>
        /// <response code="400">Kao response objekat vraca razlog loseg requesta</response>
        [HttpPost]
        [Route("/api/Magacin/Insert")]
        [SwaggerOperation(Tags = new[] { "api/Magacin" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Insert([Required] MagacinInsertDTO magacin)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(magacin.Naziv))
                        return StatusCode(400, "Morate da prosledite naziv magacina");

                    if (magacin.Naziv.Length < MIN_CHARACTERS["NAZIV"])
                        return StatusCode(400, $"Naziv magacina mora biti duzi od {MIN_CHARACTERS["NAZIV"]} karaktera");

                    if (magacin.Naziv.Length > MAX_CHARACTERS["NAZIV"])
                        return StatusCode(400, $"Naziv magacina ne moze imati vise od {MAX_CHARACTERS["NAZIV"]} karaktera");

                    if (string.IsNullOrEmpty(magacin.Adresa))
                        return StatusCode(400, "Morate proslediti adresa magacina");

                    if (magacin.Adresa.Length < MIN_CHARACTERS["ADRESA"])
                        return StatusCode(400, $"Adresa mora biti duza od {MIN_CHARACTERS["ADRESA"]} karaktera");

                    if (magacin.Adresa.Length > MAX_CHARACTERS["ADRESA"])
                        return StatusCode(400, $"Adresa ne moze da sadrzi vise od {MAX_CHARACTERS["ADRESA"]} karaktera");

                    Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regexMail.Match(magacin.Email);

                    if (!match.Success)
                        return StatusCode(400, "Email Adresa nije validna");

                    if (string.IsNullOrEmpty(magacin.Telefon))
                        return StatusCode(400, "Morate proslediti telefon magacina");

                    if (magacin.Telefon.Length < 6)
                        return StatusCode(400, "Broj telefona nije validan");

                    if (magacin.Telefon[0] != '+' && magacin.Telefon[0] != '0')
                        return StatusCode(400, "Broj telefona nije validan, mora poceti sa 0 ili +");

                    if (magacin.Telefon.Length > 13)
                        return StatusCode(400, "Broj telefona nije validan");

                    if (string.IsNullOrEmpty(magacin.Koordinate))
                        return StatusCode(400, "Morate proslediti koordinate magacina");

                    if (string.IsNullOrEmpty(magacin.Grad))
                        return StatusCode(400, "Morate proslediti grad u kome se nalazi magacin");

                    if (magacin.Grad.Length < MIN_CHARACTERS["GRAD"])
                        return StatusCode(400, $"Grad ne moze imati manje od {MIN_CHARACTERS["GRAD"]} karaktera");

                    if (magacin.Grad.Length > MAX_CHARACTERS["GRAD"])
                        return StatusCode(400, $"Grad ne moze imati vise od {MAX_CHARACTERS["GRAD"]} karaktera");


                    lock (_____insertLock)
                    {
                        using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                        {
                            con.Open();
                            int id = -1;
                            using (MySqlCommand cmd = new MySqlCommand("SELECT COALESCE(MAX(ID),0) FROM MAGACIN", con))
                            {
                                using (MySqlDataReader dt = cmd.ExecuteReader())
                                    if (dt.Read())
                                        id = Convert.ToInt32(dt[0]);
                            }
                            if (id == -1)
                                return StatusCode(500);
                            using (MySqlCommand cmd = new MySqlCommand("INSERT INTO MAGACIN(ID, ADRESA, GRAD, EMAIL, KOORDINATE, TELEFON, NAZIV) VALUES(@ID, @ADRESA, @GRAD, @EMAIL, @KOORDINATE, @TELEFON, @NAZIV)", con))
                            {
                                cmd.Parameters.AddWithValue("@ID", id);
                                cmd.Parameters.AddWithValue("@ADRESA", magacin.Adresa);
                                cmd.Parameters.AddWithValue("@GRAD", magacin.Grad.ToLower());
                                cmd.Parameters.AddWithValue("@EMAIL", magacin.Email);
                                cmd.Parameters.AddWithValue("@KOORDINATE", magacin.Koordinate);
                                cmd.Parameters.AddWithValue("@TELEFON", magacin.Telefon);
                                cmd.Parameters.AddWithValue("@NAZIV", magacin.Naziv);
                                cmd.ExecuteNonQuery();
                            }
                            return StatusCode(201, id);
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
        /// Vraca odredjeni magacin po datom ID-om
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Magacin/Get")]
        [SwaggerOperation(Tags = new[] { "api/Magacin" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> Get([Required] int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT ID, ADRESA, GRAD, EMAIL, KOORDINATE, TELEFON, NAZIV FROM MAGACIN WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                                if (dt.Read())
                                    return StatusCode(200, new Models.Webshop.Magacin()
                                    {
                                        ID = Convert.ToInt32(dt["ID"]),
                                        Adresa = dt["ADRESA"].ToString(),
                                        Email = dt["EMAIL"].ToString(),
                                        Grad = dt["GRAD"].ToString(),
                                        Koordinate = dt["KOORDINATE"].ToString(),
                                        Telefon = dt["TELEFON"].ToString(),
                                        Naziv = dt["NAZIV"].ToString()
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
        /// Koriste update magacina
        /// </summary>
        /// <param name="magacin"></param>
        /// <returns></returns>
        /// <response code="200"></response>
        /// <response code="400">Kao response objekat vraca razlog loseg requesta</response>
        [HttpPost]
        [Route("/api/Magacin/Update")]
        [SwaggerOperation(Tags = new[] { "api/Magacin" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Update([Required] MagacinUpdateDTO magacin)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (string.IsNullOrEmpty(magacin.Naziv))
                        return StatusCode(400, "Morate da prosledite naziv magacina");

                    if (magacin.Naziv.Length < MIN_CHARACTERS["NAZIV"])
                        return StatusCode(400, $"Naziv magacina mora biti duzi od {MIN_CHARACTERS["NAZIV"]} karaktera");

                    if (magacin.Naziv.Length > MAX_CHARACTERS["NAZIV"])
                        return StatusCode(400, $"Naziv magacina ne moze imati vise od {MAX_CHARACTERS["NAZIV"]} karaktera");

                    if (string.IsNullOrEmpty(magacin.Adresa))
                        return StatusCode(400, "Morate proslediti adresa magacina");

                    if (magacin.Adresa.Length < MIN_CHARACTERS["ADRESA"])
                        return StatusCode(400, $"Adresa mora biti duza od {MIN_CHARACTERS["ADRESA"]} karaktera");

                    if (magacin.Adresa.Length > MAX_CHARACTERS["ADRESA"])
                        return StatusCode(400, $"Adresa ne moze da sadrzi vise od {MAX_CHARACTERS["ADRESA"]} karaktera");

                    Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match match = regexMail.Match(magacin.Email);

                    if (!match.Success)
                        return StatusCode(400, "Email Adresa nije validna");

                    if (string.IsNullOrEmpty(magacin.Telefon))
                        return StatusCode(400, "Morate proslediti telefon magacina");

                    if (magacin.Telefon.Length < 6)
                        return StatusCode(400, "Broj telefona nije validan");

                    if (magacin.Telefon[0] != '+' && magacin.Telefon[0] != '0')
                        return StatusCode(400, "Broj telefona nije validan, mora poceti sa 0 ili +");

                    if (magacin.Telefon.Length > 13)
                        return StatusCode(400, "Broj telefona nije validan");

                    if (string.IsNullOrEmpty(magacin.Koordinate))
                        return StatusCode(400, "Morate proslediti koordinate magacina");

                    if (string.IsNullOrEmpty(magacin.Grad))
                        return StatusCode(400, "Morate proslediti grad u kome se nalazi magacin");

                    if (magacin.Grad.Length < MIN_CHARACTERS["GRAD"])
                        return StatusCode(400, $"Grad ne moze imati manje od {MIN_CHARACTERS["GRAD"]} karaktera");

                    if (magacin.Grad.Length > MAX_CHARACTERS["GRAD"])
                        return StatusCode(400, $"Grad ne moze imati vise od {MAX_CHARACTERS["GRAD"]} karaktera");

                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("UPDATE MAGACIN SET ADRESA = @ADRESA, GRAD = @GRAD, EMAIL = @EMAIL, KOORDINATE = @KOORDINATE, TELEFON = @TELEFON WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", magacin.ID);
                            cmd.Parameters.AddWithValue("@ADRESA", magacin.Adresa);
                            cmd.Parameters.AddWithValue("@GRAD", magacin.Grad.ToLower());
                            cmd.Parameters.AddWithValue("@EMAIL", magacin.Email);
                            cmd.Parameters.AddWithValue("@KOORDINATE", magacin.Koordinate);
                            cmd.Parameters.AddWithValue("@TELEFON", magacin.Telefon);
                            cmd.ExecuteNonQuery();
                        }
                        return StatusCode(200);
                    }
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Vraca listu magacina
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/api/Magacin/List")]
        [SwaggerOperation(Tags = new[] { "api/Magacin" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> List(string grad, string adresa, string email, string koordinate, string naziv, string telefon)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    // Dictionary parametara koji ce se kasnije koristiti za izgradnju MySqlCommand.CommandText-a
                    Dictionary<string, string> parameters = new Dictionary<string, string>();
                    if (!string.IsNullOrEmpty(grad))
                        parameters.Add("GRAD", grad);
                    if (!string.IsNullOrEmpty(adresa))
                        parameters.Add("ADRESA", adresa);
                    if (!string.IsNullOrEmpty(email))
                        parameters.Add("EMAIL", email);
                    if (!string.IsNullOrEmpty(koordinate))
                        parameters.Add("KOORDINATE", koordinate);
                    if (!string.IsNullOrEmpty(naziv))
                        parameters.Add("NAZIV", naziv);
                    if (!string.IsNullOrEmpty(telefon))
                        parameters.Add("TELEFON", telefon);

                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        List<Models.Webshop.Magacin> list = new List<Models.Webshop.Magacin>();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT ID, ADRESA, GRAD, EMAIL, KOORDINATE, TELEFON, NAZIV FROM MAGACIN WHERE 1 = 1", con))
                        {
                            // Dodavanje parametara u command text kao i u Parameters Collection komande
                            foreach (string key in parameters.Keys)
                            {
                                cmd.CommandText += $" AND {key} = @{parameters[key]}";
                                cmd.Parameters.AddWithValue($"@{key}", parameters[key]);
                            }

                            using (MySqlDataReader dt = cmd.ExecuteReader())
                                while (dt.Read())
                                    list.Add(new Models.Webshop.Magacin
                                    {
                                        ID = Convert.ToInt32(dt["ID"]),
                                        Adresa = dt["ADRESA"].ToString(),
                                        Email = dt["EMAIL"].ToString(),
                                        Grad = dt["GRAD"].ToString(),
                                        Koordinate = dt["KOORDINATE"].ToString(),
                                        Telefon = dt["TELEFON"].ToString(),
                                        Naziv = dt["NAZIV"].ToString()
                                    });

                            if (list.Count == 0)
                                return StatusCode(204);

                            return StatusCode(200, list);
                        }

                    }
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        #region Getters & Setters
        #region Getters
        /// <summary>
        /// Getuje parametar iz baze
        /// </summary>
        /// <param name="magacinID"></param>
        /// <param name="parametar"></param>
        /// <returns></returns>
        private IActionResult GetParameter<T>(int magacinID, string parametar)
        {
            parametar = parametar.ToUpper();

            if (!VALID_PARAMETERS.Contains(parametar))
                throw new Exception("Parametar nije validan!");

            if (magacinID == 0)
                return StatusCode(400, "Neispravan magacinID!");

            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand($@"SELECT
                                {parametar}
                                FROM MAGACIN
                                WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", magacinID);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            object readData = dr[0];

                            return StatusCode(200, (T)Convert.ChangeType(readData, typeof(T)));
                        }

                        return StatusCode(404, "Magacin sa datim ID-em nije pronadjen!");
                    }
                }
            }
        }
        /// <summary>
        /// Getuje 'Adresa' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID magacina nije ispravan</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Adresa/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Magacin" })]
        public Task<IActionResult> AdresaGet([Required] int magacinID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(magacinID, "ADRESA");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'Grad' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID magacina nije ispravan</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Grad/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> GradGet([Required] int magacinID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(magacinID, "GRAD");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'Email' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID magacina nije ispravan</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Email/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> EmailGet([Required] int magacinID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(magacinID, "EMAIL");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'Koordinate' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID magacina nije ispravan</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Koordinate/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> KoordinateGet([Required] int magacinID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(magacinID, "KOORDINATE");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'Telefon' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID magacina nije ispravan</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Telefon/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> TelefoneGet([Required] int magacinID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(magacinID, "TELEFON");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'Naziv' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID magacina nije ispravan</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Naziv/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> NazivGet([Required] int magacinID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(magacinID, "NAZIV");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        #endregion

        // =========================================================
        // =========================================================
        // =========================================================

        #region Setters
        /// <summary>
        /// Setuje parametar u bazi
        /// </summary>
        /// <param name="magacinID"></param>
        /// <param name="parametar"></param>
        /// <param name="vrednost"></param>
        /// <returns></returns>
        private IActionResult SetujParametar(int magacinID, string parametar, object vrednost)
        {
            parametar = parametar.ToUpper();

            if (!VALID_PARAMETERS.Contains(parametar))
                throw new Exception("Parametar nije validan!");

            if (magacinID == 0)
                return StatusCode(400, "Neispravan magacinID!");

            if (vrednost is string)
            {
                if (string.IsNullOrWhiteSpace(vrednost.ToString()))
                    return StatusCode(400, "Morate proslediti vrednost!");

                if (MIN_CHARACTERS.ContainsKey(parametar) && vrednost.ToString().Length < MIN_CHARACTERS[parametar])
                    return StatusCode(400, $"Vrednost za parametar {parametar} mora biti duza od {MIN_CHARACTERS[parametar]} karaktera!");

                if (MAX_CHARACTERS.ContainsKey(parametar) && vrednost.ToString().Length < MAX_CHARACTERS[parametar])
                    return StatusCode(400, $"Vrednost za parametar ADRESA ne sme biti duza od {MAX_CHARACTERS[parametar]} karaktera!");
            }

            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT ID FROM MAGACIN WHERE ID = @ID", con))
                {
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                        if (!dr.Read())
                            return StatusCode(404, "Magacin sa datim ID-em nije pronadjen!");
                }

                using (MySqlCommand cmd = new MySqlCommand($@"UPDATE
                            MAGACIN
                            SET {parametar} = @VREDNOST
                            WHERE
                            ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@VREDNOST", vrednost);
                    cmd.Parameters.AddWithValue("@ID", magacinID);

                    cmd.ExecuteNonQuery();

                    return StatusCode(200);
                }
            }
        }
        /// <summary>
        /// Setuje 'Adresa' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Adresa/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Magacin" })]
        public Task<IActionResult> AdresaSet([Required] int magacinID, [Required] string vrednost)
        {
            return Task.Run(() =>
            {
                try
                {
                    return SetujParametar(magacinID, "ADRESA", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'Grad' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Grad/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Magacin" })]
        public Task<IActionResult> GradSet([Required] int magacinID, [Required] string vrednost)
        {
            return Task.Run(() =>
            {
                try
                {
                    return SetujParametar(magacinID, "GRAD", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'Email' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Email/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Magacin" })]
        public Task<IActionResult> EmailSet([Required] int magacinID, [Required] string vrednost)
        {
            return Task.Run(() =>
            {
                try
                {
                    return SetujParametar(magacinID, "EMAIL", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'Koordinate' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Koordinate/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Magacin" })]
        public Task<IActionResult> KoordinateSet([Required] int magacinID, [Required] string vrednost)
        {
            return Task.Run(() =>
            {
                try
                {
                    return SetujParametar(magacinID, "KOORDINATE", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'Telefon' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Telefon/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Magacin" })]
        public Task<IActionResult> TelefonSet([Required] int magacinID, [Required] string vrednost)
        {
            return Task.Run(() =>
            {
                try
                {
                    return SetujParametar(magacinID, "TELEFON", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'Naziv' parametar magacina
        /// </summary>
        /// <param name="magacinID">ID Magacina</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Magacin sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Magacin/Naziv/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Magacin" })]
        public Task<IActionResult> NazivSet([Required] int magacinID, [Required] string vrednost)
        {
            return Task.Run(() =>
            {
                try
                {
                    return SetujParametar(magacinID, "NAZIV", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        #endregion
        #endregion
    }
}