using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDOffice_v2.TDOffice
{
    public class NalogZaPrevoz
    {
        public class Detalji
        {
            public List<Destinacija> Destinacije { get; set; } = new List<Destinacija>();
        }

        public class Destinacija
        {
            public string Adresa { get; set; }
            public string Veza { get; set; }
            public string MiKupcuNaplatili { get; set; }
            public string Kontakt { get; set; }
            public string Naplatiti { get; set; }
            public Enums.NalogZaPrevozOsnov Osnov { get; set; } = Enums.NalogZaPrevozOsnov.Razno;
            public Enums.NalogZaPrevozOdredisteNacinPlacanja NacinPlacanja { get; set; } = Enums.NalogZaPrevozOdredisteNacinPlacanja.NaRuke;
        }

        public enum NacinPlacanjaPrevozniku
        {
            Virmanom = 0,
            Gotovinom = 1
        }

        public int ID { get; set; }
        public DateTime Datum { get; set; }
        public int MagacinID { get; set; }
        public int UserID { get; private set; }
        public int Status { get; set; }
        public NacinPlacanjaPrevozniku NacinPlacanja { get; set; }
        public double NamaNaplacenPrevozBezPDV { get; set; }
        public int Prevoznik { get; set; }
        public Detalji Tag { get; set; }

        public NalogZaPrevoz()
        {
        }

        public void Update()
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand(@"UPDATE NALOG_ZA_PREVOZ SET DATUM = @D,
                    MAGACINID = @M, STATUS = @S, NACIN_PLACANJA = @NP, CENA_MARSUTE_BEZ_PDV = @CMBP,
                    PREVOZNIK = @P, TAG = @T WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@D", Datum);
                    cmd.Parameters.AddWithValue("@M", MagacinID);
                    cmd.Parameters.AddWithValue("@S", Status);
                    cmd.Parameters.AddWithValue("@NP", (int)NacinPlacanja);
                    cmd.Parameters.AddWithValue("@CMBP", NamaNaplacenPrevozBezPDV);
                    cmd.Parameters.AddWithValue("@P", Prevoznik);
                    cmd.Parameters.AddWithValue("@T", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Tag)));

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Kreira nalog za prevoz na danasnji dan
        /// </summary>
        public static int Insert(int magacinID, int userID, Detalji detalji)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand(@"INSERT INTO NALOG_ZA_PREVOZ
                    (ID, DATUM, KORISNIKID, MAGACINID, STATUS, NACIN_PLACANJA, CENA_MARSUTE_BEZ_PDV, PREVOZNIK, TAG)
                    VALUES
                    (((SELECT COALESCE(MAX(ID), 0) FROM NALOG_ZA_PREVOZ) + 1), @D, @U, @M, @S, @NP, @CMBP, 0, @T) RETURNING ID", con))
                {
                    cmd.Parameters.AddWithValue("@D", new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day));
                    cmd.Parameters.AddWithValue("@M", magacinID);
                    cmd.Parameters.AddWithValue("@U", userID);
                    cmd.Parameters.AddWithValue("@S", 0);
                    cmd.Parameters.AddWithValue("@NP", NacinPlacanjaPrevozniku.Gotovinom);
                    cmd.Parameters.AddWithValue("@CMBP", 0);
                    cmd.Parameters.AddWithValue("@T", Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(detalji)));
                    cmd.Parameters.Add(new FbParameter { ParameterName = "ID", FbDbType = FbDbType.Integer, Direction = System.Data.ParameterDirection.Output });
                    cmd.ExecuteNonQuery();

                    return cmd.Parameters["ID"] == null ? -1 : Convert.ToInt32(cmd.Parameters["ID"].Value);
                }
            }
        }

        public static NalogZaPrevoz Get(int id)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand(@"SELECT ID, DATUM, STATUS, MAGACINID, KORISNIKID, NACIN_PLACANJA, CENA_MARSUTE_BEZ_PDV,
                    PREVOZNIK, TAG FROM NALOG_ZA_PREVOZ WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (FbDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                            return new NalogZaPrevoz()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                Status = Convert.ToInt32(dr["STATUS"]),
                                MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                                Datum = Convert.ToDateTime(dr["DATUM"]),
                                UserID = Convert.ToInt32(dr["KORISNIKID"]),
                                NacinPlacanja = (NacinPlacanjaPrevozniku)Convert.ToInt32(dr["NACIN_PLACANJA"]),
                                NamaNaplacenPrevozBezPDV = dr["CENA_MARSUTE_BEZ_PDV"] is DBNull ? 0 : Convert.ToDouble(dr["CENA_MARSUTE_BEZ_PDV"]),
                                Prevoznik = Convert.ToInt32(dr["PREVOZNIK"]),
                                Tag = JsonConvert.DeserializeObject<Detalji>(Encoding.UTF8.GetString((byte[])dr["TAG"]))
                            };
                    }
                }
            }
            return null;
        }
        public static List<NalogZaPrevoz> List()
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<NalogZaPrevoz> List(FbConnection con)
        {
            List<NalogZaPrevoz> nzp = new List<NalogZaPrevoz>();
            using (FbCommand cmd = new FbCommand("SELECT ID, DATUM, MAGACINID, KORISNIKID, NACIN_PLACANJA, CENA_MARSUTE_BEZ_PDV, PREVOZNIK, TAG, STATUS FROM NALOG_ZA_PREVOZ", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        nzp.Add(new NalogZaPrevoz()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Status = Convert.ToInt32(dr["STATUS"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            UserID = Convert.ToInt32(dr["KORISNIKID"]),
                            NacinPlacanja = (NacinPlacanjaPrevozniku)Convert.ToInt32(dr["NACIN_PLACANJA"]),
                            NamaNaplacenPrevozBezPDV = dr["CENA_MARSUTE_BEZ_PDV"] is DBNull ? 0 : Convert.ToDouble(dr["CENA_MARSUTE_BEZ_PDV"]),
                            Prevoznik = Convert.ToInt32(dr["PREVOZNIK"]),
                            Tag = JsonConvert.DeserializeObject<Detalji>(Encoding.UTF8.GetString((byte[])dr["TAG"]))
                        });
                }
            }
            return nzp;
        }
        public static List<NalogZaPrevoz> List(DateTime odDatuma, DateTime doDatuma)
        {
            return List($@"DATUM >=
                        '{odDatuma.ToSystemShortDateFormatString()}'
                        AND DATUM < '{doDatuma.ToSystemShortDateFormatString()}'");
        }
        public static List<NalogZaPrevoz> List(string whereQuery)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con, whereQuery);
            }
        }
        public static List<NalogZaPrevoz> List(FbConnection con, string whereQuery)
        {
            List<NalogZaPrevoz> nzp = new List<NalogZaPrevoz>();
            whereQuery = " OR " + whereQuery;

            using (FbCommand cmd = new FbCommand("SELECT ID, DATUM, MAGACINID, KORISNIKID, NACIN_PLACANJA, CENA_MARSUTE_BEZ_PDV, PREVOZNIK, TAG, STATUS FROM NALOG_ZA_PREVOZ WHERE 1 = 2" + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        nzp.Add(new NalogZaPrevoz()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Status = Convert.ToInt32(dr["STATUS"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            UserID = Convert.ToInt32(dr["KORISNIKID"]),
                            NacinPlacanja = (NacinPlacanjaPrevozniku)Convert.ToInt32(dr["NACIN_PLACANJA"]),
                            NamaNaplacenPrevozBezPDV = dr["CENA_MARSUTE_BEZ_PDV"] is DBNull ? 0 : Convert.ToDouble(dr["CENA_MARSUTE_BEZ_PDV"]),
                            Prevoznik = Convert.ToInt32(dr["PREVOZNIK"]),
                            Tag = JsonConvert.DeserializeObject<Detalji>(Encoding.UTF8.GetString((byte[])dr["TAG"]))
                        });
                }
            }
            return nzp;
        }
        public static List<NalogZaPrevoz> ListByMagacinID(int magacinID)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return ListByMagacinID(con, magacinID);
            }
        }
        public static List<NalogZaPrevoz> ListByMagacinID(FbConnection con, int magacinID)
        {
            List<NalogZaPrevoz> nzp = new List<NalogZaPrevoz>();
            using (FbCommand cmd = new FbCommand("SELECT ID, DATUM, MAGACINID, KORISNIKID, NACIN_PLACANJA, CENA_MARSUTE_BEZ_PDV, PREVOZNIK, TAG, STATUS FROM NALOG_ZA_PREVOZ WHERE MAGACINID = @MID", con))
            {
                cmd.Parameters.AddWithValue("@MID", magacinID);
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        nzp.Add(new NalogZaPrevoz()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Status = Convert.ToInt32(dr["STATUS"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            UserID = Convert.ToInt32(dr["KORISNIKID"]),
                            NacinPlacanja = (NacinPlacanjaPrevozniku)Convert.ToInt32(dr["NACIN_PLACANJA"]),
                            NamaNaplacenPrevozBezPDV = dr["CENA_MARSUTE_BEZ_PDV"] is DBNull ? 0 : Convert.ToDouble(dr["CENA_MARSUTE_BEZ_PDV"]),
                            Prevoznik = Convert.ToInt32(dr["PREVOZNIK"]),
                            Tag = JsonConvert.DeserializeObject<Detalji>(Encoding.UTF8.GetString((byte[])dr["TAG"]))
                        });
                }
            }
            return nzp;
        }
        public static DateTime? MinDatum()
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return MinDatum(con);
            }
        }
        public static DateTime? MinDatum(FbConnection con)
        {
            using(FbCommand cmd = new FbCommand("SELECT MIN(DATUM) FROM NALOG_ZA_PREVOZ", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return Convert.ToDateTime(dr[0]);
            }
            return null;
        }
    }
}
