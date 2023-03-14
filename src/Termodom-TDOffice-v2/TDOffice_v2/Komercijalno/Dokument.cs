using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using LimitlessSoft.Buffer;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using Newtonsoft.Json;

namespace TDOffice_v2.Komercijalno
{
    public class Dokument
    {
        public enum OstvarenaMarzaTipNabavneCene
        {
            NaDanDokumenta = 0,
            ProsecnaZaPeriodDoDokumenta = 1
        }
        public int VrDok { get; set; }
        public int BrDok { get; set; }
        public string IntBroj { get; set; }
        public int Flag { get; set; }
        public int Placen { get; set; }
        public DateTime Datum { get; set; }
        public DateTime? DatRoka { get; set; }
        public int MagacinID { get; set; }
        public string MTID { get; set; }
        public int RefID { get; set; }
        public int ZapID { get; set; }
        public int KodDok { get; set; }
        public double Duguje { get; set; }
        public double Potrazuje { get; set; }
        public int? PPID { get; set; }
        public NacinUplate NUID { get; set; }
        public double? Troskovi { get; set; }
        public int? VrDokIn { get; set; }
        public int? BrDokIn { get; set; }
        public int? VrDokOut { get; set; }
        public int? BrDokOut { get; set; }
        public Int16? MagID { get; set; }
        public int? AliasU { get; set; }
        public int? Popust1Dana { get; set; }
        public double Razlika { get; set; }
        public string Linked { get; set; }
        static Dokument()
        {
            //_buffer.StartUpdatingLoopAsync(TimeSpan.FromMilliseconds(200));
        }

        /// <summary>
        /// Pokrece proceduru za presabiranje vrednosti trenutnog dokumenta
        /// </summary>
        public void Presaberi()
        {
            Procedure.PresaberiDokument(VrDok, BrDok);
        }

        /// <summary>
        /// Vraca realno ostvarenu marzu na dokumentu
        /// </summary>
        /// <param name="dokumentiNabavkeBuffer">Buffer koji sadrzi dokumente vrste 0, 1, 2, 36. Ako je buffer null bice ucitan iz baze</param>
        /// <param name="stavkeBuffer">Buffer koji sadrzi stavke. Ako je buffer null bice ucitan iz baze</param>
        /// <returns></returns>
        public double OstvarenaMarza(List<Dokument> dokumentiNabavke, List<Stavka> stavkeNabavke, OstvarenaMarzaTipNabavneCene tipCene = OstvarenaMarzaTipNabavneCene.NaDanDokumenta)
        {
            List<Stavka> stavkeOvogDokumenta = Stavka.ListByDokument(DateTime.Now.Year, VrDok, BrDok);

            if (stavkeNabavke == null)
                stavkeNabavke = Stavka.List(DateTime.Now.Year, $"ROBAID IN (" + string.Join(", ", stavkeOvogDokumenta.Select(x => x.RobaID)) + $") AND MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)");

            if (dokumentiNabavke == null)
                dokumentiNabavke = Dokument.List($"MAGACINID = 50 AND VRDOK IN (0, 1, 2, 36)");

            double ukupnaVrednostDokumentaBezPopusta = 0;
            double ukupnaVrednostDokumentaSaPopustom = 0;
            double ukupnaNabavnaVrednostDokumenta = 0;
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[Datum.Year]))
            {
                con.Open();
                foreach (Stavka stavka in stavkeOvogDokumenta)
                {
                    int robaID = Convert.ToInt32(stavka.RobaID);
                    double prodajnaCena = Convert.ToDouble(stavka.ProdCenaBP + stavka.Korekcija);
                    double nabavnaCena = stavka.Vrsta != 1 ? prodajnaCena : tipCene == OstvarenaMarzaTipNabavneCene.NaDanDokumenta ?
                        Komercijalno.GetRealnaNabavnaCena(stavka.RobaID, Datum, dokumentiNabavke, stavkeNabavke):
                        Komercijalno.GetProsecnaNabavnaCena(con, robaID, new DateTime(Datum.Year, 1, 1), Datum);
                    double kolicina = Convert.ToDouble(stavka.Kolicina);
                    double rabat = Convert.ToDouble(stavka.Rabat);
                    double prodajnaVrednostBezPopusta = prodajnaCena * kolicina;
                    double prodajnaVrednostSaPopustom = (prodajnaCena * (1 - (rabat / 100))) * kolicina;
                    double nabavnaVrednost = nabavnaCena * kolicina;
                    double marza = (prodajnaCena / nabavnaCena - 1) * 100;
                    double maxRabat = (nabavnaCena / prodajnaCena - 1) * -100;

                    ukupnaVrednostDokumentaBezPopusta += prodajnaVrednostBezPopusta;
                    ukupnaVrednostDokumentaSaPopustom += prodajnaVrednostSaPopustom;
                    ukupnaNabavnaVrednostDokumenta += nabavnaVrednost;
                }
            }

