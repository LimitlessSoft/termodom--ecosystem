using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Komercijalno
{
    public class Tarife
    {
        private static List<Tarife> _bufferedList = new List<Tarife>();

        public string TarifaID { get; set; }
        public string Naziv { get; set; }
        public double? Stopa { get; set; }
        public int? FKod { get; set; }
        public int Vrsta { get; set; }

        public static Tarife Get(string tarifaID)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
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
        public static List<Tarife> List()
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
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
            _bufferedList = list;
            return list;
        }
        public static List<Tarife> BufferedList()
        {
            if (_bufferedList == null || _bufferedList.Count == 0)
                List();

            return _bufferedList;
        }
    }
}
