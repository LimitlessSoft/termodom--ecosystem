using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using LimitlessSoft.Buffer;
using System.Threading.Tasks;

namespace TDOffice_v2.Komercijalno
{
    public class Roba
    {
        public int ID { get; set; }
        public string KatBr { get; set; }
        public string KatBrPro { get; set; }
        public string Naziv { get; set; }
        public string JM { get; set; }
        public string TarifaID { get; set; }
        public int Vrsta { get; set; }
        public string NazivZaStampu { get; set; }
        public string GrupaID { get; set; } = null;
        public int? Podgrupa { get; set; }
        public string ProID { get; set; } = null;
        public int? DOB_PPID { get; set; }
        public double? TrKol { get; set; }

        public Roba()
        {

        }


        public void Update()
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using(FbCommand cmd = new FbCommand("UPDATE ROBA SET NAZIV = @N, KATBR = @KB, TRKOL = @TRKOL, KATBRPRO = @KBP, JM = @JM, NAZIVZASTAMPU = @NZS WHERE ROBAID = @RID", con))
                {
                    cmd.Parameters.AddWithValue("@N", Naziv);
                    cmd.Parameters.AddWithValue("@KB", KatBr);
                    cmd.Parameters.AddWithValue("@KBP", KatBrPro);
                    cmd.Parameters.AddWithValue("@JM", JM);
                    cmd.Parameters.AddWithValue("@NZS", NazivZaStampu);
                    cmd.Parameters.AddWithValue("@RID", ID);
                    cmd.Parameters.AddWithValue("@TRKOL", TrKol);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Update(FbConnection con)
        {
            using (FbCommand cmd = new FbCommand("UPDATE ROBA SET NAZIV = @N, KATBR = @KB, TRKOL = @TRKOL, KATBRPRO = @KBP, JM = @JM, NAZIVZASTAMPU = @NZS WHERE ROBAID = @RID", con))
            {
                cmd.Parameters.AddWithValue("@N", Naziv);
                cmd.Parameters.AddWithValue("@KB", KatBr);
                cmd.Parameters.AddWithValue("@KBP", KatBrPro);
                cmd.Parameters.AddWithValue("@JM", JM);
                cmd.Parameters.AddWithValue("@NZS", NazivZaStampu);
                cmd.Parameters.AddWithValue("@RID", ID);
                cmd.Parameters.AddWithValue("@TRKOL", TrKol);

                cmd.ExecuteNonQuery();
            }
        }

        public static Roba Get(int godina, int robaID)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Get(con, robaID);
            }
        }
        public static Roba Get(FbConnection con, int robaID)
        {
            using (FbCommand cmd = new FbCommand("SELECT ROBAID, NAZIV, KATBR, KATBRPRO, JM, TRKOL, TARIFAID, VRSTA, NAZIVZASTAMPU, GRUPAID, PODGRUPA, PROID, DOB_PPID FROM ROBA WHERE ROBAID = @RID", con))
            {
                cmd.Parameters.AddWithValue("@RID", robaID);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new Roba()
                        {
                            ID = Convert.ToInt32(dr["ROBAID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            KatBr = dr["KATBR"].ToString(),
                            KatBrPro = dr["KATBRPRO"].ToString(),
                            JM = dr["JM"].ToString(),
                            TarifaID = dr["TARIFAID"].ToString(),
                            Vrsta = Convert.ToInt32(dr["VRSTA"]),
                            NazivZaStampu = dr["NAZIVZASTAMPU"].ToString(),
                            GrupaID = dr["GRUPAID"].ToString(),
                            Podgrupa = dr["PODGRUPA"] is DBNull ? null : (int?)Convert.ToInt32(dr["PODGRUPA"]),
                            TrKol = dr["TRKOL"] is DBNull ? null : (double?)Convert.ToDouble(dr["TRKOL"]),
                            ProID = dr["PROID"].ToString(),
                            DOB_PPID = dr["DOB_PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["DOB_PPID"])
                        };
            }

            return null;
        }
        public static List<Roba> List(int godina)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return List(con);
            }
        }
        public static Task<List<Roba>> ListAsync(int godina)
        {
            return Task.Run(() =>
            {
                return List(godina);
            });
        }
        public static Dictionary<int, Roba> DictList(FbConnection con)
        {
            Dictionary<int, Roba> dict = new Dictionary<int, Roba>();
            using (FbCommand cmd = new FbCommand("SELECT ROBAID, NAZIV, KATBR, TRKOL, KATBRPRO, JM, TARIFAID, VRSTA, NAZIVZASTAMPU, GRUPAID, PODGRUPA, PROID, DOB_PPID FROM ROBA", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        dict.Add(Convert.ToInt32(dr["ROBAID"]), new Roba()
                        {
                            ID = Convert.ToInt32(dr["ROBAID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            KatBr = dr["KATBR"].ToString(),
                            KatBrPro = dr["KATBRPRO"].ToString(),
                            JM = dr["JM"].ToString(),
                            TarifaID = dr["TARIFAID"].ToString(),
                            Vrsta = Convert.ToInt32(dr["VRSTA"]),
                            NazivZaStampu = dr["NAZIVZASTAMPU"].ToString(),
                            TrKol = dr["TRKOL"] is DBNull ? null : (double?)Convert.ToDouble(dr["TRKOL"]),
                            GrupaID = dr["GRUPAID"].ToString(),
                            Podgrupa = dr["PODGRUPA"] is DBNull ? null : (int?)Convert.ToInt32(dr["PODGRUPA"]),
                            ProID = dr["PROID"].ToString(),
                            DOB_PPID = dr["DOB_PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["DOB_PPID"]),

                        });
            }

            return dict;
        }
        public static List<Roba> List(FbConnection con)
        {
            List<Roba> list = new List<Roba>();
            using (FbCommand cmd = new FbCommand("SELECT ROBAID, NAZIV, KATBR, TRKOL, KATBRPRO, JM, TARIFAID, VRSTA, NAZIVZASTAMPU, GRUPAID, PODGRUPA, PROID, DOB_PPID FROM ROBA", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new Roba()
                        {
                            ID = Convert.ToInt32(dr["ROBAID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            KatBr = dr["KATBR"].ToString(),
                            KatBrPro = dr["KATBRPRO"].ToString(),
                            JM = dr["JM"].ToString(),
                            TarifaID = dr["TARIFAID"].ToString(),
                            Vrsta = Convert.ToInt32(dr["VRSTA"]),
                            NazivZaStampu = dr["NAZIVZASTAMPU"].ToString(),
                            TrKol = dr["TRKOL"] is DBNull ? null : (double?)Convert.ToDouble(dr["TRKOL"]),
                            GrupaID = dr["GRUPAID"].ToString(),
                            Podgrupa = dr["PODGRUPA"] is DBNull ? null : (int?)Convert.ToInt32(dr["PODGRUPA"]),
                            ProID = dr["PROID"].ToString(),
                            DOB_PPID = dr["DOB_PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["DOB_PPID"]),

                        });
            }

            return list;
        }
        public static void Delete(int godina, int robaID)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                Delete(con, robaID);
            }
        }
        public static void Delete(FbConnection con, int robaID)
        {
            using(FbCommand cmd = new FbCommand("DELETE FROM ROBA WHERE ROBAID = @RID", con))
            {
                cmd.Parameters.AddWithValue("@RID", robaID);

                cmd.ExecuteNonQuery();
            }
        }
    }
}