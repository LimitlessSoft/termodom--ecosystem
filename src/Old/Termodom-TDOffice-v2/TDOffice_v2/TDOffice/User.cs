using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;
using LimitlessSoft.Buffer;
using Newtonsoft.Json;

namespace TDOffice_v2.TDOffice
{
	public partial class User
	{
		public int ID { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public int MagacinID { get; set; } = 0;
		public int? KomercijalnoUserID { get; set; }
		public int Grad { get; set; }
		public Info Tag { get; set; }
		public bool OpomeniZaNeizvrseneZadatke { get; set; }

		public int BonusZakljucavanjaCount { get; set; }

		public double BonusZakljucavanjaLimit { get; set; }

		public User() { }

		/// <summary>
		/// Proverava da li korisnik ima pravo za koriscenje modula
		/// </summary>
		/// <param name="modulID"></param>
		/// <returns></returns>
		public bool ImaPravo(int modulID)
		{
			Pravo pravo = Pravo.Get(modulID, ID);

			if (pravo != null && pravo.Value == 1)
				return true;

			return false;
		}

		/// <summary>
		/// Azurira podatke korisnik. Osnov: ID
		/// </summary>
		public void Update()
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Update(con);
			}
		}

		/// <summary>
		/// Azurira podatke korisnik. Osnov: ID
		/// </summary>
		public void Update(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"UPDATE USERS SET MAGACINID = @MID,
                KOMERCIJALNO_USER_ID = @KID,
                GRAD = @GRAD,
                TAG = @TAG,
                OPOMENI_ZA_NEIZVRSENI_ZADATAK = @OZN,
                PW = @PW,
                BONUS_ZAKLJUCAVANJA_COUNT = @BZC,
                BONUS_ZAKLJUCAVANJA_LIMIT = @BZL
                WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@GRAD", Grad);
				cmd.Parameters.AddWithValue("@PW", Password);
				cmd.Parameters.AddWithValue("@MID", MagacinID);
				cmd.Parameters.AddWithValue("@KID", KomercijalnoUserID);
				cmd.Parameters.AddWithValue(
					"@TAG",
					Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(Tag))
				);
				cmd.Parameters.AddWithValue("@ID", ID);
				cmd.Parameters.AddWithValue("@OZN", OpomeniZaNeizvrseneZadatke);
				cmd.Parameters.AddWithValue("@BZC", BonusZakljucavanjaCount);
				cmd.Parameters.AddWithValue("@BZL", BonusZakljucavanjaLimit);

				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Vraca korisnika iz baze na osnovu ID-a
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		public static User Get(int userID)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return Get(con, userID);
			}
		}

		/// <summary>
		/// Vraca korisnika iz baze na osnovu ID-a
		/// </summary>
		/// <param name="userID"></param>
		/// <returns></returns>
		public static User Get(FbConnection con, int userID)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, USERNAME, PW, MAGACINID, KOMERCIJALNO_USER_ID, GRAD, TAG, OPOMENI_ZA_NEIZVRSENI_ZADATAK, BONUS_ZAKLJUCAVANJA_COUNT, BONUS_ZAKLJUCAVANJA_LIMIT FROM USERS WHERE ID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ID", userID);
				FbDataReader dr = cmd.ExecuteReader();

				if (dr.Read())
					return new User()
					{
						ID = Convert.ToInt32(dr["ID"]),
						Username = dr["USERNAME"].ToString(),
						Password = dr["PW"].ToString(),
						MagacinID = Convert.ToInt32(dr["MAGACINID"]),
						KomercijalnoUserID =
							dr["KOMERCIJALNO_USER_ID"] is DBNull
								? null
								: (int?)Convert.ToInt32(dr["KOMERCIJALNO_USER_ID"]),
						Grad = Convert.ToInt32(dr["GRAD"]),
						Tag =
							dr["TAG"] is DBNull
								? new Info()
								: JsonConvert.DeserializeObject<Info>(
									Encoding.UTF8.GetString((byte[])dr["TAG"])
								),
						OpomeniZaNeizvrseneZadatke =
							Convert.ToInt32(dr["OPOMENI_ZA_NEIZVRSENI_ZADATAK"]) == 1,
						BonusZakljucavanjaCount = Convert.ToInt32(dr["BONUS_ZAKLJUCAVANJA_COUNT"]),
						BonusZakljucavanjaLimit = Convert.ToDouble(dr["BONUS_ZAKLJUCAVANJA_LIMIT"])
					};
			}

			return null;
		}

		/// <summary>
		/// Dodaje novog korisnika u bazu
		/// </summary>
		/// <param name="username"></param>
		/// <param name="pw"></param>
		public static void Insert(string username, string pw, int grad)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				Insert(con, username, pw, grad);
			}
		}

		/// <summary>
		/// Dodaje novog korisnika u bazu
		/// </summary>
		/// <param name="username"></param>
		/// <param name="pw"></param>
		public static void Insert(FbConnection con, string username, string pw, int grad)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO USERS (ID, USERNAME, PW, MAGACINID, GRAD) VALUES
