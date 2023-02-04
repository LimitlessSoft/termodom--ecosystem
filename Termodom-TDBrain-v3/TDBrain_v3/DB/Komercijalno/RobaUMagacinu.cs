using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Collections;

namespace TDBrain_v3.DB.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public class RobaUMagacinu
    {
        /// <summary>
        /// Kolekcija koja sadrzi podatke o robi u magacinu.
        /// Kolekcija je dvodimenzionalni Dictionary.
        /// U prvom nivou key predstavlja magacinid.
        /// U drugom nivou key predstavlja robaid, a value je objekat RobaUMagacinu
        /// </summary>
        public class RobaUMagacinuCollection : IEnumerable<Dictionary<int, RobaUMagacinu>>
        {
            private Dictionary<int, Dictionary<int, RobaUMagacinu>> _dict { get; set; }

            /// <summary>
            /// Vraca objekat RobaUMagacinu za dati magacin i robu id
            /// </summary>
            /// <param name="magacinid"></param>
            /// <param name="robaid"></param>
            /// <returns></returns>
            public RobaUMagacinu this[int magacinid, int robaid] => _dict[magacinid][robaid];

            /// <summary>
            /// Vraca dictionary sa listom robe na osnovu prosledjenog magacina.
            /// Key = robaid, Value = RobaUMagacinu
            /// </summary>
            /// <param name="magacinid"></param>
            /// <returns></returns>
            public Dictionary<int, RobaUMagacinu> this[int magacinid] => _dict[magacinid];

            /// <summary>
            /// Kreira kolekciju robe u magacinu
            /// </summary>
            /// <param name="dict"></param>
            public RobaUMagacinuCollection(Dictionary<int, Dictionary<int, RobaUMagacinu>> dict) => _dict = dict;

            /// <summary>
            /// Vraca enumerator nad vrednostima kolekcije
            /// </summary>
            /// <returns></returns>
            IEnumerator<Dictionary<int, RobaUMagacinu>> IEnumerable<Dictionary<int, RobaUMagacinu>>.GetEnumerator() => _dict.Values.GetEnumerator();

            /// <summary>
            /// Vraca enumerator nad vrednostima kolekcije
            /// </summary>
            /// <returns></returns>
            public IEnumerator GetEnumerator() => _dict.Values.GetEnumerator();
        }
        #region Properties
        public int MagacinID { get; set; }
        public int RobaID { get; set; }
        public double ProdajnaCena { get; set; }
        public double Stanje { get; set; }
        public double OptimalneZalihe { get; set; }
        public double KriticneZalihe { get; set; }
        public double NabavnaCena { get; set; }
        #endregion

        public RobaUMagacinu()
        {

        }

        public static void Insert(FbConnection con, int magacinID, int robaID)
        {
            using(FbCommand cmd = new FbCommand("INSERT INTO ROBAUMAGACINU (MAGACINID, ROBAID) VALUES (@M, @R)", con))
            {
                cmd.Parameters.AddWithValue("@M", magacinID);
                cmd.Parameters.AddWithValue("@R", robaID);

                cmd.ExecuteNonQuery();
            }
        }
        public static RobaUMagacinu? Get(int magacinID, int godina, int robaID)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                return Get(con, magacinID, robaID);
            }
        }
        public static RobaUMagacinu? Get(FbConnection con, int magacinID, int robaID)
        {
            RobaUMagacinu? robum = null;

            using (FbCommand cmd = new FbCommand("SELECT MAGACINID, ROBAID, PRODAJNACENA, STANJE, OPTZAL, KRITZAL, NABAVNACENA FROM ROBAUMAGACINU WHERE MAGACINID = @MID AND ROBAID = @RID", con))
            {
                cmd.Parameters.AddWithValue("@MID", magacinID);
                cmd.Parameters.AddWithValue("@RID", robaID);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        robum = new RobaUMagacinu()
                        {
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            RobaID = Convert.ToInt32(dr["ROBAID"]),
                            KriticneZalihe = Convert.ToDouble(dr["KRITZAL"]),
                            OptimalneZalihe = Convert.ToDouble(dr["OPTZAL"]),
                            ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
                            NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
                            Stanje = Convert.ToDouble(dr["STANJE"])
                        };


            }
            return robum;
        }
        public static List<RobaUMagacinu> List(int magacinID, int godina, List<string>? whereParameters = null)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                return List(con, whereParameters);
            }
        }
        public static List<RobaUMagacinu> List(FbConnection con, List<string>? whereParameters = null)
        {
            string whereQuery = "";
            if (whereParameters != null && whereParameters.Count > 0)
                whereQuery = $" WHERE {string.Join(", ", whereParameters)}";

            List<RobaUMagacinu> list = new List<RobaUMagacinu>();

            using (FbCommand cmd = new FbCommand("SELECT MAGACINID, ROBAID, PRODAJNACENA, STANJE, OPTZAL, KRITZAL, NABAVNACENA FROM ROBAUMAGACINU" + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new RobaUMagacinu()
                        {
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            RobaID = Convert.ToInt32(dr["ROBAID"]),
                            KriticneZalihe = Convert.ToDouble(dr["KRITZAL"]),
                            OptimalneZalihe = Convert.ToDouble(dr["OPTZAL"]),
                            ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
                            NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
                            Stanje = Convert.ToDouble(dr["STANJE"])
                        });
            }
            return list;
        }
        /// <summary>
        /// Vraca kolekciju robe u magacinu
        /// </summary>
        /// <param name="con"></param>
        /// <param name="whereParameters"></param>
        /// <returns></returns>
        public static RobaUMagacinuCollection Collection(FbConnection con, List<string> whereParameters = null)
        {
            string whereQuery = "";
            if (whereParameters != null && whereParameters.Count > 0)
                whereQuery = $" WHERE {string.Join(", ", whereParameters)}";

            Dictionary<int, Dictionary<int, RobaUMagacinu>> dict = new Dictionary<int, Dictionary<int, RobaUMagacinu>>();

            using (FbCommand cmd = new FbCommand("SELECT MAGACINID, ROBAID, PRODAJNACENA, STANJE, OPTZAL, KRITZAL, NABAVNACENA FROM ROBAUMAGACINU" + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        int magId = Convert.ToInt32(dr["MAGACINID"]);
                        if (!dict.ContainsKey(magId))
                            dict.Add(magId, new Dictionary<int, RobaUMagacinu>());

                        dict[magId].Add(Convert.ToInt32(dr["ROBAID"]), new RobaUMagacinu()
                        {
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            RobaID = Convert.ToInt32(dr["ROBAID"]),
                            KriticneZalihe = Convert.ToDouble(dr["KRITZAL"]),
                            OptimalneZalihe = Convert.ToDouble(dr["OPTZAL"]),
                            ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
                            NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
                            Stanje = Convert.ToDouble(dr["STANJE"])
                        });
                    }
            }
            return new RobaUMagacinuCollection(dict);
        }
    }
}
