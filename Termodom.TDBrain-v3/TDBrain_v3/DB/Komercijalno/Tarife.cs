using FirebirdSql.Data.FirebirdClient;

namespace TDBrain_v3.DB.Komercijalno
{
    public class Tarife
    {
        public string? TarifaID { get; set; }
        public string? Naziv { get; set; }
        public double? Stopa { get; set; }
        public int? FKod { get; set; }
        public int Vrsta { get; set; }

        public static Tarife? Get(int magacinID, int godina, string tarifaID)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT TARIFAID, NAZIV, STOPA, F_KOD, VRSTA FROM TARIFE WHERE TARIFAID = @TID", con))
                {
                    cmd.Parameters.AddWithValue("@TID", tarifaID);
                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new Tarife()
                            {
                                TarifaID = dr["TARIFAID"].ToString(),
                                Naziv = dr["NAZIV"].ToString(),
                                Stopa = dr["STOPA"] is DBNull ? null : (double?)Convert.ToDouble(dr["STOPA"]),
                                FKod = dr["F_KOD"] is DBNull ? null : (int?)Convert.ToInt32(dr["F_KOD"]),
                                Vrsta = Convert.ToInt32(dr["VRSTA"])
                            };
                }
            }

            return null;
        }
        public static List<Tarife> List(int magacinID, int godina)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<Tarife> List(FbConnection con)
        {
            List<Tarife> list = new List<Tarife>();

            using (FbCommand cmd = new FbCommand("SELECT TARIFAID, NAZIV, STOPA, F_KOD, VRSTA FROM TARIFE", con))
            using (FbDataReader dr = cmd.ExecuteReader())
                while (dr.Read())
                    list.Add(new Tarife()
                    {
                        TarifaID = dr["TARIFAID"].ToString(),
                        Naziv = dr["NAZIV"].ToString(),
                        Stopa = dr["STOPA"] is DBNull ? null : (double?)Convert.ToDouble(dr["STOPA"]),
                        FKod = dr["F_KOD"] is DBNull ? null : (int?)Convert.ToInt32(dr["F_KOD"]),
                        Vrsta = Convert.ToInt32(dr["VRSTA"])
                    });

            return list;
        }
    }
}
