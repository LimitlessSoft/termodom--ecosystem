using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.Komercijalno
{
	public class IstUpl
	{
		public int IstUplID { get; set; }
		public int VrDok { get; set; }
		public int BrDok { get; set; }
		public DateTime Datum { get; set; }
		public double Iznos { get; set; }
		public int PPID { get; set; }
		public int IO { get; set; }
		public int NUID { get; set; }
		public int? PromenaID { get; set; }

		public void Update(int godina)
		{
			using (FbConnection con = new FbConnection(Komercijalno.CONNECTION_STRING[godina]))
			{
				con.Open();
				Update(con);
			}
		}

		public void Update(FbConnection con)
		{
			using (
				FbCommand cmd = new FbCommand(
					"UPDATE ISTUPL SET NUID = @NUID WHERE ISTUPLID = @ID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@NUID", NUID);
				cmd.Parameters.AddWithValue("@ID", IstUplID);

				cmd.ExecuteNonQuery();
			}
		}

		public static void Insert(
			int vrDok,
			int brDok,
			double iznos,
			string opis,
			int ppid,
			int zapid,
			int io,
			int nuid,
			int a,
			int kasaid,
			string mtid,
			string valuta,
			double kurs,
			int promenaID,
			int pkID,
			int? linkedID,
			int? placanjeID,
			int? godina
		)
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				Insert(
					vrDok,
					brDok,
					iznos,
					opis,
					ppid,
					zapid,
					io,
					nuid,
					a,
					kasaid,
					mtid,
					valuta,
					kurs,
					promenaID,
					pkID,
					linkedID,
					placanjeID,
					godina
				);
			}
		}

		public static void Insert(
			FbConnection con,
			int vrDok,
			int brDok,
			double iznos,
			string opis,
			int ppid,
			int zapid,
			int io,
			int nuid,
			int a,
			int kasaid,
			string mtid,
			string valuta,
			double kurs,
			int promenaID,
			int pkID,
			int? linkedID,
			int? placanjeID,
			int? godina
		)
		{
			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO ISTUPL
                    (ISTUPLID, VRDOK, BRDOK, DATUM, IZNOS, OPIS, PPID, ZAPID, IO, NUID, A, KASAID, MTID, VALUTA, KURS, PROMENAID, PKID, LINKEDID, PLACANJEID, GODINA)
                    VALUES
                    (
                        ((SELECT COALESCE(0, MAX(ISTUPLID)) FROM ISTUPL) + 1),
                        @VRDOK,
                        @BRDOK,
                        @DATUM,
                        @IZNOS,
                        @OPIS,
                        @PPID,
                        @ZAPID,
                        @IO,
                        @NUID,
                        @A,
                        @KASAID,
                        @MTID,
                        @VALUTA,
                        @KURS,
                        @PROMENAID,
                        @PKID,
                        @LINKEDID,
                        @PLACANJEID,
                        @GODINA
                    )",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@VRDOK", vrDok);
				cmd.Parameters.AddWithValue("@BRDOK", brDok);
				cmd.Parameters.AddWithValue("@DATUM", DateTime.Now);
				cmd.Parameters.AddWithValue("@IZNOS", iznos);
				cmd.Parameters.AddWithValue("@OPIS", opis);
				cmd.Parameters.AddWithValue("@PPID", ppid);
				cmd.Parameters.AddWithValue("@ZAPID", zapid);
				cmd.Parameters.AddWithValue("@IO", io);
				cmd.Parameters.AddWithValue("@NUID", nuid);
				cmd.Parameters.AddWithValue("@A", a);
				cmd.Parameters.AddWithValue("@KASAID", kasaid);
				cmd.Parameters.AddWithValue("@MTID", mtid);
				cmd.Parameters.AddWithValue("@VALUTA", valuta);
				cmd.Parameters.AddWithValue("@KURS", kurs);
				cmd.Parameters.AddWithValue("@PROMENAID", promenaID);
				cmd.Parameters.AddWithValue("@PKID", pkID);
				cmd.Parameters.AddWithValue("@LINKEDID", linkedID);
				cmd.Parameters.AddWithValue("@PLACANJEID", placanjeID);
				cmd.Parameters.AddWithValue("@GODINA", godina);

				cmd.ExecuteNonQuery();
			}
		}

		public static List<IstUpl> List(string whereQuery = null)
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

		public static List<IstUpl> List(FbConnection con, string whereQuery = null)
		{
			if (!string.IsNullOrWhiteSpace(whereQuery))
				whereQuery = " WHERE " + whereQuery;

			List<IstUpl> list = new List<IstUpl>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT ISTUPLID, VRDOK, BRDOK, DATUM, IZNOS, PPID, IO, NUID, PROMENAID FROM ISTUPL"
						+ whereQuery,
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
				{
					while (dr.Read())
						list.Add(
							new IstUpl()
							{
								IstUplID = Convert.ToInt32(dr["ISTUPLID"]),
								VrDok = Convert.ToInt32(dr["VRDOK"]),
								BrDok = Convert.ToInt32(dr["BRDOK"]),
								Datum = Convert.ToDateTime(dr["DATUM"]),
								Iznos = Convert.ToDouble(dr["IZNOS"]),
								PPID = Convert.ToInt32(dr["PPID"]),
								IO = Convert.ToInt32(dr["IO"]),
								NUID = Convert.ToInt32(dr["NUID"]),
								PromenaID =
									dr["PROMENAID"] is DBNull
										? null
										: (int?)Convert.ToInt32(dr["PROMENAID"])
							}
						);
				}
			}
			return list;
		}

		public static Task<List<IstUpl>> ListAsync(string whereQuery = null)
		{
			return Task.Run(() =>
			{
				return List(whereQuery);
			});
		}
	}
}
