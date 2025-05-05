using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class MagacinClan
	{
		public int ID { get; set; }
		public int MagacinID { get; set; }
		public int KorisnikID { get; set; }
		public Enums.MagacinClanTip Tip { get; set; }

		public static MagacinClan Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static MagacinClan Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT ID, MAGACINID, USERID, TIP FROM MAGACIN_CLAN WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
						return new MagacinClan()
						{
							ID = Convert.ToInt32(dr["ID"]),
							MagacinID = Convert.ToInt32(dr["MAGACINID"]),
							KorisnikID = Convert.ToInt32(dr["USERID"]),
							Tip = (Enums.MagacinClanTip)Convert.ToInt16(dr["TIP"])
						};
				}
			}

			return null;
		}

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
					"UPDATE MAGACIN_CLAN SET USERID = @U, TIP = @T, MAGACINID = @MID WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@U", KorisnikID);
				cmd.Parameters.AddWithValue("@T", Tip);
				cmd.Parameters.AddWithValue("@MID", MagacinID);
				cmd.Parameters.AddWithValue("@ID", ID);

				cmd.ExecuteNonQuery();
			}
		}

		public static void Insert(int magacinID, int korisnikID, Enums.MagacinClanTip tip)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Insert(con, magacinID, korisnikID, tip);
			}
		}

		public static void Insert(
			FbConnection con,
			int magacinID,
			int korisnikID,
			Enums.MagacinClanTip tip
		)
		{
			if (tip == Enums.MagacinClanTip.Vlasnik)
			{
				List<MagacinClan> mcs = ListByMagacinID(con, magacinID);
				MagacinClan mc = mcs.Where(x => x.Tip == Enums.MagacinClanTip.Vlasnik)
					.FirstOrDefault();
				if (mc != null && mc.KorisnikID == korisnikID)
					throw new Exception("Ovaj magacin vec ima vlasnika!");

				mcs = ListByKorisnikID(con, korisnikID);
				mc = mcs.Where(x => x.Tip == Enums.MagacinClanTip.Vlasnik).FirstOrDefault();
				if (mc != null && mc.KorisnikID == korisnikID)
					throw new Exception("Ovaj korisnik je vec magacin drugog vlasnika!");
			}

			using (
				FbCommand cmd = new FbCommand(
					"INSERT INTO MAGACIN_CLAN (ID, MAGACINID, USERID, TIP) VALUES (((SELECT COALESCE(MAX(ID), 0) FROM MAGACIN_CLAN) + 1), @MID, @U, @TIP)",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@MID", magacinID);
				cmd.Parameters.AddWithValue("@U", korisnikID);
				cmd.Parameters.AddWithValue("@TIP", tip);

				cmd.ExecuteNonQuery();
			}
		}

		public static List<MagacinClan> List()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con);
			}
		}

		public static List<MagacinClan> List(FbConnection con)
		{
			List<MagacinClan> list = new List<MagacinClan>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, MAGACINID, USERID, TIP FROM MAGACIN_CLAN",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new MagacinClan()
							{
								ID = Convert.ToInt32(dr["ID"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								KorisnikID = Convert.ToInt32(dr["USERID"]),
								Tip = (Enums.MagacinClanTip)Convert.ToInt32(dr["TIP"])
							}
						);
			}

			return list;
		}

		public static Task<List<MagacinClan>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}

		public static List<MagacinClan> ListByMagacinID(int magacinID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return ListByMagacinID(con, magacinID);
			}
		}

		public static List<MagacinClan> ListByMagacinID(FbConnection con, int magacinID)
		{
			List<MagacinClan> list = new List<MagacinClan>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, MAGACINID, USERID, TIP FROM MAGACIN_CLAN WHERE MAGACINID = @MID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@MID", magacinID);
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new MagacinClan()
							{
								ID = Convert.ToInt32(dr["ID"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								KorisnikID = Convert.ToInt32(dr["USERID"]),
								Tip = (Enums.MagacinClanTip)Convert.ToInt32(dr["TIP"])
							}
						);
			}

			return list;
		}

		public static List<MagacinClan> ListByKorisnikID(int korisnikID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return ListByKorisnikID(con, korisnikID);
			}
		}

		public static List<MagacinClan> ListByKorisnikID(FbConnection con, int korisnikID)
		{
			List<MagacinClan> list = new List<MagacinClan>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, MAGACINID, USERID, TIP FROM MAGACIN_CLAN WHERE USERID = @UID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@UID", korisnikID);
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new MagacinClan()
							{
								ID = Convert.ToInt32(dr["ID"]),
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								KorisnikID = Convert.ToInt32(dr["USERID"]),
								Tip = (Enums.MagacinClanTip)Convert.ToInt32(dr["TIP"])
							}
						);
			}

			return list;
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
			using (FbCommand cmd = new FbCommand("DELETE FROM MAGACIN_CLAN WHERE ID = @ID", con))
			{
				cmd.Parameters.AddWithValue("@ID", id);

				cmd.ExecuteNonQuery();
			}
		}
	}
}
