using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using System.Linq;
using System.Text;
using LimitlessSoft.Buffer;

namespace TDOffice_v2.Komercijalno
{
    public class Dobavljac
    {
        public int DobavljacID { get; set; }
        public string Naziv { get; set; }

        public Dobavljac()
        {

        }

        /// <summary>
        /// Vraca listu dobavljaca iz baze
        /// </summary>
        /// <returns></returns>
        public static List<Dobavljac> List(int godina)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return List(con);
            }
        }
        /// <summary>
        /// Vraca listu dobavljaca iz baze
        /// </summary>
        /// <param name="con"></param>
        /// <returns></returns>
        public static List<Dobavljac> List(FbConnection con)
        {
            List<Dobavljac> dob = new List<Dobavljac>();
            using (FbCommand cmd = new FbCommand("Select DISTINCT P.PPID, P.NAZIV from ( ROBA R join PARTNER P on R.DOB_PPID = P.PPID)", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        dob.Add(new Dobavljac()
                        {
                            DobavljacID = Convert.ToInt32(dr["PPID"]),
                            Naziv = Convert.ToString(dr["NAZIV"])
                        });
            }
            return dob;
        }
    }
}
