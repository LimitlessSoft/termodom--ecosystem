using FirebirdSql.Data.FirebirdClient;
using TDBrain_v3.DB.Komercijalno;

namespace TDBrain_v3.Managers.Komercijalno
{
	public static class StavkaManager
	{
		public static int Insert(
			FbConnection con,
			int vrDok,
			int brDok,
			int robaId,
			double kolicina,
			double rabat,
			double? prodajnaCenaBezPDV = null
		)
		{
			var dokument = DokumentManager.Get(con, vrDok, brDok);

			if (dokument == null)
				throw new Exception("Dokument nije pronadjen!");

			var roba = RobaManager.Get(con, robaId);

			if (roba == null)
				throw new Exception("Roba nije pronadjena!");

			var robaUMagacinu = RobaUMagacinuManager.Get(con, dokument.MagacinID, robaId);

			if (robaUMagacinu == null)
				throw new Exception("Roba u magacinu nije pronadjena!");

			var tarife = TarifeManager.Dictionary(con);
			var magacini = MagacinManager.Dictionary(con);

			using (
				FbCommand cmd = new FbCommand(
					@"INSERT INTO STAVKA
                (VRDOK, BRDOK, MAGACINID, ROBAID, VRSTA, NAZIV, NABCENSAPOR, FAKTURNACENA, NABCENABT,
                TROSKOVI, NABAVNACENA, PRODCENABP, KOREKCIJA, PRODAJNACENA, DEVIZNACENA, DEVPRODCENA, KOLICINA,
                NIVKOL, TARIFAID, IMAPOREZ, POREZ, RABAT, MARZA, TAKSA, AKCIZA, PROSNAB, PRECENA, PRENAB, PROSPROD,
                MTID, PT, TREN_STANJE, POREZ_ULAZ, DEVNABCENA, POREZ_IZ)
                VALUES (@VRDOK, @BRDOK, @MAGACINID, @ROBAID, 1, @NAZIV, 0, 0, 0, 
                0, @NABAVNACENA, @CENA_BEZ_PDV, 0, @CENA_SA_PDV, 0, 0, @KOL,
                0, @TARIFAID, 0, @POREZ, @RABAT, 0, 0, 0, 0, 0, 0, 0, 
                @MTID, 'P', 0, 0, 0, @POREZ) RETURNING STAVKAID",
					con
				)
			)
			{
				cmd.Parameters.AddWithValue("@VRDOK", dokument.VrDok);
				cmd.Parameters.AddWithValue("@BRDOK", dokument.BrDok);
				cmd.Parameters.AddWithValue("@MAGACINID", dokument.MagacinID);
				cmd.Parameters.AddWithValue("@ROBAID", roba.ID);
				cmd.Parameters.AddWithValue("@NAZIV", roba.Naziv);
				cmd.Parameters.AddWithValue("@NABAVNACENA", robaUMagacinu.NabavnaCena);
				cmd.Parameters.AddWithValue(
					"@CENA_SA_PDV",
					prodajnaCenaBezPDV == null
						? ProceduraManager.ProdajnaCenaNaDan(
							con,
							dokument.MagacinID,
							roba.ID,
							dokument.Datum
						)
						: prodajnaCenaBezPDV * (1 + (tarife[roba.TarifaID].Stopa / 100))
				);
				cmd.Parameters.AddWithValue(
					"@CENA_BEZ_PDV",
					prodajnaCenaBezPDV == null
						? (double)cmd.Parameters["@CENA_SA_PDV"].Value
							* (
								1
								- (
									tarife[roba.TarifaID].Stopa
									/ (100 + tarife[roba.TarifaID].Stopa)
								)
							)
						: ProceduraManager.ProdajnaCenaNaDan(
							con,
							dokument.MagacinID,
							roba.ID,
							dokument.Datum
						)
				);
				cmd.Parameters.AddWithValue("@KOL", kolicina);
				cmd.Parameters.AddWithValue("@TARIFAID", roba.TarifaID);
				cmd.Parameters.AddWithValue("@POREZ", tarife[roba.TarifaID].Stopa);
				cmd.Parameters.AddWithValue("@RABAT", rabat);
				cmd.Parameters.AddWithValue("@MTID", magacini[dokument.MagacinID].MTID);

				cmd.Parameters.Add(
					new FbParameter
					{
						ParameterName = "STAVKAID",
						FbDbType = FbDbType.Integer,
						Direction = System.Data.ParameterDirection.Output
					}
				);

				cmd.ExecuteNonQuery();
				return Convert.ToInt32(cmd.Parameters["STAVKAID"].Value);
			}
		}
	}
}
