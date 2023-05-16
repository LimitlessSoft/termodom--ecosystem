using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using System.Linq;
using System.Text;

namespace TDOffice_v2.Komercijalno
{

    class PodGrupa
    {
        public string GrupaID { get; set; }
        public string Naziv { get; set; }
        public int PodGrupaID { get; set; }

        public PodGrupa()
        {

        }

        public static List<PodGrupa> List()
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();

                return List(con);
            }
        }

        public static List<PodGrupa> List(FbConnection con)
        {
            List<PodGrupa> PG = new List<PodGrupa>();
            using (FbCommand cmd = new FbCommand("SELECT GRUPAID, NAZIV, PODGRUPAID FROM PODGRUPA", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        PG.Add(new PodGrupa()
                        {
                            GrupaID = Convert.ToString(dr["GRUPAID"]),
                            Naziv = Convert.ToString(dr["NAZIV"]),
                            PodGrupaID = Convert.ToInt32(dr["PODGRUPAID"])
                        });
            }
            return PG;
        }
    }
}
