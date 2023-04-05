using FirebirdSql.Data.FirebirdClient;
using LimitlessSoft.Buffer;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TDOffice_v2.API;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2.Komercijalno
{
    public class Magacin
    {
        /// <summary>
        /// 
        /// </summary>
        public class MagacinCollection : IEnumerable<Magacin>
        {
            private Dictionary<int, Magacin> _dict { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="magacinID"></param>
            /// <returns></returns>
            public Magacin this[int magacinID] => _dict[magacinID];

            /// <summary>
            /// 
            /// </summary>
            /// <param name="dict"></param>
            public MagacinCollection(Dictionary<int, Magacin> dict) => _dict = dict;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IEnumerator<Magacin> GetEnumerator() => _dict.Values.GetEnumerator();

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        public int ID { get; set; }
        public string Naziv { get; set; }
        public string MTID { get; set; }
        public int MozeMinus { get; set; }
        public int Vrsta { get; set; }
        public int? PFRID { get; set; }
        public Magacin()
        {

        }

        public void Update(int godina)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                Update(con);
            }
        }
        public void Update(FbConnection con)
        {
            using (FbCommand cmd = new FbCommand("UPDATE MAGACIN SET NAZIV = @NAZIV, MTID = @MTID, MOZEMINUS = @MOZEMINUS, VRSTA = @VRSTA, PFRID = @PFRID WHERE MAGACINID = @MID", con))
            {
                cmd.Parameters.AddWithValue("@MID", ID);
                cmd.Parameters.AddWithValue("@NAZIV", Naziv);
                cmd.Parameters.AddWithValue("@MTID", MTID);
                cmd.Parameters.AddWithValue("@MOZEMINUS", MozeMinus);
                cmd.Parameters.AddWithValue("@VRSTA", Vrsta);
                cmd.Parameters.AddWithValue("@PFRID", PFRID);

                cmd.ExecuteNonQuery();
            }
        }
        [Obsolete("Treba prebaciti na API!")]
        public static Magacin Get(int godina, int magacinID)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Get(con, magacinID);
            }
        }
        [Obsolete("Treba prebaciti na API!")]
        public static Magacin Get(FbConnection con, int magacinID)
        {
            using (FbCommand cmd = new FbCommand("SELECT MAGACINID, NAZIV, MTID, MOZEMINUS, VRSTA, PFRID FROM MAGACIN WHERE MAGACINID = @MID", con))
            {
                cmd.Parameters.AddWithValue("@MID", magacinID);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new Magacin()
                        {
                            ID = Convert.ToInt16(dr["MAGACINID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            MTID = dr["MTID"].ToString(),
                            MozeMinus = Convert.ToInt32(dr["MOZEMINUS"]),
                            Vrsta = Convert.ToInt32(dr["VRSTA"]),
                            PFRID = dr["PFRID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PFRID"])
                        };
            }
            return null;
        }
        [Obsolete("Treba prebaciti na API!")]
        public static Task<Magacin> GetAsync(int godina, int magacinID)
        {
            return Task.Run(() =>
            {
                return Get(godina, magacinID);
            });
        }
        /// <summary>
        /// Vraca listu svih magacina iz baze
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        [Obsolete("Koristiti CollectionAsync() ili ListAsync(int? godina)")]
        public static List<Magacin> List(FbConnection con)
        {
            return Dict(con).Values.ToList();
        }
        /// <summary>
        /// Vraca dictionary sa svim magacinima iz baze izabrane godine.
        /// Key = MagacinID, Value = Magacin object
        /// </summary>
        /// <param name="godina"></param>
        /// <returns></returns>
        [Obsolete("Koristiti CollectionAsync() ili DictionaryAsync(int? godina)")]
        public static Dictionary<int, Magacin> Dict(int godina)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Dict(con);
            }
        }
        /// <summary>
        /// Vraca dictionary sa svim magacinima iz baze.
        /// Key = MagacinID, Value = Magacin object
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        [Obsolete("Koristiti CollectionAsync() ili DictionaryAsync(int? godina)")]
        public static Dictionary<int, Magacin> Dict(FbConnection con)
        {
            Dictionary<int, Magacin> dict = new Dictionary<int, Magacin>();

            using (FbCommand cmd = new FbCommand("SELECT MAGACINID, NAZIV, MTID, MOZEMINUS, VRSTA, PFRID FROM MAGACIN", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        dict.Add(Convert.ToInt32(dr["MAGACINID"]), new Magacin()
                        {
                            ID = Convert.ToInt16(dr["MAGACINID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            MTID = dr["MTID"].ToString(),
                            MozeMinus = Convert.ToInt32(dr["MOZEMINUS"]),
                            Vrsta = Convert.ToInt32(dr["VRSTA"]),
                            PFRID = dr["PFRID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PFRID"])
                        });
            }
            return dict;
        }

        /// <summary>
        /// Vraca listu magacina
        /// </summary>
        /// <param name="godina"></param>
        /// <returns></returns>
        public static async Task<List<Magacin>> ListAsync(int? godina = null)
        {
            return (await CollectionAsync(godina)).ToList();
        }

        public static List<Tuple<int, double>> PrometList(DateTime datum)
        {
            DateTime odDatuma = new DateTime(datum.Year, datum.Month, datum.Day, 0, 0, 1);
            DateTime doDatuma = new DateTime(datum.Year, datum.Month, datum.Day, 23, 59, 59);

            List<Tuple<int, double>> list = new List<Tuple<int, double>>();
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand(@"SELECT SUM(POTRAZUJE) FROM DOKUMENT WHERE
                            VRDOK = 15 AND MAGACINID = @MagacinID AND DATUM >= @DatumOd AND DATUM <= @DatumDo", con))
                {
                    cmd.Parameters.Add("@MagacinID", FbDbType.Integer);
                    cmd.Parameters.AddWithValue("@DatumOd", odDatuma);
                    cmd.Parameters.AddWithValue("@DatumDo", doDatuma);

                    for (int i = 12; i < 29; i++)
                    {
                        cmd.Parameters["@MagacinID"].Value = i;
                        using (FbDataReader dr = cmd.ExecuteReader())
                            if (dr.Read())
                                list.Add(new Tuple<int, double>(i, (dr[0] is DBNull) ? 0 : Convert.ToDouble(dr[0])));
                    }
                }
                con.Close();
            }
            return list;
        }
        public static List<Tuple<int, double>> PrometList(DateTime odDatuma, DateTime doDatuma)
        {
            odDatuma = new DateTime(odDatuma.Year, odDatuma.Month, odDatuma.Day, 0, 0, 1);
            doDatuma = new DateTime(doDatuma.Year, doDatuma.Month, doDatuma.Day, 23, 59, 59);
            List<Tuple<int, double>> list = new List<Tuple<int, double>>();
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT SUM(POTRAZUJE) FROM DOKUMENT WHERE VRDOK = 15 AND MAGACINID = @MagacinID AND DATUM >= @DatumOd AND DATUM <= @DatumDo", con))
                {
                    cmd.Parameters.Add("@MagacinID", FbDbType.Integer);
                    cmd.Parameters.AddWithValue("@DatumOd", odDatuma);
                    cmd.Parameters.AddWithValue("@DatumDo", doDatuma);

                    for (int i = 12; i < 29; i++)
                    {
                        cmd.Parameters["@MagacinID"].Value = i;
                        using (FbDataReader dr = cmd.ExecuteReader())
                            if (dr.Read())
                                list.Add(new Tuple<int, double>(i, (dr[0] is DBNull) ? 0 : Convert.ToDouble(dr[0])));
                    }
                }
                con.Close();
            }
            return list;
        }
        public static List<Tuple<int, double>> Prometist(DateTime odDatuma, DateTime doDatuma, NacinUplate nacinUplate)
        {
            odDatuma = new DateTime(odDatuma.Year, odDatuma.Month, odDatuma.Day, 0, 0, 1);
            doDatuma = new DateTime(doDatuma.Year, doDatuma.Month, doDatuma.Day, 23, 59, 59);
            List<Tuple<int, double>> list = new List<Tuple<int, double>>();
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT SUM(POTRAZUJE) FROM DOKUMENT WHERE VRDOK = 15 AND MAGACINID = @MagacinID AND DATUM >= @DatumOd AND DATUM <= @DatumDo AND NUID = @NUID", con))
                {
                    cmd.Parameters.Add("@MagacinID", FbDbType.Integer);
                    cmd.Parameters.AddWithValue("@DatumOd", odDatuma);
                    cmd.Parameters.AddWithValue("@DatumDo", doDatuma);
                    cmd.Parameters.AddWithValue("@NUID", (int)nacinUplate);

                    for (int i = 12; i < 29; i++)
                    {
                        cmd.Parameters["@MagacinID"].Value = i;
                        using (FbDataReader dr = cmd.ExecuteReader())
                            if (dr.Read())
                                list.Add(new Tuple<int, double>(i, (dr[0] is DBNull) ? 0 : Convert.ToDouble(dr[0])));
                    }
                }
                con.Close();
            }
            return list;
        }

        public static Tuple<int, double> Promet(int magacinID, DateTime datum)
        {
            DateTime odDatuma = new DateTime(datum.Year, datum.Month, datum.Day, 0, 0, 1);
            DateTime doDatuma = new DateTime(datum.Year, datum.Month, datum.Day, 23, 59, 59);
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT SUM(POTRAZUJE) FROM DOKUMENT WHERE VRDOK = 15 AND MAGACINID = @MagacinID AND DATUM = @DatumOd AND DATUM <= @DatumDo", con))
                {
                    cmd.Parameters.AddWithValue("@MagacinID", magacinID);
                    cmd.Parameters.AddWithValue("@DatumOd", odDatuma);
                    cmd.Parameters.AddWithValue("@DatumDo", doDatuma);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new Tuple<int, double>(magacinID, (dr[0] is DBNull) ? 0 : Convert.ToDouble(dr[0]));
                }
                con.Close();
            }
            return null;
        }
        public static Tuple<int, double> Promet(int magacinID, DateTime datum, NacinUplate nacinUplate)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT SUM(POTRAZUJE) FROM DOKUMENT WHERE VRDOK = 15 AND MAGACINID = @MagacinID AND DATUM = @Datum AND NUID = @N", con))
                {
                    cmd.Parameters.AddWithValue("@MagacinID", magacinID);
                    cmd.Parameters.AddWithValue("@Datum", datum);
                    cmd.Parameters.AddWithValue("@N", nacinUplate);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new Tuple<int, double>(magacinID, (dr[0] is DBNull) ? 0 : Convert.ToDouble(dr[0]));
                }
                con.Close();
            }
            return null;
        }
        public static Tuple<int, double> Promet(int magacinID, DateTime odDatuma, DateTime doDatuma)
        {
           
            odDatuma = new DateTime(odDatuma.Year, odDatuma.Month, odDatuma.Day, 0, 0, 1);
            doDatuma = new DateTime(doDatuma.Year, doDatuma.Month, doDatuma.Day, 23, 59, 59);
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT SUM(POTRAZUJE) FROM DOKUMENT WHERE VRDOK = 15 AND MAGACINID = @MagacinID AND DATUM >= @DatumOd AND DATUM <= @DatumDo", con))
                {
                    cmd.Parameters.AddWithValue("@MagacinID", magacinID);
                    cmd.Parameters.AddWithValue("@DatumOd", odDatuma);
                    cmd.Parameters.AddWithValue("@DatumDo", doDatuma);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new Tuple<int, double>(magacinID, (dr[0] is DBNull) ? 0 : Convert.ToDouble(dr[0]));
                }
                con.Close();
            }
            return null;
        }
        public static Tuple<int, double> Promet(int magacinID, DateTime odDatuma, DateTime doDatuma, NacinUplate nacinUplate)
        {
            odDatuma = new DateTime(odDatuma.Year, odDatuma.Month, odDatuma.Day, 0, 0, 1);
            doDatuma = new DateTime(doDatuma.Year, doDatuma.Month, doDatuma.Day, 23, 59, 59);
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT SUM(POTRAZUJE) FROM DOKUMENT WHERE VRDOK = 15 AND MAGACINID = @MagacinID AND DATUM >= @DatumOd AND DATUM <= @DatumDo AND NUID = @NUID", con))
                {
                    cmd.Parameters.AddWithValue("@MagacinID", magacinID);
                    cmd.Parameters.AddWithValue("@DatumOd", odDatuma);
                    cmd.Parameters.AddWithValue("@DatumDo", doDatuma);
                    cmd.Parameters.AddWithValue("@NUID", (int)nacinUplate);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new Tuple<int, double>(magacinID, (dr[0] is DBNull) ? 0 : Convert.ToDouble(dr[0]));
                }
                con.Close();
            }

            return null;
        }

        /// <summary>
        /// Vraca kolekciju magacina iz baze.
        /// </summary>
        /// <param name="godina">Opcioni parametar. Ukoliko je null onda se uzima trenutna godina u obzir.</param>
        /// <returns></returns>
        /// <
        public async static Task<MagacinCollection> CollectionAsync(int? godina = null)
        {
            var response = await TDBrain_v3.GetAsync($"/komercijalno/magacin/dictionary?godinaBaze={godina ?? DateTime.Now.Year}");

            if((int)response.StatusCode == 200)
            {
                var result = JsonConvert.DeserializeObject<Dictionary<int, Magacin>>(await response.Content.ReadAsStringAsync());
                return new MagacinCollection(result);
            }
            else if ((int)response.StatusCode == 400)
            {
                throw new APIBadRequestException(await response.Content.ReadAsStringAsync());
            }
            else
            {
                throw new Exception($"Greska prilikom komunikacije sa API-jem [{response.StatusCode}]");
            }
        }

        public async static Task<MagacinDictionary> DictionaryAsync(int? godinaBaze = null)
        {
            var response = await TDBrain_v3.GetAsync($"/komercijalno/magacin/dictionary?godinaBaze={godinaBaze ?? DateTime.Now.Year}");

            switch ((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<MagacinDictionary>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
