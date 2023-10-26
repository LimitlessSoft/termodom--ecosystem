using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class StavkaUlaznaPonuda
    {
        public int ID { get; set; }
        public int BrDok { get; set; }
        public int RobaID { get; set; }
        public double NabavnaCena { get; set; }

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
            using(FbCommand cmd = new FbCommand(@"
UPDATE STAVKA_ULAZNA_PONUDA
SET
NABAVNA_CENA = @NC
WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@NC", NabavnaCena);
                cmd.Parameters.AddWithValue("@ID", ID);

                cmd.ExecuteNonQuery();
            }
        }

        public static StavkaUlaznaPonuda Get(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Get(con, id);
            }
        }
        public static StavkaUlaznaPonuda Get(FbConnection con, int id)
        {
            using(FbCommand cmd = new FbCommand("SELECT ID, ROBAID, NABAVNA_CENA, BRDOK FROM STAVKA_ULAZNA_PONUDA WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                using(FbDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                        return new StavkaUlaznaPonuda()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            RobaID = Convert.ToInt32(dr["ROBAID"]),
                            NabavnaCena = Convert.ToDouble(dr["NABAVNA_CENA"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"])
                        };
                }
            }
            return null;
        }
        public static List<StavkaUlaznaPonuda> List(string whereQuery = null)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return List(con, whereQuery);
            }
        }
        public static List<StavkaUlaznaPonuda> List(FbConnection con, string whereQuery = null)
        {
            if (!string.IsNullOrWhiteSpace(whereQuery))
                whereQuery = " AND " + whereQuery;

            List<StavkaUlaznaPonuda> list = new List<StavkaUlaznaPonuda>();

            using (FbCommand cmd = new FbCommand("SELECT ID, ROBAID, NABAVNA_CENA, BRDOK FROM STAVKA_ULAZNA_PONUDA WHERE 1 = 1 " + whereQuery, con))
            {
                using (FbDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                        list.Add(new StavkaUlaznaPonuda()
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            RobaID = Convert.ToInt32(dr["ROBAID"]),
                            NabavnaCena = Convert.ToDouble(dr["NABAVNA_CENA"]),
                            BrDok = Convert.ToInt32(dr["BRDOK"])
                        });
                }
            }
            return list;
        }
        public static int Insert(int brDok, int robaID, double nabavnaCena)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                return Insert(con, brDok, robaID, nabavnaCena);
            }
        }
        public static int Insert(FbConnection con, int brDok, int robaID, double nabavnaCena)
        {
            using(FbCommand cmd = new FbCommand("INSERT INTO STAVKA_ULAZNA_PONUDA (ID, ROBAID, NABAVNA_CENA, BRDOK) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM STAVKA_ULAZNA_PONUDA) + 1), @RID, @NC, @BRDOK) RETURNING ID", con))
            {
                cmd.Parameters.Add(new FbParameter("ID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                cmd.Parameters.AddWithValue("@RID", robaID);
                cmd.Parameters.AddWithValue("@BRDOK", brDok);
                cmd.Parameters.AddWithValue("@NC", nabavnaCena);

                cmd.ExecuteNonQuery();

                return Convert.ToInt32(cmd.Parameters["ID"].Value);
            }
        }
        public static void Delete(int id)
        {
            using(FbConnection con = new FbConnection(TDOffice.connectionString))
            {
                con.Open();
                Delete(con, id);
            }
        }
        public static void Delete(FbConnection con, int id)
        {
            using(FbCommand cmd = new FbCommand("DELETE FROM STAVKA_ULAZNA_PONUDA WHERE ID = @ID", con))
            {
                cmd.Parameters.AddWithValue("@ID", id);

                cmd.ExecuteNonQuery();
            }
        }
    }
}
