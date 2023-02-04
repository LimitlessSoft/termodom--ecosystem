using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public partial class Taskboard
    {
        public class Item
        {
            public static readonly Color REQUESTED_COLOR = Color.Yellow;
            public static readonly Color IN_QUEUE_COLOR = Color.Orange;
            public static readonly Color IN_PROGRESS_COLOR = Color.Green;
            public static readonly Color FINISHED_COLOR = Color.Red;

            public static readonly Dictionary<ItemStatus, Color> COLORS_BY_ITEM_STATUS = new Dictionary<ItemStatus, Color>()
            {
                { ItemStatus.Requested, REQUESTED_COLOR },
                { ItemStatus.InQueue, IN_QUEUE_COLOR },
                { ItemStatus.InProgress, IN_PROGRESS_COLOR },
                { ItemStatus.Finished, FINISHED_COLOR }
            };

            public int ID { get; set; }
            public int TaskboardID { get; set; }
            public string Naslov { get; set; }
            public string Text { get; set; }
            public ItemStatus Status { get; set; }
            public int KorisnikID { get; set; }
            public DateTime Datum { get; set; }

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
                using(FbCommand cmd = new FbCommand("UPDATE TASKBOARD_TASK SET NASLOV = @N, TEXT = @T, STATUS = @S WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@N", Naslov);
                    cmd.Parameters.AddWithValue("@T", Text);
                    cmd.Parameters.AddWithValue("@S", (int)Status);
                    cmd.Parameters.AddWithValue("@ID", ID);

                    cmd.ExecuteNonQuery();
                }
            }

            public static Item Get(int id)
            {
                using(FbConnection con = new  FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    return Get(con, id);
                }
            }
            public static Item Get(FbConnection con, int id)
            {
                using(FbCommand cmd = new FbCommand("SELECT ID, TASKBOARD_ID, NASLOV, TEXT, STATUS, KORISNIKID, DATUM FROM TASKBOARD_TASK WHERE ID = @ID", con))
                {
                    cmd.Parameters.AddWithValue("@ID", id);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        if (dr.Read())
                            return new Item()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                TaskboardID = Convert.ToInt32(dr["TASKBOARD_ID"]),
                                Text = dr["TEXT"].ToString(),
                                Naslov = dr["NASLOV"].ToString(),
                                Status = (ItemStatus)Convert.ToInt32(dr["STATUS"]),
                                KorisnikID = Convert.ToInt32(dr["KORISNIKID"]),
                                Datum = Convert.ToDateTime(dr["DATUM"])
                            };
                }
                return null;
            }
            public static List<Item> List()
            {
                using (FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    return List(con);
                }
            }
            public static Task<List<Item>> ListAsync()
            {
                return Task.Run(() =>
                {
                    return List();
                });
            }
            public static List<Item> List(FbConnection con)
            {
                List<Item> list = new List<Item>();
                using (FbCommand cmd = new FbCommand("SELECT ID, TASKBOARD_ID, NASLOV, TEXT, STATUS, KORISNIKID, DATUM FROM TASKBOARD_TASK", con))
                {
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            list.Add(new Item()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                TaskboardID = Convert.ToInt32(dr["TASKBOARD_ID"]),
                                Naslov = dr["NASLOV"].ToString(),
                                Text = dr["TEXT"].ToString(),
                                Status = (ItemStatus)Convert.ToInt32(dr["STATUS"]),
                                KorisnikID = Convert.ToInt32(dr["KORISNIKID"]),
                                Datum = Convert.ToDateTime(dr["DATUM"])
                            });
                }
                return list;
            }
            public static List<Item> ListByTaskboard(int taskboardID)
            {
                using(FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    return ListByTaskboard(con, taskboardID);
                }
            }
            public static Task<List<Item>> ListByTaskboardAsync(int taskboardID)
            {
                return Task.Run(() =>
                {
                    return ListByTaskboard(taskboardID);
                });
            }
            public static List<Item> ListByTaskboard(FbConnection con, int taskboardID)
            {
                List<Item> list = new List<Item>();
                using (FbCommand cmd = new FbCommand("SELECT ID, TASKBOARD_ID, NASLOV, TEXT, STATUS, KORISNIKID, DATUM FROM TASKBOARD_TASK WHERE TASKBOARD_ID = @TID", con))
                {
                    cmd.Parameters.AddWithValue("@TID", taskboardID);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            list.Add(new Item()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                TaskboardID = Convert.ToInt32(dr["TASKBOARD_ID"]),
                                Naslov = dr["NASLOV"].ToString(),
                                Text = dr["TEXT"].ToString(),
                                Status = (ItemStatus)Convert.ToInt32(dr["STATUS"]),
                                KorisnikID = Convert.ToInt32(dr["KORISNIKID"]),
                                Datum = Convert.ToDateTime(dr["DATUM"])
                            });
                }
                return list;
            }
            public static int Insert(int taskboardID, string naslov, string text, int korisnikID)
            {
                using(FbConnection con = new FbConnection(TDOffice.connectionString))
                {
                    con.Open();
                    return Insert(con, taskboardID, naslov, text, korisnikID);
                }
            }
            public static int Insert(FbConnection con, int taskboardID, string naslov, string text, int korisnikID)
            {
                using(FbCommand cmd = new FbCommand("INSERT INTO TASKBOARD_TASK (ID, TASKBOARD_ID, NASLOV, TEXT, STATUS, KORISNIKID, DATUM) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM TASKBOARD_TASK) + 1), @TID, @NASLOV,  @TEXT, 0, @KID, @DATUM) RETURNING ID", con))
                {
                    cmd.Parameters.AddWithValue("@TID", taskboardID);
                    cmd.Parameters.AddWithValue("@KID", korisnikID);
                    cmd.Parameters.AddWithValue("@NASLOV", naslov);
                    cmd.Parameters.AddWithValue("@TEXT", text);
                    cmd.Parameters.AddWithValue("@DATUM", DateTime.Now);
                    cmd.Parameters.Add(new FbParameter("ID", FbDbType.Integer) { Direction = System.Data.ParameterDirection.Output });

                    cmd.ExecuteNonQuery();

                    return Convert.ToInt32(cmd.Parameters["ID"].Value);
                }
            }
        }
    }
}
