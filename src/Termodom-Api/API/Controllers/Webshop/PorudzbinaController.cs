using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers.Webshop
{
    /// <summary>
    /// Koristi se za kontrolisanje porudzbina
    /// </summary>
    [ApiController]
    [RequireBearer]

    public class PorudzbinaController : ControllerBase
    {
        private static object _insertLock = new object();
        /// <summary>
        /// Azurira podatke porudzbine u bazi po zadatom ID-u
        /// </summary>
        /// <returns></returns>
        /// <response code="404">Porudzbina sa ID-om nije pronadjena</response>
        [HttpPost]
        [Route("/Webshop/Porudzbina/Update")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Porudzbina" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public Task<IActionResult> Update([Required] DTO.Webshop.PorudzbinaUpdateDTO porudzbina)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(ID) FROM PORUDZBINA WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", porudzbina.ID);
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                                if (dt.Read())
                                    if (Convert.ToInt32(dt[0]) == 0)
                                        return StatusCode(404);
                        }

                        using (MySqlCommand cmd = new MySqlCommand(@"UPDATE PORUDZBINA SET 
                        KORISNIK_ID = @KORISNIK_ID, 
                        BR_DOK_KOMERCIJALNO = @BR_DOK_KOMERCIJALNO,
                        DATUM = @DATUM, 
                        STATUS = @STATUS, 
                        MAGACIN_ID = @MAGACIN_ID, 
                        PPID = @PPID, 
                        INTERNI_KOMENTAR = @INTERNI_KOMENTAR, 
                        REFERENT_OBRADE = @REFERENT_OBRADE, 
                        NACIN_PLACANJA = @NACIN_PLACANJA,
                        HASH = @HASH, 
                        K = @K, 
                        USTEDA_KORISNIK = @USTEDA_KORISNIKA, 
                        USTEDA_KLIJENT = @USTEDA_KLIJENTA,
                        DOSTAVA = @DOSTAVA, 
                        KOMERCIJALNO_INTERNI_KOMENTAR = @KOMERCIJALNO_INTERNI_KOMENTAR, 
                        KOMERCIJALNO_KOMENTAR = @KOMERICJLNO_KOMENTAR, 
                        KOMERCIJALNO_NAPOMENA = @KOMERCIJALNO_NAPOMENA, 
                        NAPOMENA = @NAPOMENA,
                        ADRESA_ISPORUKE = @ADRESA_ISPORUKE, 
                        KONTAKT_MOBILNI = @KONTAKT_MOBILNI, 
                        IME_I_PREZIME = @IME_I_PREZIME WHERE ID = @I", con))
                        {
                            cmd.Parameters.AddWithValue("@I", porudzbina.ID);
                            cmd.Parameters.AddWithValue("@KORISNIK_ID", porudzbina.KorisnikID);
                            cmd.Parameters.AddWithValue("@BR_DOK_KOMERCIJALNO", porudzbina.BrDokKomercijalno);
                            cmd.Parameters.AddWithValue("@DATUM", porudzbina.Datum);
                            cmd.Parameters.AddWithValue("@STATUS", porudzbina.Status);
                            cmd.Parameters.AddWithValue("@MAGACIN_ID", porudzbina.MagacinID);
                            cmd.Parameters.AddWithValue("@PPID", porudzbina.PPID);
                            cmd.Parameters.AddWithValue("@INTERNI_KOMENTAR", porudzbina.InterniKomentar);
                            cmd.Parameters.AddWithValue("@NACIN_PLACANJA", porudzbina.NacinPlacanja);
                            cmd.Parameters.AddWithValue("@HASH", porudzbina.Hash);
                            cmd.Parameters.AddWithValue("@K", porudzbina.K);
                            cmd.Parameters.AddWithValue("@USTEDA_KORISNIKA", porudzbina.UstedaKorisnik);
                            cmd.Parameters.AddWithValue("@USTEDA_KLIJENTA", porudzbina.UstedaKlijent);
                            cmd.Parameters.AddWithValue("@DOSTAVA", porudzbina.Dostava);
                            cmd.Parameters.AddWithValue("@KOMERCIJALNO_INTERNI_KOMENTAR", porudzbina.KomercijalnoInterniKomentar);
                            cmd.Parameters.AddWithValue("@KOMERCIJALNO_KOMENTAR", porudzbina.KomercijalnoKomentar);
                            cmd.Parameters.AddWithValue("@KOMERCIJALNO_NAPOMENA", porudzbina.KomercijalnoNapomena);
                            cmd.Parameters.AddWithValue("@NAPOMENA", porudzbina.Napomena);
                            cmd.Parameters.AddWithValue("@ADRESA_ISPORUKE", porudzbina.AdresaIsporuke);
                            cmd.Parameters.AddWithValue("@KONTAKT_MOBILNI", porudzbina.KontaktMobilni);
                            cmd.Parameters.AddWithValue("@IME_I_PREZIME", porudzbina.ImeIPrezime);
                            cmd.Parameters.AddWithValue("@REFERENT_OBRADE", porudzbina.ReferentObrade);
                            cmd.Parameters.AddWithValue("@KOMERICJLNO_KOMENTAR", porudzbina.KomercijalnoKomentar);
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
        /// Kreira novu porudzbinu u bazi i vraca hash novokreirane porudzbine
        /// </summary>
        /// <returns>HASH nove porudzbine</returns>
        [HttpPost]
        [Route("/Webshop/Porudzbina/Insert")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Porudzbina" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Insert([Required]DTO.Webshop.PorudzbinaInsertDTO porudzbina)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (porudzbina == null)
                    return StatusCode(400);
                try
                {
                    lock (_insertLock)
                    {
                        int id = 0;
                        using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                        {
                            con.Open();
                            using (MySqlCommand cmd = new MySqlCommand("SELECT COALESCE(MAX(ID), 0) FROM PORUDZBINA", con))
                            {
                                using (MySqlDataReader dr = cmd.ExecuteReader())
                                    if(dr.Read())
                                        id = Convert.ToInt32(dr[0]) + 1;
                            }
                            string hash = Extensions.Hash(id.ToString());
                            using (MySqlCommand cmd = new MySqlCommand(@"INSERT INTO PORUDZBINA(ID, KORISNIK_ID, BR_DOK_KOMERCIJALNO, DATUM, STATUS, 
                            MAGACIN_ID, PPID, INTERNI_KOMENTAR, REFERENT_OBRADE, NACIN_PLACANJA, HASH, K, USTEDA_KORISNIK, USTEDA_KLIJENT,
                            DOSTAVA, KOMERCIJALNO_INTERNI_KOMENTAR, KOMERCIJALNO_KOMENTAR, KOMERCIJALNO_NAPOMENA, NAPOMENA,
                            ADRESA_ISPORUKE, KONTAKT_MOBILNI, IME_I_PREZIME) VALUES(@ID, @KORISNIK_ID, @BR_DOK_KOMERCIJALNO, @DATUM,
                            @STATUS, @MAGACIN_ID, @PPID, @INTERNI_KOMENTAR, @REFERENT_OBRADE, @NACIN_PLACANJA, @HASH, @K,
                            @USTEDA_KORISNIK, @USTEDA_KLIJENT, @DOSTAVA, @KOMERCIJALNO_INTERNI_KOMENTAR, @KOMERCIJALNO_KOMENTAR, @KOMERCIJALNO_NAPOMENA,
                            @NAPOMENA, @ADRESA_ISPORUKE, @KONTAKT_MOBILNI, @IME_I_PREZIME)", con))
                            {
                                cmd.Parameters.AddWithValue("@ID", id);
                                cmd.Parameters.AddWithValue("@KORISNIK_ID", porudzbina.KorisnikID);
                                cmd.Parameters.AddWithValue("@BR_DOK_KOMERCIJALNO", porudzbina.BrDokKomercijalno);
                                cmd.Parameters.AddWithValue("@DATUM", porudzbina.Datum);
                                cmd.Parameters.AddWithValue("@STATUS", porudzbina.Status);
                                cmd.Parameters.AddWithValue("@MAGACIN_ID", porudzbina.MagacinID);
                                cmd.Parameters.AddWithValue("@PPID", porudzbina.PPID);
                                cmd.Parameters.AddWithValue("@INTERNI_KOMENTAR", porudzbina.InterniKomentar);
                                cmd.Parameters.AddWithValue("@REFERENT_OBRADE", porudzbina.ReferentObrade);
                                cmd.Parameters.AddWithValue("@NACIN_PLACANJA", porudzbina.NacinPlacanja);
                                cmd.Parameters.AddWithValue("@HASH", hash);
                                cmd.Parameters.AddWithValue("@K", porudzbina.K);
                                cmd.Parameters.AddWithValue("@USTEDA_KORISNIK", porudzbina.UstedaKorisnik);
                                cmd.Parameters.AddWithValue("@USTEDA_KLIJENT", porudzbina.UstedaKlijent);
                                cmd.Parameters.AddWithValue("@DOSTAVA", porudzbina.Dostava);
                                cmd.Parameters.AddWithValue("@KOMERCIJALNO_INTERNI_KOMENTAR", porudzbina.KomercijalnoInterniKomentar);
                                cmd.Parameters.AddWithValue("@KOMERCIJALNO_KOMENTAR", porudzbina.KomercijalnoKomentar);
                                cmd.Parameters.AddWithValue("@KOMERCIJALNO_NAPOMENA", porudzbina.KomercijalnoNapomena);
                                cmd.Parameters.AddWithValue("@NAPOMENA", porudzbina.Napomena);
                                cmd.Parameters.AddWithValue("@ADRESA_ISPORUKE", porudzbina.AdresaIsporuke);
                                cmd.Parameters.AddWithValue("@KONTAKT_MOBILNI", porudzbina.KontaktMobilni);
                                cmd.Parameters.AddWithValue("@IME_I_PREZIME", porudzbina.ImeIPrezime);
                                cmd.ExecuteNonQuery();
                            }
                            return StatusCode(201, hash);
                        }
                    }
                }
                catch(Exception ex)
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Vraca objekat porudzbine sa zadatim ID-em
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/Webshop/Porudzbina/Get")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Porudzbina" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Get([Required] int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (id <= 0)
                        return StatusCode(400);
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand(@"SELECT ID, KORISNIK_ID, BR_DOK_KOMERCIJALNO, DATUM, STATUS, 
                            MAGACIN_ID, PPID, INTERNI_KOMENTAR, REFERENT_OBRADE, NACIN_PLACANJA, HASH, K, USTEDA_KORISNIK, USTEDA_KLIJENT,
                            DOSTAVA, KOMERCIJALNO_INTERNI_KOMENTAR, KOMERCIJALNO_KOMENTAR, KOMERCIJALNO_NAPOMENA, NAPOMENA,
                            ADRESA_ISPORUKE, KONTAKT_MOBILNI, IME_I_PREZIME FROM PORUDZBINA WHERE ID = @I", con))
                        {
                            cmd.Parameters.AddWithValue("@I", id);
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                            {
                                if (dt.Read())
                                {
                                    return StatusCode(200, new Models.Webshop.Porudzbina()
                                    {
                                        ID = Convert.ToInt32(dt["ID"]),
                                        KorisnikID = Convert.ToInt32(dt["KORISNIK_ID"]),
                                        BrDokKomercijalno = dt["BR_DOK_KOMERCIJALNO"] is DBNull ? null : Convert.ToInt32(dt["BR_DOK_KOMERCIJALNO"]),
                                        Datum = DateTime.SpecifyKind(Convert.ToDateTime(dt["DATUM"]), DateTimeKind.Utc),
                                        Status = Convert.ToInt32(dt["STATUS"]),
                                        MagacinID = Convert.ToInt32(dt["MAGACIN_ID"]),
                                        PPID = dt["PPID"] is DBNull ? null : Convert.ToInt32(dt["PPID"]),
                                        InterniKomentar = dt["INTERNI_KOMENTAR"].ToString(),
                                        ReferentObrade = dt["REFERENT_OBRADE"] is DBNull ? null : Convert.ToInt32(dt["REFERENT_OBRADE"]),
                                        NacinPlacanja = Convert.ToInt32(dt["NACIN_PLACANJA"]),
                                        Hash = dt["HASH"].ToString(),
                                        K = Convert.ToDouble(dt["K"]),
                                        UstedaKorisnik = Convert.ToDouble(dt["USTEDA_KORISNIK"]),
                                        UstedaKlijent = Convert.ToDouble(dt["USTEDA_KLIJENT"]),
                                        Dostava = Convert.ToInt32(dt["DOSTAVA"]),
                                        KomercijalnoInterniKomentar = dt["KOMERCIJALNO_INTERNI_KOMENTAR"].ToString(),
                                        KomercijalnoKomentar = dt["KOMERCIJALNO_KOMENTAR"].ToString(),
                                        KomercijalnoNapomena = dt["KOMERCIJALNO_NAPOMENA"].ToString(),
                                        Napomena = dt["NAPOMENA"].ToString(),
                                        AdresaIsporuke = dt["ADRESA_ISPORUKE"].ToString(),
                                        KontaktMobilni = dt["KONTAKT_MOBILNI"].ToString(),
                                        ImeIPrezime = dt["IME_I_PREZIME"].ToString()
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
        /// Vraca listu svih porudzbina
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/Webshop/Porudzbina/List")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Porudzbina" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> List()
        {
            return Task.Run<IActionResult>(() =>
            {
                List<Models.Webshop.Porudzbina> list = new List<Models.Webshop.Porudzbina>();
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand(@"SELECT ID, KORISNIK_ID, BR_DOK_KOMERCIJALNO, DATUM, STATUS, 
                    MAGACIN_ID, PPID, INTERNI_KOMENTAR, REFERENT_OBRADE, NACIN_PLACANJA, HASH, K, USTEDA_KORISNIK, USTEDA_KLIJENT,
                    DOSTAVA, KOMERCIJALNO_INTERNI_KOMENTAR, KOMERCIJALNO_KOMENTAR, KOMERCIJALNO_NAPOMENA, NAPOMENA,
                    ADRESA_ISPORUKE, KONTAKT_MOBILNI, IME_I_PREZIME FROM PORUDZBINA ORDER BY DATUM DESC LIMIT 1000", con))
                        {
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                            {
                                while (dt.Read())
                                    list.Add(new Models.Webshop.Porudzbina()
                                    {
                                        ID = Convert.ToInt32(dt["ID"]),
                                        KorisnikID = Convert.ToInt32(dt["KORISNIK_ID"]),
                                        BrDokKomercijalno = dt["BR_DOK_KOMERCIJALNO"] is DBNull ? null : Convert.ToInt32(dt["BR_DOK_KOMERCIJALNO"]),
                                        Datum = DateTime.SpecifyKind(Convert.ToDateTime(dt["DATUM"]), DateTimeKind.Utc),
                                        Status = Convert.ToInt32(dt["STATUS"]),
                                        MagacinID = Convert.ToInt32(dt["MAGACIN_ID"]),
                                        PPID = dt["PPID"] is DBNull ? null : Convert.ToInt32(dt["PPID"]),
                                        InterniKomentar = dt["INTERNI_KOMENTAR"].ToString(),
                                        ReferentObrade = dt["REFERENT_OBRADE"] is DBNull ? null : Convert.ToInt32(dt["REFERENT_OBRADE"]),
                                        NacinPlacanja = Convert.ToInt32(dt["NACIN_PLACANJA"]),
                                        Hash = dt["HASH"].ToString(),
                                        K = Convert.ToDouble(dt["K"]),
                                        UstedaKorisnik = Convert.ToDouble(dt["USTEDA_KORISNIK"]),
                                        UstedaKlijent = Convert.ToDouble(dt["USTEDA_KLIJENT"]),
                                        Dostava = Convert.ToInt32(dt["DOSTAVA"]),
                                        KomercijalnoInterniKomentar = dt["KOMERCIJALNO_INTERNI_KOMENTAR"].ToString(),
                                        KomercijalnoKomentar = dt["KOMERCIJALNO_KOMENTAR"].ToString(),
                                        KomercijalnoNapomena = dt["KOMERCIJALNO_NAPOMENA"].ToString(),
                                        Napomena = dt["NAPOMENA"].ToString(),
                                        AdresaIsporuke = dt["ADRESA_ISPORUKE"].ToString(),
                                        KontaktMobilni = dt["KONTAKT_MOBILNI"].ToString(),
                                        ImeIPrezime = dt["IME_I_PREZIME"].ToString()
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
        /// Brisanje porudzbine po hashu, koristimo za automatske testove
        /// </summary>
        /// <param name="hash"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Webshop/Porudzbina/Remove")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Porudzbina" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public Task<IActionResult> Remove(string hash, string mobilni)
        {
            return Task.Run<IActionResult>(() =>
            {
                int id = -1;
                try
                {
                    if (string.IsNullOrEmpty(hash) || string.IsNullOrEmpty(mobilni))
                        return StatusCode(400, "Morate proslediti hash ili mobilni");

                    if (!string.IsNullOrEmpty(hash) || !string.IsNullOrEmpty(mobilni))
                        return StatusCode(400, "Mozete proslediti samo jedan parametar");

                    string query = string.IsNullOrEmpty(hash) ? " KONTAKT_MOBILNI = @MOBILNI" : " HASH = @HASH";
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand(@"SELECT ID FROM PORUDZBINA WHERE" + query, con))
                        {
                            if (string.IsNullOrEmpty(hash))
                                cmd.Parameters.AddWithValue("@MOBILNI", mobilni);
                            else
                                cmd.Parameters.AddWithValue("@HASH", hash);
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                            {
                                if (dt.Read())
                                    id = Convert.ToInt32(dt[0]);
                            }
                        }
                        if (id == -1)
                            return StatusCode(400, "Porudzbina nije pronadjena");

                        using (MySqlCommand cmd = new MySqlCommand("DELETE FROM STAVKA WHERE PORUDZBINA_ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.ExecuteNonQuery();
                        }
                        using (MySqlCommand cmd = new MySqlCommand("DELETE FROM PORUDZBINA WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
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
        /// 
        /// </summary>
        /// <param name="porudzbinaid"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Webshop/Porudzbina/Status/Set")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Porudzbina" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult>StatusSet([Required] int porudzbinaid, [Required] int status)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(ID) FROM PORUDZBINA WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", porudzbinaid);
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                                if (dt.Read())
                                    if (Convert.ToInt32(dt[0]) == 0)
                                        return StatusCode(404);
                        }

                        using (MySqlCommand cmd = new MySqlCommand(@"UPDATE PORUDZBINA SET 
                        STATUS = @STATUS
                        WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", porudzbinaid);
                            cmd.Parameters.AddWithValue("@STATUS", status);
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
        /// 
        /// </summary>
        /// <param name="porudzbinaid"></param>
        /// <param name="brDokKomercijalno"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/Webshop/Porudzbina/BrDokKomercijalno/Set")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Porudzbina" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> BrDokKomercijalnoSet([Required] int porudzbinaid, [Required] int brDokKomercijalno)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(ID) FROM PORUDZBINA WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", porudzbinaid);
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                                if (dt.Read())
                                    if (Convert.ToInt32(dt[0]) == 0)
                                        return StatusCode(404);
                        }

                        using (MySqlCommand cmd = new MySqlCommand(@"UPDATE PORUDZBINA SET 
                        BR_DOK_KOMERCIJALNO = @BR_DOK_KOMERCIJALNO
                        WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", porudzbinaid);
                            cmd.Parameters.AddWithValue("@BR_DOK_KOMERCIJALNO", brDokKomercijalno);
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

    }
}
