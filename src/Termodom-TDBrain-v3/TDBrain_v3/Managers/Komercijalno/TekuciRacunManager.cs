using FirebirdSql.Data.FirebirdClient;
using TDBrain_v3.RequestBodies.Komercijalno;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
    /// <summary>
    /// 
    /// </summary>
    public static class TekuciRacunManager
    {
        /// <summary>
        /// 
        /// </summary>
        public static void Insert(TekuciRacunInsertRequestBody requestBody)
        {
            var connStrings = new List<string>();
            connStrings.AddRange(DB.Settings.ConnectionStringKomercijalno.GetConnectionStringsDistinct(DateTime.Now.Year));
            connStrings.AddRange(DB.Settings.ConnectionStringKomercijalno.GetConnectionStringsDistinct(DateTime.Now.Year - 1));

            foreach (string connString in connStrings)
            {
                using (FbConnection con = new FbConnection(connString))
                {
                    con.Open();
                    using (FbCommand cmd = new FbCommand("INSERT INTO TEKUCIRACUN (RACUN, PPID, BANKAID, VALUTA, STANJE, MAGACINID) VALUES (@R, @P, @BID, @V, @S, @M)", con))
                    {
                        cmd.Parameters.AddWithValue("@R", requestBody.Racun);
                        cmd.Parameters.AddWithValue("@P", requestBody.PPID);
                        cmd.Parameters.AddWithValue("@BID", requestBody.BankaID);
                        cmd.Parameters.AddWithValue("@V", requestBody.Valuta);
                        cmd.Parameters.AddWithValue("@S", requestBody.Stanje);
                        cmd.Parameters.AddWithValue("@M", requestBody.MagacinID);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
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
