using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Task UpdateAsync()
        {
            throw new NotImplementedException();
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

        public static Task<int> InsertAsync(int godina, int PPID, string racun, int bankaID, string valuta, double stanje, int? magacinID)
        {
            return Task.Run(() =>
            {
                return Insert(godina, PPID, racun, bankaID, valuta, stanje, magacinID);
            });
        }
        public static TekuciRacun Get(int godina, int ppid)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Get(con, ppid);
            }
        }
        public static TekuciRacun Get(FbConnection con, int ppid)
        {
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
        public static Task<TekuciRacun> GetAsync(int godina, int ppid)
        {
            return Task.Run(() =>
            {
                return Get(godina, ppid);
            });
        }
        public static List<TekuciRacun> List(int godina,string whereQuery = null)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return List(con, whereQuery);
            }
        }
        public static List<TekuciRacun> List(FbConnection con, string whereQuery = null)
        {
            if (!string.IsNullOrWhiteSpace(whereQuery))
                whereQuery = " AND " + whereQuery;
            List<TekuciRacun> list = new List<TekuciRacun>();
            using (FbCommand cmd = new FbCommand("SELECT RACUN, PPID, BANKAID, VALUTA, STANJE, MAGACINID FROM TEKUCIRACUN  WHERE 1 = 1" + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new TekuciRacun()
                        {
                            PPID = Convert.ToInt32(dr["PPID"]),
                            Racun = dr["RACUN"].ToString(),
                            BankaID = Convert.ToInt32(dr["BANKAID"]),
                            Valuta = dr["VALUTA"].ToString(),
                            Stanje = Convert.ToDouble(dr["STANJE"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"])
                        });
            }
            return list;
        }
        public static Task<List<TekuciRacun>> ListAsync(int godina, string whereQuery = null)
        {
            return Task.Run(() =>
            {
                return List(godina,whereQuery);
            });
        }

    }
}
