using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Komercijalno
{
    public class VrstaDok
    {
        public int VrDok { get; set; }
        public int Interni { get; set; }
        public string NazivDok { get; set; }
        public int KljucMenjaDatum { get; set; }
        public int IzmenaMenjaDatum { get; set; }

        public void Update(int godina)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                Update(con);
            }
        }
        public void Update(FbConnection con)
        {
            using(FbCommand cmd = new FbCommand("UPDATE VRSTADOK SET INTERNI = @INT, NAZIVDOK = @ND, " +
                "KLJUCMENJADATUM = @KMD, IZMENAMENJADATUM = @IMD WHERE VRDOK = @VRDOK", con))
            {
                cmd.Parameters.AddWithValue("@INT", Interni);
                cmd.Parameters.AddWithValue("@ND", NazivDok);
                cmd.Parameters.AddWithValue("@KMD", KljucMenjaDatum);
                cmd.Parameters.AddWithValue("@IMD", IzmenaMenjaDatum);
                cmd.Parameters.AddWithValue("@VRDOK", VrDok);

                cmd.ExecuteNonQuery();
            }
        }
        public static VrstaDok Get(int godina, int vrDok)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Get(con, vrDok);
            }
        }
        public static VrstaDok Get(FbConnection con, int vrDok)
        {
            using(FbCommand cmd = new FbCommand("SELECT VRDOK, INTERNI, NAZIVDOK, KLJUCMENJADATUM, IZMENAMENJADATUM FROM VRSTADOK WHERE VRDOK = @VRDOK", con))
            {
                cmd.Parameters.AddWithValue("@VRDOK", vrDok);

                using(FbDataReader dr = cmd.ExecuteReader())
                    if(dr.Read())
                        return new VrstaDok()
                        {
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            Interni = Convert.ToInt32(dr["INTERNI"]),
                            NazivDok = dr["NAZIVDOK"].ToString(),
                            KljucMenjaDatum = Convert.ToInt32(dr["KLJUCMENJADATUM"]),
                            IzmenaMenjaDatum = Convert.ToInt32(dr["IZMENAMENJADATUM"])
                        };
            }

            return null;
        }
        public static List<VrstaDok> List()
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<VrstaDok> List(FbConnection con)
        {
            List<VrstaDok> list = new List<VrstaDok>();

            {
                using (FbCommand cmd = new FbCommand("SELECT VRDOK, INTERNI, NAZIVDOK FROM VRSTADOK", con))
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new VrstaDok()
                        {
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            Interni = Convert.ToInt32(dr["INTERNI"]),
                            NazivDok = dr["NAZIVDOK"].ToString()
                        });
            }

            return list;
        }
        public static Task<List<VrstaDok>> ListAsync()
        {
            return Task.Run(() =>
            {
                return List();
            });
        }
    }
}
