using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
    public static class TekuciRacunManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static TekuciRacunList List()
        {
            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[DB.Settings.MainMagacinKomercijalno, DateTime.Now.Year]))
            {
                con.Open();
                return List(con);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static TekuciRacunList List(FbConnection con)
        {
            List<TekuciRacun> list = new List<TekuciRacun>();

            using(FbCommand cmd = new FbCommand("SELECT * FROM TEKUCIRACUN", con))
            {
                using(FbDataReader dr = cmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        list.Add(new TekuciRacun()
                            {
                                Racun = dr["RACUN"].ToString(),
                                PPID = Convert.ToInt32(dr["PPID"]),
                                BankaID = Convert.ToInt32(dr["BANKAID"]),
                                Valuta = dr["VALUTA"].ToString(),
                                Stanje = Convert.ToDouble(dr["STANJE"]),
                                MagacinID = dr["MAGACINID"] is DBNull ? null : (int?)Convert.ToInt32(dr["MAGACINID"])
                            });
                    }
                }
            }

            return new TekuciRacunList(list);
        }
    }
}
