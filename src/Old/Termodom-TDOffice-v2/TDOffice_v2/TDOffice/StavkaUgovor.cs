using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;

namespace TDOffice_v2.TDOffice
{
	public class StavkaUgovor
	{
		public int ID { get; set; }
		public int BrDok { get; set; }
		public int RobaID { get; set; }
		public DateTime DatumValute { get; set; }
		public double Iznos { get; set; }
		public Valuta Valuta { get; set; }
		public List<int> Izvodi
		{
			get
			{
				if (_izvodi == null)
					if (string.IsNullOrWhiteSpace(_izvodiJsonStringReadBuffer))
						_izvodi = new List<int>();
					else
						_izvodi = JsonConvert.DeserializeObject<List<int>>(
							_izvodiJsonStringReadBuffer
						);

				return _izvodi;
			}
		}
		private List<int> _izvodi { get; set; } = null;
		private string _izvodiJsonStringReadBuffer { get; set; }

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
					@"UPDATE STAVKA_UGOVOR
                SET
                BRDOK = @BRDOK,
                ROBAID = @ROBAID,
                DATUM_VALUTE = @DV,
                IZNOS = @IZN,
                VALUTA = @VAL,
                IZVODI = @IZVODI
                WHERE ID = @ID
                ",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@BRDOK", BrDok);
				cmd.Parameters.AddWithValue("@ROBAID", RobaID);
				cmd.Parameters.AddWithValue("@DV", DatumValute);
				cmd.Parameters.AddWithValue("@IZN", Iznos);
				cmd.Parameters.AddWithValue("@VAL", (int)Valuta);
				cmd.Parameters.AddWithValue("@IZVODI", JsonConvert.SerializeObject(_izvodi));

				cmd.ExecuteNonQuery();
			}
		}

		public static StavkaUgovor Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static StavkaUgovor Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, BRDOK, ROBAID, DATUM_VALUTE, IZNOS, VALUTA, IZVODI FROM STAVKA_UGOVOR WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new StavkaUgovor()
						{
							ID = Convert.ToInt32(dr["ID"]),
							BrDok = Convert.ToInt32(dr["BRDOK"]),
							RobaID = Convert.ToInt32(dr["ROBAID"]),
							DatumValute = Convert.ToDateTime(dr["DATUM_VALUTE"]),
							Iznos = Convert.ToDouble(dr["IZNOS"]),
							Valuta = (Valuta)Convert.ToInt32(dr["VALUTA"]),
							_izvodiJsonStringReadBuffer = dr["IZVODI"].ToString()
						};
			}

			return null;
		}

		public static List<StavkaUgovor> List(string whereQuery = null)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		public static List<StavkaUgovor> List(FbConnection con, string whereQuery = null)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " AND " + whereQuery;

			List<StavkaUgovor> list = new List<StavkaUgovor>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, BRDOK, ROBAID, DATUM_VALUTE, IZNOS, VALUTA, IZVODI FROM STAVKA_UGOVOR WHERE 1 = 1 "
						+ whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new StavkaUgovor()
							{
								ID = Convert.ToInt32(dr["ID"]),
								BrDok = Convert.ToInt32(dr["BRDOK"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								DatumValute = Convert.ToDateTime(dr["DATUM_VALUTE"]),
								Iznos = Convert.ToDouble(dr["IZNOS"]),
								Valuta = (Valuta)Convert.ToInt32(dr["VALUTA"]),
								_izvodiJsonStringReadBuffer = dr["IZVODI"].ToString()
							}
						);
			}

			return list;
		}

		public static int Insert(
			int brDok,
			int robaID,
			DateTime datumValute,
			double iznos,
			Valuta valuta
		)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, brDok, robaID, datumValute, iznos, valuta);
			}
		}

		public static int Insert(
			FbConnection con,
			int brDok,
			int robaID,
			DateTime datumValute,
			double iznos,
			Valuta valuta
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO STAVKA_UGOVOR
(ID, BRDOK, ROBAID, DATUM_VALUTE, IZNOS, VALUTA, IZVODI)
VALUES
(((SELECT COALESCE(MAX(ID), 0)) + 1), @BRDOK, @RID, @DV, @IZNOS, @VALUTA, @IZVODI)",
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

				cmd.Parameters.AddWithValue("@BRDOK", brDok);
				cmd.Parameters.AddWithValue("@RID", robaID);
				cmd.Parameters.AddWithValue("@DV", datumValute);
				cmd.Parameters.AddWithValue("@IZNOS", iznos);
				cmd.Parameters.AddWithValue("@VALUTA", valuta);
				cmd.Parameters.AddWithValue("@IZVODI", null);

				cmd.ExecuteNonQuery();

				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
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
			using (FbCommand cmd = new FbCommand("DELETE FROM STAVKA_UGOVOR WHERE ID = @ID", con))
			{
				cmd.Parameters.AddWithValue("@ID", id);

				cmd.ExecuteNonQuery();
			}
		}
	}
}
