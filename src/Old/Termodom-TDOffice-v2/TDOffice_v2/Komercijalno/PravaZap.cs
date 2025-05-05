using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.Komercijalno
{
	class PravaZap
	{
		public Int16 ModulID { get; set; }
		public int ObjekatID { get; set; }
		public Int16 ZapID { get; set; }
		public Int16? V { get; set; } = null;
		public Int16? I { get; set; } = null;
		public Int16? M { get; set; } = null;
		public Int16? D { get; set; } = null;
		public Int16? P { get; set; } = null;
		public Int16? EkspID { get; set; } = null;

		public PravaZap() { }

		public void Update()
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
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
					@"UPDATE PRAVAZAP SET MODULID = @MID, V = @V,
                 I = @I, M = @M, D = @D, P = @P, EKSPID = @EKSPID
                 WHERE ZAPID = @ZID AND OBJEKATID = @OID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@MID", ModulID);
				cmd.Parameters.AddWithValue("@V", V);
				cmd.Parameters.AddWithValue("@I", I);
				cmd.Parameters.AddWithValue("@M", M);
				cmd.Parameters.AddWithValue("@D", D);
				cmd.Parameters.AddWithValue("@P", P);
				cmd.Parameters.AddWithValue("@EKSPID", EkspID);
				cmd.Parameters.AddWithValue("@OID", ObjekatID);
				cmd.Parameters.AddWithValue("@ZID", ZapID);

				cmd.ExecuteNonQuery();
			}
		}

		public static PravaZap Get(int zapID, int modulID, int objekatID)
		{
			using (
				FbConnection con = new FbConnection(
					Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				return Get(con, zapID, modulID, objekatID);
			}
		}

		public static PravaZap Get(FbConnection con, int zapID, int modulID, int objekatID)
		{
			using (
				FbCommand cmd = new FbCommand(
					"SELECT MODULID, OBJEKATID, ZAPID, V, I, M, D, P, EKSPID FROM PRAVAZAP  WHERE ZAPID = @ZID AND MODULID = @MID AND OBJEKATID = @OID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@ZID", zapID);
				cmd.Parameters.AddWithValue("@MID", modulID);
				cmd.Parameters.AddWithValue("@OID", objekatID);

				using (FbDataReader dr = cmd.ExecuteReader())
					if (dr.Read())
						return new PravaZap()
						{
							ModulID = Convert.ToInt16(dr["MODULID"]),
							ZapID = Convert.ToInt16(dr["ZAPID"]),
							ObjekatID = Convert.ToInt16(dr["OBJEKATID"]),
							EkspID =
								dr["EKSPID"] is DBNull
									? (Int16?)null
									: Convert.ToInt16(dr["EKSPID"]),
							V = dr["V"] is DBNull ? (Int16?)null : Convert.ToInt16(dr["V"]),
							I = dr["I"] is DBNull ? (Int16?)null : Convert.ToInt16(dr["I"]),
							M = dr["M"] is DBNull ? (Int16?)null : Convert.ToInt16(dr["M"]),
							D = dr["D"] is DBNull ? (Int16?)null : Convert.ToInt16(dr["D"]),
							P = dr["P"] is DBNull ? (Int16?)null : Convert.ToInt16(dr["P"])
						};
			}

			return null;
		}

		public static Task<PravaZap> GetAsync(int zapID, int modulID, int objekatID)
		{
			return Task.Run(() =>
			{
				return Get(zapID, modulID, objekatID);
			});
		}

		public static List<PravaZap> List()
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

		public static List<PravaZap> List(FbConnection con)
		{
			List<PravaZap> list = new List<PravaZap>();
			using (
				FbCommand cmd = new FbCommand(
					"SELECT MODULID, OBJEKATID, ZAPID, V, I, M, D, P, EKSPID FROM PRAVAZAP ",
					con
				)
			)
			{
				using (FbDataReader dr = cmd.ExecuteReader())
					while (dr.Read())
						list.Add(
							new PravaZap()
							{
								ModulID = Convert.ToInt16(dr["MODULID"]),
								ZapID = Convert.ToInt16(dr["ZAPID"]),
								ObjekatID = Convert.ToInt16(dr["OBJEKATID"]),
								EkspID =
									dr["EKSPID"] is DBNull
										? (Int16?)null
										: Convert.ToInt16(dr["EKSPID"]),
								V = dr["V"] is DBNull ? (Int16?)null : Convert.ToInt16(dr["V"]),
								I = dr["I"] is DBNull ? (Int16?)null : Convert.ToInt16(dr["I"]),
								M = dr["M"] is DBNull ? (Int16?)null : Convert.ToInt16(dr["M"]),
								D = dr["D"] is DBNull ? (Int16?)null : Convert.ToInt16(dr["D"]),
								P = dr["P"] is DBNull ? (Int16?)null : Convert.ToInt16(dr["P"])
							}
						);
			}
			return list;
		}

		public static Task<List<PravaZap>> ListAsync()
		{
			return Task.Run(() =>
			{
				return List();
			});
		}
	}
}
