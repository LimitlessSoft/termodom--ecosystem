using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using TDOffice_v2.Komercijalno;
using Termodom.Data.Dtos;

namespace TDOffice_v2.Tasks
{
	public static class IspravnostIzvodStavkeTask
	{
		public static async Task Run()
		{
			try
			{
				var config = TDOffice.Config<IspravnostIzvodStavkeTaskConfigDto>.Get(
					TDOffice.ConfigParameter.IspravnostIzvodStavkeTaskConfig
				);
				if (config.Tag == null)
				{
					TDOffice.Poruka.Insert(
						new TDOffice.Poruka()
						{
							Datum = DateTime.Now,
							Naslov = "[Task] Izvestaj ispravnosti izvoda uplata pazara",
							Posiljalac = Program.TrenutniKorisnik.ID,
							Primalac = Program.TrenutniKorisnik.ID,
							Status = TDOffice.PorukaTip.Standard,
							Tag = new TDOffice.PorukaAdditionalInfo(),
							Tekst =
								"Doslo je do greske prilikom generisanja izvestaja ispravnosti stavki izvoda uplata pazara iz razloga"
								+ "sto config ("
								+ TDOffice.ConfigParameter.IspravnostIzvodStavkeTaskConfig.ToString()
								+ ") nije podesen."
						}
					);
					return;
				}
				var izvodi = await IzvodManager.DictionaryAsync(150);

				var configItems = new Dictionary<int, IspravnostIzvodStavkeTaskConfigItemDto>();
				foreach (var item in config.Tag.Items)
					configItems.Add(item.PPID, item);

				var dt = new DataTable();
				dt.Columns.Add("BrDok", typeof(int));
				dt.Columns.Add("PPID", typeof(int));
				dt.Columns.Add("Opis", typeof(string));

				foreach (var izvod in izvodi.Values)
				{
					if (!configItems.ContainsKey(izvod.PPID))
						continue;

					if (izvod.SifPlac != "165")
						continue;

					var configItem = configItems[izvod.PPID];

					if (string.IsNullOrWhiteSpace(izvod.PozNaBroj))
					{
						dt.GenerateRow(izvod.BrDok, izvod.PPID, "Prazan poziv na broj");
						continue;
					}

					if (izvod.PozNaBroj.Length != 7)
					{
						dt.GenerateRow(
							izvod.BrDok,
							izvod.PPID,
							$"Neispravan poziv na broj [{izvod.PozNaBroj}]"
						);
						continue;
					}

					if (
						!int.TryParse(
							izvod.PozNaBroj.Substring(izvod.PozNaBroj.Length - 4, 3),
							out _
						)
					)
					{
						dt.GenerateRow(
							izvod.BrDok,
							izvod.PPID,
							$"Poslednja 3 karaktera diktira na broj [{izvod.PozNaBroj}] moraju biti cifre"
						);
						continue;
					}

					if (
						izvod.PozNaBroj.Substring(izvod.PozNaBroj.Length - 3, 3)
						!= configItem.PozivNaBrojPoslednjeTriCifre.ToString()
					)
					{
						dt.GenerateRow(
							izvod.BrDok,
							izvod.PPID,
							$"Podesavanje partnera diktira da poslednja 3 karaktera poziva na broj moraju biti [{configItem.PozivNaBrojPoslednjeTriCifre}]. Trenutno [{izvod.PozNaBroj.Substring(izvod.PozNaBroj.Length - 4, 3)}]"
						);
						continue;
					}

					// ====

					if (string.IsNullOrWhiteSpace(izvod.Konto))
					{
						dt.GenerateRow(izvod.BrDok, izvod.PPID, "Prazan konto");
						continue;
					}

					if (izvod.Konto.Length != 5)
					{
						dt.GenerateRow(
							izvod.BrDok,
							izvod.PPID,
							$"Konto [{izvod.Konto}] treba ima duzinu 5"
						);
						continue;
					}

					if (izvod.Konto.Substring(0, 3) != configItem.KontoPrveTriCifre.ToString())
					{
						dt.GenerateRow(
							izvod.BrDok,
							izvod.PPID,
							$"Podesavanje partnera diktira da prve tri cifre konta moraju biti [{configItem.KontoPrveTriCifre}]. Trenutno [{izvod.Konto.Substring(0, 3)}]"
						);
						continue;
					}
					if (izvod.Konto.Substring(3, 2) != configItem.KontoPoslednjeDveCifre.ToString())
					{
						dt.GenerateRow(
							izvod.BrDok,
							izvod.PPID,
							$"Podesavanje partnera diktira da poslednje dve cifre konta moraju biti [{configItem.KontoPoslednjeDveCifre}]. Trenutno [{izvod.Konto.Substring(3, 2)}]"
						);
						continue;
					}
				}

				if (dt.Rows.Count > 0)
				{
					TDOffice.Poruka.Insert(
						new TDOffice.Poruka()
						{
							Datum = DateTime.Now,
							Naslov = "[Task] Izvestaj ispravnosti izvoda uplata pazara",
							Posiljalac = Program.TrenutniKorisnik.ID,
							Primalac = Program.TrenutniKorisnik.ID,
							Status = TDOffice.PorukaTip.Standard,
							Tag = new TDOffice.PorukaAdditionalInfo()
							{
								Action = TDOffice.PorukaAction.DataTableAttachment,
								AdditionalInfo = dt
							},
							Tekst =
								"Postoje neispravno unete stavke izvoda koje su vezane za uplate pazara. Pogledajte prilog."
						}
					);
				}
				else
				{
					TDOffice.Poruka.Insert(
						new TDOffice.Poruka()
						{
							Datum = DateTime.Now,
							Naslov = "[Task] Izvestaj ispravnosti izvoda uplata pazara",
							Posiljalac = Program.TrenutniKorisnik.ID,
							Primalac = Program.TrenutniKorisnik.ID,
							Status = TDOffice.PorukaTip.Standard,
							Tag = new TDOffice.PorukaAdditionalInfo(),
							Tekst = "Sve stavke izvoda vezane za uplate pazara su ispravne!"
						}
					);
				}
			}
			catch (Exception ex)
			{
				TDOffice.Poruka.Insert(
					new TDOffice.Poruka()
					{
						Datum = DateTime.Now,
						Naslov = "[Task] Izvestaj ispravnosti izvoda uplata pazara",
						Posiljalac = Program.TrenutniKorisnik.ID,
						Primalac = Program.TrenutniKorisnik.ID,
						Status = TDOffice.PorukaTip.Standard,
						Tag = new TDOffice.PorukaAdditionalInfo(),
						Tekst = ex.ToString()
					}
				);
			}
		}

		private static void GenerateRow(this DataTable dataTable, int brDok, int ppid, string opis)
		{
			DataRow row = dataTable.NewRow();
			row["BrDok"] = brDok;
			row["PPID"] = ppid;
			row["Opis"] = opis;
			dataTable.Rows.Add(row);
		}
	}
}
