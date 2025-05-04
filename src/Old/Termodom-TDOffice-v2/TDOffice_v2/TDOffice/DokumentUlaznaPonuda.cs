using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public class DokumentUlaznaPonuda
	{
		public int ID { get; set; }
		public DateTime Datum { get; set; }
		public int Korisnik { get; set; }
		public DokumentStatus Status { get; set; }
		public DateTime DatumPocetkaVazenja { get; set; }
		public DateTime DatumKrajaVazenja { get; set; }

		public DokumentUlaznaPonuda() { }

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
					@"
UPDATE DOKUMENT_ULAZNA_PONUDA
SET
KORISNIK = @K,
STATUS = @S,
DATUM_POCETAK_VAZENJA = @DPV,
DATUM_KRAJ_VAZENJA = @DKV",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@K", Korisnik);
				cmd.Parameters.AddWithValue("@S", (int)Status);
				cmd.Parameters.AddWithValue("@DPV", DatumPocetkaVazenja);
				cmd.Parameters.AddWithValue("@DKV", DatumKrajaVazenja);

				cmd.ExecuteNonQuery();
			}
		}

		public static DokumentUlaznaPonuda Get(int id)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, id);
			}
		}

		public static DokumentUlaznaPonuda Get(FbConnection con, int id)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
ID, DATUM, KORISNIK, STATUS, DATUM_POCETAK_VAZENJA, DATUM_KRAJ_VAZENJA
FROM DOKUMENT_ULAZNA_PONUDA WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("ID", id);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new DokumentUlaznaPonuda()
						{
							ID = Convert.ToInt32(dr["ID"]),
							Datum = Convert.ToDateTime(dr["DATUM"]),
							Korisnik = Convert.ToInt32(dr["KORISNIK"]),
							Status = (DokumentStatus)Convert.ToInt32(dr["KORISNIK"]),
							DatumPocetkaVazenja = Convert.ToDateTime(dr["DATUM_POCETAK_VAZENJA"]),
							DatumKrajaVazenja = Convert.ToDateTime(dr["DATUM_KRAJ_VAZENJA"])
						};
			}
			return null;
		}

		public static List<DokumentUlaznaPonuda> List(string whereQuery = null)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		public static List<DokumentUlaznaPonuda> List(FbConnection con, string whereQuery = null)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " AND " + whereQuery;

			List<DokumentUlaznaPonuda> list = new List<DokumentUlaznaPonuda>();
			using (
				FbCommand cmd = new FbCommand(
					@"SELECT
ID, DATUM, KORISNIK, STATUS, DATUM_POCETAK_VAZENJA, DATUM_KRAJ_VAZENJA
FROM DOKUMENT_ULAZNA_PONUDA
WHERE 1 = 1 " + whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new DokumentUlaznaPonuda()
							{
								ID = Convert.ToInt32(dr["ID"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								Korisnik = Convert.ToInt32(dr["KORISNIK"]),
								Status = (DokumentStatus)Convert.ToInt32(dr["KORISNIK"]),
								DatumPocetkaVazenja = Convert.ToDateTime(
									dr["DATUM_POCETAK_VAZENJA"]
								),
								DatumKrajaVazenja = Convert.ToDateTime(dr["DATUM_KRAJ_VAZENJA"])
							}
						);
			}
			return list;
		}

		public static int Insert(
			int korisnikID,
			DateTime datumPocetkaVazenja,
			DateTime datumKrajaVazenja
		)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Insert(con, korisnikID, datumPocetkaVazenja, datumKrajaVazenja);
			}
		}

		public static int Insert(
			FbConnection con,
			int korisnikID,
			DateTime datumPocetkaVazenja,
			DateTime datumKrajaVazenja
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"
INSERT INTO DOKUMENT_ULAZNA_PONUDA
(ID, DATUM, KORISNIK, STATUS, DATUM_POCETAK_VAZENJA, DATUM_KRAJ_VAZENJA)
VALUES
(((SELECT COALESCE(MAX(ID), 0) FROM DOKUMENT_ULAZNA_PONUDA) + 1), @DATUM, @KORISNIK, 0, @DPV, @DKV)
RETURNING ID",
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
				cmd.Parameters.AddWithValue("@DATUM", DateTime.Now);
				cmd.Parameters.AddWithValue("@KORISNIK", korisnikID);
				cmd.Parameters.AddWithValue("@DPV", datumPocetkaVazenja);
				cmd.Parameters.AddWithValue("@DKV", datumKrajaVazenja);

				cmd.ExecuteNonQuery();

				return Convert.ToInt32(cmd.Parameters["ID"].Value);
			}
		}
	}
}
