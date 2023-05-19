using System;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Swashbuckle.AspNetCore.Annotations;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using API.Models.Webshop;
using Newtonsoft.Json;
using System.Data;
using System.Net.Mime;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers.Webshop
{
    /// <summary>
    /// Koristi se za kontrolisanje tipa korisnika
    /// </summary>
    [ApiController]
    public class KorisnikController : ControllerBase
    {
        private static double OD_UKUPNE_RAZLIKE_NAMA_OSTAJE_SIGURNIH = 0.25;
        private static int nProfiCenovnikNivoa { get; set; } = 4;
        private static object _lock { get; set; } = new object();

        [HttpGet]
        [Route("/Webshop/Korisnik/Cenovnik/Profi/Get")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Korisnik"})]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public Task<IActionResult> CenovnikProfiGet()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    int korisnikID = 462;
                    if (korisnikID == 0)
                        return StatusCode(401);

                    Models.Cenovnik c = new Models.Cenovnik();
                    List<Models.Webshop.Proizvod> proizvodi = new List<Models.Webshop.Proizvod>();
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand(@"SELECT ROBAID, SLIKA, PODGRUPAID, AKTIVAN, PDV, DISPLAY_INDEX, KRATAK_OPIS,
                                POSETA, RASPRODAJA, KEYWORDS, NABAVNA_CENA, PRODAJNA_CENA, REL, DETALJAN_OPIS, KLASIFIKACIJA,
                                TRANSPORTNO_PAKOVANJE, TRANSPORTNO_PAKOVANJE_JM, KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU, ISTAKNUTI_PROIZVODI,
                                UPOZORENJE_ZALIHA_MALIH_STOVARISTA, CENOVNA_GRUPA_ID, POVEZANI_PROIZVODI, AKTUELNI_RABAT, SUBGROUPS, PARENT,
                                BREND_ID FROM PROIZVOD", con))
                        using (MySqlDataReader dr = cmd.ExecuteReader())
                            while (dr.Read())
                                proizvodi.Add(new Models.Webshop.Proizvod()
                                {
                                    RobaID = Convert.ToInt32(dr["ROBAID"]),
                                    Slika = dr["SLIKA"].ToString(),
                                    PodgrupaID = Convert.ToInt32(dr["PODGRUPAID"]),
                                    Aktivan = Convert.ToInt16(dr["AKTIVAN"]),
                                    PDV = Convert.ToDouble(dr["PDV"]),
                                    DisplayIndex = Convert.ToInt32(dr["DISPLAY_INDEX"]),
                                    KratakOpis = dr["KRATAK_OPIS"].ToString(),
                                    Poseta = Convert.ToInt32(dr["POSETA"]),
                                    Rasprodaja = Convert.ToInt16(dr["RASPRODAJA"]),
                                    Keywords = dr["KEYWORDS"] is DBNull ? null : dr["KEYWORDS"].ToString(),
                                    NabavnaCena = Convert.ToDouble(dr["NABAVNA_CENA"]),
                                    ProdajnaCena = Convert.ToDouble(dr["PRODAJNA_CENA"]),
                                    Rel = dr["REL"].ToString(),
                                    DetaljanOpis = dr["DETALJAN_OPIS"] is DBNull ? null : dr["DETALJAN_OPIS"].ToString(),
                                    Klasifikacija = Convert.ToInt16(dr["KLASIFIKACIJA"]),
                                    TransportnoPakovanje = dr["TRANSPORTNO_PAKOVANJE"] is DBNull ? 0 : Convert.ToDouble(dr["TRANSPORTNO_PAKOVANJE"]),
                                    TransportnoPakovanjeJM = dr["TRANSPORTNO_PAKOVANJE_JM"] is DBNull ? null : dr["TRANSPORTNO_PAKOVANJE_JM"].ToString(),
                                    KupovinaSamoUTransportnomPakovanju = Convert.ToInt16(dr["KUPOVINA_SAMO_U_TRANSPORTNOM_PAKOVANJU"]),
                                    IstaknutiProizvodi = dr["ISTAKNUTI_PROIZVODI"] is DBNull ? null : dr["ISTAKNUTI_PROIZVODI"].ToString(),
                                    UpozorenjeZalihaMalihStovarista = Convert.ToInt16(dr["UPOZORENJE_ZALIHA_MALIH_STOVARISTA"]),
                                    CenovnaGrupaID = Convert.ToInt16(dr["CENOVNA_GRUPA_ID"]),
                                    PovezaniProizvodi = dr["POVEZANI_PROIZVODI"] is DBNull ? null : dr["POVEZANI_PROIZVODI"].ToString(),
                                    AktuelniRabat = Convert.ToDouble(dr["AKTUELNI_RABAT"]),
                                    Podgrupe = dr["SUBGROUPS"] is DBNull ? new List<int>() : JsonConvert.DeserializeObject<List<int>>(dr["SUBGROUPS"].ToString()),
                                    Parent = Convert.ToInt32(dr["PARENT"]),
                                    BrendID = Convert.ToInt32(dr["BREND_ID"])
                                });
                    }

                    List<Tuple<int, int>> uslovi = new List<Tuple<int, int>>();

                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT NIVO, CENOVNIK_GRUPAID FROM USER_CENOVNIK WHERE USERID = @UID", con))
                        {
                            cmd.Parameters.AddWithValue("@UID", korisnikID);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                while (dr.Read())
                                    uslovi.Add(new Tuple<int, int>(Convert.ToInt32(dr["CENOVNIK_GRUPAID"]), Convert.ToInt32(dr["NIVO"])));
                        }

                    }

                    foreach (Models.Webshop.Proizvod proizvod in proizvodi)
                    {
                        double minCena = proizvod.NabavnaCena;
                        double maxCena = proizvod.ProdajnaCena;
                        double razlika = (maxCena - minCena) * (1 - OD_UKUPNE_RAZLIKE_NAMA_OSTAJE_SIGURNIH);
                        double namaOstaje = razlika;

                        Tuple<int, int> korisnikovCenovniUslovZaOvajProizvod = null;
                        try
                        {
                            korisnikovCenovniUslovZaOvajProizvod = uslovi.FirstOrDefault(x => x.Item1 == proizvod.CenovnaGrupaID);
                        }
                        catch
                        {

                        }

                        int nivo = korisnikovCenovniUslovZaOvajProizvod == null ? 0 : korisnikovCenovniUslovZaOvajProizvod.Item2;

                        double K = razlika / (nProfiCenovnikNivoa - 1) * nivo;

                        if (minCena <= 0 || maxCena <= 0)
                            c.Add(new Models.Cenovnik.Artikal() { ID = proizvod.RobaID, Cena = new Models.Cena() { VPCena = -999999 } });
                        else
                            c.Add(new Models.Cenovnik.Artikal() { ID = proizvod.RobaID, Cena = new Models.Cena() { VPCena = maxCena - K, PDV = proizvod.PDV / 100 } });
                    }

                    return StatusCode(200, JsonConvert.SerializeObject(c));
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        [HttpGet]
        [Route("/Webshop/Korisnik/Status/Insert")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> InsertStatus(int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("INSERT INTO KORISNIK (ID, STATUS) VALUES (@ID, @STATUS)", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            cmd.Parameters.AddWithValue("@STATUS", 2);
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
        /// Vraca tip korisnika. Ukoliko tip nije pronadjen vraca 204
        /// </summary>
        [HttpGet]
        [Route("/Webshop/Korisnik/Tip/Get")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> TipGet([Required]int korisnikID)
        {
            return Task.Run<IActionResult>(() => {
                try
                {
                    using(MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using(MySqlCommand cmd = new MySqlCommand("SELECT TIP FROM KORISNIK_TIP WHERE KORISNIK_ID = @KID", con))
                        {
                            cmd.Parameters.AddWithValue("@KID", korisnikID);
                            MySqlDataReader dr = cmd.ExecuteReader();

                            return dr.Read() ? StatusCode(200, Convert.ToInt32(dr[0])) : StatusCode(204);
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
        /// Setuje tip korisnika (UPDATE / INSERT)
        /// </summary>
        [HttpPost]
        [Route("/Webshop/Korisnik/Tip/Set")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> TipSet([Required]int korisnikID, [Required]int tip)
        {
            return Task.Run<IActionResult>(() => {
               try
               {
                    using(MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using(MySqlCommand cmd = new MySqlCommand("INSERT INTO KORISNIK_TIP (KORISNIK_ID, TIP) VALUES (@KID, @TIP) ON DUPLICATE KEY UPDATE TIP = @TIP", con))
                        {
                            cmd.Parameters.AddWithValue("@KID", korisnikID);
                            cmd.Parameters.AddWithValue("@TIP", tip);

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
        /// Loguje posetu korisnika
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">uspesno je update poslednji put vidjen</response>
        /// <response code="400">prosledjeni id nije validan</response>
        /// <response code="500">doslo je do problema na serveru</response>
        [HttpPost]
        [Route("/Webshop/Korisnik/Poseta/Log")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> PosetaLog([Required]int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    if (id <= 0)
                        return StatusCode(400, "ID nije validan");

                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT ID FROM KORISNIK WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                                if (!dt.Read())
                                    return StatusCode(400, "Korisnik nije pronadjen");
                        }

                        using (MySqlCommand cmd = new MySqlCommand(@"INSERT INTO POSLEDNJI_PUT_VIDJEN
                            (KORISNIK_ID, DATUM)
                            VALUES
                            (@KORISNIK_ID, @DATUM)
                            ON DUPLICATE KEY UPDATE DATUM = @DATUM", con))
                        {
                            cmd.Parameters.AddWithValue("@KORISNIK_ID", id);
                            cmd.Parameters.AddWithValue("@DATUM", DateTime.Now);
                            cmd.ExecuteNonQuery();

                            return StatusCode(200);
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
        /// Vraca informacije o poslednjoj poseti korisnika na webshop-u
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Webshop/Korisnik/Poseta/Get")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> PosetaGet(int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT DATUM FROM POSLEDNJI_PUT_VIDJEN WHERE KORISNIK_ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                                if (dt.Read())
                                    return StatusCode(200, Convert.ToDateTime(dt["DATUM"]));
                        }
                    }
                    return StatusCode(204, null);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }

        /// <summary>
        /// Vraca zanimanje od korisnika
        /// </summary>
        /// <param name="id">Id korisnika</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Webshop/Korisnik/Zanimanje/Get")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public Task<IActionResult> ZanimanjeGet(int id)
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT ZANIMANJE FROM KORISNIK WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", id);
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                                if (dt.Read())
                                    return StatusCode(200, Convert.ToInt32(dt["ZANIMANJE"]));
                        }
                    }
                    return StatusCode(204, null);
                }
                catch
                {
                    return StatusCode(500);
                }
            });
        }


       
        #region Korisnik


        /// <summary>
        /// Azurira podatke korisnika u bazi po ID-u
        /// </summary>
        /// <param name="korisnik"></param>
        /// <returns></returns>
        [HttpPost]
        [RequireBearer]
        [Route("/Webshop/Korisnik/Update")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Korisnik" })]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public Task<IActionResult> Update(Korisnik korisnik)
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
                            MAIL = @MAIL,
                            ADRESA_STANOVANJA = @ADRESA_STANOVANJA,
                            OPSTINA = @OPSTINA,
                            DATUM_RODJENJA = @DATUM_RODJENJA,
                            PIB = @PIB,
                            PPID = @PPID
                            WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", korisnik.ID);
                            cmd.Parameters.AddWithValue("@IME", korisnik.Ime);
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
                    using(MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using(MySqlCommand cmd = new MySqlCommand(@"UPDATE KORISNIK SET
                            ZANIMANJE = @ZANIMANJE,
                            POSETA  = @POSETA,
                            STATUS = @STATUS,
                            MAGACINID = @MAGACINID,
                            PRIMA_OBAVESTENJA = @PRIMA_OBAVESTENJA,
                            REFERENT = @REFERENT,
                            DATUM_ODOBRENJA = @DATUM_ODOBRENJA,
                            POSLEDNJI_PUT_VIDJEN = @POSLEDNJI_PUT_VIDJEN,
                            KOMENTAR = @KOMENTAR,
                            NADIMAK = @NADIMAK 
                            WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", korisnik.ID);
                            cmd.Parameters.AddWithValue("@ZANIMANJE", korisnik.Zanimanje);
                            cmd.Parameters.AddWithValue("@STATUS", korisnik.Status);
                            cmd.Parameters.AddWithValue("@POSETA", korisnik.Poseta);
                            cmd.Parameters.AddWithValue("@MAGACINID", korisnik.MagacinID);
                            cmd.Parameters.AddWithValue("@PRIMA_OBAVESTENJA", korisnik.PrimaObavestenja);
                            cmd.Parameters.AddWithValue("@REFERENT", korisnik.Referent);
                            cmd.Parameters.AddWithValue("@DATUM_ODOBRENJA", korisnik.DatumOdobrenja);
                            cmd.Parameters.AddWithValue("@POSLEDNJI_PUT_VIDJEN", korisnik.PoslednjiPutVidjen);
                            cmd.Parameters.AddWithValue("@KOMENTAR", korisnik.Komentar);
                            cmd.Parameters.AddWithValue("@NADIMAK", korisnik.Nadimak);
                            cmd.ExecuteNonQuery();

                        }
                    }
                    return StatusCode(200);
                }
                catch(Exception )
                {
                    return StatusCode(500);
                }
            });
        }
        /// <summary>
        /// Vraca korisnika
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ime">Korisnicko ime</param>
        /// <returns></returns>
        [HttpGet]
        [Route("/Webshop/Korisnik/Get")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Get(int? id, string ime)
        {
            return Task.Run<IActionResult>(() =>
            {
                if (id == null && string.IsNullOrWhiteSpace(ime))
                    return StatusCode(400, "Morate da prolsedite ID ili Ime");

                try
                {
                    Korisnik k = null;
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();

                        using (MySqlCommand cmd = new MySqlCommand(@"SELECT
                                ID,
                                IME,
                                PW,
                                STATUS,
                                TIP,
                                MOBILNI,
                                MAIL,
                                ADRESA_STANOVANJA,
                                OPSTINA,
                                DATUM_RODJENJA,
                                PPID,
                                DATUM_KREIRANJA,
                                POSLAT_RODJENDANSKI_SMS,
                                PIB
                                FROM KORISNIK
                                WHERE " + (id == null ? "IME" : "ID") + " = @PAR", con))
                        {
                            cmd.Parameters.AddWithValue("@PAR", id == null ? ime : id);

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                if (dr.Read())
                                    k = new Korisnik()
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
                                    };
                                else return StatusCode(204);
                        }
                    }
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT STATUS, ZANIMANJE, REFERENT, POSETA, MAGACINID, PRIMA_OBAVESTENJA, DATUM_ODOBRENJA, POSLEDNJI_PUT_VIDJEN, KOMENTAR, NADIMAK, ZANIMANJE FROM KORISNIK WHERE ID = @ID", con))
                        {
                            cmd.Parameters.AddWithValue("@ID", k.ID);
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                            {
                                if (dt.Read())
                                {
                                    k.Status = k.Status == 1 ? Convert.ToInt32(dt["STATUS"]) : k.Status;
                                    k.Referent = dt["REFERENT"] is DBNull ? null : Convert.ToInt16(dt["REFERENT"]);
                                    k.Poseta = dt["POSETA"] is DBNull ? 0 : Convert.ToInt32(dt["POSETA"]);
                                    k.MagacinID = Convert.ToInt16(dt["MAGACINID"]);
                                    k.PrimaObavestenja = Convert.ToBoolean(dt["PRIMA_OBAVESTENJA"]);
                                    k.DatumOdobrenja = dt["DATUM_ODOBRENJA"] is DBNull ? null : Convert.ToDateTime(dt["DATUM_ODOBRENJA"]);
                                    k.PoslednjiPutVidjen = dt["POSLEDNJI_PUT_VIDJEN"] is DBNull ? null : Convert.ToDateTime(dt["POSLEDNJI_PUT_VIDJEN"]);
                                    k.Komentar = dt["KOMENTAR"] is DBNull ? null : dt["KOMENTAR"].ToString();
                                    k.Nadimak = dt["NADIMAK"].ToString();
                                    k.Zanimanje = Convert.ToInt16(dt["ZANIMANJE"]);
                                }
                                else return StatusCode(400, "Korisnik nije pronadjen");
                            }
                        }
                        return StatusCode(200, k);
                    }
                }
                catch (Exception)
                {
                    return StatusCode(500);
                }

            });
        }
        /// <summary>
        /// Vraca listu korisnika
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/Webshop/Korisnik/List")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> List()
        {
            return Task.Run<IActionResult>(() =>
            {
                try
                {
                    List<Korisnik> list = new List<Korisnik>();
                    DataTable apiKorisnik = new DataTable();
                    DataTable webShopKorisnik = new DataTable();
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                    {
                        con.Open();

                        using (MySqlCommand cmd = new MySqlCommand(@"SELECT
                                ID,
                                IME,
                                PW,
                                TIP,
                                STATUS,
                                MOBILNI,
                                MAIL,
                                ADRESA_STANOVANJA,
                                OPSTINA,
                                DATUM_RODJENJA,
                                PPID,
                                DATUM_KREIRANJA,
                                POSLAT_RODJENDANSKI_SMS,
                                PIB
                                FROM KORISNIK", con))
                        {

                            using (MySqlDataReader dr = cmd.ExecuteReader())
                                if (dr.Read())
                                    apiKorisnik.Load(dr);
                        }
                    }
                    using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                    {
                        con.Open();
                        using (MySqlCommand cmd = new MySqlCommand("SELECT ID, STATUS, ZANIMANJE, REFERENT, POSETA, MAGACINID, PRIMA_OBAVESTENJA, DATUM_ODOBRENJA, POSLEDNJI_PUT_VIDJEN, KOMENTAR, NADIMAK FROM KORISNIK", con))
                        {
                            using (MySqlDataReader dt = cmd.ExecuteReader())
                            {
                                if (dt.Read())
                                    webShopKorisnik.Load(dt);
                                else return StatusCode(400, "Korisnik nije pronadjen");
                            }
                        }
                    }
                    foreach(DataRow api in apiKorisnik.Rows)
                    {
                        DataRow webShop = webShopKorisnik.Select("ID = " + api["ID"]).FirstOrDefault();
                        if (webShop == null)
                            continue;
                        list.Add(new Korisnik()
                        {
                            ID = Convert.ToInt16(api["ID"]),
                            Ime = api["IME"].ToString(),
                            PW = api["PW"].ToString(),
                            Tip = (Models.KorisnikTip)Convert.ToInt32(api["TIP"]),
                            Status = Convert.ToInt32(api["STATUS"]) == 1 ? Convert.ToInt32(webShop["STATUS"]) : Convert.ToInt32(api["STATUS"]),
                            Mobilni = api["MOBILNI"].ToString(),
                            Mail = api["MAIL"].ToString(),
                            AdresaStanovanja = api["ADRESA_STANOVANJA"].ToString(),
                            Opstina = api["OPSTINA"].ToString(),
                            DatumRodjenja = Convert.ToDateTime(api["DATUM_RODJENJA"]),
                            PPID = api["PPID"] is DBNull ? null : Convert.ToInt16(api["PPID"]),
                            DatumKreiranja = Convert.ToDateTime(api["DATUM_KREIRANJA"]),
                            PoslatRodjendanskiSMS = Convert.ToBoolean(api["POSLAT_RODJENDANSKI_SMS"]),
                            PIB = api["PIB"] is DBNull ? null : api["PIB"].ToString(),
                            Zanimanje = Convert.ToInt16(webShop["ZANIMANJE"]),
                            Poseta = webShop["POSETA"] is DBNull ? 0 : Convert.ToInt32(webShop["POSETA"]),
                            MagacinID = Convert.ToInt16(webShop["MAGACINID"]),
                            PrimaObavestenja = Convert.ToBoolean(webShop["PRIMA_OBAVESTENJA"]),
                            Referent = webShop["REFERENT"] is DBNull ? null : Convert.ToInt16(webShop["REFERENT"]),
                            DatumOdobrenja = webShop["DATUM_ODOBRENJA"] is DBNull ? null : Convert.ToDateTime(webShop["DATUM_ODOBRENJA"]),
                            PoslednjiPutVidjen = webShop["POSLEDNJI_PUT_VIDJEN"] is DBNull ? null : Convert.ToDateTime(webShop["POSLEDNJI_PUT_VIDJEN"]),
                            Komentar = webShop["KOMENTAR"] is DBNull ? null : webShop["KOMENTAR"].ToString(),
                            Nadimak = webShop["NADIMAK"].ToString()
                        });
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
        /// Vrsi insert korisnika
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/Webshop/Korisnik/Insert")]
        [SwaggerOperation(Tags = new[] { "/Webshop/Korisnik" })]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public Task<IActionResult> Insert(DTO.Webshop.KorisnikInsertDTO korisnik)
        {
            return Task.Run<IActionResult>(() =>
            {
                lock (_lock)
                {
                    try
                    {
                        int id = 0;
                        using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
                        {
                            con.Open();
                            using (MySqlCommand cmd = new MySqlCommand(@"INSERT INTO KORISNIK(
IME, PW, TIP, STATUS, MOBILNI, MAIL, ADRESA_STANOVANJA, OPSTINA, DATUM_RODJENJA, PPID, DATUM_KREIRANJA, POSLAT_RODJENDANSKI_SMS, PIB
) VALUES(
@IME, @PW, @TIP, @STATUS, @MOBILNI, @MAIL, @ADRESA_STANOVANJA, @OPSTINA, @DATUM_RODJENJA, @PPID, @DATUM_KREIRANJA, @POSLAT_RODJENDANSKI_SMS, @PIB
)", con))
                            {
                                cmd.Parameters.AddWithValue("@IME", korisnik.Ime);
                                cmd.Parameters.AddWithValue("@PW", HashPassword(korisnik.PW));
                                cmd.Parameters.AddWithValue("@TIP", (int)korisnik.Tip);
                                cmd.Parameters.AddWithValue("@STATUS", 1);
                                cmd.Parameters.AddWithValue("@MOBILNI", korisnik.Mobilni);
                                cmd.Parameters.AddWithValue("@MAIL", korisnik.Mail);
                                cmd.Parameters.AddWithValue("@ADRESA_STANOVANJA", korisnik.AdresaStanovanja);
                                cmd.Parameters.AddWithValue("@OPSTINA", korisnik.Opstina);
                                cmd.Parameters.AddWithValue("@DATUM_RODJENJA", korisnik.DatumRodjenja);
                                cmd.Parameters.AddWithValue("@PPID", null);
                                cmd.Parameters.AddWithValue("@DATUM_KREIRANJA", DateTime.Now);
                                cmd.Parameters.AddWithValue("@POSLAT_RODJENDANSKI_SMS", 0);
                                cmd.Parameters.AddWithValue("@PIB", korisnik.PIB);

                                cmd.ExecuteNonQuery();
                                cmd.Cancel();

                            }
                            using (MySqlCommand cmd = new MySqlCommand("SELECT MAX(ID) FROM KORISNIK", con))
                            {
                                using (MySqlDataReader dt = cmd.ExecuteReader())
                                {
                                    if (dt.Read())
                                        id = Convert.ToInt32(dt[0]);
                                    else
                                        return StatusCode(500);
                                }
                            }
                        }
                        using (MySqlConnection con = new MySqlConnection(Program.ConnectionStringWebshop))
                        {
                            con.Open();
                            using (MySqlCommand cmd = new MySqlCommand("INSERT INTO KORISNIK(ID ,ZANIMANJE, POSETA, STATUS, MAGACINID, PRIMA_OBAVESTENJA, NADIMAK) VALUES(@ID, @ZANIMANJE, 0, 0, @MAGACINID, 1, @NADIMAK)", con))
                            {
                                cmd.Parameters.AddWithValue("@ID", id);
                                cmd.Parameters.AddWithValue("@ZANIMANJE", korisnik.Zanimanje);
                                cmd.Parameters.AddWithValue("@MAGACINID", korisnik.MagacinID);
                                cmd.Parameters.AddWithValue("@NADIMAK", korisnik.Nadimak);
                                cmd.ExecuteNonQuery();
                            }
                        }
                        return StatusCode(201, id);
                    }
                    catch(Exception)
                    {
                        return StatusCode(500);
                    }
                }
            });
        }
        #endregion

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
    }
}