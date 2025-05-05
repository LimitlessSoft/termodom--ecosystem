using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using LimitlessSoft.Buffer;
using Newtonsoft.Json;

namespace TDOffice_v2.Komercijalno
{
	public class RobaUMagacinu
	{
		private static Buffer<List<RobaUMagacinu>> _buffer = new Buffer<List<RobaUMagacinu>>(List);

		public int MagacinID { get; set; }
		public int RobaID { get; set; }
		public double ProdajnaCena { get; set; }
		public double Stanje { get; set; }
		public double OptimalneZalihe { get; set; }
		public double KriticneZalihe { get; set; }
		public double NabavnaCena { get; set; }

		public RobaUMagacinu() { }

		public static RobaUMagacinu Get(int magacinID, int robaID)
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return Get(con, magacinID, robaID);
			}
		}

		public static RobaUMagacinu Get(FbConnection con, int magacinID, int robaID)
		{
			RobaUMagacinu robum = null;

			using (
				FbCommand cmd = new FbCommand(
					"SELECT MAGACINID, ROBAID, PRODAJNACENA, STANJE, OPTZAL, KRITZAL, NABAVNACENA FROM ROBAUMAGACINU WHERE MAGACINID = @MID AND ROBAID = @RID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@MID", magacinID);
				cmd.Parameters.AddWithValue("@RID", robaID);
				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						robum = new RobaUMagacinu()
						{
							MagacinID = Convert.ToInt32(dr["MAGACINID"]),
							RobaID = Convert.ToInt32(dr["ROBAID"]),
							KriticneZalihe = Convert.ToDouble(dr["KRITZAL"]),
							OptimalneZalihe = Convert.ToDouble(dr["OPTZAL"]),
							ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
							NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
							Stanje = Convert.ToDouble(dr["STANJE"])
						};
			}
			return robum;
		}

		public static Task<RobaUMagacinu> GetAsync(int magacinID, int robaID)
		{
			return Task.Run(() =>
			{
				return RobaUMagacinu.Get(magacinID, robaID);
			});
		}

		public static List<RobaUMagacinu> List()
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return List(con);
			}
		}

		public static Task<List<RobaUMagacinu>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}

		public static List<RobaUMagacinu> List(FbConnection con)
		{
			List<RobaUMagacinu> list = new List<RobaUMagacinu>();

			using (
				FbCommand cmd = new FbCommand(
					"SELECT MAGACINID, ROBAID, PRODAJNACENA, STANJE, OPTZAL, KRITZAL, NABAVNACENA FROM ROBAUMAGACINU",
					con
				)
			)
			using (FbDataReader dr = cmd.ExecuteReader())
				while (dr.Read())
					list.Add(
						new RobaUMagacinu()
						{
							MagacinID = Convert.ToInt32(dr["MAGACINID"]),
							RobaID = Convert.ToInt32(dr["ROBAID"]),
							KriticneZalihe = Convert.ToDouble(dr["KRITZAL"]),
							OptimalneZalihe = Convert.ToDouble(dr["OPTZAL"]),
							ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
							NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
							Stanje = Convert.ToDouble(dr["STANJE"])
						}
					);

			_buffer.Set(list);
			return list;
		}

		public static Task<List<RobaUMagacinu>> ListAsync(FbConnection con)
		{
			return Task.Run(() =>
			{
				return List(con);
			});
		}

		public static List<RobaUMagacinu> ListByMagacinID(int magacinID)
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return ListByMagacinID(con, magacinID);
			}
		}

		public static Task<List<RobaUMagacinu>> ListByMagacinIDAsync(int magacinID)
		{
			return Task.Run(() =>
			{
				return ListByMagacinID(magacinID);
			});
		}

		public static Task<List<RobaUMagacinu>> ListByMagacinIDAsync(int godina, int magacinID)
		{
			return Task.Run(() =>
			{
				using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
				{
					con.Open();
					return ListByMagacinID(con, magacinID);
				}
			});
		}

		public static List<RobaUMagacinu> ListByMagacinID(FbConnection con, int magacinID)
		{
			List<RobaUMagacinu> list = new List<RobaUMagacinu>();

			using (
				FbCommand cmd = new FbCommand(
					"SELECT MAGACINID, ROBAID, PRODAJNACENA, STANJE, OPTZAL, KRITZAL, NABAVNACENA FROM ROBAUMAGACINU WHERE MAGACINID = @MID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@MID", magacinID);
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new RobaUMagacinu()
							{
								MagacinID = Convert.ToInt32(dr["MAGACINID"]),
								RobaID = Convert.ToInt32(dr["ROBAID"]),
								KriticneZalihe = Convert.ToDouble(dr["KRITZAL"]),
								OptimalneZalihe = Convert.ToDouble(dr["OPTZAL"]),
								ProdajnaCena = Convert.ToDouble(dr["PRODAJNACENA"]),
								NabavnaCena = Convert.ToDouble(dr["NABAVNACENA"]),
								Stanje = Convert.ToDouble(dr["STANJE"])
							}
						);
				_buffer.Set(list);
				return list;
			}
		}

		public static List<RobaUMagacinu> BufferedList(TimeSpan interval)
		{
			return _buffer.Get(interval);
		}

		public static async Task<Termodom.Data.Entities.Komercijalno.RobaUMagacinuDictionary> DictionaryAsync(
			int? magacinID = null
		)
		{
			var response = await TDBrain_v3.GetAsync(
				$"/Komercijalno/RobaUMagacinu/Dictionary?magacinid={magacinID}"
			);

			switch ((int)response.StatusCode)
			{
				case 200:
					return JsonConvert.DeserializeObject<Termodom.Data.Entities.Komercijalno.RobaUMagacinuDictionary>(
						await response.Content.ReadAsStringAsync()
					);
				case 500:
					throw new Termodom.Data.Exceptions.APIServerException();
				default:
					throw new Termodom.Data.Exceptions.APIUnhandledStatusException(
						response.StatusCode
					);
			}
		}
	}
}
