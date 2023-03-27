using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;
using Microsoft.AspNetCore.Http;
using Termodom.API;
using Termodom.DTO.API;
using System.Threading;

namespace Termodom.Models
{
    public partial class Korisnik
    {
        public int ID { get; set; }
        public string Ime { get; set; }
        public string PW { get; set; }
        public int Tip { get; set; }
        public string Nadimak { get; set; }
        public int Status { get; set; }
        public string Mobilni { get; set; }
        public string Komentar { get; set; }
        public string Mail { get; set; }
        public int Poseta { get; set; }
        public string AdresaStanovanja { get; set; }
        public string Opstina { get; set; }
        public int MagacinID { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public bool PrimaObavestenja { get; set; }
        public string PIB { get; set; }
        public int? PPID { get; set; }
        public int? Referent { get; set; }
        public DateTime DatumKreiranja { get; set; }
        public bool PoslatRodjendanskiSMS { get; set; }
        public DateTime? DatumOdobrenja { get; set; }
        public DateTime? PoslednjiPutVidjen {
            get
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/Webshop/Korisnik/Poseta/Get");
                request.Content = new StringContent($"ID={this.ID}");

                APIRequestFailedLog failedLog = null;
                HttpResponseMessage response = APIRequest.Send(request, out failedLog);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return Convert.ToDateTime(response.Content.ReadAsStringAsync().Result);
                else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                    throw new APIRequestTimeoutException(failedLog);
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new APIRequestInternalServerErrorException();
                else if (response.StatusCode == HttpStatusCode.NoContent)
                    return null;

                throw new APIResponseNotProcessedException();

            }
        }
        public int Zanimanje { get; set; }

        private static CenovnikBuffer _cenovnikBuffer { get; set; } = new CenovnikBuffer(); // Cenovnik buffer

        public Korisnik()
        {

        }