(((SELECT COALESCE(MAX(ID), 0) FROM USERS) + 1), @UN, @PW, 0, @GRAD)",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@UN", username);
				cmd.Parameters.AddWithValue("@PW", pw);
				cmd.Parameters.AddWithValue("@GRAD", grad);
				cmd.ExecuteNonQuery();
			}
		}

		/// <summary>
		/// Vraca listu svih korisnika iz baze
		/// </summary>
		/// <returns></returns>
		public static List<User> List(string whereQuery = null)
		{
			using (FbConnection con = new FbConnection(TDOffice.connectionString))
			{
				con.Open();
				return List(con, whereQuery);
			}
		}

		/// <summary>
		/// Vraca listu svih korisnika iz baze
		/// </summary>
		/// <returns></returns>
		public static List<User> List(FbConnection con, string whereQuery = null)
		{
			return Dict(con, whereQuery).Values.ToList();
		}

		/// <summary>
		/// Vraca dictionary sa svim korisnicima iz baze.
		/// Key = ID, Value = User object
		/// </summary>
		/// <param name="con"></param>
		/// <param name="whereQuery"></param>
		/// <returns></returns>
		public static Dictionary<int, User> Dict(FbConnection con, string whereQuery = null)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " AND " + whereQuery;

			Dictionary<int, User> dict = new Dictionary<int, User>();

			using (
				FbCommand cmd = new FbCommand(
					"SELECT ID, USERNAME, PW, MAGACINID, KOMERCIJALNO_USER_ID, GRAD, TAG, OPOMENI_ZA_NEIZVRSENI_ZADATAK, BONUS_ZAKLJUCAVANJA_COUNT, BONUS_ZAKLJUCAVANJA_LIMIT FROM USERS WHERE 1 = 1 "
						+ whereQuery,
					con
				)
			)
			{
				FbDataReader dr = cmd.ExecuteReader();

				while (dr.Read())
				{
					User u = new User();
					u.ID = Convert.ToInt32(dr["ID"]);
					u.Username = dr["USERNAME"].ToString();
					u.Password = dr["PW"].ToString();
					u.Grad = Convert.ToInt32(dr["GRAD"]);
					u.MagacinID = Convert.ToInt32(dr["MAGACINID"]);
					u.KomercijalnoUserID =
						dr["KOMERCIJALNO_USER_ID"] is DBNull
							? null
							: (int?)Convert.ToInt32(dr["KOMERCIJALNO_USER_ID"]);
					u.BonusZakljucavanjaCount = Convert.ToInt32(dr["BONUS_ZAKLJUCAVANJA_COUNT"]);
					u.BonusZakljucavanjaLimit = Convert.ToDouble(dr["BONUS_ZAKLJUCAVANJA_LIMIT"]);
					u.OpomeniZaNeizvrseneZadatke =
						Convert.ToInt32(dr["OPOMENI_ZA_NEIZVRSENI_ZADATAK"]) == 1;
					try
					{
						u.Tag =
							dr["TAG"] is DBNull
								? new Info()
								: JsonConvert.DeserializeObject<Info>(
									Encoding.UTF8.GetString((byte[])dr["TAG"])
								);
					}
					catch
					{
						u.Tag = new Info();
					}
					dict.Add(u.ID, u);
				}
			}

			return dict;
		}

		public static Task<List<User>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}
	}
}
