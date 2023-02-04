using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Komercijalno
{
    class Mesta
    {
        public string MestoID { get; set; }
        public string Naziv { get; set; }
        public Mesta()
        {
                
        }
        public static List<Mesta> List()
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<Mesta> List(FbConnection con)
        {
            List<Mesta> op = new List<Mesta>();
            using (FbCommand cmd = new FbCommand("SELECT MESTOID, NAZIV FROM MESTA", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        op.Add(new Mesta()
                        {
                            MestoID = Convert.ToString(dr["MESTOID"]),
                            Naziv = Convert.ToString(dr["NAZIV"]),
                        });
            }
            return op;
        }
        public static Task<List<Mesta>> ListAsync()
        {
            return Task.Run(() =>
            {
                return List();
            });
        }
    }
}
