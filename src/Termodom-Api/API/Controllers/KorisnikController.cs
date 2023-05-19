using API.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    /// <summary>
    /// Koristi se za kontrolisanje korisnika
    /// </summary>
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private Dictionary<string, int> MAX_CHARACTERS = new Dictionary<string, int>()
        {
            { "IME", 64 },
            { "NADIMAK", 64 },
            { "KOMENTAR", 1024 },
            { "ADRESA_STANOVANJA", 64 },
            { "OPSTINA", 64 }
        };
        private Dictionary<string, int> MIN_CHARACTERS = new Dictionary<string, int>()
        {
            { "IME", 4 },
            { "NADIMAK", 4 },
            { "ADRESA_STANOVANJA", 4 },
            { "OPSTINA", 2 }
        };
        private string[] VALID_PARAMETERS = new string[21]
        {
            "IME", "PW", "TIP", "NADIMAK", "STATUS", "MOBILNI", "KOMENTAR", "MAIL", "POSETA",
            "ADRESA_STANOVANJA", "OPSTINA", "MAGACINID", "DATUM_RODJENJA", "PRIMA_OBAVESTENJA",
            "PPID", "REFERENT", "DATUM_KREIRANJA", "POSLAT_RODJENDANSKI_SMS", "DATUM_ODOBRENJA",
            "POSLEDNJI_PUT_VIDJEN", "PIB"
        };

        /// <summary>
        /// Kreira novog korisnika u bazi
        /// </summary>
        /// <param name="korisnik"></param>
        /// <returns></returns>
        /// <response code="201">Kao response objekat vraca ID novokreiraong korisnika</response>
        /// <response code="400">Kao response objekat vraca razlog loseg requesta</response>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Insert")]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Insert([Required] KorisnikInsertDTO korisnik)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(korisnik.Ime))
                        return StatusCode(400, "Morate da prosledite ime korisnika!");

                    if (korisnik.Ime.Length < MIN_CHARACTERS["IME"])
                        return StatusCode(400, $"Ime korisnika mora biti duze od {MIN_CHARACTERS["IME"]} karaktera!");

                    if (korisnik.Ime.Length > MAX_CHARACTERS["IME"])
                        return StatusCode(400, $"Ime korisnika mora biti krace od {MAX_CHARACTERS["IME"]} karaktera!");

                    if (string.IsNullOrWhiteSpace(korisnik.Nadimak))
                        return StatusCode(400, "Morate da prosledite nadimak korisnika!");

                    if (korisnik.Nadimak.Length < MIN_CHARACTERS["NADIMAK"])
                        return StatusCode(400, $"Nadimak korisnika mora biti duze od {MIN_CHARACTERS["NADIMAK"]} karaktera!");

                    if (korisnik.Nadimak.Length > MAX_CHARACTERS["NADIMAK"])
                        return StatusCode(400, $"Nadimak korisnika mora biti krace od {MAX_CHARACTERS["NADIMAK"]} karaktera!");

                    if (string.IsNullOrWhiteSpace(korisnik.AdresaStanovanja))
                        return StatusCode(400, "Morate da prosledite adresu stanovanja korisnika!");

                    if (korisnik.AdresaStanovanja.Length < MIN_CHARACTERS["ADRESA_STANOVANJA"])
                        return StatusCode(400, $"Adresa stanovanja korisnika mora biti duza od {MIN_CHARACTERS["ADRESA_STANOVANJA"]} karaktera!");

                    if (korisnik.AdresaStanovanja.Length > MAX_CHARACTERS["ADRESA_STANOVANJA"])
                        return StatusCode(400, $"Adresa stanovanja korisnika mora biti kraca od {MAX_CHARACTERS["ADRESA_STANOVANJA"]} karaktera!");

                    if (string.IsNullOrWhiteSpace(korisnik.Opstina))
                        return StatusCode(400, "Morate da prosledite optstinu korisnika!");

                    if (korisnik.Opstina.Length < MIN_CHARACTERS["OPSTINA"])
                        return StatusCode(400, $"Opstina korisnika mora biti duza od {MIN_CHARACTERS["OPSTINA"]} karaktera!");

                    if (korisnik.Opstina.Length > MAX_CHARACTERS["OPSTINA"])
                        return StatusCode(400, $"Opstina korisnika mora biti kraca od {MAX_CHARACTERS["OPSTINA"]} karaktera!");

                    if (!string.IsNullOrWhiteSpace(korisnik.Komentar) && korisnik.Komentar.Length > MAX_CHARACTERS["KOMENTAR"])
                        return StatusCode(400, $"Komentar korisnika mora biti kraci od {MAX_CHARACTERS["KOMENTAR"]} karaktera!");

                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand(@"INSERT INTO KORISNIK 
                        (
                            IME,
                            PW,
                            TIP,
                            NADIMAK,
                            STATUS,
                            MOBILNI,
                            KOMENTAR,
                            MAIL,
                            POSETA,
                            ADRESA_STANOVANJA,
                            OPSTINA,
                            MAGACINID,
                            DATUM_RODJENJA,
                            PRIMA_OBAVESTENJA,
                            PPID,
                            REFERENT,
                            DATUM_KREIRANJA,
                            POSLAT_RODJENDANSKI_SMS,
                            DATUM_ODOBRENJA,
                            POSLEDNJI_PUT_VIDJEN,
                            PIB
                        )
                        VALUES
                        (
                            @IME,
                            @PW,
                            @TIP,
                            @NADIMAK,
                            @STATUS,
                            @MOBILNI,
                            @KOMENTAR,
                            @MAIL,
                            @POSETA,
                            @ADRESA_STANOVANJA,
                            @OPSTINA,
                            @MAGACINID,
                            @DATUM_RODJENJA,
                            @PRIMA_OBAVESTENJA,
                            @PPID,
                            @REFERENT,
                            @DATUM_KREIRANJA,
                            @POSLAT_RODJENDANSKI_SMS,
                            @DATUM_ODOBRENJA,
                            @POSLEDNJI_PUT_VIDJEN,
                            @PIB
                        )", con))
                        {
                            cmd.Parameters.AddWithValue("@IME", korisnik.Ime);
                            cmd.Parameters.AddWithValue("@PW", HashPassword(korisnik.PW));
                            cmd.Parameters.AddWithValue("@TIP", (int)korisnik.Tip);
                            cmd.Parameters.AddWithValue("@NADIMAK", korisnik.Nadimak);
                            cmd.Parameters.AddWithValue("@STATUS", 0);
                            cmd.Parameters.AddWithValue("@MOBILNI", korisnik.Mobilni);
                            cmd.Parameters.AddWithValue("@KOMENTAR", korisnik.Komentar);
                            cmd.Parameters.AddWithValue("@MAIL", korisnik.Mail);
                            cmd.Parameters.AddWithValue("@POSETA", 0);
                            cmd.Parameters.AddWithValue("@ADRESA_STANOVANJA", korisnik.AdresaStanovanja);
                            cmd.Parameters.AddWithValue("@OPSTINA", korisnik.Opstina);
                            cmd.Parameters.AddWithValue("@MAGACINID", korisnik.MagacinID);
                            cmd.Parameters.AddWithValue("@DATUM_RODJENJA", korisnik.DatumRodjenja);
                            cmd.Parameters.AddWithValue("@PRIMA_OBAVESTENJA", 1);
                            cmd.Parameters.AddWithValue("@PPID", null);
                            cmd.Parameters.AddWithValue("@REFERENT", null);
                            cmd.Parameters.AddWithValue("@DATUM_KREIRANJA", DateTime.Now);
                            cmd.Parameters.AddWithValue("@POSLAT_RODJENDANSKI_SMS", 0);
                            cmd.Parameters.AddWithValue("@DATUM_ODOBRENJA", null);
                            cmd.Parameters.AddWithValue("@POSLEDNJI_PUT_VIDJEN", null);
                            cmd.Parameters.AddWithValue("@PIB", korisnik.PIB);

                            cmd.ExecuteNonQuery();
                            cmd.Cancel();

                        }

                        using (MySqlCommand cmd = new MySqlCommand("SELECT MAX(ID) FROM KORISNIK", con))
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                                if (dt.Read())
                                    return StatusCode(201, Convert.ToInt32(dt[0]));
                    }
                    return StatusCode(500);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Vraca korisnika sa zadatim ID-om ili imenom.
        /// Ne moze se zadati oba uslova odjednom.
        /// Mora biti zadat barem jedan uslov.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ime"></param>
        /// <returns></returns>
        /// <response code="200">Korisnik je pronadjen i vracen</response>
        /// <response code="204">Korisnik sa datim ID-om nije pronadjen</response>
        [HttpGet]
        [RequireBearer]
        [Route("/api/Korisnik/Get")]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> Get(int? id, string ime)
        {
            return Task.Run<IActionResult>(() =>
            {

                if (id == null && string.IsNullOrWhiteSpace(ime))
                    return StatusCode(400, "Morate proslediti ili ID ili Ime korisnika!");

                if (id > 0 && !string.IsNullOrWhiteSpace(ime))
                    return StatusCode(400, "Ne mozete proslediti i Ime i ID korisnika.");

                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();

                        using (MySqlCommand cmd = new MySqlCommand(@"SELECT
                                ID,
                                IME,
                                PW,
                                STATUS,
                                MOBILNI,
                                MAIL,
                                ADRESA_STANOVANJA,
                                OPSTINA,
                                DATUM_RODJENJA,
                                DATUM_KREIRANJA,
                                POSLAT_RODJENDANSKI_SMS,
                                PIB,
                                PPID
                                FROM KORISNIK
                                WHERE " + (id == 0 ? "IME" : "ID") + " = @PAR", con))
                        {
                            cmd.Parameters.AddWithValue("@PAR", id == 0 ? ime : id);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                if (dr.Read())
                                    return StatusCode(200, new Models.Korisnik()
                                    {
                                        ID = Convert.ToInt16(dr["ID"]),
                                        Ime = dr["IME"].ToString(),
                                        PW = dr["PW"].ToString(),
                                        Tip = (Models.KorisnikTip)Convert.ToInt32(dr["TIP"]),
                                        Status = Convert.ToInt16(dr["STATUS"]),
                                        Mobilni = dr["MOBILNI"] is DBNull ? null : dr["MOBILNI"].ToString(),
                                        Mail = dr["MAIL"] is DBNull ? null : dr["MAIL"].ToString(),
                                        AdresaStanovanja = dr["ADRESA_STANOVANJA"].ToStringOrDefault(),
                                        Opstina = dr["OPSTINA"].ToStringOrDefault(),
                                        DatumRodjenja = Convert.ToDateTime(dr["DATUM_RODJENJA"]),
                                        PPID = dr["PPID"] is DBNull ? null : (Int16?)Convert.ToInt32(dr["PPID"]),
                                        DatumKreiranja = Convert.ToDateTime(dr["DATUM_KREIRANJA"]),
                                        PoslatRodjendanskiSMS = Convert.ToInt16(dr["POSLAT_RODJENDANSKI_SMS"]) == 1,
                                        PIB = dr["PIB"].ToStringOrDefault()
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
        /// Azurira podatke korisnika u bazi po ID-u
        /// </summary>
        /// <param name="korisnik">Update DTO </param>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Update")]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> Update([Required] KorisnikUpdateDTO korisnik)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (string.IsNullOrWhiteSpace(korisnik.Ime))
                    return StatusCode(400, 4010);
                if (korisnik.Ime.Length > 64)
                    return StatusCode(400, 4011);
                if ((int)korisnik.Tip > sbyte.MaxValue)
                    return StatusCode(400, 4021);
                if ((int)korisnik.Tip < sbyte.MinValue)
                    return StatusCode(400, 4022);
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand(@"UPDATE KORISNIK SET
                            IME = @IME,
                            MOBILNI = @MOBILNI,
                            STATUS = @STATUS,
                            MAIL = @MAIL,
                            ADRESA_STANOVANJA = @ADRESA_STANOVANJA,
                            OPSTINA = @OPSTINA,
                            DATUM_RODJENJA = @DATUM_RODJENJA,
                            PIB = @PIB,
                            PPID = @PPID,
                            WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", korisnik.ID);
                            cmd.Parameters.AddWithValue("@IME", korisnik.Ime);
                            cmd.Parameters.AddWithValue("@STATUS", korisnik.Status);
                            cmd.Parameters.AddWithValue("@MOBILNI", korisnik.Mobilni);
                            cmd.Parameters.AddWithValue("@MAIL", korisnik.Mail);
                            cmd.Parameters.AddWithValue("@ADRESA_STANOVANJA", korisnik.AdresaStanovanja);
                            cmd.Parameters.AddWithValue("@OPSTINA", korisnik.Opstina);
                            cmd.Parameters.AddWithValue("@DATUM_RODJENJA", korisnik.DatumRodjenja);
                            cmd.Parameters.AddWithValue("@PPID", korisnik.PPID);
                            cmd.Parameters.AddWithValue("@PIB", korisnik.PIB);

                            cmd.ExecuteNonQuery();
                        }
                    }

                    return StatusCode(200);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Vraca listu svih korisnika
        /// </summary>
        /// <returns></returns>
        /// <response code="200"></response>
        [HttpGet]
        [RequireBearer]
        [Route("/api/Korisnik/List")]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> List()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        List<Models.Korisnik> list = new List<Models.Korisnik>();

                        using (MySqlCommand cmd = new MySqlCommand(@"SELECT
                                ID,
                                IME,
                                PW,
                                STATUS,
                                MOBILNI,
                                MAIL,
                                ADRESA_STANOVANJA,
                                OPSTINA,
                                DATUM_RODJENJA,
                                PPID,
                                REFERENT,
                                DATUM_KREIRANJA,
                                POSLAT_RODJENDANSKI_SMS,
                                PIB
                                FROM KORISNIK", con))
                        {
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                while (dr.Read())
                                {
                                    Models.Korisnik k = new Models.Korisnik();
                                    list.Add(new Models.Korisnik()
                                    {
                                        ID = Convert.ToInt16(dr["ID"]),
                                        Ime = dr["IME"].ToString(),
                                        PW = dr["PW"].ToString(),
                                        Tip = (Models.KorisnikTip)Convert.ToInt32(dr["TIP"]),
                                        Status = Convert.ToInt16(dr["STATUS"]),
                                        Mobilni = dr["MOBILNI"] is DBNull ? null : dr["MOBILNI"].ToString(),
                                        Mail = dr["MAIL"] is DBNull ? null : dr["MAIL"].ToString(),
                                        AdresaStanovanja = dr["ADRESA_STANOVANJA"].ToStringOrDefault(),
                                        Opstina = dr["OPSTINA"].ToStringOrDefault(),
                                        DatumRodjenja = Convert.ToDateTime(dr["DATUM_RODJENJA"]),
                                        PPID = dr["PPID"] is DBNull ? null : Convert.ToInt16(dr["PPID"]),
                                        DatumKreiranja = Convert.ToDateTime(dr["DATUM_KREIRANJA"]),
                                        PoslatRodjendanskiSMS = Convert.ToInt16(dr["POSLAT_RODJENDANSKI_SMS"]) == 1,
                                        PIB = dr["PIB"].ToStringOrDefault()
                                    });
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
        /// Verifikuje unete podatke sa podacima korisnika iz baze.
        /// Ukoliko su podaci ispravni vratice OK
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <response code="200">username i password su ispravni</response>
        /// <response code="401">username ili sifra nisu ispravni</response>
        [HttpPost]
        [Route("/api/Korisnik/Validate")]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public Task<IActionResult> Validate([Required] string username, [Required] string password)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();

                        using (MySqlCommand cmd = new MySqlCommand(@"SELECT
                                PW
                                FROM KORISNIK
                                WHERE IME = @NAME", con))
                        {
                            cmd.Parameters.AddWithValue("@NAME", username);
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                if (dr.Read())
                                    if (HashPassword(password) == dr[0].ToString())
                                        return StatusCode(200);
                        }
                    }
                }
                catch
                {
                    return StatusCode(500);
                }
                return StatusCode(401);
            });
        }

        /// <summary>
        /// Verifikuje unete podatke sa podacima korisnika iz baze.
        /// Ukoliko su podaci ispravni vratice Bearer Token.
        /// Korisnik mora biti admin.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        /// <response code="200">username i password su ispravni</response>
        /// <response code="401">username ili sifra nisu ispravni</response>
        [HttpPost]
        [Route("/api/Korisnik/GetToken")] // TODO: /api/Korisnik/GenerateToken
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public Task<IActionResult> GetToken([Required] string username, [Required] string password)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();

                        using (MySqlCommand cmd = new MySqlCommand(@"SELECT
                                ID,
                                PW
                                FROM KORISNIK
                                WHERE IME = @NAME", con))
                        {
                            cmd.Parameters.AddWithValue("@NAME", username);
                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                if (dr.Read())
                                    if (HashPassword(password) == dr["PW"].ToString())
                                    {
                                        string bearer = SimpleHash(Random.Next(Int32.MaxValue).ToString());

                                        while(Program.GetSessions().ContainsValue(bearer))
                                            bearer = SimpleHash(Random.Next(Int32.MaxValue).ToString());

                                        Program.AddSession(Convert.ToInt32(dr["ID"]), bearer);

                                        return StatusCode(200, bearer);
                                    }
                        }
                    }
                }
                catch
                {
                    return StatusCode(500);
                }
                return StatusCode(401);
            });
        }

        #region Parameter Getters & Setters

        #region Getters
        /// <summary>
        /// Getuje parametar iz baze
        /// </summary>
        /// <param name="korisnikID"></param>
        /// <param name="parametar"></param>
        /// <returns></returns>
        private IActionResult GetParameter<T>(int korisnikID, string parametar)
        {
            parametar = parametar.ToUpper();

            if (!VALID_PARAMETERS.Contains(parametar))
                throw new Exception("Parametar nije validan!");

            if (korisnikID == 0)
                return StatusCode(400, "Neispravan korisnikID!");

            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand($@"SELECT
                                {parametar}
                                FROM KORISNIK
                                WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", korisnikID);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            object readData = dr[0];

                            return StatusCode(200, (T)Convert.ChangeType(readData, typeof(T)));
                        }

                        return StatusCode(404, "Korisnik sa datim ID-em nije pronadjen!");
                    }
                }
            }
        }
        /// <summary>
        /// Getuje 'Ime' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Ime/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> ImeGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "IME");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'PW' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/PW/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> PWGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "PW");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'Tip' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Tip/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> TipGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "Tip");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'Status' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Status/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> StatusGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "STATUS");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'Mobilni' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Mobilni/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> MobilniGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "MOBILNI");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'Mail' parametar korinsika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Mail/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> MailGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "MAIL");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'AdresaStanovanja' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/AdresaStanovanja/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> AdresaStanovanjaGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "ADRESA_STANOVANJA");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'Opstina' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Opstina/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> OpstinaGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "OPSTINA");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'DatumRodjenja' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/DatumRodjenja/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> DatumRodjenjaGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "DATUM_ROJDENJA");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'PPID' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/PPID/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> PPIDGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "PPID");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'PoslatRodjendanskiSMS' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/PoslatRodjendanskiSMS/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> PoslatRodjendanskiSMSGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "POSLAT_RODJENDANSKI_SMS");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Getuje 'PIB' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <response code="200">Parametar uspesno nadjen</response>
        /// <response code="400">ID korisnika nije ispravan</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/PIB/Get")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> PIBGet([Required] int korisnikID)
        {
            return Task.Run(() =>
            {
                try
                {
                    return GetParameter<string>(korisnikID, "PIB");
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        #endregion

        // ===================================================================================
        // ===================================================================================
        // ===================================================================================

        #region Setters
        /// <summary>
        /// Setuje parametar u bazi
        /// </summary>
        /// <param name="korisnikID"></param>
        /// <param name="parametar"></param>
        /// <param name="vrednost"></param>
        /// <returns></returns>
        private IActionResult SetujParametar(int korisnikID, string parametar, object vrednost)
        {
            parametar = parametar.ToUpper();

            if (!VALID_PARAMETERS.Contains(parametar))
                throw new Exception("Parametar nije validan!");

            if (korisnikID == 0)
                return StatusCode(400, "Neispravan korisnikID!");

            if (vrednost is string)
            {
                if (string.IsNullOrWhiteSpace(vrednost.ToString()))
                    return StatusCode(400, "Morate proslediti vrednost!");

                if (MIN_CHARACTERS.ContainsKey(parametar) && vrednost.ToString().Length < MIN_CHARACTERS[parametar])
                    return StatusCode(400, $"Vrednost za parametar {parametar} mora biti duza od {MIN_CHARACTERS[parametar]} karaktera!");

                if (MAX_CHARACTERS.ContainsKey(parametar) && vrednost.ToString().Length < MAX_CHARACTERS[parametar])
                    return StatusCode(400, $"Vrednost za parametar {parametar} ne sme biti duza od {MAX_CHARACTERS[parametar]} karaktera!");
            }

            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();

                using (MySqlCommand cmd = new MySqlCommand("SELECT ID FROM KORISNIK WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", korisnikID);
                    using (MySqlDataReader dr = cmd.ExecuteReader())
                        if (!dr.Read())
                            return StatusCode(404, "Korisnik sa datim ID-em nije pronadjen!");
                }

                using (MySqlCommand cmd = new MySqlCommand($@"UPDATE
                            KORISNIK
                            SET {parametar} = @VREDNOST
                            WHERE
                            ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@VREDNOST", vrednost);
                    cmd.Parameters.AddWithValue("@ID", korisnikID);

                    cmd.ExecuteNonQuery();

                    return StatusCode(200);
                }
            }
        }
        /// <summary>
        /// Setuje 'Ime' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Ime/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> ImeSet([Required] int korisnikID, [Required] string vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "IME", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'PW' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/PW/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> PWSet([Required] int korisnikID, [Required] string vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "PW", HashPassword(vrednost));
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'Tip' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Tip/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> TipSet([Required] int korisnikID, [Required] int vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "TIP", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'Status' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Status/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> StatusSet([Required] int korisnikID, [Required] int vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "STATUS", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'Mobilni' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Mobilni/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> MobilniSet([Required] int korisnikID, [Required] string vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "MOBILNI", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'Mail' parametar korinsika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Mail/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> MailSet([Required] int korisnikID, [Required] string vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "MAIL", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'AdresaStanovanja' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/AdresaStanovanja/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> AdresaStanovanjaSet([Required] int korisnikID, [Required] string vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "ADRESA_STANOVANJA", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'Opstina' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/Opstina/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> OpstinaSet([Required] int korisnikID, [Required] string vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "OPSTINA", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'DatumRodjenja' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/DatumRodjenja/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> DatumRodjenjaSet([Required] int korisnikID, [Required] DateTime vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "DATUM_RODJENJA", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'PPID' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/PPID/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> PPIDSet([Required] int korisnikID, [Required] int? vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "PPID", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'PoslatRodjendanskiSMS' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <response code="200">Parametar uspesno setovan</response>
        /// <response code="400">Neki od parametra nije ispravan. Pogledaj Status Objekat</response>
        /// <response code="404">Korisnik sa datim ID-em nije pronadjen</response>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/PoslatRodjendanskiSMS/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> PoslatRodjendanskiSMSSet([Required] int korisnikID, [Required] bool vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "POSLAT_RODJENDANSKI_SMS", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Setuje 'PIB' parametar korisnika
        /// </summary>
        /// <param name="korisnikID">ID Korisnika</param>
        /// <param name="vrednost">Nova vrednost parametra</param>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/api/Korisnik/PIB/Set")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status501NotImplemented)]
        [SwaggerOperation(Tags = new[] { "api/Korisnik" })]
        public Task<IActionResult> PIBSet([Required] int korisnikID, [Required] string vrednost)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    return SetujParametar(korisnikID, "PIB", vrednost);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }
        #endregion

        #endregion
        #region Password Hash
        private static string SimpleHash(string value)
        {
            HashAlgorithm algorithm = SHA256.Create();
            byte[] res = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));
            StringBuilder sb = new StringBuilder();
            foreach (byte b in res)
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        private static string HashPassword(string RawPassword)
        {
            return SimpleHash(SimpleHash(SimpleHash(SimpleHash(SimpleHash(SimpleHash(RawPassword))))));
        }
        #endregion
    }

}
