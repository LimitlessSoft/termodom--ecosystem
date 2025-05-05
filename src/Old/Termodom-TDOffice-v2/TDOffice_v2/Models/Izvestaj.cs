using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2.Models
{
	public static class Izvestaj
	{
		/// <summary>
		/// Vraca podatke o prodaji robe sa realnim cenama iz dokumenata
		/// u tom trenutku.
		/// </summary>
		/// <returns></returns>
		public static DataTable ProdajaRobePoRealnimCenama(
			Models.Periodi periodi,
			List<int> magacini,
			List<int> listaRobe
		)
		{
			// Primer
			// MagacinID    Naziv   RobaID      kGOD2021    kGOD2022    JM      vGOD2021     vGOD2022
			// 12           GIPS    39          100         200         m2      10000        20000
			// 12           LEPAK   58          450                     kg      2450
			// 13           GIPS    39          150                     m2      15000
			// 13           LEPAK   58          550                     kg      3450

			if (periodi == null)
				throw new NullReferenceException("Varijabla periodi ne moze biti null!");

			if (periodi.Count == 0)
				throw new Exception("Ne postoji ni jedan period u varijabli periodi!");

			if (magacini == null)
				throw new NullReferenceException("Varijabla magacini ne moze biti null!");

			if (magacini.Count == 0)
				throw new Exception("Lista magacina ne moze biti prazna!");

			if (listaRobe != null && listaRobe.Count == 0)
				throw new Exception(
					"Lista robe nije null ali je prazna. Morate je popuniti ili deklarisati kao null za svu robu!"
				);

			DataTable dt = new DataTable();
			foreach (int godina in periodi.Godine.OrderByDescending(x => x))
			{
				string realnaVrednostCommand =
					@"
                    SELECT s.MAGACINID, r.NAZIV, s.ROBAID, COALESCE(SUM(s.KOLICINA), 0) as KGOD"
					+ godina
					+ @", r.JM,
                    COALESCE(SUM((s.KOLICINA * s.PRODCENABP * (1 + (t.STOPA / 100)))), 0) AS VGOD"
					+ godina
					+ @"
                    FROM STAVKA s
                    LEFT OUTER JOIN DOKUMENT d ON d.VRDOK = s.VRDOK AND d.BRDOK = s.BRDOK
                    LEFT OUTER JOIN ROBA r on s.ROBAID = r.ROBAID
                    LEFT OUTER JOIN TARIFE t on t.TARIFAID = r.TARIFAID
                    WHERE d.MAGACINID IN ("
					+ string.Join(", ", magacini)
					+ @") AND
                        (d.VRDOK = 13 OR d.VRDOK = 15) AND
                        DATUM >= @ODDATUMA AND
                        DATUM <= @DODATUMA
                    GROUP BY s.MAGACINID, r.NAZIV, s.ROBAID, r.JM
                    ORDER BY s.ROBAID";
				using (
					FbConnection con = new FbConnection(
						Komercijalno.Komercijalno.CONNECTION_STRING[godina]
					)
				)
				{
					con.Open();
					using (FbCommand cmd = new FbCommand(realnaVrednostCommand, con))
					{
						cmd.Parameters.AddWithValue("@ODDATUMA", periodi[godina].OdDatuma);
						cmd.Parameters.AddWithValue("@DODATUMA", periodi[godina].DoDatuma);

						using (FbDataAdapter da = new FbDataAdapter(cmd))
						{
							DataTable tempDT = new DataTable();
							if (dt.Rows.Count == 0)
							{
								da.Fill(dt);
								dt.PrimaryKey = new DataColumn[]
								{
									dt.Columns["ROBAID"],
									dt.Columns["MAGACINID"]
								};
								continue;
							}

							da.Fill(tempDT);
							tempDT.PrimaryKey = new DataColumn[]
							{
								tempDT.Columns["ROBAID"],
								tempDT.Columns["MAGACINID"]
							};

							dt.Merge(tempDT, false, MissingSchemaAction.Add);
						}
					}
				}
			}
			;

			if (listaRobe != null)
				for (int i = dt.Rows.Count - 1; i >= 0; i--)
					if (!listaRobe.Contains(Convert.ToInt32(dt.Rows[i]["ROBAID"])))
						dt.Rows.RemoveAt(i);

			return dt;
		}

		public static DataTable ProdajaRobePoRealnimCenama(
			Models.Periodi periodi,
			List<int> magacini,
			List<int> listaRobe,
			List<int> reperniMagacini
		)
		{
			DataTable baseDT = ProdajaRobePoRealnimCenama(periodi, magacini, listaRobe);
			DataTable reperniTemp = ProdajaRobePoRealnimCenama(periodi, reperniMagacini, listaRobe);
			DataTable reperniDT = reperniTemp
				.AsEnumerable()
				.GroupBy(x => x.Field<int>("ROBAID"))
				.Select(g =>
				{
					var row = reperniTemp.NewRow();

					row["MAGACINID"] = -8;
					row["NAZIV"] = g.First().Field<string>("NAZIV");
					row["ROBAID"] = g.Key;
					row["JM"] = g.First().Field<string>("JM");

					foreach (int godina in periodi.Godine)
					{
						row["KGOD" + godina] = g.Sum(r =>
							r.Field<decimal?>("KGOD" + godina) is null
								? 0
								: r.Field<decimal>("KGOD" + godina)
						);
						row["VGOD" + godina] = g.Sum(r =>
							r.Field<decimal?>("VGOD" + godina) is null
								? 0
								: r.Field<decimal>("VGOD" + godina)
						);
					}

					return row;
				})
				.CopyToDataTable();

			baseDT.Merge(reperniDT);
			return baseDT;
		}

		public static DataTable ProdajaRobePoPoslednjimCenama(
			Models.Periodi periodi,
			List<int> magacini,
			List<int> listaRobe
		)
		{
			// Primer
			// MagacinID    Naziv   RobaID      kGOD2021    kGOD2022    JM      vGOD2021     vGOD2022
			// 12           GIPS    39          100         200         m2      10000        20000
			// 12           LEPAK   58          450                     kg      2450
			// 13           GIPS    39          150                     m2      15000
			// 13           LEPAK   58          550                     kg      3450

			if (periodi == null)
				throw new NullReferenceException("Varijabla periodi ne moze biti null!");

			if (periodi.Count == 0)
				throw new Exception("Ne postoji ni jedan period u varijabli periodi!");

			if (magacini == null)
				throw new NullReferenceException("Varijabla magacini ne moze biti null!");

			if (magacini.Count == 0)
				throw new Exception("Lista magacina ne moze biti prazna!");

			if (listaRobe != null && listaRobe.Count == 0)
				throw new Exception(
					"Lista robe nije null ali je prazna. Morate je popuniti ili deklarisati kao null za svu robu!"
				);

			List<Komercijalno.RobaUMagacinu> _komercijalnoRobaUMagacinu50 =
				Komercijalno.RobaUMagacinu.ListByMagacinID(50);
			List<Komercijalno.Roba> _komercijalnoRoba = Komercijalno.Roba.List(DateTime.Now.Year);
			List<Komercijalno.Tarife> _tarife = Komercijalno.Tarife.List();

			DataTable dt = new DataTable();
			foreach (int godina in periodi.Godine.OrderByDescending(x => x))
			{
				string ponderVrednostCommand =
					@"
    SELECT s.MAGACINID, r.NAZIV, s.ROBAID, COALESCE(SUM(s.KOLICINA), 0) AS KGOD"
					+ godina
					+ @", r.JM
    FROM STAVKA s
    LEFT OUTER JOIN DOKUMENT d ON d.VRDOK = s.VRDOK AND d.BRDOK = s.BRDOK
    LEFT OUTER JOIN ROBA r on s.ROBAID = r.ROBAID
    WHERE d.MAGACINID IN ("
					+ string.Join(", ", magacini)
					+ @") AND (d.VRDOK = 13 OR d.VRDOK = 15) AND DATUM >= @ODDATUMA AND DATUM <= @DODATUMA
    GROUP BY s.MAGACINID, s.ROBAID, r.NAZIV, r.JM
    ORDER BY s.ROBAID";
				using (
					FbConnection con = new FbConnection(
						Komercijalno.Komercijalno.CONNECTION_STRING[godina]
					)
				)
				{
					con.Open();
					using (FbCommand cmd = new FbCommand(ponderVrednostCommand, con))
					{
						cmd.Parameters.AddWithValue("@ODDATUMA", periodi[godina].OdDatuma);
						cmd.Parameters.AddWithValue("@DODATUMA", periodi[godina].DoDatuma);

						using (FbDataAdapter da = new FbDataAdapter(cmd))
						{
							DataTable tempDT = new DataTable();
							if (dt.Rows.Count == 0)
							{
								da.Fill(dt);
								dt.PrimaryKey = new DataColumn[]
								{
									dt.Columns["ROBAID"],
									dt.Columns["MAGACINID"]
								};
								continue;
							}

							da.Fill(tempDT);
							tempDT.PrimaryKey = new DataColumn[]
							{
								tempDT.Columns["ROBAID"],
								tempDT.Columns["MAGACINID"]
							};

							dt.Merge(tempDT, false, MissingSchemaAction.Add);
						}
					}
				}
			}
			;

			foreach (int god in periodi.Godine)
				dt.Columns.Add("VGOD" + god, typeof(decimal));

			if (listaRobe != null)
				for (int i = dt.Rows.Count - 1; i >= 0; i--)
					if (!listaRobe.Contains(Convert.ToInt32(dt.Rows[i]["ROBAID"])))
						dt.Rows.RemoveAt(i);

			foreach (DataRow row in dt.Rows)
			{
				Komercijalno.RobaUMagacinu rum = _komercijalnoRobaUMagacinu50.FirstOrDefault(x =>
					x.RobaID == Convert.ToInt32(row["ROBAID"])
				);
				Komercijalno.Roba r = _komercijalnoRoba.FirstOrDefault(x =>
					x.ID == Convert.ToInt32(row["ROBAID"])
				);
				Komercijalno.Tarife tar = _tarife.FirstOrDefault(x => x.TarifaID == r.TarifaID);

				bool err = rum == null || r == null || tar == null;

				foreach (int god in periodi.Godine)
				{
					decimal kol = 0;
					try
					{
						kol = Convert.ToDecimal(row["KGOD" + god]);
					}
					catch { }
					row["VGOD" + god] = err
						? 0
						: Convert.ToDecimal(
							Convert.ToDecimal(kol)
								* Convert.ToDecimal((rum.ProdajnaCena * (1 + (tar.Stopa / 100))))
						);
				}
			}
			return dt;
		}

		public static DataTable ProdajaRobePoPoslednjimCenama(
			Models.Periodi periodi,
			List<int> magacini,
			List<int> listaRobe,
			List<int> reperniMagacini
		)
		{
			DataTable baseDT = ProdajaRobePoPoslednjimCenama(periodi, magacini, listaRobe);
			DataTable reperniTemp = ProdajaRobePoPoslednjimCenama(
				periodi,
				reperniMagacini,
				listaRobe
			);
			DataTable reperniDT = reperniTemp
				.AsEnumerable()
				.GroupBy(x => x.Field<int>("ROBAID"))
				.Select(g =>
				{
					var row = reperniTemp.NewRow();

					row["MAGACINID"] = -8;
					row["NAZIV"] = g.First().Field<string>("NAZIV");
					row["ROBAID"] = g.Key;
					row["JM"] = g.First().Field<string>("JM");

					foreach (int godina in periodi.Godine)
					{
						row["KGOD" + godina] = g.Sum(r =>
							r.Field<decimal?>("KGOD" + godina) is null
								? 0
								: r.Field<decimal>("KGOD" + godina)
						);
						row["VGOD" + godina] = g.Sum(r =>
							r.Field<decimal?>("VGOD" + godina) is null
								? 0
								: r.Field<decimal>("VGOD" + godina)
						);
					}

					return row;
				})
				.CopyToDataTable();

			baseDT.Merge(reperniDT);
			return baseDT;
		}

		public static DataTable OdobreniRabati(
			List<int> magacini,
			int godina,
			DateTime odDat,
			DateTime doDat
		)
		{
			DataTable dt = new DataTable();
			using (
				FbConnection con = new FbConnection(
					Komercijalno.Komercijalno.CONNECTION_STRING[godina]
				)
			)
			{
				con.Open();
				using (
					FbCommand cmd = new FbCommand(
						@"
    SELECT MAGACINID, SUM(DUGUJE) AS UKUPANPROMET, SUM(DUGUJE - POTRAZUJE) AS UKUPANRABAT, (SUM(DUGUJE - POTRAZUJE) / SUM(DUGUJE)) * 100 as PROCENAT
    FROM DOKUMENT
    WHERE MAGACINID IN ("
							+ string.Join(", ", magacini)
							+ @") AND FLAG = 1 AND (VRDOK = 13 OR VRDOK = 15) AND DATUM >= @ODDATUMA AND DATUM <= @DODATUMA
    GROUP BY MAGACINID",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@ODDATUMA", odDat);
					cmd.Parameters.AddWithValue("@DODATUMA", doDat);
					using (FbDataAdapter da = new FbDataAdapter(cmd))
					{
						da.Fill(dt);
					}
				}
			}

			return dt;
		}

		/// <summary>
		/// Vraca podatke o prosecnoj prodaji robe prethodna tri meseca
		/// po magacinima u vidu tabele.
		/// Minimalne zalihe se obracunavaju tako sto se uzme prosecna prodaja robe za protekla 3 meseca
		/// i ukoliko [taj prosek * 0.9 (koeficijent) ] je manji od njegove trenutne zalihe to znaci da je to minimalna zaliha.
		/// </summary>
		/// <returns></returns>
		//public static DataTable ProsecnaProdajaPrethodnaTriMeseca(List<int> magacini, double koeficijent)
		//{
		//    throw new NotImplementedException();

		//    /// ==============================
		//    /// ==============================
		//    /// ==============================
		//    /// Ovo ne funkcionise kako treba.
		//    /// ==============================
		//    /// ==============================
		//    /// ==============================


		//    DataTable dt = new DataTable();
		//    //DateTime odDat = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day - 90);
		//    DateTime odDat = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddDays(-90);
		//    DateTime doDat = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
		//    int godina = DateTime.Now.Year;
		//    string prosecnaProdajaCommand = @"
		//            SELECT s.MAGACINID, r.NAZIV, s.ROBAID, r.JM, left(r.KATBR,8) as KATBR, SUM(s.KOLICINA)/3*" + koeficijent + @" as PP, rum.STANJE
		//            FROM STAVKA s
		//            LEFT OUTER JOIN DOKUMENT d ON d.VRDOK = s.VRDOK AND d.BRDOK = s.BRDOK
		//            LEFT OUTER JOIN ROBA r ON s.ROBAID = r.ROBAID
		//            LEFT OUTER JOIN ROBAUMAGACINU rum ON rum.ROBAID = s.ROBAID AND rum.MAGACINID = s.MAGACINID
		//            WHERE d.MAGACINID IN (" + string.Join(", ", magacini) + @") AND
		//                (d.VRDOK = 13 OR d.VRDOK = 15) AND
		//                DATUM >= @ODDATUMA AND DATUM <= @DODATUMA
		//            GROUP BY s.MAGACINID, s.ROBAID, r.NAZIV,  r.JM, r.KATBR, rum.STANJE
		//            HAVING SUM(s.KOLICINA)/3*" + koeficijent + @" < rum.STANJE
		//            ORDER BY s.MAGACINID";
		//    using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[godina]))
		//    {
		//        con.Open();
		//        using (FbCommand cmd = new FbCommand(prosecnaProdajaCommand, con))
		//        {
		//            cmd.Parameters.AddWithValue("@ODDATUMA", odDat);
		//            cmd.Parameters.AddWithValue("@DODATUMA", doDat);
		//            using (FbDataAdapter da = new FbDataAdapter(cmd))
		//            {
		//                da.Fill(dt);
		//            }
		//        }
		//    }
		//    return dt;
		//}
		/// <summary>
		/// Vraca i kolicinski i vrednosno (izmeniti summary) (kao ProdajaRobeZaDatiPeriod_Kolicinski)
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static DataTable ProdajaRobeZaDatiPeriod()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Vraca i vrednosno (izmeniti summary) (kao ProdajaRobeZaDatiPeriod_Kolicinski)
		/// </summary>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		public static DataTable ProdajaRobeZaDatiPeriod_Vrednosno()
		{
			throw new NotImplementedException();
		}

		public static DataTable ProdajaRobeZaDatiPeriod_Kolicinski(
			int magacinID,
			DateTime odDatuma,
			DateTime doDatuma
		)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("MagacinID", typeof(int));
			dt.Columns.Add("RobaID", typeof(int));
			dt.Columns.Add("Kolicina", typeof(double));

			List<int> godine = new List<int>();
			for (int i = odDatuma.Year; i <= doDatuma.Year; i++)
				godine.Add(i);

			foreach (int god in godine)
			{
				using (
					FbConnection con = new FbConnection(
						Komercijalno.Komercijalno.CONNECTION_STRING[god]
					)
				)
				{
					con.Open();

					using (
						FbCommand cmd = new FbCommand(
							@"SELECT s.ROBAID, COALESCE(SUM(s.KOLICINA), 0) AS KOLICINA
FROM STAVKA s
LEFT OUTER JOIN DOKUMENT d ON d.VRDOK = s.VRDOK AND d.BRDOK = s.BRDOK
WHERE d.VRDOK IN (15, 13) AND d.MAGACINID = @MAGACINID AND d.DATUM >= @ODDATUMA AND d.DATUM <= @DODATUMA
GROUP BY s.ROBAID",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@MAGACINID", magacinID);
						cmd.Parameters.AddWithValue("@ODDATUMA", odDatuma);
						cmd.Parameters.AddWithValue("@DODATUMA", doDatuma);

						using (FbDataReader dr = cmd.ExecuteReader())
						{
							while (dr.Read())
							{
								DataRow row = dt.NewRow();
								row["MagacinID"] = magacinID;
								row["RobaID"] = Convert.ToInt32(dr["ROBAID"]);
								row["Kolicina"] = Convert.ToDouble(dr["KOLICINA"]);
								dt.Rows.Add(row);
							}
						}
					}
				}
			}

			return dt;
		}

		/// <summary>
		/// Vraca DataTable sa podacima prodaje robe razvrstane po magacinima, zbirno po robi.
		/// Kolone: MagacinID, RobaID, Kolicina
		/// </summary>
		/// <param name="magacini"></param>
		/// <param name="odDatuma"></param>
		/// <param name="doDatuma"></param>
		/// <returns></returns>
		public static DataTable ProdajaRobeZaDatiPeriod_Kolicinski(
			List<int> magacini,
			DateTime odDatuma,
			DateTime doDatuma
		)
		{
			List<int> godine = new List<int>();
			for (int i = odDatuma.Year; i <= doDatuma.Year; i++)
				godine.Add(i);

			Dictionary<int, Dictionary<int, double>> dict =
				new Dictionary<int, Dictionary<int, double>>();

			foreach (int god in godine)
			{
				using (
					FbConnection con = new FbConnection(
						Komercijalno.Komercijalno.CONNECTION_STRING[god]
					)
				)
				{
					con.Open();

					using (
						FbCommand cmd = new FbCommand(
							@"SELECT s.MAGACINID, s.ROBAID, COALESCE(SUM(s.KOLICINA), 0) AS KOLICINA
FROM STAVKA s
LEFT OUTER JOIN DOKUMENT d ON d.VRDOK = s.VRDOK AND d.BRDOK = s.BRDOK
WHERE d.VRDOK IN (15, 13) AND d.MAGACINID IN ("
								+ string.Join(", ", magacini)
								+ @") AND d.DATUM >= @ODDATUMA AND d.DATUM <= @DODATUMA
GROUP BY s.MAGACINID, s.ROBAID",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@ODDATUMA", odDatuma);
						cmd.Parameters.AddWithValue("@DODATUMA", doDatuma);

						using (FbDataReader dr = cmd.ExecuteReader())
						{
							while (dr.Read())
							{
								int magacinID = Convert.ToInt32(dr["MAGACINID"]);
								int robaID = Convert.ToInt32(dr["ROBAID"]);
								int kolicina = Convert.ToInt32(dr["KOLICINA"]);

								if (!dict.ContainsKey(magacinID))
									dict[magacinID] = new Dictionary<int, double>();

								if (!dict[magacinID].ContainsKey(robaID))
									dict[magacinID][robaID] = 0;

								dict[magacinID][robaID] += kolicina;
							}
						}
					}
				}
			}

			DataTable dt = new DataTable();
			dt.Columns.Add("MagacinID", typeof(int));
			dt.Columns.Add("RobaID", typeof(int));
			dt.Columns.Add("Kolicina", typeof(double));

			foreach (int magacin in dict.Keys)
			{
				foreach (int robaID in dict[magacin].Keys)
				{
					DataRow row = dt.NewRow();
					row["MagacinID"] = magacin;
					row["RobaID"] = robaID;
					row["Kolicina"] = dict[magacin][robaID];
					dt.Rows.Add(row);
				}
			}

			return dt;
		}
	}
}
