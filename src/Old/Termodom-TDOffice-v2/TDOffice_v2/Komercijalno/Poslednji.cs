using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TDOffice_v2.Komercijalno
{
    public class Poslednji
    {
        public string Naziv { get; set; }
        public int? ID { get; set; }
        public int? OpsegOd { get; set; }
        public int? OpsegDo { get; set; }
        public int ObjekatID { get; set; }
        public int Korak { get; set; }

        public Poslednji()
        {

        }

        public static Poslednji Get(string Naziv)
        {
            return List().Where(x => x.Naziv == Naziv).FirstOrDefault();
        }
        public static List<Poslednji> List()
        {
            List<Poslednji> list = new List<Poslednji>();
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using(FbCommand cmd = new FbCommand("SELECT NAZIV, ID, OPSEGOD, OPSEGDO, KORAK, OBJEKATID FROM POSLEDNJI", con))
                {
                    using(FbDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            list.Add(new Poslednji()
                            {
                                ID = dr["ID"] is DBNull ? (int?)null : Convert.ToInt32(dr["ID"]),
                                Naziv = dr["NAZIV"].ToString(),
                                OpsegDo = dr["OPSEGDO"] is DBNull ? (int?)null : Convert.ToInt32(dr["OPSEGDO"]),
                                OpsegOd = dr["OPSEGOD"] is DBNull ? (int?) null : Convert.ToInt32(dr["OPSEGOD"]),
                                Korak = Convert.ToInt32(dr["KORAK"]),
                                ObjekatID = Convert.ToInt32(dr["OBJEKATID"])
                            });
                        }
                    }
                }
            }
            return list;
        }
    }
}
