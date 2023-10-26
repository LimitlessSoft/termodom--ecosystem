using FirebirdSql.Data.FirebirdClient;
using Termodom.Data.Entities.Komercijalno;

namespace TDBrain_v3.DB.Komercijalno
{
    public class VrstaDokManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="vrDok"></param>
        /// <param name="godinaBaze"></param>
        /// <returns></returns>
        public static VrstaDok Get(int vrDok, int? godinaBaze = null)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[DB.Settings.MainMagacinKomercijalno, godinaBaze ?? DateTime.Now.Year]))
            {
                con.Open();
                return Get(con, vrDok);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="con"></param>
        /// <param name="vrDok"></param>
        /// <returns></returns>
        public static VrstaDok Get(FbConnection con, int vrDok)
        {
            using (FbCommand cmd = new FbCommand("SELECT * FROM VRSTADOK WHERE VRDOK = @VRDOK", con))
            {
                cmd.Parameters.AddWithValue("@VRDOk", vrDok);
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return new VrstaDok()
                        {
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            Interni = Convert.ToInt32(dr["INTERNI"]),
                            NazivDok = dr["NAZIVDOK"].ToString(),
                            DatumPosled = dr["DATUMPOSLED"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATUMPOSLED"]),
                            KljucMenjaDatum = Convert.ToInt32(dr["KLJUCMENJADATUM"]),
                            IzmenaMenjaDatum = Convert.ToInt32(dr["IZMENAMENJADATUM"]),
                            Poslednji = dr["POSLEDNJI"] is DBNull ? null : (int?)Convert.ToInt32(dr["POSLEDNJI"])
                        };
                    }
                }
            }

            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="godinaBaze"></param>
        /// <returns></returns>
        public static VrstaDokDictionary Dictionary(int? godinaBaze = null)
        {
            using(FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[DB.Settings.MainMagacinKomercijalno, godinaBaze ?? DateTime.Now.Year]))
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
        public static VrstaDokDictionary Dictionary(FbConnection con)
        {
            Dictionary<int, VrstaDok> dict = new Dictionary<int, VrstaDok>();
            using(FbCommand cmd = new FbCommand("SELECT * FROM VRSTADOK", con))
            {
                using(FbDataReader dr = cmd.ExecuteReader())
                {
                    while(dr.Read())
                    {
                        dict.Add(Convert.ToInt32(dr["VRDOK"]), new VrstaDok()
                        {
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            Interni = Convert.ToInt32(dr["INTERNI"]),
                            NazivDok = dr["NAZIVDOK"].ToString(),
                            DatumPosled = dr["DATUMPOSLED"] is DBNull ? null : (DateTime?)Convert.ToDateTime(dr["DATUMPOSLED"]),
                            KljucMenjaDatum = Convert.ToInt32(dr["KLJUCMENJADATUM"]),
                            IzmenaMenjaDatum = Convert.ToInt32(dr["IZMENAMENJADATUM"]),
                            Poslednji = dr["POSLEDNJI"] is DBNull ? null : (int?)Convert.ToInt32(dr["POSLEDNJI"])
                        });
                    }
                }
            }

            return new VrstaDokDictionary(dict);
        }
    }
}
