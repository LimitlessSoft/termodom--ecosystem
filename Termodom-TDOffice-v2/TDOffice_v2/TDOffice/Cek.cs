using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TDOffice_v2.TDOffice.Enums;

namespace TDOffice_v2.TDOffice
{
    public class Cek
    {
        // Gotovinska povratnica - Racunar (manje u specifikaciji)
        // Virmanska povratnica - Racunar (ne utice na specifikaciju)
        // Racunar - kasa - boja

        // Kasa / Storno / Visak

        // Istorija izmena

        // odakle je cek odlozeno dosao
        public int ID { get; set; }
        public CekStatus Status { get; set; }
        public int MagacinID { get; set; }
        public int PodnosilacBanka { get; set; }
        public string TRGradjana { get; set; }
        public string BrojCeka { get; set; }
        public DateTime DatumValute { get; set; }
        public double Vrednost { get; set; }
        public int? Zaduzio { get; set; }
        public DateTime Datum { get; set; }

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
            using(FbCommand cmd = new FbCommand(@"UPDATE CEK SET 
STATUS = @S,
MAGACINID = @MAG,
PODNOSILAC_BANKA = @PB,
TR_GRADJANA = @TRG,
BROJ_CEKA = @BRC,
DATUM_VALUTE = @DV,
VREDNOST = @V,
DATUM = @DATUM,
ZADUZIO = @Z
WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", ID);
                cmd.Parameters.AddWithValue("@DV", DatumValute);
                cmd.Parameters.AddWithValue("@TRG", TRGradjana);
                cmd.Parameters.AddWithValue("@BRC", BrojCeka);
                cmd.Parameters.AddWithValue("@PB", PodnosilacBanka);
                cmd.Parameters.AddWithValue("@S", Status);
                cmd.Parameters.AddWithValue("@V", Vrednost);
                cmd.Parameters.AddWithValue("@Z", Zaduzio);
                cmd.Parameters.AddWithValue("@DATUM", Datum);
                cmd.Parameters.AddWithValue("@MAG", MagacinID);

                cmd.ExecuteNonQuery();
            }
        }

