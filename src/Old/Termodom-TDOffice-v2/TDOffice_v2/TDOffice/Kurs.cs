using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class Kurs
    {
        public DateTime Datum { get; set; }
        public double Eur { get; set; }
        public double Rsd { get; set; }
        public double Usd { get; set; }

        public void Update()
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Update(con);
            }
        }
        public void Update(FbConnection con)
        {
            using(FbCommand cmd = new FbCommand("UPDATE KURS SET EUR = @E, RSD = @R, USD = @U WHERE DATUM = @DAT", con))
            {
                cmd.Parameters.AddWithValue("@E", Eur);
                cmd.Parameters.AddWithValue("@R", Rsd);
                cmd.Parameters.AddWithValue("@U", Usd);
                cmd.Parameters.AddWithValue("@DAT", Datum);

                cmd.ExecuteNonQuery();
            }
        }

        public static void Insert(DateTime datum, double eur, double rsd, double usd)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Insert(con, datum, eur, rsd, usd);
            }
        }
        public static void Insert(FbConnection con, DateTime datum, double eur, double rsd, double usd)
        {
            using(FbCommand cmd = new FbCommand("INSERT INTO KURS (DATUM, EUR, RSD, DOLAR) VALUES (@DATUM, @E, @R, @D)", con))
            {
                cmd.Parameters.AddWithValue("@DATUM", datum.Date);
                cmd.Parameters.AddWithValue("@E", eur);
                cmd.Parameters.AddWithValue("@R", rsd);
                cmd.Parameters.AddWithValue("@D", usd);

                cmd.ExecuteNonQuery();
            }
        }

        public static Kurs Get(DateTime datum)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, datum);
            }
        }
        public static Kurs Get(FbConnection con, DateTime datum)
        {
            using (FbCommand cmd = new FbCommand("SELECT DATUM, RSD, EUR, DOLAR FROM KURS WHERE DATUM = @DAT", con))
            {
                cmd.Parameters.AddWithValue("@DAT", datum.Date);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new Kurs()
                        {
                            Datum = Convert.ToDateTime(dr[0]),
                            Rsd = Convert.ToDouble(dr[1]),
                            Eur = Convert.ToDouble(dr[2]),
                            Usd = Convert.ToDouble(dr[3])
                        };
            }
            return null;
        }
        public static Task<Kurs> GetAsync(DateTime datum)
        {
            return Task.Run(() =>
            {
                return Get(datum);
            });
        }

        public static List<Kurs> List()
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<Kurs> List(FbConnection con)
        {
            List<Kurs> list = new List<Kurs>();
            using(FbCommand cmd = new FbCommand("SELECT DATUM, RSD, EUR, DOLAR FROM KURS", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new Kurs()
                        {
                            Datum = Convert.ToDateTime(dr[0]),
                            Rsd = Convert.ToDouble(dr[1]),
                            Eur = Convert.ToDouble(dr[2]),
                            Usd = Convert.ToDouble(dr[3])
                        });
            }
            return list;
        }
        public static Task<List<Kurs>> ListAsync()
        {
            return Task.Run(() =>
            {
                return List();
            });
        }


        /// <summary>
        /// Proverava da li na danasnji dan postoji kurs u bazi.
        /// Ukoliko ne postoji, cita trenutni kurs sa interneta i zapisuje ga u bazu.
        /// </summary>
        public static Task UcitajIZapisiKursUBazuAsync()
        {
            return Task.Run(() =>
            {
                Kurs danasnjiKurs = Get(DateTime.Now);
                if (danasnjiKurs == null && true == false)
                {
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "https://freecurrencyapi.net/api/v2/latest?apikey=2b2bc880-567f-11ec-9556-a3a5d919fa42&base_currency=EUR");

                    HttpClient client = new HttpClient();
                    HttpResponseMessage response = client.SendAsync(request).Result;

                    DTO.KursGetDTO resp = JsonConvert.DeserializeObject<DTO.KursGetDTO>(response.Content.ReadAsStringAsync().Result);
                    Kurs.Insert(DateTime.Now, 1, resp.data.RSD, resp.data.USD);
                }
            });
        }
    }
}
