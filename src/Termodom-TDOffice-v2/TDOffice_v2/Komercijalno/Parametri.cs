using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Komercijalno
{
    public class Parametri
    {
        public string Naziv { get; set; }
        public string Vrednost { get; set; }    
        public string Tip { get; set; }
        public string Opis { get; set; }
        public int? Modul_0 { get; set; }
        public int? Modul_1 { get; set; }
        public int? Modul_2 { get; set; }
        public int? Modul_3 { get; set; }
        public int? Modul_9 { get; set; }
        public int? Modul_16 { get; set; }
        public int? Modul_24 { get; set; }
        public int? Modul_60 { get; set; }
        public int? Modul_18 { get; set; }
        public int? Modul_20 { get; set; }
        public int? GrupaID { get; set; }

        public void Update(int godina)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                Update(con);
            }
        }
        public void Update(FbConnection con)
        {
            using (FbCommand cmd = new FbCommand("UPDATE PARAMETRI SET VREDNOST = @V, TIP = @T, OPIS = @O, MODUL_0 = @M0, MODUL_1 = @M1, MODUL_2 = @M2, MODUL_3 = @M3, MODUL_9 = @M9, " +
                "MODUL_16 = @M16, MODUL_24 = @M24, MODUL_60 = @M60, MODUL_18 = @M18, MODUL_20 = @M20, GRUPAID = @G WHERE NAZIV = @NAZIV", con))
            {
                cmd.Parameters.AddWithValue("@V", Vrednost);
                cmd.Parameters.AddWithValue("@T", Tip);
                cmd.Parameters.AddWithValue("@O", Opis);
                cmd.Parameters.AddWithValue("@M0", Modul_0);
                cmd.Parameters.AddWithValue("@M1", Modul_1);
                cmd.Parameters.AddWithValue("@M2", Modul_2);
                cmd.Parameters.AddWithValue("@M3", Modul_3);
                cmd.Parameters.AddWithValue("@M9", Modul_9);
                cmd.Parameters.AddWithValue("@M16", Modul_16);
                cmd.Parameters.AddWithValue("@M24", Modul_24);
                cmd.Parameters.AddWithValue("@M60", Modul_60);
                cmd.Parameters.AddWithValue("@M18", Modul_18);
                cmd.Parameters.AddWithValue("@M20", Modul_20);
                cmd.Parameters.AddWithValue("@G", GrupaID);
                cmd.Parameters.AddWithValue("@NAZIV", Naziv);

                cmd.ExecuteNonQuery();
            }
        }

        public static Parametri Get(int godina, string naziv)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Get(con, naziv);
            }
        }
        public static Parametri Get(FbConnection con, string naziv)
        {
            using (FbCommand cmd = new FbCommand("SELECT NAZIV, VREDNOST, TIP, OPIS, MODUL_0, MODUL_1, MODUL_2, MODUL_3, MODUL_9, MODUL_16, MODUL_24, MODUL_60, MODUL_18, MODUL_20, GRUPAID FROM PARAMETRI WHERE NAZIV = @NAZIV", con))
            {
                cmd.Parameters.AddWithValue("@NAZIV", naziv);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new Parametri()
                        {
                            Vrednost = dr["VREDNOST"].ToString(),
                            Naziv = dr["NAZIV"].ToString(),
                            Tip = dr["TIP"].ToString(),
                            Opis = dr["OPIS"].ToString(),    
                            Modul_0 = dr["MODUL_0"]is DBNull ? null : (int?)Convert.ToInt32(dr["MODUL_0"]),
                            Modul_1 = dr["MODUL_1"] is DBNull ? null : (int?)Convert.ToInt32(dr["MODUL_1"]),
                            Modul_2 = dr["MODUL_2"] is DBNull ? null : (int?)Convert.ToInt32(dr["MODUL_2"]),
                            Modul_3 = dr["MODUL_3"] is DBNull ? null : (int?)Convert.ToInt32(dr["MODUL_3"]),
                            Modul_9 = dr["MODUL_9"] is DBNull ? null : (int?)Convert.ToInt32(dr["MODUL_9"]),
                            Modul_16 = dr["MODUL_16"] is DBNull ? null : (int?)Convert.ToInt32(dr["MODUL_16"]),
                            Modul_24 = dr["MODUL_24"] is DBNull ? null : (int?)Convert.ToInt32(dr["MODUL_24"]),
                            Modul_60 = dr["MODUL_60"] is DBNull ? null : (int?)Convert.ToInt32(dr["MODUL_60"]),
                            Modul_18 = dr["MODUL_18"] is DBNull ? null : (int?)Convert.ToInt32(dr["MODUL_18"]),
                            Modul_20 = dr["MODUL_20"] is DBNull ? null : (int?)Convert.ToInt32(dr["MODUL_20"]),
                            GrupaID = dr["GRUPAID"] is DBNull ? null : (int?)Convert.ToInt32(dr["GRUPAID"])
                        };
            }

            return null;
        }
        public static List<Parametri> List(int godina)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<Parametri> List(FbConnection con)
        {
            List<Parametri> list = new List<Parametri>();

            {
                using (FbCommand cmd = new FbCommand("SELECT NAZIV, VREDNOST, TIP, OPIS, MODUL_0, MODUL_1, MODUL_2, MODUL_3, MODUL_9, MODUL_16, MODUL_24, MODUL_60, MODUL_18, MODUL_20, GRUPAID  FROM PARAMETRI", con))
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new Parametri()
                        {
                            Naziv = dr["NAZIV"].ToString(),
                            Vrednost = dr["VREDNOST"].ToString(),
                            Tip = dr["TIP"].ToString(),
                            Modul_0 = Convert.ToInt32(dr["MODUL_0"]),
                            Modul_1 = Convert.ToInt32(dr["MODUL_1"]),
                            Modul_2 = Convert.ToInt32(dr["MODUL_2"]),
                            Modul_3 = Convert.ToInt32(dr["MODUL_3"]),
                            Modul_9 = Convert.ToInt32(dr["MODUL_9"]),
                            Modul_16 = Convert.ToInt32(dr["MODUL_16"]),
                            Modul_24 = Convert.ToInt32(dr["MODUL_24"]),
                            Modul_60 = Convert.ToInt32(dr["MODUL_60"]),
                            Modul_18 = Convert.ToInt32(dr["MODUL_18"]),
                            Modul_20 = Convert.ToInt32(dr["MODUL_20"]),
                            GrupaID = Convert.ToInt32(dr["GRUPAID"])
                        });
            }

            return list;
        }
        public static Task<List<Parametri>> ListAsync(int godina)
        {
            return Task.Run(() =>
            {
                return List(godina);
            });
        }
    }
}
