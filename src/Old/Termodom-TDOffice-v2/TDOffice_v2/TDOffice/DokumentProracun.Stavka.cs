using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.TDOffice
{
	public partial class DokumentProracun
	{
		public class Stavka
		{
			public int ID { get; set; }
			public int DokumentID { get; set; }
			public int RobaID { get; set; }
			public double Kolicina { get; set; }
			public double ProdajnaCenaBezPDV { get; set; }

			private static ManualResetEventSlim _updateMRE = new ManualResetEventSlim(true);

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
						"UPDATE DOKUMENT_PRORACUN_STAVKA SET KOLICINA = @K, PRODAJNA_CENA_BEZ_PDV = @PC WHERE ID = @ID",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@ID", ID);
					cmd.Parameters.AddWithValue("@K", Kolicina);
					cmd.Parameters.AddWithValue("@PC", ProdajnaCenaBezPDV);

					cmd.ExecuteNonQuery();
				}
			}

			public static Stavka Get(int stavkaID)
			{
				using (FbConnection con = new FbConnection(TDOffice.connectionString))
				{
					con.Open();
					return Get(con, stavkaID);
				}
			}

			public static Stavka Get(FbConnection con, int stavkaID)
			{
				using (
					FbCommand cmd = new FbCommand(
						"SELECT ID, DOKUMENT_ID, ROBAID, KOLICINA, PRODAJNA_CENA_BEZ_PDV FROM DOKUMENT_PRORACUN_STAVKA WHERE ID = @ID",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@ID", stavkaID);

					using (FbDataReader dr = cmd.ExecuteReader())
					{
						if (dr.Read())
						{
							return new Stavka()
							{
								ID = Convert.ToInt32(dr["ID"]),
								DokumentID = Convert.ToInt32(dr["DOKUMENT_ID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								Kolicina = Convert.ToDouble(dr["KOLICINA"]),
								ProdajnaCenaBezPDV = Convert.ToDouble(dr["PRODAJNA_CENA_BEZ_PDV"])
							};
						}
					}
				}

				return null;
			}

			public static int Insert(
				DokumentProracun dokument,
				int robaID,
				double kolicina,
				double prodajnaCenaBezPDV
			)
			{
				using (FbConnection con = new FbConnection(TDOffice.connectionString))
				{
					con.Open();
					return Insert(con, dokument, robaID, kolicina, prodajnaCenaBezPDV);
				}
			}

			public static int Insert(
				FbConnection con,
				DokumentProracun dokument,
				int robaID,
				double kolicina,
				double prodajnaCenaBezPDV
			)
			{
				_updateMRE.Wait();
				_updateMRE.Reset();

				int nextID = MaxID(con) + 1;

				using (
					FbCommand cmd = new FbCommand(
						@"INSERT INTO DOKUMENT_PRORACUN_STAVKA
(ID, DOKUMENT_ID, ROBAID, KOLICINA, PRODAJNA_CENA_BEZ_PDV) VALUES (@ID, @DID, @ROBAID, @KOLICINA, @PCBP)",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@ID", nextID);
					cmd.Parameters.AddWithValue("@DID", dokument.ID);
					cmd.Parameters.AddWithValue("@ROBAID", robaID);
					cmd.Parameters.AddWithValue("@KOLICINA", kolicina);
					cmd.Parameters.AddWithValue("@PCBP", prodajnaCenaBezPDV);

					cmd.ExecuteNonQuery();
				}
				_updateMRE.Set();

				return nextID;
			}

			public static List<Stavka> List()
			{
				using (FbConnection con = new FbConnection(TDOffice.connectionString))
				{
					con.Open();
					return List(con);
				}
			}

			public static List<Stavka> List(FbConnection con)
			{
				List<Stavka> list = new List<Stavka>();

				using (
					FbCommand cmd = new FbCommand(
						"SELECT ID, DOKUMENT_ID, ROBAID, KOLICINA, PRODAJNA_CENA_BEZ_PDV FROM DOKUMENT_PRORACUN_STAVKA",
						con
					)
				)
				{
					using (FbDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							list.Add(
								new Stavka()
								{
									ID = Convert.ToInt32(dr["ID"]),
									DokumentID = Convert.ToInt32(dr["DOKUMENT_ID"]),
									RobaID = Convert.ToInt32(dr["ROBAID"]),
									Kolicina = Convert.ToDouble(dr["KOLICINA"]),
									ProdajnaCenaBezPDV = Convert.ToDouble(
										dr["PRODAJNA_CENA_BEZ_PDV"]
									)
								}
							);
						}
					}
				}

				return list;
			}

			public static List<Stavka> ListByDokument(DokumentProracun dokument)
			{
				using (FbConnection con = new FbConnection(TDOffice.connectionString))
				{
					con.Open();
					return ListByDokument(con, dokument);
				}
			}

			public static List<Stavka> ListByDokument(FbConnection con, DokumentProracun dokument)
			{
				List<Stavka> list = new List<Stavka>();

				using (
					FbCommand cmd = new FbCommand(
						"SELECT ID, DOKUMENT_ID, ROBAID, KOLICINA, PRODAJNA_CENA_BEZ_PDV FROM DOKUMENT_PRORACUN_STAVKA WHERE DOKUMENT_ID = @DID",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@DID", dokument.ID);
					using (FbDataReader dr = cmd.ExecuteReader())
					{
						while (dr.Read())
						{
							list.Add(
								new Stavka()
								{
									ID = Convert.ToInt32(dr["ID"]),
									DokumentID = Convert.ToInt32(dr["DOKUMENT_ID"]),
									RobaID = Convert.ToInt32(dr["ROBAID"]),
									Kolicina = Convert.ToDouble(dr["KOLICINA"]),
									ProdajnaCenaBezPDV = Convert.ToDouble(
										dr["PRODAJNA_CENA_BEZ_PDV"]
									)
								}
							);
						}
					}
				}

				return list;
			}

			public static Task<List<Stavka>> ListByDokumentAsync(DokumentProracun dokument)
			{
				return Task.Run(() =>
				{
					return ListByDokument(dokument);
				});
			}

			public static int MaxID(FbConnection con)
			{
				using (
					FbCommand cmd = new FbCommand(
						"SELECT COALESCE(MAX(ID), 0) FROM DOKUMENT_PRORACUN_STAVKA",
						con
					)
				)
				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return Convert.ToInt32(dr[0]);

				return 0;
			}

			public void Delete(int id)
			{
				using (FbConnection con = new FbConnection(TDOffice.connectionString))
				{
					con.Open();
					Delete(con, id);
				}
			}

			public void Delete(FbConnection con, int idStavke)
			{
				using (
					FbCommand cmd = new FbCommand(
						"DELETE FROM DOKUMENT_PRORACUN_STAVKA WHERE ID = @ID",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@ID", idStavke);

					cmd.ExecuteNonQuery();
				}
			}

			public static void DeleteAll(int id)
			{
				using (FbConnection con = new FbConnection(TDOffice.connectionString))
				{
					con.Open();
					DeleteAll(con, id);
				}
			}

			public static void DeleteAll(FbConnection con, int idDokumenta)
			{
				using (
					FbCommand cmd = new FbCommand(
						"DELETE FROM DOKUMENT_PRORACUN_STAVKA WHERE DOKUMENT_ID = @ID",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@ID", idDokumenta);

					cmd.ExecuteNonQuery();
				}
			}
		}
	}
}
