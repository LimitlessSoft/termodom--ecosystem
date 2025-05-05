using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public partial class TaskboardTaskKomentar
	{
		public int ID { get; set; }
		public int KorisnikID { get; set; }
		public DateTime Datum { get; set; }
		public int TaskID { get; set; }
		public string Text { get; set; }

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
					"UPDATE TASKBOARD_TASK_KOMENTAR SET KORISNIKID = @K, DATUM = @D, TASK_ID = @TID,  TEXT = @T WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@K", KorisnikID);
				cmd.Parameters.AddWithValue("@D", Datum);
				cmd.Parameters.AddWithValue("@TID", TaskID);
				cmd.Parameters.AddWithValue("@T", Text);
				cmd.Parameters.AddWithValue("@ID", ID);

				cmd.ExecuteNonQuery();
			}
		}

		public static TaskboardTaskKomentar Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static TaskboardTaskKomentar Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, KORISNIKID, DATUM, TASK_ID,  TEXT FROM TASKBOARD_TASK_KOMENTAR WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new TaskboardTaskKomentar()
						{
							ID = Convert.ToInt32(dr["ID"]),
							KorisnikID = Convert.ToInt32(dr["KORISNIKID"]),
							Datum = Convert.ToDateTime(dr["DATUM"]),
							TaskID = Convert.ToInt32(dr["TASK_ID"]),
							Text = dr["TEXT"].ToString()
						};
			}
			return null;
		}

		public static List<TaskboardTaskKomentar> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<TaskboardTaskKomentar> List(FbConnection con)
		{
			List<TaskboardTaskKomentar> list = new List<TaskboardTaskKomentar>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, KORISNIKID, DATUM, TASK_ID, TEXT FROM TASKBOARD_TASK_KOMENTAR ",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new TaskboardTaskKomentar()
							{
								ID = Convert.ToInt32(dr["ID"]),
								KorisnikID = Convert.ToInt32(dr["KORISNIKID"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								TaskID = Convert.ToInt32(dr["TASK_ID"]),
								Text = dr["TEXT"].ToString()
							}
						);
			}
			return list;
		}

		public static List<TaskboardTaskKomentar> ListByTaskID(int TaskID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return ListByTaskID(con, TaskID);
			}
		}

		public static List<TaskboardTaskKomentar> ListByTaskID(FbConnection con, int TaskID)
		{
			List<TaskboardTaskKomentar> list = new List<TaskboardTaskKomentar>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, KORISNIKID, DATUM, TASK_ID, TEXT FROM TASKBOARD_TASK_KOMENTAR WHERE TASK_ID = @TID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@TID", TaskID);
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new TaskboardTaskKomentar()
							{
								ID = Convert.ToInt32(dr["ID"]),
								KorisnikID = Convert.ToInt32(dr["KORISNIKID"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								TaskID = Convert.ToInt32(dr["TASK_ID"]),
								Text = dr["TEXT"].ToString()
							}
						);
			}
			return list;
		}

		public static int Insert(int KorisnikID, int TaskID, DateTime datum, string tekst)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, KorisnikID, TaskID, datum, tekst);
			}
		}

		public static int Insert(
			FbConnection con,
			int KorisnikID,
			int TaskID,
			DateTime datum,
			string tekst
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					"INSERT INTO TASKBOARD_TASK_KOMENTAR (ID, KORISNIKID, DATUM, TASK_ID, TEXT) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM TASKBOARD_TASK_KOMENTAR) + 1), @KID, @D, @TID, @T)",
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
				cmd.Parameters.AddWithValue("@KID", KorisnikID);
				cmd.Parameters.AddWithValue("@D", datum);
				cmd.Parameters.AddWithValue("@TID", TaskID);
				cmd.Parameters.AddWithValue("@T", tekst);

				cmd.ExecuteNonQuery();

				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}
	}
}
