using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;



namespace TDOffice_v2.Komercijalno
{
    public class  Izvod
    {
        public int BrDok { get; set; }
        public int VrDok { get; set; }
        public int PPID { get; set; }
        public string SifraPlac { get; set; }
        public double Potrazuje { get; set; }
        public double Duguje { get; set; }
        public int IzvodID { get; set; }
        public string Konto { get; set; }
        public double RPotrazuje { get; set; }
        public double RDuguje { get; set; }


        public Izvod()
        {

        }

        public static List<Izvod> List()
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();

                return List(con);
            }
        }
        public static List<Izvod> List(FbConnection con)
        {
            List<Izvod> Iz = new List<Izvod>();
            using (FbCommand cmd = new FbCommand("SELECT PPID, POTRAZUJE, KONTO, IZVODID, BRDOK, VRDOK, DUGUJE, RPOTRAZUJE, RDUGUJE, SIFPLAC FROM IZVOD", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        Iz.Add(new Izvod()
                        {
                            PPID = Convert.ToInt32(dr["PPID"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"]),
                            VrDok = Convert.ToInt32(dr["VRDOK"]),
                            IzvodID = Convert.ToInt32(dr["IZVODID"]),
                            Konto = dr["KONTO"] is DBNull ? (string)null : Convert.ToString(dr["KONTO"]),
                            Potrazuje = Convert.ToDouble(dr["POTRAZUJE"]),
                            Duguje = Convert.ToDouble(dr["DUGUJE"]),
                            RDuguje = Convert.ToDouble(dr["RDUGUJE"]),
                            RPotrazuje = Convert.ToDouble(dr["RPOTRAZUJE"]),
                            SifraPlac = dr["SIFPLAC"].ToString()
                        });
            }
            return Iz;
        }
    }

}
