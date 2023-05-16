using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class WebUredjivanjeCenaStavka
    {
        public int RobaID { get; set; }
        public Enums.WebUredjivanjeCenaUslov Uslov { get; set; } = Enums.WebUredjivanjeCenaUslov.None;
        public double Modifikator { get; set; }
        public int MagacinID { get; set; }
        public int? ReferentniProizvod { get; set; }

        public void Update()
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Update(con);
            }
        }
        public void Update(FbConnection con)
        {
            using(FbCommand cmd = new FbCommand(@"UPDATE WEB_UREDJIVANJE_CENA_STAVKA SET
                USLOV = @U,
                USLOV_MODIFIKATOR = @M,
                MAGACINID = @MID,
                REFERENTNI_PROIZVOD = @RP
                WHERE ROBAID = @RID", con))
            {
                cmd.Parameters.AddWithValue("@U", Uslov);
                cmd.Parameters.AddWithValue("@M", (double)Modifikator);
                cmd.Parameters.AddWithValue("@MID", MagacinID);
                cmd.Parameters.AddWithValue("@RID", RobaID);
                cmd.Parameters.AddWithValue("@RP", ReferentniProizvod);

                cmd.ExecuteNonQuery();
            }
        }

        public static void Insert(int robaID, Enums.WebUredjivanjeCenaUslov uslov, double modifikator, int magacinID)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Insert(con, robaID, uslov, modifikator, magacinID, null);
            }
        }
        public static void Insert(int robaID, Enums.WebUredjivanjeCenaUslov uslov, double modifikator, int magacinID, int referentniProizvod)
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Insert(con, robaID, uslov, modifikator, magacinID, referentniProizvod);
            }
        }
        public static void Insert(FbConnection con, int robaID, Enums.WebUredjivanjeCenaUslov uslov, double modifikator, int magacinID, int? referentniProizvod)
        {
            using(FbCommand cmd = new FbCommand(@"INSERT INTO WEB_UREDJIVANJE_CENA_STAVKA
                (ROBAID, USLOV, USLOV_MODIFIKATOR, MAGACINID) VALUES (@RID, @U, @UM, @M)", con))
            {
                cmd.Parameters.AddWithValue("@RID", robaID);
                cmd.Parameters.AddWithValue("@U", (int)uslov);
                cmd.Parameters.AddWithValue("@UM", modifikator);
                cmd.Parameters.AddWithValue("@M", magacinID);

                cmd.ExecuteNonQuery();
            }
        }

        public static WebUredjivanjeCenaStavka Get(int robaID)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, robaID);
            }
        }
        public static WebUredjivanjeCenaStavka Get(FbConnection con, int robaID)
        {
            using(FbCommand cmd = new FbCommand("SELECT ROBAID, USLOV, USLOV_MODIFIKATOR, MAGACINID, REFERENTNI_PROIZVOD FROM WEB_UREDJIVANJE_CENA_STAVKA WHERE ROBAID = @RID", con))
            {
                cmd.Parameters.AddWithValue("@RID", robaID);

                using (FbDataReader dr = cmd.ExecuteReader())
                    if (dr.Read())
                        return new WebUredjivanjeCenaStavka()
                        {
                            RobaID = Convert.ToInt32(dr["ROBAID"]),
                            Uslov = (Enums.WebUredjivanjeCenaUslov)Convert.ToInt32(dr["USLOV"]),
                            Modifikator = Convert.ToDouble(dr["USLOV_MODIFIKATOR"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            ReferentniProizvod = dr["REFERENTNI_PROIZVOD"] is DBNull ? null : (int?)Convert.ToInt32(dr["REFERENTNI_PROIZVOD"])
                        };
            }

            return null;
        }

        public static List<WebUredjivanjeCenaStavka> List()
        {
            using (FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con);
            }
        }
        public static List<WebUredjivanjeCenaStavka> List(FbConnection con)
        {
            List<WebUredjivanjeCenaStavka> list = new List<WebUredjivanjeCenaStavka>();
            using (FbCommand cmd = new FbCommand("SELECT ROBAID, USLOV, USLOV_MODIFIKATOR, MAGACINID, REFERENTNI_PROIZVOD FROM WEB_UREDJIVANJE_CENA_STAVKA", con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                    while (dr.Read())
                        list.Add(new WebUredjivanjeCenaStavka()
                        {
                            RobaID = Convert.ToInt32(dr["ROBAID"]),
                            Uslov = (Enums.WebUredjivanjeCenaUslov)Convert.ToInt32(dr["USLOV"]),
                            Modifikator = Convert.ToDouble(dr["USLOV_MODIFIKATOR"]),
                            MagacinID = Convert.ToInt32(dr["MAGACINID"]),
                            ReferentniProizvod = dr["REFERENTNI_PROIZVOD"] is DBNull ? null : (int?)Convert.ToInt32(dr["REFERENTNI_PROIZVOD"])
                        });
            }

            return list;
        }
    }
}
