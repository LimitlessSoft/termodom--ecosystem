using FirebirdSql.Data.FirebirdClient;

namespace TDBrain_v3.DB.Komercijalno
{
    public class Mesta
    {
        /// <summary>
        /// Vraca dictionary objekata mesta iz baze
        /// </summary>
        /// <returns></returns>
        public static Termodom.Data.Entities.Komercijalno.MestoDictionary Dictionary(int? godina = null)
        {
            Dictionary<string, Termodom.Data.Entities.Komercijalno.Mesto> dict = new Dictionary<string, Termodom.Data.Entities.Komercijalno.Mesto>();

            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[DB.Settings.MainMagacinKomercijalno, godina ?? DateTime.Now.Year]))
            {
                con.Open();
                using(FbCommand cmd = new FbCommand("SELECT * FROM MESTA", con))
                {
                    using(FbDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {
                            dict.Add(dr["MESTOID"].ToString(), new Termodom.Data.Entities.Komercijalno.Mesto()
                            {
                                MestoID = dr["MESTOID"].ToString(),
                                Naziv = dr["NAZIV"] is DBNull ? null : dr["NAZIV"].ToString()
                            });
                        }
                    }
                }
            }
            return new Termodom.Data.Entities.Komercijalno.MestoDictionary(dict);
        }
    }
}
