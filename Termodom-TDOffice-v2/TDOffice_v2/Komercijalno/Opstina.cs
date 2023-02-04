using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using System.Linq;
using System.Text;

namespace TDOffice_v2.Komercijalno
{
    class Opstina
    {
        public int ID { get; set; }
        public string Naziv { get; set; }
        public Opstina()
        {

        }
        public static List<Opstina> List()
        {
            List<Opstina> op = new List<Opstina>();
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT ID, NAZIV FROM OPSTINA", con))
                {
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            op.Add(new Opstina()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                Naziv = Convert.ToString(dr["NAZIV"]),
                            });
                }
            }
            return op;
        }
    }
    
}
