using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class Partner
	{
		public int ID { get; set; }
		public string Naziv { get; set; }
		public string Mobilni { get; set; }
		public string Mejl { get; set; }
		public bool SMSBlok { get; set; } = false;
		public int Grad { get; set; }
		public string Komentar { get; set; }
		public List<int> Grupe { get; set; }
		public int? KomercijalnoPPID { get; set; }
		public int? WebID { get; set; }
		public int? MagacinID { get; set; }

		public Partner() { }

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
			if (!MobileNumber.IsValid(Mobilni))
				throw new Exception("Morate uneti validan mobilni telefon!");

			using (
				FbCommand cmd = new FbCommand(
					@"UPDATE PARTNER SET NAZIV = @N, MOBILNI = @MOB, SMS_BLOK = @BLOK, MEJL = @MEJL,
                GRAD = @GRAD, KOMENTAR = @KOMENTAR, KOMERCIJALNO_PPID = @KPPID, WEB_ID = @WID,
                MAGACINID = @MID WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@N", Naziv);
				cmd.Parameters.AddWithValue("@MOB", MobileNumber.Collate(Mobilni));
				cmd.Parameters.AddWithValue("@BLOK", SMSBlok ? 1 : 0);
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@MEJL", Mejl);
				cmd.Parameters.AddWithValue("@GRAD", Grad);
				cmd.Parameters.AddWithValue("@KOMENTAR", Komentar);
				cmd.Parameters.AddWithValue("@KPPID", KomercijalnoPPID);
				cmd.Parameters.AddWithValue("@WID", WebID);
				cmd.Parameters.AddWithValue("@MID", MagacinID);

				cmd.ExecuteNonQuery();
			}

			PartnerGrupaItem.Delete(con, ID);

			foreach (int i in Grupe)
				PartnerGrupaItem.Insert(con, ID, i);
		}

		public static int Insert(
			string naziv,
			string mobilni,
			string mejl,
			int grad,
			string komentar,
			List<int> grupe,
			int? komercijalnoPPID,
			int? webID,
			int? magacinID
		)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(
					con,
					naziv,
					mobilni,
					mejl,
					grad,
					komentar,
					grupe,
					komercijalnoPPID,
					webID,
					magacinID
				);
			}
		}

		public static int Insert(
			FbConnection con,
			string naziv,
			string mobilni,
			string mejl,
			int grad,
			string komentar,
			List<int> grupe,
			int? komercijalnoPPID,
			int? webID,
			int? magacinID
		)
		{
			if (!MobileNumber.IsValid(mobilni))
				throw new Exception("Morate uneti validan mobilni telefon!");

			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO PARTNER (ID, NAZIV, MOBILNI, MEJL, GRAD, KOMENTAR, KOMERCIJALNO_PPID, WEB_ID, MAGACINID) VALUES
                (((SELECT COALESCE(MAX(ID), 0) FROM PARTNER) + 1), @NAZIV, @MOBILNI, @MEJL, @GRAD, @KOMENTAR, @KID, @WID, @MID) RETURNING ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@NAZIV", naziv);
				cmd.Parameters.AddWithValue("@MEJL", mejl);
				cmd.Parameters.AddWithValue("@GRAD", grad);
				cmd.Parameters.AddWithValue("@KOMENTAR", komentar);
				cmd.Parameters.AddWithValue("@MOBILNI", MobileNumber.Collate(mobilni));
				cmd.Parameters.AddWithValue("@KID", komercijalnoPPID);
				cmd.Parameters.AddWithValue("@WID", webID);
				cmd.Parameters.AddWithValue("@MID", magacinID);
				cmd.Parameters.Add(
					new FbParameter("ID", FbDbType.Integer)
					{
						Direction = System.Data.ParameterDirection.Output
					}
				);

				cmd.ExecuteNonQuery();

				try
				{
					int rID = Convert.ToInt32(cmd.Parameters["ID"].Value);
					if (grupe != null)
					{
						using (FbConnection con1 = new FbConnection(TDOffice.connectionString))
						{
							con1.Open();
							foreach (int pgi in grupe)
								PartnerGrupaItem.Insert(con1, rID, pgi);
						}
					}

					return rID;
				}
				catch
				{
					throw new Exceptions.FailedDatabaseInsertException();
				}
			}
		}

		public static int InsertBrziKontakt(string mobilni)
		{
			/// Proveravam da li ovaj kontakt postoji u bazi, a ako ne postoji insertujem ga
			Partner part = List($"MOBILNI = '{MobileNumber.Collate(mobilni)}'").FirstOrDefault();

			if (part == null)
				return Insert(
					"*** brzi kontakt ***",
					MobileNumber.Collate(mobilni),
					null,
					0,
					null,
					null,
					null,
					null,
					null
				);

			return -1;
		}

		public static Partner Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static Partner Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, NAZIV, MOBILNI, SMS_BLOK, MEJL, GRAD, KOMENTAR, KOMERCIJALNO_PPID, WEB_ID, MAGACINID FROM PARTNER WHERE ID = @ID ",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
				{
					if (dr.Read())
						return new Partner()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Naziv = dr["NAZIV"].ToString(),
							Mobilni = dr["MOBILNI"].ToStringOrDefault(),
							SMSBlok = Convert.ToInt16(dr["SMS_BLOK"]) == 1,
							Mejl = dr["MEJL"].ToStringOrDefault(),
							Grad = Convert.ToInt32(dr["GRAD"]),
							Komentar = dr["KOMENTAR"].ToStringOrDefault(),
							Grupe = PartnerGrupaItem
								.ListByPartner(Convert.ToInt32(dr["ID"]))
								.Select(x => x.PartnerGrupaID)
								.ToList(),
							KomercijalnoPPID =
								dr["KOMERCIJALNO_PPID"] is DBNull
									? null
									: (int?)Convert.ToInt32(dr["KOMERCIJALNO_PPID"]),
							WebID =
								dr["WEB_ID"] is DBNull ? null : (int?)Convert.ToInt32(dr["WEB_ID"]),
							MagacinID =
								dr["MAGACINID"] is DBNull
									? null
									: (int?)Convert.ToInt32(dr["MAGACINID"])
						};
				}
			}
			return null;
		}

		public static List<Partner> List(string whereQuery = null)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		public static List<Partner> List(FbConnection con, string whereQuery = null)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " WHERE " + whereQuery;

			List<Partner> list = new List<Partner>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, NAZIV, MOBILNI, SMS_BLOK, MEJL, GRAD, KOMENTAR, KOMERCIJALNO_PPID, WEB_ID, MAGACINID FROM PARTNER"
						+ whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new Partner()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Naziv = dr["NAZIV"].ToString(),
								Mobilni = dr["MOBILNI"].ToStringOrDefault(),
								SMSBlok = Convert.ToInt16(dr["SMS_BLOK"]) == 1,
								Mejl = dr["MEJL"].ToStringOrDefault(),
								Grad = Convert.ToInt32(dr["GRAD"]),
								Komentar = dr["KOMENTAR"].ToStringOrDefault(),
								Grupe = PartnerGrupaItem
									.ListByPartner(Convert.ToInt32(dr["ID"]))
									.Select(x => x.PartnerGrupaID)
									.ToList(),
								KomercijalnoPPID =
									dr["KOMERCIJALNO_PPID"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["KOMERCIJALNO_PPID"]),
								WebID =
									dr["WEB_ID"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["WEB_ID"]),
								MagacinID =
									dr["MAGACINID"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["MAGACINID"])
							}
						);
				}
			}
			return list;
		}

		public static Task<List<Partner>> ListAsync(string whereQuery = null)
		{
			return Task.Run(() =>
			{
				return List(whereQuery);
			});
		}
	}
}
