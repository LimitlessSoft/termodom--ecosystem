using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDOffice_v2.Komercijalno
{
    public static class Procedure
    {
        public static double StanjeDoDatuma(DateTime datum, int magacinID, int robaID)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("STANJE_DO_DATUMA", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("MAGACINID", magacinID);
                    cmd.Parameters.AddWithValue("ROBAID", robaID);
                    cmd.Parameters.AddWithValue("DATUM", datum);

                    cmd.Parameters.Add("PRETHCENA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add("PRETHNAB", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add("PRETHSTANJE", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add("PRETHDEVNAB", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add("PRETHDEVCNA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;

                    cmd.ExecuteScalar();

                    return Convert.ToDouble(cmd.Parameters["PRETHSTANJE"].Value);
                }
            }
        }
        public static int NapraviUslugu(int vrDok, int brDok, int robaID, double cenaBezPdv, double kolicina, double rabat)
        {
            Dokument dokument = Dokument.Get(DateTime.Now.Year, vrDok, brDok);

            if(dokument == null)
                throw new Exception("Dokument ne postoji!");

            Roba roba = Roba.Get(DateTime.Now.Year, robaID);

            if (roba == null)
                throw new Exception("Roba sa datim ID-om nije pronadjena!");

            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();

                double stopa = 20;

                using(FbCommand cmd = new FbCommand("SELECT STOPA FROM TARIFE WHERE TARIFAID = @TARIFAID", con))
                {
                    cmd.Parameters.AddWithValue("@TARIFAID", roba.TarifaID);
                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            stopa = Convert.ToDouble(dr["STOPA"]);
                }

                using(FbCommand cmd = new FbCommand(@"INSERT INTO STAVKA (VRDOK, BRDOK, MAGACINID, ROBAID, VRSTA, NAZIV, NABCENSAPOR, FAKTURNACENA, NABCENABT,
                                            TROSKOVI, NABAVNACENA, PRODCENABP, KOREKCIJA, PRODAJNACENA, DEVIZNACENA, DEVPRODCENA, KOLICINA,
                                            NIVKOL, TARIFAID, IMAPOREZ, POREZ, RABAT, MARZA, TAKSA, AKCIZA, PROSNAB, PRECENA, PRENAB, PROSPROD,
                                            MTID, PT, TREN_STANJE, POREZ_ULAZ, DEVNABCENA, POREZ_IZ)
                                            VALUES (@VRDOK, @BRDOK, @MAGACINID, @ROBAID, 1, @NAZIV, 0, 0, 0, 
                                            0, 0, @CENA, 0, @CENA, 0, 0, @KOL,
                                            0, @TARIFAID, 0, @POREZ, @RABAT, 0, 0, 0, 0, 0, 0, 0, 
                                            @MTID, 'P', 0, 0, 0, @POREZ) RETURNING STAVKAID", con))
                {
                    cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                    cmd.Parameters.AddWithValue("@BRDOK", brDok);
                    cmd.Parameters.AddWithValue("@MAGACINID", dokument.MagacinID);
                    cmd.Parameters.AddWithValue("@NAZIV", roba.Naziv);
                    cmd.Parameters.AddWithValue("@TARIFAID", roba.TarifaID);
                    cmd.Parameters.AddWithValue("@POREZ", stopa);
                    cmd.Parameters.AddWithValue("@MTID", dokument.MTID);
                    cmd.Parameters.AddWithValue("@ROBAID", robaID);
                    cmd.Parameters.AddWithValue("@CENA", cenaBezPdv);
                    cmd.Parameters.AddWithValue("@KOL", kolicina);
                    cmd.Parameters.AddWithValue("@RABAT", rabat);

                    cmd.Parameters.Add(new FbParameter { ParameterName = "STAVKAID", FbDbType = FbDbType.Integer, Direction = System.Data.ParameterDirection.Output });

                    cmd.ExecuteNonQuery();

                    return Convert.ToInt32(cmd.Parameters["STAVKAID"].Value);
                }
            }
        }
        public static void PresaberiDokument(int vrDok, int brDok)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                PresaberiDokument(con, vrDok, brDok);
            }
        }
        public static void PresaberiDokument(FbConnection con, int vrDok, int brDok)
        {
            using (FbCommand cmd = new FbCommand("PRESABERIDOKUMENT", con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("VRDOK", vrDok);
                cmd.Parameters.AddWithValue("BRDOK", brDok);

                cmd.ExecuteNonQuery();
            }
        }
        public static double ProdajnaCenaNaDan(int magacinID, int robaID, DateTime datum)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                return ProdajnaCenaNaDan(con, magacinID, robaID, datum);
            }
        }
        public static double ProdajnaCenaNaDan(FbConnection con, int magacinID, int robaID, DateTime datum)
        {
            using (FbCommand cmd = new FbCommand("CENE_NA_DAN", con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("MAGACINID", magacinID);
                cmd.Parameters.AddWithValue("ROBAID", robaID);
                cmd.Parameters.AddWithValue("DATUM", datum);

                cmd.Parameters.Add("PRODAJNACENA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("NABAVNACENA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteScalar();

                return Math.Round(Convert.ToDouble(cmd.Parameters["PRODAJNACENA"].Value), 2);
            }
        }
        public static double NabavnaCenaNaDan(int magacinID, int robaID, DateTime datum)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("CENE_NA_DAN", con))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("MAGACINID", magacinID);
                    cmd.Parameters.AddWithValue("ROBAID", robaID);
                    cmd.Parameters.AddWithValue("DATUM", datum);

                    cmd.Parameters.Add("PRODAJNACENA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                    cmd.Parameters.Add("NABAVNACENA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;

                    cmd.ExecuteScalar();

                    return Math.Round(Convert.ToDouble(cmd.Parameters["NABAVNACENA"].Value), 4);
                }
            }
        }
        /// <summary>
        /// Item 1 = Nabavna bez PDV
        /// Item 2 = Prodajna sa PDV
        /// </summary>
        /// <param name="magacinID"></param>
        /// <param name="robaID"></param>
        /// <param name="datum"></param>
        /// <returns></returns>
        public static Tuple<double, double> CenaNaDan(FbConnection con, int magacinID, int robaID, DateTime datum)
        {
            using (FbCommand cmd = new FbCommand("CENE_NA_DAN", con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("MAGACINID", magacinID);
                cmd.Parameters.AddWithValue("ROBAID", robaID);
                cmd.Parameters.AddWithValue("DATUM", datum);

                cmd.Parameters.Add("PRODAJNACENA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("NABAVNACENA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteScalar();

                return new Tuple<double, double>(Math.Round(Convert.ToDouble(cmd.Parameters["NABAVNACENA"].Value), 4), Math.Round(Convert.ToDouble(cmd.Parameters["PRODAJNACENA"].Value), 4));
            }
        }
        public static string NextLinked(DateTime datum, int magacinID)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand(@"SELECT MAX(CAST(LINKED AS INTEGER)) FROM DOKUMENT
                                                        WHERE DATUM = @DATUM
                                                        AND MAGACINID = @MAGACINID
                                                        AND(LINKED <> '9999999999') AND(LINKED IS NOT NULL) AND(LINKED <> '')", con))
                {
                    cmd.Parameters.AddWithValue("@DATUM", datum);
                    cmd.Parameters.AddWithValue("@MAGACINID", magacinID);

                    using(FbDataReader dr = cmd.ExecuteReader())
                        if(dr.Read())
                            return (dr[0] is DBNull ? 1 : Convert.ToInt32(dr[0])).ToString("0000000000");
                }
            }

            return "0000000000";
        }
        public static void SrediKarticu(int magacinID, int robaID, DateTime datumOd, bool stopNaMinus = false)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                SrediKarticu(con, magacinID, robaID, datumOd, stopNaMinus);
            }
        }
        public static void SrediKarticu(FbConnection con, int magacinID, int robaID, DateTime datumOd, bool stopNaMinus = false)
        {
            using (FbCommand cmd = new FbCommand("SREDIKARTICU", con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("MAGACINID", magacinID);
                cmd.Parameters.AddWithValue("ROBAID", robaID);
                cmd.Parameters.AddWithValue("DATUM_OD", datumOd);
                cmd.Parameters.AddWithValue("STOP_NA_MINUS", stopNaMinus);

                cmd.Parameters.Add("OK", FbDbType.Integer).Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();
            }
        }
        public static Dictionary<string, double> SrediStanjePP(FbConnection con, int ppid, string mtID, DateTime datum, string valuta, int upisi, int ipodredjeni)
        {
            Dictionary<string, double> dict = new Dictionary<string, double>();
            using (FbCommand cmd = new FbCommand("SREDI_STANJE_PP", con))
            {
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("PID", ppid);
                cmd.Parameters.AddWithValue("MTID", mtID);
                cmd.Parameters.AddWithValue("DATUM", datum);
                cmd.Parameters.AddWithValue("VALUTA", valuta);
                cmd.Parameters.AddWithValue("UPISI", upisi);
                cmd.Parameters.AddWithValue("IPODREDJENI", ipodredjeni);

                cmd.Parameters.Add("ULAZ", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("IZLAZ", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("ULAZ_NA_DAN", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("IZLAZ_NA_DAN", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("ULAZ_NA_DAN_ROK", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("IZLAZ_NA_DAN_DOK", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("KUP_ULAZ", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("KUP_IZLAZ", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("KUP_ULAZ_NA_DAN", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("KUP_IZLAZ_NA_DAN", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("KUP_ULAZ_NA_DAN_ROK", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("KUP_IZLAZ_NA_DAN_ROK", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("DOB_ULAZ", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("DOB_IZLAZ", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("DOB_ULAZ_NA_DAN", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("DOB_IZLAZ_NA_DAN", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("DOB_ULAZ_NA_DAN_ROK", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("DOB_IZLAZ_NA_DAN_ROK", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("POC_STA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("KUP_POC_STA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("DOB_POC_STA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("UK_UPLATA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;
                cmd.Parameters.Add("UK_ISPLATA", FbDbType.Double).Direction = System.Data.ParameterDirection.Output;

                cmd.ExecuteNonQuery();

                dict.Add("ULAZ", Convert.ToDouble(cmd.Parameters["ULAZ"].Value));
                dict.Add("IZLAZ", Convert.ToDouble(cmd.Parameters["IZLAZ"].Value));
                dict.Add("ULAZ_NA_DAN", Convert.ToDouble(cmd.Parameters["ULAZ_NA_DAN"].Value));
                dict.Add("IZLAZ_NA_DAN", Convert.ToDouble(cmd.Parameters["IZLAZ_NA_DAN"].Value));
                dict.Add("ULAZ_NA_DAN_ROK", Convert.ToDouble(cmd.Parameters["ULAZ_NA_DAN_ROK"].Value));
                dict.Add("IZLAZ_NA_DAN_DOK", Convert.ToDouble(cmd.Parameters["IZLAZ_NA_DAN_DOK"].Value));
                dict.Add("KUP_ULAZ", Convert.ToDouble(cmd.Parameters["KUP_ULAZ"].Value));
                dict.Add("KUP_IZLAZ", Convert.ToDouble(cmd.Parameters["KUP_IZLAZ"].Value));
                dict.Add("KUP_ULAZ_NA_DAN", Convert.ToDouble(cmd.Parameters["KUP_ULAZ_NA_DAN"].Value));
                dict.Add("KUP_IZLAZ_NA_DAN", Convert.ToDouble(cmd.Parameters["KUP_IZLAZ_NA_DAN"].Value));
                dict.Add("KUP_ULAZ_NA_DAN_ROK", Convert.ToDouble(cmd.Parameters["KUP_ULAZ_NA_DAN_ROK"].Value));
                dict.Add("KUP_IZLAZ_NA_DAN_ROK", Convert.ToDouble(cmd.Parameters["KUP_IZLAZ_NA_DAN_ROK"].Value));
                dict.Add("DOB_ULAZ", Convert.ToDouble(cmd.Parameters["DOB_ULAZ"].Value));
                dict.Add("DOB_IZLAZ", Convert.ToDouble(cmd.Parameters["DOB_IZLAZ"].Value));
                dict.Add("DOB_ULAZ_NA_DAN", Convert.ToDouble(cmd.Parameters["DOB_ULAZ_NA_DAN"].Value));
                dict.Add("DOB_IZLAZ_NA_DAN", Convert.ToDouble(cmd.Parameters["DOB_IZLAZ_NA_DAN"].Value));
                dict.Add("DOB_ULAZ_NA_DAN_ROK", Convert.ToDouble(cmd.Parameters["DOB_ULAZ_NA_DAN_ROK"].Value));
                dict.Add("DOB_IZLAZ_NA_DAN_ROK", Convert.ToDouble(cmd.Parameters["DOB_IZLAZ_NA_DAN_ROK"].Value));
                dict.Add("POC_STA", Convert.ToDouble(cmd.Parameters["POC_STA"].Value));
                dict.Add("KUP_POC_STA", Convert.ToDouble(cmd.Parameters["KUP_POC_STA"].Value));
                dict.Add("DOB_POC_STA", Convert.ToDouble(cmd.Parameters["DOB_POC_STA"].Value));
                dict.Add("UK_UPLATA", Convert.ToDouble(cmd.Parameters["UK_UPLATA"].Value));
                dict.Add("UK_ISPLATA", Convert.ToDouble(cmd.Parameters["UK_ISPLATA"].Value));
                return dict;
            }
        }
        public static Dictionary<string, double> SrediStanjePP(int ppid, string mtID, DateTime datum, string valuta, int upisi, int ipodredjeni)
        {
            Dictionary<string, double> dict = new Dictionary<string, double>();
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                return SrediStanjePP(con, ppid, mtID, datum, valuta, upisi, ipodredjeni);
            }
        }
    }
}
