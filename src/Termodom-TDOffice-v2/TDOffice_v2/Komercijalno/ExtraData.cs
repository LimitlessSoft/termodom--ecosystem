using System;
using System.Collections.Generic;
using FirebirdSql.Data.FirebirdClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Komercijalno
{
    class ExtraData
    {
        public int VrDok { get; set; }
        public int BrDok { get; set; }
        public int EdID { get; set; }
        public int Redosled { get; set; }
        public string Vrednost { get; set; }
        public ExtraData()
        {

        }
        public static List<ExtraData> List()
        {
            List<ExtraData> ed = new List<ExtraData>();
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
            {
                con.Open();
                using (FbCommand cmd = new FbCommand("SELECT VRDOK, BRDOK, EDID, VREDNOST, REDOSLED FROM EXTRA_DATA", con))
                {
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            ed.Add(new ExtraData()
                            {
                                VrDok = Convert.ToInt32(dr["VRDOK"]),
                                BrDok = Convert.ToInt32(dr["BRDOK"]),
                                EdID = Convert.ToInt32(dr["EDID"]),
                                Redosled = Convert.ToInt32(dr["REDOSLED"]),
                                Vrednost = Convert.ToString(dr["NAZIV"]),
                            });
                } 
                return ed;
            }
        }
    }
}
