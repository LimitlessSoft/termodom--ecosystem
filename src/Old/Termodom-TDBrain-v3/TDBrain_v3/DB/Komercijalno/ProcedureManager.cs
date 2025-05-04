using FirebirdSql.Data.FirebirdClient;

namespace TDBrain_v3.DB.Komercijalno
{
	public static class ProcedureManager
	{
		public static double ProdajnaCenaNaDan(
			int magacinID,
			int godina,
			int robaID,
			DateTime datum
		)
		{
			using (
				FbConnection con = new FbConnection(
					DB.Settings.ConnectionStringKomercijalno[magacinID, godina]
				)
			)
			{
				con.Open();
				return ProdajnaCenaNaDan(con, magacinID, robaID, datum);
			}
		}

		public static double ProdajnaCenaNaDan(
			FbConnection con,
			int magacinID,
			int robaID,
			DateTime datum
		)
		{
			using (FbCommand cmd = new FbCommand("CENE_NA_DAN", con))
			{
				cmd.CommandType = System.Data.CommandType.StoredProcedure;

				cmd.Parameters.AddWithValue("MAGACINID", magacinID);
				cmd.Parameters.AddWithValue("ROBAID", robaID);
				cmd.Parameters.AddWithValue("DATUM", datum);

				cmd.Parameters.Add("PRODAJNACENA", FbDbType.Double).Direction = System
					.Data
					.ParameterDirection
					.Output;
				cmd.Parameters.Add("NABAVNACENA", FbDbType.Double).Direction = System
					.Data
					.ParameterDirection
					.Output;

				cmd.ExecuteScalar();

				return Math.Round(Convert.ToDouble(cmd.Parameters["PRODAJNACENA"].Value), 2);
			}
		}

		public static string NextLinked(DateTime datum, int magacinID)
		{
			using (
				FbConnection con = new FbConnection(
					DB.Settings.ConnectionStringKomercijalno[magacinID, datum.Year]
				)
			)
			{
				con.Open();
				using (
					FbCommand cmd = new FbCommand(
						@"SELECT MAX(CAST(LINKED AS INTEGER)) FROM DOKUMENT
                                                        WHERE DATUM = @DATUM
                                                        AND MAGACINID = @MAGACINID
                                                        AND(LINKED <> '9999999999') AND(LINKED IS NOT NULL) AND(LINKED <> '')",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@DATUM", datum);
					cmd.Parameters.AddWithValue("@MAGACINID", magacinID);

					using (FbDataReader dr = cmd.ExecuteReader())
						if (dr.Read())
							return (dr[0] is DBNull ? 1 : Convert.ToInt32(dr[0])).ToString(
								"0000000000"
							);
				}
			}

			return "0000000000";
		}
	}
}
