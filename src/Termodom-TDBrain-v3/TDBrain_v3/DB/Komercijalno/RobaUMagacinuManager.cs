using FirebirdSql.Data.FirebirdClient;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Collections;
using System.Collections.ObjectModel;

namespace TDBrain_v3.DB.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public class RobaUMagacinuManager
    {
        /// <summary>
        /// Kolekcija koja sadrzi podatke o robi u magacinu.
        /// Kolekcija je dvodimenzionalni Dictionary.
        /// U prvom nivou key predstavlja magacinid.
        /// U drugom nivou key predstavlja robaid, a value je objekat RobaUMagacinu
        /// </summary>
        public class RobaUMagacinuCollection : IEnumerable<Dictionary<int, RobaUMagacinuManager>>
        {
            private Dictionary<int, Dictionary<int, RobaUMagacinuManager>> _dict { get; set; }

            /// <summary>
            /// Vraca objekat RobaUMagacinu za dati magacin i robu id
            /// </summary>
            /// <param name="magacinid"></param>
            /// <param name="robaid"></param>
            /// <returns></returns>
            public RobaUMagacinuManager this[int magacinid, int robaid] => _dict[magacinid][robaid];

            /// <summary>
            /// Vraca dictionary sa listom robe na osnovu prosledjenog magacina.
            /// Key = robaid, Value = RobaUMagacinu
            /// </summary>
            /// <param name="magacinid"></param>
            /// <returns></returns>
            public Dictionary<int, RobaUMagacinuManager> this[int magacinid] => _dict[magacinid];

            /// <summary>
            /// Kreira kolekciju robe u magacinu
            /// </summary>
            /// <param name="dict"></param>
            public RobaUMagacinuCollection(Dictionary<int, Dictionary<int, RobaUMagacinuManager>> dict) => _dict = dict;

            /// <summary>
            /// Vraca enumerator nad vrednostima kolekcije
            /// </summary>
            /// <returns></returns>
            IEnumerator<Dictionary<int, RobaUMagacinuManager>> IEnumerable<Dictionary<int, RobaUMagacinuManager>>.GetEnumerator() => _dict.Values.GetEnumerator();

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

        public RobaUMagacinuManager()
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
        public static RobaUMagacinuManager? Get(int magacinID, int godina, int robaID)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                return Get(con, magacinID, robaID);
            }
        }
        public static RobaUMagacinuManager? Get(FbConnection con, int magacinID, int robaID)
        {
            RobaUMagacinuManager? robum = null;

            using (FbCommand cmd = new FbCommand("SELECT MAGACINID, ROBAID, PRODAJNACENA, STANJE, OPTZAL, KRITZAL, NABAVNACENA FROM ROBAUMAGACINU WHERE MAGACINID = @MID AND ROBAID = @RID", con))
            {
                cmd.Parameters.AddWithValue("@MID", magacinID);
                cmd.Parameters.AddWithValue("@RID", robaID);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        robum = new RobaUMagacinuManager()
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
        public static List<RobaUMagacinuManager> List(int magacinID, int godina, List<string>? whereParameters = null)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                return List(con, whereParameters);
            }
        }
        public static List<RobaUMagacinuManager> List(FbConnection con, List<string>? whereParameters = null)
        {
            string whereQuery = "";
            if (whereParameters != null && whereParameters.Count > 0)
                whereQuery = $" WHERE {string.Join(", ", whereParameters)}";

            List<RobaUMagacinuManager> list = new List<RobaUMagacinuManager>();

            using (FbCommand cmd = new FbCommand("SELECT MAGACINID, ROBAID, PRODAJNACENA, STANJE, OPTZAL, KRITZAL, NABAVNACENA FROM ROBAUMAGACINU" + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new RobaUMagacinuManager()
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

            Dictionary<int, Dictionary<int, RobaUMagacinuManager>> dict = new Dictionary<int, Dictionary<int, RobaUMagacinuManager>>();

            using (FbCommand cmd = new FbCommand("SELECT MAGACINID, ROBAID, PRODAJNACENA, STANJE, OPTZAL, KRITZAL, NABAVNACENA FROM ROBAUMAGACINU" + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        int magId = Convert.ToInt32(dr["MAGACINID"]);
                        if (!dict.ContainsKey(magId))
                            dict.Add(magId, new Dictionary<int, RobaUMagacinuManager>());

                        dict[magId].Add(Convert.ToInt32(dr["ROBAID"]), new RobaUMagacinuManager()
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

        /// <summary>
        /// Vraca dictionary robe u magacinu
        /// </summary>
        /// <param name="con"></param>
        /// <param name="whereParameters"></param>
        /// <returns></returns>
        public static Termodom.Data.Entities.Komercijalno.RobaUMagacinuDictionary Dictionary(FbConnection con, List<string> whereParameters = null)
        {
            Dictionary<int, Dictionary<int, Termodom.Data.Entities.Komercijalno.RobaUMagacinu>> dict = new Dictionary<int, Dictionary<int, Termodom.Data.Entities.Komercijalno.RobaUMagacinu>>();

            string whereQuery = "";
            if (whereParameters != null && whereParameters.Count > 0)
                whereQuery = $" WHERE {string.Join(", ", whereParameters)}";

            using (FbCommand cmd = new FbCommand("SELECT MAGACINID, ROBAID, PRODAJNACENA, STANJE, OPTZAL, KRITZAL, NABAVNACENA FROM ROBAUMAGACINU" + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                    {
                        int magId = Convert.ToInt32(dr["MAGACINID"]);

                        if (!dict.ContainsKey(magId))
                            dict.Add(magId, new Dictionary<int, Termodom.Data.Entities.Komercijalno.RobaUMagacinu>());

                        dict[magId].Add(Convert.ToInt32(dr["ROBAID"]), new Termodom.Data.Entities.Komercijalno.RobaUMagacinu()
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

            Dictionary<int, ReadOnlyDictionary<int, Termodom.Data.Entities.Komercijalno.RobaUMagacinu>> finalDict = new Dictionary<int, ReadOnlyDictionary<int, Termodom.Data.Entities.Komercijalno.RobaUMagacinu>>();

            foreach (int key in dict.Keys)
                finalDict.Add(key, new ReadOnlyDictionary<int, Termodom.Data.Entities.Komercijalno.RobaUMagacinu>(dict[key]));

            return new Termodom.Data.Entities.Komercijalno.RobaUMagacinuDictionary(finalDict);
        }
    }
}
