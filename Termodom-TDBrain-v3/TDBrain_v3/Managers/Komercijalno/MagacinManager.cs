using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public static class MagacinManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static MagacinDictionary Dictionary(FbConnection con)
        {
            Dictionary<int, Magacin> dict = new Dictionary<int, Magacin>();

            using(FbCommand cmd = new FbCommand("SELECT * FROM MAGACIN"))
            {
                using(FbDataReader dr = cmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        dict.Add(Convert.ToInt32(dr["MAGACINID"]), new Magacin()
                        {
                            ID = Convert.ToInt32(dr["MAGACINID"]),
                            MozeMinus = Convert.ToInt32(dr["MOZEMINUS"]),
                            MTID = dr["MTID"] is DBNull ? null : dr["MTID"].ToString(),
                            Naziv = dr["NAZIV"].ToString(),
                            PFRID = dr["PFRID"] is DBNull ? null : (int?)Convert.ToInt32(dr["PFRID"]),
                            Vrsta = Convert.ToInt32(dr["VRSTA"])
                        });
                    }
                }
            }
            return new MagacinDictionary(dict);
        }
    }
}
