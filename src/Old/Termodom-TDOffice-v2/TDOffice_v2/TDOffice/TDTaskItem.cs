using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class TDTaskItem
	{
		public int ID { get; set; }
		public int TDTaskID { get; set; }
		public int UserID { get; set; }
		public double Interval { get; set; }
		public DateTime? Done { get; set; }

		public void Update()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Update(con);
			}
		}

		public void Update(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand(
					"UPDATE TDTASK_ITEM SET DONE = @DONE, INTERVAL = @INT WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@DONE", Done);
				cmd.Parameters.AddWithValue("@INT", Interval);

				cmd.ExecuteNonQuery();
			}
		}

		public static TDTaskItem Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static TDTaskItem Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, TDTASKID, USERID, DONE, INTERVAL FROM TDTASK_ITEM WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new TDTaskItem()
						{
							ID = Convert.ToInt32(dr["ID"]),
							TDTaskID = Convert.ToInt32(dr["TDTASKID"]),
							UserID = Convert.ToInt32(dr["USERID"]),
							Done =
								dr["DONE"] is DBNull
									? null
									: (DateTime?)Convert.ToDateTime(dr["DONE"]),
							Interval = Convert.ToDouble(dr["INTERVAL"])
						};
			}

			return null;
		}

		public static Task<TDTaskItem> GetAsync(int id)
		{
			return Task.Run(() =>
			{
				return Get(id);
			});
		}

		public static List<TDTaskItem> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<TDTaskItem> List(FbConnection con)
		{
			List<TDTaskItem> list = new List<TDTaskItem>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, TDTASKID, USERID, DONE, INTERVAL FROM TDTASK_ITEM",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new TDTaskItem()
							{
								ID = Convert.ToInt32(dr["ID"]),
								TDTaskID = Convert.ToInt32(dr["TDTASKID"]),
								UserID = Convert.ToInt32(dr["USERID"]),
								Done =
									dr["DONE"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["DONE"]),
								Interval = Convert.ToDouble(dr["INTERVAL"])
							}
						);
			}

			return list;
		}

		public static Task<List<TDTaskItem>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}

		public static List<TDTaskItem> ListByUserID(int userID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return ListByUserID(con, userID);
			}
		}

		public static List<TDTaskItem> ListByUserID(FbConnection con, int userID)
		{
			List<TDTaskItem> list = new List<TDTaskItem>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, TDTASKID, USERID, DONE, INTERVAL FROM TDTASK_ITEM WHERE USERID = @UID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@UID", userID);

				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new TDTaskItem()
							{
								ID = Convert.ToInt32(dr["ID"]),
								TDTaskID = Convert.ToInt32(dr["TDTASKID"]),
								UserID = Convert.ToInt32(dr["USERID"]),
								Done =
									dr["DONE"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["DONE"]),
								Interval = Convert.ToDouble(dr["INTERVAL"])
							}
						);
			}

			return list;
		}

		public static Task<List<TDTaskItem>> ListByUserIDAsync(int userID)
		{
			return Task.Run(() =>
			{
				return ListByUserID(userID);
			});
		}

		public static int Insert(int taskID, int userID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, taskID, userID);
			}
		}

		public static int Insert(FbConnection con, int taskID, int userID)
		{
			using (
				FbCommand cmd = new FbCommand(
					"INSERT INTO TDTASK_ITEM (ID, TDTASKID, USERID, DONE, INTERVAL) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM TDTASK_ITEM) + 1), @TID, @UID, @DONE, 60)",
					con
				)
			)
			{
				cmd.Parameters.Add(
					new FbParameter("ID", FbDbType.Integer)
					{
						Direction = System.Data.ParameterDirection.Output
					}
				);
				cmd.Parameters.AddWithValue("@TID", taskID);
				cmd.Parameters.AddWithValue("@UID", userID);
				cmd.Parameters.AddWithValue("@DONE", null);

				cmd.ExecuteNonQuery();

				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}

		public static Task<int> InsertAsync(int taskID, int userID)
		{
			return Task.Run(() =>
			{
				return Insert(taskID, userID);
			});
		}

		public static void Delete(int itemID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Delete(con, itemID);
			}
		}

		public static void Delete(FbConnection con, int itemID)
		{
			using (FbCommand cmd = new FbCommand("DELETE FROM TDTASK_ITEM WHERE ID = @ID", con))
			{
				cmd.Parameters.AddWithValue("@ID", itemID);

				cmd.ExecuteNonQuery();
			}
		}
	}
}
