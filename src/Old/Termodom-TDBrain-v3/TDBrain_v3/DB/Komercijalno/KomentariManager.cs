using FirebirdSql.Data.FirebirdClient;

namespace TDBrain_v3.DB.Komercijalno
{
	public class KomentariManager
	{
		public int VrDok { get; set; }
		public int BrDok { get; set; }
		public string? Komentar { get; set; }
		public string? IntKomentar { get; set; }
		public string? PrivKomentar { get; set; }

		public void Update(int magacinID, int godina)
		{
			using (
				FbConnection con = new FbConnection(
					DB.Settings.ConnectionStringKomercijalno[magacinID, godina]
				)
			)
			{
				con.Open();
				Update(con);
			}
		}

		public void Update(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand(
					"UPDATE KOMENTARI SET KOMENTAR = @KOMENTAR, INTKOMENTAR = @INTKOMENTAR, PRIVKOMENTAR = @PRIVKOMENTAR WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@KOMENTAR", Komentar);
				cmd.Parameters.AddWithValue("@INTKOMENTAR", IntKomentar);
				cmd.Parameters.AddWithValue("@PRIVKOMENTAR", PrivKomentar);
				cmd.Parameters.AddWithValue("@VRDOK", VrDok);
				cmd.Parameters.AddWithValue("@BRDOK", BrDok);

				cmd.ExecuteNonQuery();
			}
		}

		public static KomentariManager Get(int magacinID, int godina, int vrDok, int brDok)
		{
			using (
				FbConnection con = new FbConnection(
					DB.Settings.ConnectionStringKomercijalno[magacinID, godina]
				)
			)
			{
				con.Open();
				return Get(con, vrDok, brDok);
			}
		}

		public static KomentariManager Get(FbConnection con, int vrDok, int brDok)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT KOMENTAR, INTKOMENTAR, PRIVKOMENTAR FROM KOMENTARI WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@VRDOK", vrDok);
				cmd.Parameters.AddWithValue("@BRDOK", brDok);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new KomentariManager()
						{
							Komentar = dr["KOMENTAR"].ToStringOrDefault(),
							IntKomentar = dr["INTKOMENTAR"].ToStringOrDefault(),
							PrivKomentar = dr["PRIVKOMENTAR"].ToStringOrDefault()
						};
			}

			return null;
		}

		public static void Insert(
			int magacinID,
			int godina,
			int vrDok,
			int brDok,
			string komentar,
			string intKomentar,
			string privKomentar
		)
		{
			using (
				FbConnection con = new FbConnection(
					DB.Settings.ConnectionStringKomercijalno[magacinID, godina]
				)
			)
			{
				con.Open();
				Insert(con, vrDok, brDok, komentar, intKomentar, privKomentar);
			}
		}

		public static void Insert(
			FbConnection con,
			int vrDok,
			int brDok,
			string komentar,
			string intKomentar,
			string privKomentar
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					"INSERT INTO KOMENTARI (VRDOK, BRDOK, KOMENTAR, INTKOMENTAR, PRIVKOMENTAR) VALUES (@VRDOK, @BRDOK, @KOM, @INTKOM, @PRIVKOM)",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@VRDOK", vrDok);
				cmd.Parameters.AddWithValue("@BRDOK", brDok);
				cmd.Parameters.AddWithValue("@KOM", komentar);
				cmd.Parameters.AddWithValue("@INTKOM", intKomentar);
				cmd.Parameters.AddWithValue("@PRIVKOM", privKomentar);

				cmd.ExecuteNonQuery();
			}
		}
	}
}
