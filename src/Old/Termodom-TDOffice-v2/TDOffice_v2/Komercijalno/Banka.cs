using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Komercijalno
{
    public class Banka
    {
        public int BankaID { get; set; }
        public string Naziv { get; set; }
        public string ZiroRacun { get; set; }
        public double MaxCek { get; set; }
        public int Primamo { get; set; }
        public int? ZbirniRacun { get; set; }

        public Banka()
        {

        }

        public static Banka Get(int godina, int id)
        {
            using(FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return Get(con, id);
            }
        }
        public static Banka Get(FbConnection con, int id)
        {
            using(FbCommand cmd = new FbCommand(@"SELECT
                BANKAID,
                NAZIV,
                ZIRORACUN,
                MAXCEK,
                PRIMAMO,
                ZBIRNI_RACUN
                FROM BANKA
                WHERE BANKAID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new Banka()
                        {
                            BankaID = Convert.ToInt32(dr["BANKAID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            ZiroRacun = dr["ZIRORACUN"].ToString(),
                            MaxCek = Convert.ToDouble(dr["MAXCEK"]),
                            Primamo = Convert.ToInt32(dr["PRIMAMO"]),
                            ZbirniRacun = dr["ZBIRNI_RACUN"] is DBNull ? null : (int?)Convert.ToInt32(dr["ZBIRNI_RACUN"])
                        };
            }

            return null;
        }

        public static List<Banka> List(int godina)
        {
            using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<Banka> List(FbConnection con)
        {
            List<Banka> list = new List<Banka>();
            using (FbCommand cmd = new FbCommand(@"SELECT
                BANKAID,
                NAZIV,
                ZIRORACUN,
                MAXCEK,
                PRIMAMO,
                ZBIRNI_RACUN FROM BANKA", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new Banka()
                        {
                            BankaID = Convert.ToInt32(dr["BANKAID"]),
                            Naziv = dr["NAZIV"].ToString(),
                            ZiroRacun = dr["ZIRORACUN"].ToString(),
                            MaxCek = Convert.ToDouble(dr["MAXCEK"]),
                            Primamo = Convert.ToInt32(dr["PRIMAMO"]),
                            ZbirniRacun = dr["ZBIRNI_RACUN"] is DBNull ? null : (int?)Convert.ToInt32(dr["ZBIRNI_RACUN"])
                        });
            }
            return list;
        }
    }
}