        public void Update()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BaseAPIUrl + "/Webshop/Korisnik/Update");
            request.Content = System.Net.Http.Json.JsonContent.Create<Korisnik>(this);
            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return;
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                int subStatus = Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
                string msg = "Bad Request";
                switch (subStatus)
                {
                    case 4010:
                        msg = "Polje Korisnicko Ime mora biti popunjeno!";
                        break;
                    case 4011:
                        msg = "Polje Korisnicko Ime moze imati najvise 64 karaktera!";
                        break;
                    case 4021:
                    case 4022:
                        msg = "Polje Tip Korisnika nije ispravno!";
                        break;
                    case 4030:
                        msg = "Polje Nadimak mora biti popunjeno!";
                        break;
                    case 4031:
                        msg = "Polje Nadimak moze imati najvise 64 karaktera!";
                        break;
                }
                throw new APIBadRequestException(msg);
            }
            else
                throw new APIResponseNotProcessedException();
        }
        public void LogujPosetu()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BaseAPIUrl + "/Webshop/Korisnik/Poseta/Log?ID=" + this.ID);
            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return;
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else throw new APIResponseNotProcessedException();
        }
       
        /// <summary>
        /// Vraca vazeci cenovnik proizvoda za ovog korisnika
        /// </summary>
        /// <returns></returns>
        public Cenovnik GetCenovnik(bool getFromBufferIfLoaded = true)
        {
            CenovnikBufferItem cbi = _cenovnikBuffer[ID];

            if (getFromBufferIfLoaded && cbi != null)
                return cbi.Cenovnik;

            Cenovnik c = Cenovnik.Get(ID);

            _cenovnikBuffer[ID] = new CenovnikBufferItem() { Cenovnik = c };

            return c;
        }
        public Task<Cenovnik> GetCenovnikAsync(bool getFromBufferIfLoaded = true)
        {
            return Task.Run(() =>
            {
                return GetCenovnik(getFromBufferIfLoaded);
            });
        }
        /// <summary>
        /// Vraca korisnikove uslove za svaku cenovnu grupu.
        /// Item1 = CenovnikGrupaID
        /// Item2 = Nivo
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetCenovneUslove()
        {
            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();
                return GetCenovneUslove(con);
            }
        }
        /// <summary>
        /// Vraca korisnikove uslove za svaku cenovnu grupu.
        /// Item1 = CenovnikGrupaID
        /// Item2 = Nivo
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, int> GetCenovneUslove(MySqlConnection con)
        {
            Dictionary<int, int> uslovi = new Dictionary<int, int>();
            using (MySqlCommand cmd = new MySqlCommand("SELECT NIVO, CENOVNIK_GRUPAID FROM USER_CENOVNIK WHERE USERID = @UID", con))
            {
                cmd.Parameters.AddWithValue("@UID", ID);

                using (MySqlDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        uslovi.Add(Convert.ToInt32(dr["CENOVNIK_GRUPAID"]), Convert.ToInt32(dr["NIVO"]));
            }
            return uslovi;
        }
        /// <summary>
        /// Postavlja cenovni uslov cenovne grupe za ovog korisnika
        /// </summary>
        /// <param name="con"></param>
        public void SetCenovniUslov(int cenovnaGrupaID, int nivo)
        {
            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();
                SetCenovniUslov(con, cenovnaGrupaID, nivo);
            }
        }
        public bool ImaPlatinumCenu()
        {
            using (MySqlConnection con = new MySqlConnection(Program.ConnectionString))
            {
                con.Open();
                using (MySqlCommand cmd = new MySqlCommand("SELECT COUNT(USERID) FROM USER_CENOVNIK WHERE USERID = @UID AND NIVO = 3 AND CENOVNIK_GRUPAID <> 8", con))
                {
                    cmd.Parameters.AddWithValue("@UID", ID);

                    using (MySqlDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            if(Convert.ToInt32(dr[0]) > 0)
                                return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Postavlja cenovni uslov cenovne grupe za ovog korisnika
        /// </summary>
        /// <param name="con"></param>
        public void SetCenovniUslov(MySqlConnection con, int cenovnaGrupaID, int nivo)
        {
            using(MySqlCommand cmd = new MySqlCommand("SELECT COUNT(NIVO) FROM USER_CENOVNIK WHERE CENOVNIK_GRUPAID = @CGID AND USERID = @UID", con))
            {
                cmd.Parameters.AddWithValue("@UID", ID);
                cmd.Parameters.AddWithValue("@CGID", cenovnaGrupaID);
                cmd.Parameters.AddWithValue("@N", nivo);

                bool insert = false;

                using (MySqlDataReader dr = cmd.ExecuteReader())
                    if (dr.Read() && Convert.ToInt32(dr[0]) > 0)
                        insert = false;
                    else
                        insert = true;

                if(insert)
                {
                    using (MySqlCommand cmd1 = new MySqlCommand("INSERT INTO USER_CENOVNIK (NIVO, CENOVNIK_GRUPAID, USERID) VALUES (@N, @CGID, @UID)", con))
                    {
                        cmd1.Parameters.AddWithValue("@UID", ID);
                        cmd1.Parameters.AddWithValue("@CGID", cenovnaGrupaID);
                        cmd1.Parameters.AddWithValue("@N", nivo);

                        cmd1.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (MySqlCommand cmd1 = new MySqlCommand("UPDATE USER_CENOVNIK SET NIVO = @N WHERE CENOVNIK_GRUPAID = @CGID AND USERID = @UID", con))
                    {
                        cmd1.Parameters.AddWithValue("@UID", ID);
                        cmd1.Parameters.AddWithValue("@CGID", cenovnaGrupaID);
                        cmd1.Parameters.AddWithValue("@N", nivo);

                        cmd1.ExecuteNonQuery();
                    }
                }
            }
        }
        public static void UpdateZanimanje(int id, int zanimanje)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/Webshop/Korisnik/Zanimanje/Update?id=" + id + "&zanimanje=" + zanimanje);

            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return;
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();

        }
        public static int Insert(string ime, string rawPW, int tip, string nadimak,
            string mobilni, string komentar, string mail, string adresaStanovanja,
            string opstina, int magacinID, DateTime datumRodjenja, string pib, int zanimanje)
        {

            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BaseAPIUrl + "/Webshop/Korisnik/Insert");
            request.Content = System.Net.Http.Json.JsonContent.Create<KorisnikInsertDTO>(new KorisnikInsertDTO()
            {
                Ime = ime,
                PW = rawPW,
                Tip = tip,
                Nadimak = nadimak,
                Mobilni = mobilni,
                Komentar = komentar,
                Mail = mail,
                AdresaStanovanja = adresaStanovanja,
                Opstina = opstina,
                MagacinID = magacinID,
                DatumRodjenja = datumRodjenja,
                PIB = pib,
                Zanimanje = zanimanje
            });
            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
                return Convert.ToInt32(response.Content.ReadAsStringAsync().Result);
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else if (response.StatusCode == (HttpStatusCode)404)
                throw new APIRequestEndPointNotFoundException();
            else
                throw new APIResponseNotProcessedException();
        }
        public static Korisnik Get(int id)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/WebShop/Korisnik/Get?id=" + id);
            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Korisnik>(response.Content.ReadAsStringAsync().Result);
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else if (response.StatusCode == (HttpStatusCode)404)
                throw new APIRequestEndPointNotFoundException();
            else
                throw new APIResponseNotProcessedException();
        }
        public static Korisnik Join(DTO.Webshop.KorisnikDTO webShopKorisnik, DTO.API.KorisnikDTO apiKorisnik)
        {
            return new Korisnik
            {
                ID = apiKorisnik.ID,
                Ime = apiKorisnik.Ime,
                AdresaStanovanja = apiKorisnik.AdresaStanovanja,
                DatumKreiranja = apiKorisnik.DatumKreiranja,
                DatumRodjenja = apiKorisnik.DatumRodjenja,
                DatumOdobrenja = apiKorisnik.DatumOdobrenja,
                MagacinID = apiKorisnik.MagacinID,
                Komentar = apiKorisnik.Komentar,
                Tip = apiKorisnik.Tip,
                PIB = apiKorisnik.PIB,
                PPID = apiKorisnik.PPID,
                Status = apiKorisnik.Status,
                Mail = apiKorisnik.Mail,
                Mobilni = apiKorisnik.Mobilni,
                Nadimak = apiKorisnik.Nadimak,
                PoslatRodjendanskiSMS = apiKorisnik.PoslatRodjendanskiSMS,
                Opstina = apiKorisnik.Opstina,
                PrimaObavestenja = apiKorisnik.PrimaObavestenja,
                Referent = apiKorisnik.Referent,
                Zanimanje = webShopKorisnik.Zanimanje
            };
        }
        public static void SetSifra(int id, string password)
        {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, Program.BaseAPIUrl + "/api/Korisnik/PW/Set?korisnikID=" + id + "&vrednost=" + password);
                APIRequestFailedLog failedLog = null;
                HttpResponseMessage response = APIRequest.Send(request, out failedLog);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    return;
                else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                    throw new APIRequestTimeoutException(failedLog);
                else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                    throw new APIRequestInternalServerErrorException();
                else throw new APIResponseNotProcessedException();
        }
        public static Task<Korisnik> GetAsync(int id)
        {
            return Task.Run(() =>
            {
                return Get(id);
            });
        }
        public static Korisnik Get(string ime)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/Webshop/Korisnik/Get?ime=" + ime);
            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Korisnik>(response.Content.ReadAsStringAsync().Result);
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else throw new APIResponseNotProcessedException();


        }
        public static Task<Korisnik> GetAsync(string ime)
        {
            return Task.Run(() =>
            {
                return Get(ime);
            });
        }
        public static List<Korisnik> List()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, Program.BaseAPIUrl + "/WebShop/Korisnik/List");
            APIRequestFailedLog failedLog = null;
            HttpResponseMessage response = APIRequest.Send(request, out failedLog);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<List<Korisnik>>(response.Content.ReadAsStringAsync().Result);
            else if (response.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                throw new APIRequestTimeoutException(failedLog);
            else if (response.StatusCode == HttpStatusCode.InternalServerError)
                throw new APIRequestInternalServerErrorException();
            else throw new APIResponseNotProcessedException();
        }
        public static Task<List<Korisnik>> ListAsync()
        {
            return Task.Run(() =>
            {
                return List();
            });
        }
    }
}