using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class CheckListItem
	{
		public int ID { get; set; }
		public CheckList.Jobs Job { get; set; }
		public int KorisnikID { get; set; }
		public DateTime? DatumIzvrsenja { get; set; }
		public int IntervalDana { get; set; }

		public CheckListItem() { }

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
					"UPDATE CHECKLIST_ITEM SET DATUM_IZVRSENJA = @DI, INTERVAL_DANA = @INTERVAL WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@DI", DatumIzvrsenja);
				cmd.Parameters.AddWithValue("@INTERVAL", IntervalDana);
				cmd.Parameters.AddWithValue("@ID", ID);

				cmd.ExecuteNonQuery();
			}
		}

		public static int Insert(CheckList.Jobs job, int korisnikID, int intervalDana)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, job, korisnikID, intervalDana);
			}
		}

		public static int Insert(
			FbConnection con,
			CheckList.Jobs job,
			int korisnikID,
			int intervalDana
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO CHECKLIST_ITEM (ID, POSAO, KORISNIK_ID, DATUM_IZVRSENJA, INTERVAL_DANA) VALUES (
((SELECT COALESCE(MAX(ID), 0) FROM CHECKLIST_ITEM) + 1),
@POSAO, @KORISNIK, NULL, @INTERVAL) RETURNING ID",
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

				cmd.Parameters.AddWithValue("@POSAO", (int)job);
				cmd.Parameters.AddWithValue("@KORISNIK", korisnikID);
				cmd.Parameters.AddWithValue("@INTERVAL", intervalDana);

				int rowsAffected = cmd.ExecuteNonQuery();

				return rowsAffected == 0 ? -1 : Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}

		public static CheckListItem Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static CheckListItem Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, POSAO, KORISNIK_ID, DATUM_IZVRSENJA, INTERVAL_DANA FROM CHECKLIST_ITEM WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new CheckListItem()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Job = (CheckList.Jobs)Convert.ToInt32(dr["POSAO"]),
							KorisnikID = Convert.ToInt32(dr["KORISNIK_ID"]),
							DatumIzvrsenja =
								dr["DATUM_IZVRSENJA"] is DBNull
									? null
									: (DateTime?)Convert.ToDateTime(dr["DATUM_IZVRSENJA"]),
							IntervalDana = Convert.ToInt32(dr["INTERVAL_DANA"])
						};
			}

			return null;
		}

		public static List<CheckListItem> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<CheckListItem> List(FbConnection con)
		{
			List<CheckListItem> list = new List<CheckListItem>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, POSAO, KORISNIK_ID, DATUM_IZVRSENJA, INTERVAL_DANA FROM CHECKLIST_ITEM",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new CheckListItem()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Job = (CheckList.Jobs)Convert.ToInt32(dr["POSAO"]),
								KorisnikID = Convert.ToInt32(dr["KORISNIK_ID"]),
								DatumIzvrsenja =
									dr["DATUM_IZVRSENJA"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["DATUM_IZVRSENJA"]),
								IntervalDana = Convert.ToInt32(dr["INTERVAL_DANA"])
							}
						);
			}

			return list;
		}

		public static Task<List<CheckListItem>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}

		public static List<CheckListItem> ListByKorisnikID(int korisnikID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return ListByKorisnikID(con, korisnikID);
			}
		}

		public static List<CheckListItem> ListByKorisnikID(FbConnection con, int korisnikID)
		{
			List<CheckListItem> list = new List<CheckListItem>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, POSAO, KORISNIK_ID, DATUM_IZVRSENJA, INTERVAL_DANA FROM CHECKLIST_ITEM WHERE KORISNIK_ID = @KID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@KID", korisnikID);
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new CheckListItem()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Job = (CheckList.Jobs)Convert.ToInt32(dr["POSAO"]),
								KorisnikID = Convert.ToInt32(dr["KORISNIK_ID"]),
								DatumIzvrsenja =
									dr["DATUM_IZVRSENJA"] is DBNull
										? null
										: (DateTime?)Convert.ToDateTime(dr["DATUM_IZVRSENJA"]),
								IntervalDana = Convert.ToInt32(dr["INTERVAL_DANA"])
							}
						);
			}

			return list;
		}

		public static Task<List<CheckListItem>> ListByKorisnikIDAsync(int korisnikID)
		{
			return Task.Run(() =>
			{
				return ListByKorisnikID(korisnikID);
			});
		}

		public static void Delete(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Delete(con, id);
			}
		}

		public static void Delete(FbConnection con, int id)
		{
			using (FbCommand cmd = new FbCommand("DELETE FROM CHECKLIST_ITEM WHERE ID = @ID", con))
			{
				cmd.Parameters.AddWithValue("@ID", id);

				cmd.ExecuteNonQuery();
			}
		}
	}
}