        public static int Insert(DateTime datumCeka, CekStatus status,int magacinID, int podnosilacBanka, string tekuciRacunGradjana, string brojCeka, DateTime datumValute, double vrednost)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Insert(con, datumCeka, status, magacinID, podnosilacBanka, tekuciRacunGradjana, brojCeka, datumValute, vrednost);
            }
        }
        public static int Insert(FbConnection con, DateTime datumCeka, CekStatus status, int magacinID, int podnosilacBanka, string tekuciRacunGradjana, string brojCeka, DateTime datumValute, double vrednost)
        {
            using(FbCommand cmd = new FbCommand(@"INSERT INTO CEK
                (ID, STATUS, MAGACINID, PODNOSILAC_BANKA, TR_GRADJANA, BROJ_CEKA, DATUM_VALUTE, VREDNOST, DATUM)
                VALUES
                (((SELECT COALESCE(MAX(ID), 0) FROM CEK) + 1),
                @STATUS,
                @MAGACINID,
                @PODNOSILAC_BANKA,
                @TR,
                @BC,
                @DV,
                @V,
                @DATUM) RETURNING ID", con))
            {
                cmd.Parameters.Add(new FbParameter("ID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });

                cmd.Parameters.AddWithValue("@STATUS", status);
                cmd.Parameters.AddWithValue("@MAGACINID", magacinID);
                cmd.Parameters.AddWithValue("@PODNOSILAC_BANKA", podnosilacBanka);
                cmd.Parameters.AddWithValue("@TR", tekuciRacunGradjana);
                cmd.Parameters.AddWithValue("@BC", brojCeka);
                cmd.Parameters.AddWithValue("@DV", datumValute);
                cmd.Parameters.AddWithValue("@V", vrednost);
                cmd.Parameters.AddWithValue("@DATUM", datumCeka);

                cmd.ExecuteNonQuery();

                return Convert.ToInt32(cmd.Parameters["ID"].Value);
            }
        }

        public static Cek Get(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, id);
            }
        }
        public static Cek Get(FbConnection con, int id)
        {
            using (FbCommand cmd = new FbCommand("SELECT ID, STATUS, MAGACINID, PODNOSILAC_BANKA, TR_GRADJANA, BROJ_CEKA, DATUM_VALUTE, VREDNOST, ZADUZIO, DATUM FROM CEK WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new Cek()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Status = (CekStatus)Convert.ToInt32(dr["STATUS"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            PodnosilacBanka = Convert.ToInt32(dr["PODNOSILAC_BANKA"]),
                            TRGradjana = dr["TR_GRADJANA"].ToString(),
                            BrojCeka = dr["BROJ_CEKA"].ToString(),
                            DatumValute = Convert.ToDateTime(dr["DATUM_VALUTE"]),
                            Vrednost = Convert.ToDouble(dr["VREDNOST"]),
                            Zaduzio = dr["ZADUZIO"] is DBNull ? null : (int?)Convert.ToInt32(dr["ZADUZIO"]),
                            Datum = Convert.ToDateTime(dr["DATUM"])
                        };
                    }

                    return null;
                }
            }
        }

        public static List<Cek> List()
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<Cek> List(FbConnection con)
        {
            List<Cek> list = new List<Cek>();
            using(FbCommand cmd = new FbCommand("SELECT ID, STATUS, MAGACINID, PODNOSILAC_BANKA, TR_GRADJANA, BROJ_CEKA, DATUM_VALUTE, VREDNOST, ZADUZIO, DATUM FROM CEK", con))
            {
                using(FbDataReader dr = cmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        list.Add(new Cek()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Status = (CekStatus)Convert.ToInt32(dr["STATUS"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            PodnosilacBanka = Convert.ToInt32(dr["PODNOSILAC_BANKA"]),
                            TRGradjana = dr["TR_GRADJANA"].ToString(),
                            BrojCeka = dr["BROJ_CEKA"].ToString(),
                            DatumValute = Convert.ToDateTime(dr["DATUM_VALUTE"]),
                            Vrednost = Convert.ToDouble(dr["VREDNOST"]),
                            Zaduzio = dr["ZADUZIO"] is DBNull ? null : (int?)Convert.ToInt32(dr["ZADUZIO"]),
                            Datum = Convert.ToDateTime(dr["DATUM"])
                        });
                    }
                }
            }
            return list;
        }
        public static Task<List<Cek>> ListAsync()
        {
            return Task.Run(() =>
            {
                return List();
            });
        }

        public static List<Cek> ListByMagacinID(int magacinID)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return ListByMagacinID(con, magacinID);
            }
        }
        public static List<Cek> ListByMagacinID(FbConnection con, int magacinID)
        {
            List<Cek> list = new List<Cek>();
            using (FbCommand cmd = new FbCommand("SELECT ID, STATUS, MAGACINID, PODNOSILAC_BANKA, TR_GRADJANA, BROJ_CEKA, DATUM_VALUTE, VREDNOST, ZADUZIO, DATUM FROM CEK WHERE MAGACINID = @MID", con))
            {
                cmd.Parameters.AddWithValue("@MID", magacinID);
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Cek()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Status = (CekStatus)Convert.ToInt32(dr["STATUS"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            PodnosilacBanka = Convert.ToInt32(dr["PODNOSILAC_BANKA"]),
                            TRGradjana = dr["TR_GRADJANA"].ToString(),
                            BrojCeka = dr["BROJ_CEKA"].ToString(),
                            DatumValute = Convert.ToDateTime(dr["DATUM_VALUTE"]),
                            Vrednost = Convert.ToDouble(dr["VREDNOST"]),
                            Zaduzio = dr["ZADUZIO"] is DBNull ? null : (int?)Convert.ToInt32(dr["ZADUZIO"]),
                            Datum = Convert.ToDateTime(dr["DATUM"])
                        });
                    }
                }
            }
            return list;
        }
        public static Task<List<Cek>> ListByMagacinIDAsync(int magacinID)
        {
            return Task.Run(() =>
            {
                return ListByMagacinID(magacinID);
            });
        }

        public static List<Cek> ListByDatum(DateTime datum)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return ListByDatum(con, datum);
            }
        }
        public static List<Cek> ListByDatum(FbConnection con, DateTime datum)
        {
            List<Cek> list = new List<Cek>();
            using (FbCommand cmd = new FbCommand("SELECT ID, STATUS, MAGACINID, PODNOSILAC_BANKA, TR_GRADJANA, BROJ_CEKA, DATUM_VALUTE, VREDNOST, ZADUZIO, DATUM FROM CEK WHERE DATUM = @D", con))
            {
                cmd.Parameters.AddWithValue("@D", datum);
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new Cek()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Status = (CekStatus)Convert.ToInt32(dr["STATUS"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            PodnosilacBanka = Convert.ToInt32(dr["PODNOSILAC_BANKA"]),
                            TRGradjana = dr["TR_GRADJANA"].ToString(),
                            BrojCeka = dr["BROJ_CEKA"].ToString(),
                            DatumValute = Convert.ToDateTime(dr["DATUM_VALUTE"]),
                            Vrednost = Convert.ToDouble(dr["VREDNOST"]),
                            Zaduzio = dr["ZADUZIO"] is DBNull ? null : (int?)Convert.ToInt32(dr["ZADUZIO"]),
                            Datum = Convert.ToDateTime(dr["DATUM"])
                        });
                    }
                }
            }
            return list;
        }

        public static void Delete(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Delete(con, id);
            }
        }
        public static void Delete(FbConnection con, int id)
        {
            using(FbCommand cmd = new FbCommand("DELETE FROM CEK WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