            return ukupnaNabavnaVrednostDokumenta == 0 || ukupnaVrednostDokumentaSaPopustom == 0 ? 0 : ((ukupnaVrednostDokumentaSaPopustom / ukupnaNabavnaVrednostDokumenta) - 1) * (100);
        }

        /// <summary>
        /// Azurira podatke u bazi. Osnova: VRDOK, BRDOK
        /// </summary>
        public void Update()
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[Datum.Year]))
            {
                con.Open();
                Update(con);
            }
        }
        /// <summary>
        /// Azurira podatke u bazi. Osnova: VRDOK, BRDOK
        /// </summary>
        public void Update(FbConnection con)
        {
            using(FbCommand cmd = new FbCommand(@"UPDATE DOKUMENT SET 
                INTBROJ = @INTBROJ,
                FLAG = @FLAG,
                DATUM = @DATUM,
                DATROKA = @DATROKA,
                MAGACINID = @MAGACINID,
                MTID = @MTID,
                KODDOK = @KODDOK,
                DUGUJE = @DUGUJE,
                POTRAZUJE = @POTRAZUJE,
                PPID = @PPID,
                NUID = @NUID,
                TROSKOVI = @TROSKOVI,
                VRDOKIN = @VRDOKIN,
                BRDOKIN = @BRDOKIN,
                VRDOKOUT = @VRDOKOUT,
                BRDOKOUT = @BRDOKOUT,
                REFID = @REFID,
                ZAPID = @ZAPID,
                RAZLIKA = @RAZLIKA,
                PLACEN = @PLACEN,
                MAGID = @MAGID,
                ALIASU = @ALIASU,
                POPUST1DANA = @P1D,
                LINKED = @LINKED
                WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK", con))
            {
                cmd.Parameters.AddWithValue("@INTBROJ", IntBroj);
                cmd.Parameters.AddWithValue("@FLAG", Flag);
                cmd.Parameters.AddWithValue("@DATUM", Datum);
                cmd.Parameters.AddWithValue("@DATROKA", DatRoka);
                cmd.Parameters.AddWithValue("@MAGACINID", MagacinID);
                cmd.Parameters.AddWithValue("@MTID", MTID);
                cmd.Parameters.AddWithValue("@KODDOK", KodDok);
                cmd.Parameters.AddWithValue("@DUGUJE", Duguje);
                cmd.Parameters.AddWithValue("@POTRAZUJE", Potrazuje);
                cmd.Parameters.AddWithValue("@PPID", PPID);
                cmd.Parameters.AddWithValue("@NUID", NUID);
                cmd.Parameters.AddWithValue("@TROSKOVI", Troskovi);
                cmd.Parameters.AddWithValue("@VRDOKIN", VrDokIn);
                cmd.Parameters.AddWithValue("@VRDOKOUT", VrDokOut);
                cmd.Parameters.AddWithValue("@BRDOKIN", BrDokIn);
                cmd.Parameters.AddWithValue("@BRDOKOUT", BrDokOut);
                cmd.Parameters.AddWithValue("@RAZLIKA", Razlika);
                cmd.Parameters.AddWithValue("@VRDOK", VrDok);
                cmd.Parameters.AddWithValue("@BRDOK", BrDok);
                cmd.Parameters.AddWithValue("@REFID", RefID);
                cmd.Parameters.AddWithValue("@ZAPID", ZapID);
                cmd.Parameters.AddWithValue("@PLACEN", Placen);
                cmd.Parameters.AddWithValue("@MAGID", MagID);
                cmd.Parameters.AddWithValue("@AliasU", AliasU);
                cmd.Parameters.AddWithValue("@P1D", Popust1Dana);
                cmd.Parameters.AddWithValue("@LINKED", Linked);

                cmd.ExecuteNonQuery();
            }
        }

        public static Dokument Get(int godina, int vrDok, int brDok)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Get(con, vrDok, brDok);
            }
        }
        /// <summary>
        /// Vraca dokument iz baze. Ukoliko nije pronadjen vraca null
        /// </summary>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        /// <returns></returns>
        public static Dokument Get(FbConnection con, int vrDok, int brDok)
        {
            Dokument dok = null;
            using (FbCommand cmd = new FbCommand(@"SELECT
INTBROJ, FLAG, DATUM, MAGACINID, 
KODDOK, DUGUJE, MTID, POTRAZUJE,
PPID, NUID, VRDOKOUT, VRDOKIN, BRDOKOUT, BRDOKIN,
REFID, ZAPID, PLACEN, MAGID, DATROKA, ALIASU,
TROSKOVI, POPUST1DANA, RAZLIKA, LINKED
FROM DOKUMENT
WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK", con))
            {
                cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                cmd.Parameters.AddWithValue("@BRDOK", brDok);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        dok = new Dokument()
                        {
                            VrDok = vrDok,
                            BrDok = brDok,
                            Linked = dr["LINKED"].ToString(),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            DatRoka = dr["DATROKA"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATROKA"]),
                            Flag = Convert.ToInt32(dr["FLAG"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            IntBroj = dr["INTBROJ"].ToString(),
                            KodDok = Convert.ToInt32(dr["KODDOK"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            MTID = dr["MTID"].ToString(),
                            Razlika = Convert.ToDouble(dr["RAZLIKA"]),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                            PPID = dr["PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PPID"]),
                            NUID = (NacinUplate)Convert.ToInt32(dr["NUID"]),
                            VrDokOut = dr["VRDOKOUT"] is DBNull ? null : (int?)Convert.ToInt32(dr["VRDOKOUT"]),
                            VrDokIn = dr["VRDOKIN"] is DBNull ? null : (int?)Convert.ToInt32(dr["VRDOKIN"]),
                            BrDokOut = dr["BRDOKOUT"] is DBNull ? null : (int?)Convert.ToInt32(dr["BRDOKOUT"]),
                            BrDokIn = dr["BRDOKIN"] is DBNull ? null : (int?)Convert.ToInt32(dr["BRDOKIN"]),
                            RefID = Convert.ToInt32(dr["REFID"]),
                            ZapID = Convert.ToInt32(dr["ZAPID"]),
                            Placen = Convert.ToInt32(dr["PLACEN"]),
                            MagID = dr["MAGID"] is DBNull ? null : (Int16?)Convert.ToInt16(dr["MAGID"]),
                            AliasU = dr["ALIASU"] is DBNull ? null : (int?)Convert.ToInt32(dr["ALIASU"]),
                            Popust1Dana = dr["POPUST1DANA"] is DBNull ? null : (int?)Convert.ToInt32(dr["POPUST1DANA"]),
                            Troskovi = Convert.ToDouble(dr["TROSKOVI"])
                        };
            }
            return dok;
        }
        public static async Task<Termodom.Data.Entities.Komercijalno.Dokument> GetAsync(int bazaID, int vrDok, int brDok, int? godina)
        {
            if (godina == null)
                godina = DateTime.Now.Year;

            var response = await TDBrain_v3.GetAsync($"/komercijalno/dokument/get?bazaID={bazaID}&vrdok={vrDok}&brdok={brDok}&godina={godina}");

            if(response.StatusCode == System.Net.HttpStatusCode.OK)
                return JsonConvert.DeserializeObject<Termodom.Data.Entities.Komercijalno.Dokument>(await response.Content.ReadAsStringAsync());
            else if((int)response.StatusCode == 204)
                return null;
            else if((int)response.StatusCode == 500)
                throw new Termodom.Data.Exceptions.APIServerException();
            else
                throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
        }
        public static List<Dokument> List(string queryString = null)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                return List(con, queryString);
            }
        }
        public static List<Dokument> List(FbConnection con, string queryString = null)
        {
            if(queryString != null)
                queryString = " AND " + queryString;
            List<Dokument> dok = new List<Dokument>();
            using (FbCommand cmd = new FbCommand(@"SELECT
VRDOK, BRDOK, INTBROJ,
FLAG, DATUM, MAGACINID,
KODDOK, DUGUJE, MTID,
POTRAZUJE, NUID, PPID,
TROSKOVI, VRDOKOUT, VRDOKIN,
BRDOKOUT, BRDOKIN,
REFID, ZAPID, PLACEN, MAGID, LINKED,
DATROKA, ALIASU, POPUST1DANA, RAZLIKA
FROM DOKUMENT WHERE 1 = 1 " + queryString, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        dok.Add(new Dokument()
                        {
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            Linked = dr["LINKED"].ToString(),
                            DatRoka = dr["DATROKA"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATROKA"]),
                            Flag = Convert.ToInt32(dr["FLAG"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            IntBroj = dr["INTBROJ"].ToString(),
                            KodDok = Convert.ToInt32(dr["KODDOK"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            MTID = dr["MTID"].ToString(),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                            Razlika = Convert.ToDouble(dr["RAZLIKA"]),
                            NUID = (NacinUplate)Convert.ToInt32(dr["NUID"]),
                            PPID = dr["PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PPID"]),
                            Troskovi = dr["TROSKOVI"] is DBNull ? null : (double?)Convert.ToDouble(dr["TROSKOVI"]),
                            VrDokOut = dr["VRDOKOUT"] is DBNull ? null : (int?)Convert.ToInt32(dr["VRDOKOUT"]),
                            VrDokIn = dr["VRDOKIN"] is DBNull ? null : (int?)Convert.ToInt32(dr["VRDOKIN"]),
                            BrDokOut = dr["BRDOKOUT"] is DBNull ? null : (int?)Convert.ToInt32(dr["BRDOKOUT"]),
                            BrDokIn = dr["BRDOKIN"] is DBNull ? null : (int?)Convert.ToInt32(dr["BRDOKIN"]),
                            RefID = Convert.ToInt32(dr["REFID"]),
                            ZapID = Convert.ToInt32(dr["ZAPID"]),
                            Placen = Convert.ToInt32(dr["PLACEN"]),
                            MagID = dr["MAGID"] is DBNull ? null : (Int16?)Convert.ToInt16(dr["MAGID"]),
                            AliasU = dr["ALIASU"] is DBNull ? null : (int?)Convert.ToInt32(dr["ALIASU"]),
                            Popust1Dana = dr["POPUST1DANA"] is DBNull ? null : (int?)Convert.ToInt32(dr["POPUST1DANA"]),
                        });
            }
            return dok;
        }
        public static async Task<List<Dokument>> ListAsync(int? magacinID = null, int[] vrDok = null, DateTime? datumOd = null, DateTime? datumDo = null, int? placen = null)
        {
            string path = "/komercijalno/dokument/list";
            string query = "";

            if (magacinID != null)
                query += $"&magacinid={magacinID}";

            if (vrDok != null && vrDok.Length > 0)
                query += $"&{string.Join("&vrdok=", vrDok)}";

            if (datumOd != null)
                query += $"&daumOd={((DateTime)datumOd).ToString("MM-dd-yyyy")}";

            if (datumDo != null)
                query += $"&daumOd={((DateTime)datumDo).ToString("MM-dd-yyyy")}";

            if (placen != null)
                query += $"&placen={placen}";

            if (query.Length > 0)
                query = "?" + query.Substring(1, query.Length - 1);

            var result = await TDBrain_v3.GetAsync(path + query);

            if((int)result.StatusCode == 200)
            {
                return JsonConvert.DeserializeObject<List<Dokument>>(await result.Content.ReadAsStringAsync());
            }
            else
            {
                MessageBox.Show(result.StatusCode.ToString());
                throw new Exception("Greska prilikom ucitavanja liste dokumenata sa API-ja!");
            }
        }
        /// <summary>
        /// Vracam listu dokumenata iz baza sa odredjenim VRDOK
        /// </summary>
        /// <param name="vrDok"></param>
        /// <returns></returns>
        public static List<Dokument> ListByVrDok(int godina, int vrDok)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return ListByVrDok(con, vrDok);
            }
        }
        /// <summary>
        /// Vracam listu dokumenata iz baza sa odredjenim VRDOK
        /// </summary>
        /// <param name="vrDok"></param>
        /// <returns></returns>
        public static List<Dokument> ListByVrDok(FbConnection con, int vrDok)
        {
            List<Dokument> dok = new List<Dokument>();
            using (FbCommand cmd = new FbCommand(@"SELECT VRDOK, BRDOK, INTBROJ, FLAG, 
                    DATUM, MAGACINID, KODDOK, DUGUJE, MTID, POTRAZUJE, NUID, RAZLIKA, PPID, TROSKOVI, VRDOKOUT, VRDOKIN, BRDOKOUT, BRDOKIN, REFID, ZAPID, PLACEN, MAGID, DATROKA, ALIASU FROM DOKUMENT WHERE VRDOK = @V", con))
            {
                cmd.Parameters.AddWithValue("@V", vrDok);

                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        dok.Add(new Dokument()
                        {
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            DatRoka = dr["DATROKA"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATROKA"]),
                            Flag = Convert.ToInt32(dr["FLAG"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            IntBroj = dr["INTBROJ"].ToString(),
                            KodDok = Convert.ToInt32(dr["KODDOK"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            Razlika = Convert.ToDouble(dr["RAZLIKA"]),
                            MTID = dr["MTID"].ToString(),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                            NUID = (NacinUplate)Convert.ToInt32(dr["NUID"]),
                            PPID = dr["PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PPID"]),
                            Troskovi = dr["TROSKOVI"] is DBNull ? 0 : Convert.ToDouble(dr["TROSKOVI"]),
                            VrDokOut = dr["VRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKOUT"]),
                            VrDokIn = dr["VRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKIN"]),
                            BrDokOut = dr["BRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKOUT"]),
                            BrDokIn = dr["BRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKIN"]),
                            RefID = Convert.ToInt32(dr["REFID"]),
                            ZapID = Convert.ToInt32(dr["ZAPID"]),
                            Placen = Convert.ToInt32(dr["PLACEN"]),
                            MagID = dr["MAGID"] is DBNull ? null : (Int16?)Convert.ToInt16(dr["MAGID"]),
                            AliasU = dr["ALIASU"] is DBNull ? null : (int?)Convert.ToInt32(dr["ALIASU"])
                        });
            }
            return dok;
        }
        /// <summary>
        /// Vracam listu dokumenata iz baza sa odredjenim MagacinID-om
        /// </summary>
        /// <param name="vrDok"></param>
        /// <returns></returns>
        public static List<Dokument> ListByMagacinID(int godina, int magacinID)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return ListByMagacinID(con, magacinID);
            }
        }
        /// <summary>
        /// Vracam listu dokumenata iz baza sa odredjenim MagacinID-om
        /// </summary>
        /// <param name="vrDok"></param>
        /// <returns></returns>
        public static Task<List<Dokument>> ListByMagacinIDAsync(int godina, int magacinID)
        {
            return Task.Run(() =>
            {
                return ListByMagacinID(godina, magacinID);
            });
        }
        /// <summary>
        /// Vracam listu dokumenata iz baza sa odredjenim MagacinID-om
        /// </summary>
        /// <param name="vrDok"></param>
        /// <returns></returns>
        public static List<Dokument> ListByMagacinID(FbConnection con, int magacinID)
        {
            List<Dokument> dok = new List<Dokument>();
            using (FbCommand cmd = new FbCommand(@"SELECT VRDOK, BRDOK, INTBROJ, FLAG, 
                    DATUM, MAGACINID, KODDOK, DUGUJE, MTID, POTRAZUJE, NUID, RAZLIKA, PPID, TROSKOVI, VRDOKOUT, VRDOKIN, BRDOKOUT, BRDOKIN, REFID, ZAPID, PLACEN, MAGID, DATROKA, ALIASU FROM DOKUMENT WHERE MAGACINID = @MID", con))
            {
                cmd.Parameters.AddWithValue("@MID", magacinID);

                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        dok.Add(new Dokument()
                        {
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            DatRoka = dr["DATROKA"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATROKA"]),
                            Flag = Convert.ToInt32(dr["FLAG"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            IntBroj = dr["INTBROJ"].ToString(),
                            KodDok = Convert.ToInt32(dr["KODDOK"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            Razlika = Convert.ToDouble(dr["RAZLIKA"]),
                            MTID = dr["MTID"].ToString(),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                            NUID = (NacinUplate)Convert.ToInt32(dr["NUID"]),
                            PPID = dr["PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PPID"]),
                            Troskovi = dr["TROSKOVI"] is DBNull ? 0 : Convert.ToDouble(dr["TROSKOVI"]),
                            VrDokOut = dr["VRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKOUT"]),
                            VrDokIn = dr["VRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKIN"]),
                            BrDokOut = dr["BRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKOUT"]),
                            BrDokIn = dr["BRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKIN"]),
                            RefID = Convert.ToInt32(dr["REFID"]),
                            ZapID = Convert.ToInt32(dr["ZAPID"]),
                            Placen = Convert.ToInt32(dr["PLACEN"]),
                            MagID = dr["MAGID"] is DBNull ? null : (Int16?)Convert.ToInt16(dr["MAGID"]),
                            AliasU = dr["ALIASU"] is DBNull ? null : (int?)Convert.ToInt32(dr["ALIASU"])
                        });
            }
            return dok;
        }
        /// <summary>
        /// Vracam listu dokumenata iz baza sa odredjenim MagacinID-om
        /// </summary>
        /// <param name="vrDok"></param>
        /// <returns></returns>
        public static Task<List<Dokument>> ListByMagacinIDAsync(FbConnection con, int magacinID)
        {
            return Task.Run(() =>
            {
                return ListByMagacinID(con, magacinID);
            });
        }
        /// <summary>
        /// Vraca listu dokumenata iz baze gde PPID nije jednak null-u
        /// </summary>
        /// <returns></returns>
        public static List<Dokument> ListPPIDNotNull(int godina)
        {
            List<Dokument> dok = new List<Dokument>();
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return ListPPIDNotNull(con);
            }
        }
        /// <summary>
        /// Vraca listu dokumenata iz baze gde PPID nije jednak null-u
        /// </summary>
        /// <returns></returns>
        public static List<Dokument> ListPPIDNotNull(FbConnection con)
        {
            List<Dokument> dok = new List<Dokument>();
            using (FbCommand cmd = new FbCommand(@"SELECT VRDOK, BRDOK, INTBROJ, FLAG, DATUM,
                MAGACINID, KODDOK, DUGUJE, MTID, POTRAZUJE, NUID,
                PPID, TROSKOVI, VRDOKOUT, VRDOKIN, BRDOKIN, BRDOKOUT, RAZLIKA,
                REFID, ZAPID, PLACEN, MAGID, DATROKA, ALIASU FROM DOKUMENT WHERE PPID IS NOT NULL", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        dok.Add(new Dokument()
                        {
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            Datum = Convert.ToDateTime(dr["DATUM"]),
                            DatRoka = dr["DATROKA"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATROKA"]),
                            Flag = Convert.ToInt32(dr["FLAG"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            IntBroj = dr["INTBROJ"].ToString(),
                            KodDok = Convert.ToInt32(dr["KODDOK"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            Razlika = Convert.ToDouble(dr["RAZLIKA"]),
                            MTID = dr["MTID"].ToString(),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                            NUID = (NacinUplate)Convert.ToInt32(dr["NUID"]),
                            PPID = dr["PPID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PPID"]),
                            Troskovi = dr["TROSKOVI"] is DBNull ? 0 : Convert.ToDouble(dr["TROSKOVI"]),
                            VrDokOut = dr["VRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKOUT"]),
                            VrDokIn = dr["VRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["VRDOKIN"]),
                            BrDokOut = dr["BRDOKOUT"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKOUT"]),
                            BrDokIn = dr["BRDOKIN"] is DBNull ? 0 : Convert.ToInt32(dr["BRDOKIN"]),
                            RefID = Convert.ToInt32(dr["REFID"]),
                            ZapID = Convert.ToInt32(dr["ZAPID"]),
                            Placen = Convert.ToInt32(dr["PLACEN"]),
                            MagID = dr["MAGID"] is DBNull ? null : (Int16?)Convert.ToInt16(dr["MAGID"]),
                            AliasU = dr["ALIASU"] is DBNull ? null : (int?)Convert.ToInt32(dr["ALIASU"])
                        });
            }
            return dok;
        }
        /// <summary>
        /// Pravi novi dokument unutar baze tabela DOKUMENT i vraca novi broj dokumenta
        /// </summary>
        /// <param name="vrDok"></param>
        /// <param name="interniBroj"></param>
        /// <param name="ppid"></param>
        /// <param name="napomena"></param>
        /// <param name="nacinUplateID"></param>
        /// <param name="magacinID"></param>
        /// <returns></returns>
        /// 
        public static int Insert(int godina, int vrDok, string interniBroj, int? ppid, string napomena, int nacinUplateID, int magacinID, int? komercijalnoKorisnikID, int? magID)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Insert(con, vrDok, interniBroj, ppid, napomena, nacinUplateID, magacinID, komercijalnoKorisnikID, magID);
            }
        }
        /// <summary>
        /// Pravi novi dokument unutar baze tabela DOKUMENT i vraca novi broj dokumenta
        /// </summary>
        /// <param name="vrDok"></param>
        /// <param name="interniBroj"></param>
        /// <param name="ppid"></param>
        /// <param name="napomena"></param>
        /// <param name="nacinUplateID"></param>
        /// <param name="magacinID"></param>
        /// <returns></returns>
        public static int Insert(FbConnection con, int vrDok, string interniBroj, int? ppid, string napomena, int nacinUplateID, int magacinID, int? komercijalnoKorisnikID, int? magID, bool dozvoliDaljeIzmeneUKomercijalnom = false)
        {
            try
            {
                int poslednjiBrDok = 0;
                using (FbCommand cmd = new FbCommand(@"SELECT POSLEDNJI FROM VRSTADOKMAG WHERE VRDOK = @VRDOK AND MAGACINID = @MAGACINID", con))
                {
                    cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                    cmd.Parameters.AddWithValue("@MAGACINID", magacinID);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            poslednjiBrDok = Convert.ToInt32(dr["POSLEDNJI"]);
                }

                if (poslednjiBrDok == 0)
                {
                    using (FbCommand cmd = new FbCommand(@"SELECT POSLEDNJI FROM VRSTADOK WHERE VRDOK = @VRDOK", con))
                    {
                        cmd.Parameters.AddWithValue("@VRDOK", vrDok);

                        using (FbDataReader dr = cmd.ExecuteReader())
                            if (dr.Read())
                                poslednjiBrDok = Convert.ToInt32(dr["POSLEDNJI"]);
                    }
                }

                using (FbCommand cmd = new FbCommand(@"INSERT INTO DOKUMENT (VRDOK, BRDOK, INTBROJ, KODDOK, FLAG, DATUM, LINKED, MAGACINID, PPID, PLACEN, DATROKA,
                                                        NUID, NRID, VALUTA, KURS, ZAPID, UPLACENO, TROSKOVI, DUGUJE, POTRAZUJE, POPUST, RAZLIKA,
                                                        DODPOREZ, POREZ, PRODVREDBP, VRDOKIN, BRDOKIN, MAGID, MTID, REFID, STATUS, PPO, PRENETI_POREZ,
                                                        AKVRDOK, AKBRDOK, PREVOZROBE, DATUM_PDV, NDID, NABVREDNOST, KNJIZNAOZ, POVRATNICE,
                                                        SINHRO, STORNO, SMENAID)
                                                        VALUES (@VRDOK, @BRDOK, @INTERNIBROJ, 0, 1, @DATUM, @LINKED, @MAGACINID, @PPID, 0, @DATUM,
                                                        @NUID, 1, (SELECT VALUTA FROM VRSTADOK WHERE VRDOK = @VRDOK), 1, @ZAPID, 0, 0, 0, 0, 0, 0,
                                                        0, 0, 0, @VRDOKIN, NULL, @MAGID, @MTID, @ZAPID, 0, 1, 0,
                                                        0, 0, @PREVOZROBE, @DATUM, 0, 0, 0, 0,
                                                        0, 0, 0)", con))
                {
                    cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                    cmd.Parameters.AddWithValue("@MAGID", magID);
                    cmd.Parameters.AddWithValue("@BRDOK", poslednjiBrDok + 1);
                    cmd.Parameters.AddWithValue("@INTERNIBROJ", interniBroj);
                    cmd.Parameters.AddWithValue("@DATUM", DateTime.Now);
                    cmd.Parameters.AddWithValue("@LINKED", Procedure.NextLinked(DateTime.Now, magacinID));
                    cmd.Parameters.AddWithValue("@MAGACINID", magacinID);
                    cmd.Parameters.AddWithValue("@MTID", Magacin.Get(DateTime.Now.Year, magacinID).MTID);
                    cmd.Parameters.AddWithValue("@PPID", ppid);
                    cmd.Parameters.AddWithValue("@NUID", nacinUplateID);
                    cmd.Parameters.AddWithValue("@ZAPID", komercijalnoKorisnikID == null ? 1 : komercijalnoKorisnikID);
                    cmd.Parameters.AddWithValue("@PREVOZROBE", ppid == null ? 0 : 1);
                    cmd.Parameters.AddWithValue("@VRDOKIN", dozvoliDaljeIzmeneUKomercijalnom ? (int?)null : 8);

                    cmd.ExecuteNonQuery();

                    return poslednjiBrDok + 1;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw ex;
            }
        }


        /// <summary>
        /// Postavlja komentar za ovaj dokument. Trebalo bi prebaciti ovo u posebnu klasu za tu tabelu
        /// </summary>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        /// <param name="komentar"></param>
        public static void SetKomentar(int godina, int vrDok, int brDok, string komentar)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                bool postoji = false;

                using (FbCommand cmd = new FbCommand("SELECT COUNT(VRDOK) AS BROJ FROM KOMENTARI WHERE VRDOK = @V AND BRDOK = @B", con))
                {
                    cmd.Parameters.AddWithValue("@V", vrDok);
                    cmd.Parameters.AddWithValue("@B", brDok);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            if (Convert.ToInt32(dr[0]) > 0)
                                postoji = true;
                }

                if (postoji)
                {
                    using (FbCommand cmd = new FbCommand("UPDATE KOMENTARI SET KOMENTAR = @KOM WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK", con))
                    {
                        cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                        cmd.Parameters.AddWithValue("@BRDOK", brDok);
                        cmd.Parameters.AddWithValue("@KOM", komentar);

                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (FbCommand cmd = new FbCommand("INSERT INTO KOMENTARI (VRDOK, BRDOK, KOMENTAR) VALUES (@VRDOK, @BRDOK, @KOM)", con))
                    {
                        cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                        cmd.Parameters.AddWithValue("@BRDOK", brDok);
                        cmd.Parameters.AddWithValue("@KOM", komentar);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
        /// <summary>
        /// Postavlja interni komentar za ovaj dokument. Trebalo bi prebaciti ovo u posebnu klasu za tu tabelu
        /// </summary>
        /// <param name="vrDok"></param>
        /// <param name="brDok"></param>
        /// <param name="komentar"></param>
        public static void SetInterniKomentar(int godina, int vrDok, int brDok, string komentar)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                bool postoji = false;

                using (FbCommand cmd = new FbCommand("SELECT COUNT(VRDOK) AS BROJ FROM KOMENTARI WHERE VRDOK = @V AND BRDOK = @B", con))
                {
                    cmd.Parameters.AddWithValue("@V", vrDok);
                    cmd.Parameters.AddWithValue("@B", brDok);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            if (Convert.ToInt32(dr[0]) > 0)
                                postoji = true;
                }

                if (postoji)
                {
                    using (FbCommand cmd = new FbCommand("UPDATE KOMENTARI SET INTKOMENTAR = @KOM WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK", con))
                    {
                        cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                        cmd.Parameters.AddWithValue("@BRDOK", brDok);
                        cmd.Parameters.AddWithValue("@KOM", komentar);

                        cmd.ExecuteNonQuery();
                    }
                }
                else
                {
                    using (FbCommand cmd = new FbCommand("INSERT INTO KOMENTARI (VRDOK, BRDOK, INTKOMENTAR) VALUES (@VRDOK, @BRDOK, @KOM)", con))
                    {
                        cmd.Parameters.AddWithValue("@VRDOK", vrDok);
                        cmd.Parameters.AddWithValue("@BRDOK", brDok);
                        cmd.Parameters.AddWithValue("@KOM", komentar);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
