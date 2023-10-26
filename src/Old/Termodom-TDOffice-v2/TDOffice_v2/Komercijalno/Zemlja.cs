using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using System.Linq;
using System.Text;

namespace TDOffice_v2.Komercijalno
{
    class Zemlja
    {
        public int DrzavaID { get; set; }
        public string Naziv { get; set; }
        public Zemlja()
        {
            
        }
        public static List<Zemlja> List()
        {
            List<Zemlja> drz = new List<Zemlja>();
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT DRZAVAID, NAZIV FROM ZEMLJA", con))
                {
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            drz.Add(new Zemlja()
                            {
                                DrzavaID = Convert.ToInt32(dr["DRZAVAID"]),
                                Naziv = Convert.ToString(dr["NAZIV"]),
                            });
                }
            }
            return drz;
        }
    }
}
