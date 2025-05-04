using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;
using TDOffice_v2.TDOffice;

namespace TDOffice_v2.Komercijalno
{
	public class Promena
	{
		/// <summary>
		///
		/// </summary>
		public class PromenaCollection : IEnumerable<Promena>
		{
			private Dictionary<int, Promena> _dict = new Dictionary<int, Promena>();

			/// <summary>
			///
			/// </summary>
			/// <param name="promenaID"></param>
			/// <returns></returns>
			public Promena this[int promenaID] => _dict[promenaID];

			/// <summary>
			///
			/// </summary>
			/// <param name="dict"></param>
			public PromenaCollection(Dictionary<int, Promena> dict) => _dict = dict;

			/// <summary>
			///
			/// </summary>
			/// <returns></returns>
			public IEnumerator<Promena> GetEnumerator() => _dict.Values.GetEnumerator();

			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		}

		#region Properties
		/// <summary>
		///
		/// </summary>
		public int PromenaID { get; set; }

		/// <summary>
		///
		/// </summary>
		public int VrstaNal { get; set; }

		/// <summary>
		///
		/// </summary>
		public string BrNaloga { get; set; } = "";

		/// <summary>
		///
		/// </summary>
		public DateTime DatNal { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Konto { get; set; } = "";

		/// <summary>
		///
		/// </summary>
		public string Opis { get; set; }

		/// <summary>
		///
		/// </summary>
		public int? PPID { get; set; }

		/// <summary>
		///
		/// </summary>
		public int? BrDok { get; set; }

		/// <summary>
		///
		/// </summary>
		public int? VrDok { get; set; }

		/// <summary>
		///
		/// </summary>
		public DateTime? DatDPO { get; set; }

		/// <summary>
		///
		/// </summary>
		public double? Duguje { get; set; }

		/// <summary>
		///
		/// </summary>
		public double? Potrazuje { get; set; }

		/// <summary>
		///
		/// </summary>
		public DateTime? DatVal { get; set; }

		/// <summary>
		///
		/// </summary>
		public double? VDuguje { get; set; }

		/// <summary>
		///
		/// </summary>
		public double? VPotrazuje { get; set; }

		/// <summary>
		///
		/// </summary>
		public string Valuta { get; set; }

		/// <summary>
		///
		/// </summary>
		public double Kurs { get; set; }

		/// <summary>
		///
		/// </summary>
		public string MTID { get; set; }

		/// <summary>
		///
		/// </summary>
		public int? A { get; set; }

		/// <summary>
		///
		/// </summary>
		public int? StavkaID { get; set; }
		#endregion
		public Promena() { }

		[Obsolete("Koristiit Collection() metodu!")]
		public static List<Promena> List(string whereQuery = null)
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		[Obsolete("Koristiit Collection() metodu!")]
		public static List<Promena> List(FbConnection con, string whereQuery = null)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " WHERE " + whereQuery;

			List<Promena> pr = new List<Promena>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT PPID, POTRAZUJE, DUGUJE, KONTO, VRSTANAL, VRDOK, DATNAL FROM PROMENE"
						+ whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						pr.Add(
							new Promena()
							{
								PPID = dr["PPID"] is DBNull ? 0 : Convert.ToInt32(dr["PPID"]),
								VrstaNal = Convert.ToInt32(dr["VRSTANAL"]),
								Konto = Convert.ToString(dr["KONTO"]),
								Duguje =
									dr["DUGUJE"] is DBNull ? 0 : Convert.ToDouble(dr["DUGUJE"]),
								Potrazuje =
									dr["POTRAZUJE"] is DBNull
										? 0
										: Convert.ToDouble(dr["POTRAZUJE"]),
								VrDok =
									dr["VRDOK"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["VRDOK"]),
								DatNal = Convert.ToDateTime(dr["DATNAL"])
							}
						);
			}
			return pr;
		}

		[Obsolete("Koristiit Collection() metodu!")]
		public static List<Promena> ListByPPID(int PPID)
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return ListByPPID(PPID);
			}
		}

		[Obsolete("Koristiit Collection() metodu!")]
		public static List<Promena> ListByPPID(FbConnection con, int PPID)
		{
			List<Promena> pr = new List<Promena>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT PPID, POTRAZUJE, DUGUJE, KONTO, VRSTANAL, VRDOK FROM PROMENE WHERE PPID = @PPID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@PPID", PPID);
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						pr.Add(
							new Promena()
							{
								PPID = dr["PPID"] is DBNull ? 0 : Convert.ToInt32(dr["PPID"]),
								VrstaNal = Convert.ToInt32(dr["VRSTANAL"]),
								Konto = Convert.ToString(dr["KONTO"]),
								Duguje =
									dr["DUGUJE"] is DBNull ? 0 : Convert.ToDouble(dr["DUGUJE"]),
								Potrazuje =
									dr["POTRAZUJE"] is DBNull
										? 0
										: Convert.ToDouble(dr["POTRAZUJE"]),
								VrDok =
									dr["VRDOK"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["VRDOK"])
							}
						);
			}
			return pr;
		}

		/// <summary>
		/// Vraca kolekciju promena za datu godinu.
		/// </summary>
		/// <param name="godina">Godina za koju se promene uzimaju</param>
		/// <param name="konto">Opcioni parametar filtriranja</param>
		/// <param name="vrstaNal">Opcioni parametar filtriranja</param>
		/// <returns></returns>
		public async static Task<PromenaCollection> CollectionAsync(
			int godina,
			string konto,
			int[] vrstaNal
		)
		{
			string url = $"/komercijalno/promena/list?godina={godina}&";

			if (!string.IsNullOrWhiteSpace(konto))
				url += $"konto={konto}&";

			if (vrstaNal != null && vrstaNal.Length > 0)
				foreach (int vn in vrstaNal)
					url += $"vrstaNal={vn}&";

			var response = await TDBrain_v3.GetAsync(url);

			if ((int)response.StatusCode == 200)
			{
				var result = JsonConvert.DeserializeObject<Dictionary<int, Promena>>(
					await response.Content.ReadAsStringAsync()
				);
				return new PromenaCollection(result);
			}

			throw new Exception("Error calling " + url);
		}
	}
}
