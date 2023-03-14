using FirebirdSql.Data.FirebirdClient;
using System.Collections.Generic;

namespace TDBrain_v3.DB.Komercijalno
{
    public class TarifeManager
    {
        public string? TarifaID { get; set; }
        public string? Naziv { get; set; }
        public double? Stopa { get; set; }
        public int? FKod { get; set; }
        public int Vrsta { get; set; }

        public static TarifeManager? Get(int magacinID, int godina, string tarifaID)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT TARIFAID, NAZIV, STOPA, F_KOD, VRSTA FROM TARIFE WHERE TARIFAID = @TID", con))
                {
                    cmd.Parameters.AddWithValue("@TID", tarifaID);
                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new TarifeManager()
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
        public static List<TarifeManager> List(int magacinID, int godina)
        {
            using (FbConnection con = new FbConnection(DB.Settings.ConnectionStringKomercijalno[magacinID, godina]))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<TarifeManager> List(FbConnection con)
        {
            List<TarifeManager> list = new List<TarifeManager>();

            using (FbCommand cmd = new FbCommand("SELECT TARIFAID, NAZIV, STOPA, F_KOD, VRSTA FROM TARIFE", con))
            using (FbDataReader dr = cmd.ExecuteReader())
                while (dr.Read())
                    list.Add(new TarifeManager()
                    {
                        TarifaID = dr["TARIFAID"].ToString(),
                        Naziv = dr["NAZIV"].ToString(),
                        Stopa = dr["STOPA"] is DBNull ? null : (double?)Convert.ToDouble(dr["STOPA"]),
                        FKod = dr["F_KOD"] is DBNull ? null : (int?)Convert.ToInt32(dr["F_KOD"]),
                        Vrsta = Convert.ToInt32(dr["VRSTA"])
                    });

            return list;
        }
        /// <summary>
        /// Vraca dictionary objekata tarifa iz baze
        /// </summary>
        /// <param name="godinaBaze"></param>
        /// <returns></returns>
        public static Termodom.Data.Entities.Komercijalno.TarifaDictionary Dictionary(int? godinaBaze = null)
        {
            Dictionary<string, Termodom.Data.Entities.Komercijalno.Tarifa> dict = new Dictionary<string, Termodom.Data.Entities.Komercijalno.Tarifa>();
            using (FbConnection con = new FbConnection(Settings.ConnectionStringKomercijalno[Settings.MainMagacinKomercijalno, godinaBaze ?? DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT TARIFAID, NAZIV, STOPA, F_KOD, VRSTA FROM TARIFE", con))
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            dict.Add(dr["TARIFAID"].ToString(), new Termodom.Data.Entities.Komercijalno.Tarifa()
                            {
                                TarifaID = dr["TARIFAID"].ToString(),
                                Naziv = dr["NAZIV"].ToString(),
                                Stopa = dr["STOPA"] is DBNull ? null : (double?)Convert.ToDouble(dr["STOPA"]),
                                FKod = dr["F_KOD"] is DBNull ? null : (int?)Convert.ToInt32(dr["F_KOD"]),
                                Vrsta = Convert.ToInt32(dr["VRSTA"])
                            });
            }

            return new Termodom.Data.Entities.Komercijalno.TarifaDictionary(dict);
        }
    }
}
