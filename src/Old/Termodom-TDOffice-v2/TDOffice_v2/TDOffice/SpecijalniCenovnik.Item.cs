using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public partial class SpecijalniCenovnik
    {
        public class Item
        {
            public int ID { get; set; }
            public int CenovnikID { get; set; }
            public int RobaID { get; set; }
            public int UslovTip { get; set; }
            public double UslovModifikator { get; set; }
            public double MaxRabat { get; set; } = 100;
            public double NabavnaCenaMargina { get; set; }

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
                using(FbCommand cmd = new FbCommand("UPDATE SPECIJALNI_CENOVNIK_ITEM SET CENOVNIKID = @CID, ROBAID = @RID, USLOV_TIP = @UT," +
                    "MAX_RABAT = @MR, NABAVNA_CENA_MARGINA = @NCM, USLOV_VREDNOST = @UV WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.Parameters.AddWithValue("@CID", CenovnikID);
                    cmd.Parameters.AddWithValue("@RID", RobaID);
                    cmd.Parameters.AddWithValue("@UT", UslovTip);
                    cmd.Parameters.AddWithValue("@UV", UslovModifikator);
                    cmd.Parameters.AddWithValue("@MR", MaxRabat);
                    cmd.Parameters.AddWithValue("@NCM", NabavnaCenaMargina);

                    cmd.ExecuteNonQuery();
                }
            }

            public static int Insert(int cenovnikID, int robaID, int uslovTip, double uslovModifikator, double maxRabat, double nabavnaCenaMargina)
            {
                using(FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    return Insert(con, cenovnikID, robaID, uslovTip, uslovModifikator, maxRabat, nabavnaCenaMargina);
                }
            }
            public static int Insert(FbConnection con, int cenovnikID, int robaID, int uslovTip, double uslovModifikator, double maxRabat, double nabavnaCenaMargina)
            {
                using(FbCommand cmd = new FbCommand(@"INSERT INTO SPECIJALNI_CENOVNIK_ITEM (ID, CENOVNIKID, ROBAID, USLOV_TIP, USLOV_VREDNOST, MAX_RABAT, NABAVNA_CENA_MARGINA)
VALUES
(((SELECT COALESCE(MAX(ID), 0) FROM SPECIJALNI_CENOVNIK_ITEM) + 1), @CENOVNIKID, @ROBAID, @USLOVTIP, @USLOVVREDNOST, @MAXRABAT, @NCM)
RETURNING ID", con))
                {
                    cmd.Parameters.Add(new FbParameter("ID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });
                    cmd.Parameters.AddWithValue("@CENOVNIKID", cenovnikID);
                    cmd.Parameters.AddWithValue("@ROBAID", robaID);
                    cmd.Parameters.AddWithValue("@USLOVTIP", uslovTip);
                    cmd.Parameters.AddWithValue("@USLOVVREDNOST", uslovModifikator);
                    cmd.Parameters.AddWithValue("@MAXRABAT", maxRabat);
                    cmd.Parameters.AddWithValue("@NCM", nabavnaCenaMargina);

                    cmd.ExecuteNonQuery();

                    return Convert.ToInt32(cmd.Parameters["ID"].Value);
                }
            }


            public static Item Get(int id)
            {
                using(FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    return Get(con, id);
                }
            }
            public static Item Get(FbConnection con, int id)
            {
                using(FbCommand cmd = new FbCommand("SELECT ID, CENOVNIKID, ROBAID, USLOV_TIP, USLOV_VREDNOST, MAX_RABAT, NABAVNA_CENA_MARGINA FROM SPECIJALNI_CENOVNIK_ITEM WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new Item()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                CenovnikID = Convert.ToInt32(dr["CENOVNIKID"]),
                                RobaID = Convert.ToInt32(dr["ROBAID"]),
                                UslovTip = Convert.ToInt32(dr["USLOV_TIP"]),
                                UslovModifikator = Convert.ToDouble(dr["USLOV_VREDNOST"]),
                                MaxRabat = Convert.ToDouble(dr["MAX_RABAT"]),
                                NabavnaCenaMargina = Convert.ToDouble(dr["NABAVNA_CENA_MARGINA"])
                            };
                }

                return null;
            }

            public static List<Item> List(string whereQuery = null)
            {
                using(FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    return List(con, whereQuery);
                }
            }
            public static List<Item> List(FbConnection con, string whereQuery = null)
            {
                if (whereQuery != null)
                    whereQuery = " WHERE " + whereQuery;

                List<Item> list = new List<Item>();
                using (FbCommand cmd = new FbCommand("SELECT ID, CENOVNIKID, ROBAID, USLOV_TIP, USLOV_VREDNOST, MAX_RABAT, NABAVNA_CENA_MARGINA FROM SPECIJALNI_CENOVNIK_ITEM " + whereQuery, con))
                {
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            list.Add(new Item()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                CenovnikID = Convert.ToInt32(dr["CENOVNIKID"]),
                                RobaID = Convert.ToInt32(dr["ROBAID"]),
                                UslovTip = Convert.ToInt32(dr["USLOV_TIP"]),
                                UslovModifikator = Convert.ToDouble(dr["USLOV_VREDNOST"]),
                                MaxRabat = Convert.ToDouble(dr["MAX_RABAT"]),
                                NabavnaCenaMargina = Convert.ToDouble(dr["NABAVNA_CENA_MARGINA"])
                            });
                }
                return list;
            }

            public static void Delete(int itemID)
            {
                using(FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    Delete(con, itemID);
                }
            }
            public static void Delete(FbConnection con, int itemID)
            {
                using(FbCommand cmd = new FbCommand("DELETE FROM SPECIJALNI_CENOVNIK_ITEM WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", itemID);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
