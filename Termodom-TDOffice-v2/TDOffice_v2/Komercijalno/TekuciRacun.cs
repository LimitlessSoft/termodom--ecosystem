using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
    public class TekuciRacun
    {
        #region Properties
        public int PPID { get; set; }
        public string Racun { get; set; }
        public int BankaID { get; set; }    
        public string Valuta { get; set; }  
        public double Stanje { get; set; }  
        public int? MagacinID { get; set; }
        #endregion
        public TekuciRacun()
        {

        }
        public void Update(int godina, int PPID)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                Update(con);
            }
        }
        public void Update(FbConnection con)
        {
            throw new Exception("Nije sinhronizovano da radi sa svim bazama! Kontaktirajte administratora!");
            using (FbCommand cmd = new FbCommand("UPDATE TEKUCIRACUN SET RACUN = @R, BANKAID = @BID, VALUTA = @V, STANJE =@S, MAGACINID =@M WHERE PPID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", PPID);
                cmd.Parameters.AddWithValue("@R", Racun);
                cmd.Parameters.AddWithValue("@BID", BankaID);
                cmd.Parameters.AddWithValue("@V", Valuta);
                cmd.Parameters.AddWithValue("@S", Stanje);
                cmd.Parameters.AddWithValue("@M", MagacinID);

                cmd.ExecuteNonQuery();
            }

        }

        public static int Insert(int godina, int ppid, string racun, int bankaID, string valuta, double stanje, int? magacinID)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Insert(con, ppid, racun, bankaID, valuta, stanje, magacinID);
            }
        }
        public static int Insert(FbConnection con, int ppid, string racun, int bankaID, string valuta, double stanje, int? magacinID)
        {
            throw new Exception("Nije sinhronizovano da radi sa svim bazama! Kontaktirajte administratora!");
            using (FbCommand cmd = new FbCommand("INSERT INTO TEKUCIRACUN (RACUN, PPID, BANKAID, VALUTA, STANJE, MAGACINID) VALUES (@R, @P, @BID, @V, @S, @M)", con))
            {
                cmd.Parameters.AddWithValue("@R", racun);
                cmd.Parameters.AddWithValue("@P", ppid);
                cmd.Parameters.AddWithValue("@BID", bankaID);
                cmd.Parameters.AddWithValue("@V", valuta);
                cmd.Parameters.AddWithValue("@S", stanje);
                cmd.Parameters.AddWithValue("@M", magacinID);

                cmd.ExecuteNonQuery();
                return 0;
            }
        }

        public static TekuciRacun Get(int godina, int ppid)
        {
            // Ovo nije ispravna funkcija. Jedan PPID moze imati vise tekucih racuna!
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Get(con, ppid);
            }
        }
        public static TekuciRacun Get(FbConnection con, int ppid)
        {
            throw new Exception("Nije sinhronizovano da radi sa svim bazama! Kontaktirajte administratora!");
            using (FbCommand cmd = new FbCommand("SELECT RACUN, PPID, BANKAID, VALUTA, STANJE, MAGACINID FROM TEKUCIRACUN WHERE PPID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", ppid);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new TekuciRacun()
                        {
                            PPID = Convert.ToInt32(dr["PPID"]),
                            Racun = dr["RACUN"].ToString(),
                            BankaID = Convert.ToInt32(dr["BANKAID"]),
                            Valuta = dr["VALUTA"].ToString(),
                            Stanje = Convert.ToDouble(dr["STANJE"]),
                            MagacinID = dr["MAGACINID"] is DBNull ? null :  (int?)Convert.ToInt32(dr["MAGACINID"])
                        };
            }

            return null;
        }
        public static async Task<TekuciRacunList> ListAsync()
        {
            var response = await TDBrain_v3.GetAsync($"/komercijalno/tekuciracun/list");

            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<TekuciRacunList>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
