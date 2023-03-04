using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDBrain_v3.Managers.TDOffice_v2
{
    /// <summary>
    /// 
    /// </summary>
    public static class FirmaManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static FirmaDictionary Dictionary()
        {
            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringTDOffice_v2.ConnectionString()))
            {
                con.Open();
                return Dictionary(con);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static FirmaDictionary Dictionary(FbConnection con)
        {
            Dictionary<int, Firma> dict = new Dictionary<int, Firma>();

            using(FbCommand cmd = new FbCommand("SELECT * FROM FIRMA", con))
            {
                using(FbDataReader dr = cmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        dict.Add(Convert.ToInt32(dr["ID"]), new Firma()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            Adresa = dr["ADRESA"].ToString(),
                            GlavniMagacin = Convert.ToInt32(dr["GLAVNI_MAGACIN"]),
                            PPID = Convert.ToInt32(dr["PPID"]),
                            Grad = dr["GRAD"].ToString(),
                            MB = dr["MB"].ToString(),
                            Naziv = dr["NAZIV"].ToString(),
                            PIB = dr["PIB"].ToString(),
                            TR = dr["TR"].ToString()
                        });
                    }
                }
            }

            return new FirmaDictionary(dict);
        }
    }
}
