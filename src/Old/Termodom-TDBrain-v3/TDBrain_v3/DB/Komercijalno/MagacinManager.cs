using FirebirdSql.Data.FirebirdClient;
using System.Collections;
using TDBrain_v3.Controllers.Komercijalno;

namespace TDBrain_v3.DB.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public class MagacinManager
    {
        /// <summary>
        /// 
        /// </summary>
        public class MagacinCollection : IEnumerable<MagacinManager>
        {
            private Dictionary<int, MagacinManager> _dict { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="magacinID"></param>
            /// <returns></returns>
            public MagacinManager this[int magacinID] => _dict[magacinID];

            /// <summary>
            /// 
            /// </summary>
            /// <param name="dict"></param>
            public MagacinCollection(Dictionary<int, MagacinManager> dict) => _dict = dict;

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IEnumerator<MagacinManager> GetEnumerator() => _dict.Values.GetEnumerator();

            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        public int ID { get; set; }
        public string? Naziv { get; set; }
        public string? MTID { get; set; }
        public int MozeMinus { get; set; }
        public int Vrsta { get; set; }
        public int? PFRID { get; set; }

        public static MagacinManager? Get(int godina, int magacinID)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                return Get(con, magacinID);
            }
        }
        public static MagacinManager? Get(FbConnection con, int magacinID)
        {
            using (FbCommand cmd = new FbCommand("SELECT MAGACINID, NAZIV, MTID, MOZEMINUS, VRSTA, PFRID FROM MAGACIN WHERE MAGACINID = @MID", con))
            {
                cmd.Parameters.AddWithValue("@MID", magacinID);
                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new MagacinManager()
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

        /// <summary>
        /// Vraca kolekciju magacina iz baze za prosledjenu godinu.
        /// Podaci se uzimaju iz baze koja je postavljena kao baza TDBrain_v3.DB.MainMagacinKomercijalno magacina.
        /// </summary>
        /// <param name="godina"></param>
        /// <returns></returns>
        public static MagacinCollection Collection(int godina)
        {
            using(FbConnection con = new FbConnection(Settings.ConnectionStringKomercijalno[Settings.MainMagacinKomercijalno, godina]))
            {
                con.Open();
                return Collection(con);
            }
        }
        /// <summary>
        /// Vraca kolekciju magacina iz baze prosledjene konekcije
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static MagacinCollection Collection(FbConnection con)
        {
            Dictionary<int, MagacinManager> dict = new Dictionary<int, MagacinManager>();
            using (FbCommand cmd = new FbCommand("SELECT * FROM MAGACIN", con))
            {
                using(FbDataReader dr = cmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        dict.Add(Convert.ToInt32(dr["MAGACINID"]), new MagacinManager()
                        {
                            ID = Convert.ToInt16(dr["MAGACINID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            MTID = dr["MTID"].ToString(),
                            MozeMinus = Convert.ToInt32(dr["MOZEMINUS"]),
                            Vrsta = Convert.ToInt32(dr["VRSTA"]),
                            PFRID = dr["PFRID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PFRID"])
                        });
                    }
                }
            }
            return new MagacinCollection(dict);
        }
    }
}
