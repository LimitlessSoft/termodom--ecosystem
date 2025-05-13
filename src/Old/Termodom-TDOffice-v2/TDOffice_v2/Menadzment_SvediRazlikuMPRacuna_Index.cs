using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using Newtonsoft.Json;

namespace TDOffice_v2
{
	public partial class Menadzment_SvediRazlikuMPRacuna_Index : Form
	{
		private List<Komercijalno.Tarife> _tarife = Komercijalno.Tarife.List();

		public Menadzment_SvediRazlikuMPRacuna_Index()
		{
			InitializeComponent();
		}

		private void Log(string message)
		{
			this.Invoke(
				(MethodInvoker)
					delegate
					{
						richTextBox1.AppendText(message + Environment.NewLine);
					}
			);
		}

		private void button1_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				using (
					FbConnection con = new FbConnection(
						Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
					)
				)
				{
					con.Open();

					Log("Selektujem dokumente...");
					List<Komercijalno.Dokument> dokumenti = Komercijalno.Dokument.List(
						con,
						"VRDOK = 15 AND MAGACINID = 12 AND PPID IS NULL AND POTRAZUJE > 10000"
					);
					Log("Selektujem robu...");
					List<Komercijalno.Roba> roba = Komercijalno.Roba.List(con);

					Log("Zapocinjem loop kroz dokumente...");

					int uspesnih = 0;
					int neuspesnih = 0;
					double totalTime = 0;
					Stopwatch sw = Stopwatch.StartNew();
					foreach (int brDok in dokumenti.Select(x => x.BrDok))
					{
						sw.Restart();
						try
						{
							//MessageBox.Show($"Pritiskom na OK zapocinjem rad na dokumentu {brDok}.");
							Log($"Zapocinjem rad na dokumentu {brDok}");
							Log("Selektujem robu u magacinu...");
							List<Komercijalno.RobaUMagacinu> robaUMagacinu =
								Komercijalno.RobaUMagacinu.List(con);
							robaUMagacinu.RemoveAll(x =>
								x.MagacinID != 12 || x.Stanje <= 0 || x.ProdajnaCena >= 1000
							);
							Log($"Presabiram dokument {brDok}...");
							Komercijalno.Procedure.PresaberiDokument(15, brDok);
							Log($"Ucitavam podatke dokumenta {brDok}...");
							Komercijalno.Dokument dokument = Komercijalno.Dokument.Get(
								con,
								15,
								brDok
							);
							Log($"Ucitavam stavke dokumenta {brDok}...");
							List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.List(
								con,
								"VRDOK = 15 AND BRDOK = " + brDok
							);

							double pocetnoPotrazuje = dokument.Potrazuje;
							Log($"Pocetno potrazuje: {pocetnoPotrazuje.ToString("#,##0.00")}");

							Log(
								$"Zapocinjem loop kroz stavke kako bih dao maksimalni moguci rabat do marze od 5%..."
							);
							foreach (Komercijalno.Stavka s in stavke)
							{
								double nabavnaCena = Math.Round(s.NabavnaCena, 4);
								double prodajnaCena = Math.Round(
									s.ProdCenaBP + (s.Korekcija == null ? 0 : (double)s.Korekcija),
									2
								);
								if (prodajnaCena == 0 || nabavnaCena == 0)
								{
									continue;
								}
								double ciljanaProdajnaCena = Math.Round(nabavnaCena * 1.05, 2);
								double noviRabat = (
									((ciljanaProdajnaCena / prodajnaCena) - 1) * -100
								);

								Log(
									$@"Stavka ID: {s.StavkaID}\n
                                    Nabavna Cena: {nabavnaCena.ToString("#,##0.00")}\n
                                    Prodajna Cena: {prodajnaCena.ToString("#,##0.00")}\n
                                    Ciljana Prodajna Cena: {ciljanaProdajnaCena.ToString("#,##0.00")}\n
                                    Novi Rabat: {noviRabat.ToString("#,##0.00")}"
								);

								s.Rabat = Math.Round(noviRabat, 2);
								Log("Azuriram rabat stavke u bazi...");
								s.Update(con);
							}
							Log($"Zavrsen loop kroz stavke.");

							Log($"Presabiram dokument {brDok}...");
							Komercijalno.Procedure.PresaberiDokument(15, brDok);
							Log($"Osvezavam podatke dokumenta u memoriji {brDok}...");
							dokument = Komercijalno.Dokument.Get(con, 15, brDok);

							Log($"Unosim dodatne stavke u dokument {brDok}...");
							int iSafeSwitch = 0;
							while (Math.Round(dokument.Potrazuje - pocetnoPotrazuje, 2) < -5)
							{
								iSafeSwitch++;
								if (iSafeSwitch > 1000)
								{
									iSafeSwitch = 0;
									throw new Exception("Safe Switch Kill");
								}
								Log($"Osvezavam stavke dokumenta {brDok}...");
								stavke = Komercijalno.Stavka.List(
									con,
									"VRDOK = 15 AND BRDOK = " + brDok
								);
								Komercijalno.RobaUMagacinu nasumicnaRoba = null;

								Log($"Trazim nasumicnu robu za ubacivanje u dokument {brDok}...");
								while (
									nasumicnaRoba == null
									|| stavke.Any(x => x.RobaID == nasumicnaRoba.RobaID)
								)
									nasumicnaRoba = robaUMagacinu[Random.Next(robaUMagacinu.Count)];

								Log($"Ucitavam informacije nasumicno izabrane robe...");
								Komercijalno.Roba r = roba.FirstOrDefault(x =>
									x.ID == nasumicnaRoba.RobaID
								);
								Log($"Ucitavam tarifu nasumicno izabrane robe...");
								Komercijalno.Tarife tarifa = _tarife.FirstOrDefault(x =>
									x.TarifaID == r.TarifaID
								);

								Log("Ucitavam prodajnu cenu na dan nasumicno izabrane robe...");
								double prodajnaCenaNaDan = Math.Round(
									Komercijalno.Procedure.ProdajnaCenaNaDan(
										dokument.MagacinID,
										nasumicnaRoba.RobaID,
										dokument.Datum
									) * (1 - ((double)tarifa.Stopa / (100 + (double)tarifa.Stopa))),
									2
								);
								Log("Ucitavam nabavnu cenu na dan nasumicno izabrane robe...");
								double nabavnaCenaNaDan = Komercijalno.Procedure.NabavnaCenaNaDan(
									dokument.MagacinID,
									nasumicnaRoba.RobaID,
									dokument.Datum
								);
								Log(
									$"Izabrana nasumicna roba [{r.ID}]{r.Naziv}"
										+ $"Nabavna Cena Na Dan: {nabavnaCenaNaDan.ToString("#,##0.00")}"
										+ $"Prodajna Cena Na Dan: {prodajnaCenaNaDan.ToString("#,##0.00")}"
								);

								Log(
									$"Proveravam da li je nabavna cena nasumicno izabrane robe validna..."
								);
								if (nabavnaCenaNaDan == 0)
								{
									Log(
										$"Nabavna cena nije validna. Uklanjam robu iz liste i trazim novu nasumicnu robu."
									);
									robaUMagacinu.RemoveAll(x => x.RobaID == nasumicnaRoba.RobaID);
									continue;
								}

								Log($"Nabavna cena je validna. Nasavljam dalje...");
								Log(
									$"Proracunavam zeljenu cenu i potreban rabat da dodjem do nje..."
								);
								double zeljenaCena = Math.Round(nabavnaCenaNaDan * 1.05, 2);
								double potrebanRabat = Math.Round(
									((zeljenaCena / prodajnaCenaNaDan) - 1) * -100,
									2
								);
								Log(
									$"Zeljena Cena: {zeljenaCena.ToString("#,##0.00")}\nPotreban Rabat: {potrebanRabat.ToString("#,##0.00")}"
								);

								Log(
									$"Proveravam da li mogu da unesem celokupno stanje ili ce to biti previse pa moram da racunam koliko mi treba..."
								);
								if (
									(
										nasumicnaRoba.Stanje
										* zeljenaCena
										* ((100 + tarifa.Stopa) / 100)
									) <= (pocetnoPotrazuje - dokument.Potrazuje)
								)
								{
									Log(
										$"Mogu uneti celokupno stanje... Unosim stavku u dokument..."
									);
									Komercijalno.Stavka.Insert(
										con,
										dokument,
										r,
										nasumicnaRoba,
										nasumicnaRoba.Stanje,
										potrebanRabat
									);
								}
								else
								{
									//if ((pocetnoPotrazuje - dokument.Potrazuje) < 100)
									//    zeljenaCena = nabavnaCenaNaDan;

									Log(
										$"Unosenjem celokupnog stanja cu prevazici vrednost. Racunam koliko mi treba..."
									);
									double transportnoPakovanje =
										r.TrKol == null || r.TrKol == 0 ? 1 : (double)r.TrKol;
									double raz = Math.Round(
										(pocetnoPotrazuje - dokument.Potrazuje)
											/ ((100 + (double)tarifa.Stopa) / 100),
										2
									);
									double kolicinaZaUneti =
										((int)(raz / zeljenaCena / transportnoPakovanje))
										* transportnoPakovanje;
									Log(
										$"Transportno pakovanje: {transportnoPakovanje.ToString("#,##0.00")}"
											+ $"Razlika dokumenta: {raz.ToString("#,##0.00")}"
											+ $"Kolicina za uneti: {kolicinaZaUneti.ToString("#,##0.00")}"
									);

									Log(
										$"Proveravam kolicinu za uneti i korigujem je ako je potrebno..."
									);
									if (
										kolicinaZaUneti == 0
										&& (pocetnoPotrazuje - dokument.Potrazuje)
											> nabavnaCenaNaDan
									)
										kolicinaZaUneti = 1;

									if (kolicinaZaUneti == 0)
										continue;
									Log(
										$"Final kolicina za uneti: {kolicinaZaUneti.ToString("#,##0.00")}"
									);

									//if ((pocetnoPotrazuje - dokument.Potrazuje) < 100 && (prodajnaCenaNaDan * kolicinaZaUneti) >= raz)
									//{
									//    double vrednostBezPopusta = prodajnaCenaNaDan * kolicinaZaUneti;
									//    double potrebanPopust = vrednostBezPopusta - raz;
									//    double K = potrebanPopust / kolicinaZaUneti;
									//    potrebanRabat = (K / prodajnaCenaNaDan) * 100;
									//}

									Log($"Unosim stavku u dokument...");
									Komercijalno.Stavka.Insert(
										con,
										dokument,
										r,
										nasumicnaRoba,
										kolicinaZaUneti,
										potrebanRabat
									);
								}
								Log($"Presabiram dokument {brDok}...");
								Komercijalno.Procedure.PresaberiDokument(15, brDok);
								Log($"Osvezavam podatke dokumenta u memoriji {brDok}...");
								dokument = Komercijalno.Dokument.Get(con, 15, brDok);
							}

							Log($"Presabiram dokument {brDok}...");
							Komercijalno.Procedure.PresaberiDokument(15, brDok);
							Log($"Osvezavam podatke dokumenta u memoriji {brDok}...");
							dokument = Komercijalno.Dokument.Get(con, 15, brDok);
							Log($"Osvezavam stavke dokumenta {brDok}...");
							stavke = Komercijalno.Stavka.List(
								con,
								"VRDOK = 15 AND BRDOK = " + brDok
							);

							Log($"Prolazim kroz stavke dokumenta za finalni obracun...");
							int nTries = 3;
							int iTries = 0;
							while (dokument.Potrazuje != pocetnoPotrazuje && iTries < nTries)
							{
								iSafeSwitch++;
								if (iSafeSwitch > 1000)
								{
									iSafeSwitch = 0;
									throw new Exception("Safe Switch Kill");
								}
								Log($"Pokusavam da sravnim dokument {iTries} put...");
								foreach (
									Komercijalno.Stavka s in stavke
										.OrderByDescending(x => x.ProdCenaBP)
										.Take(10)
										.OrderBy(x => Random.Next())
								)
								{
									double pocetniRabat = s.Rabat;
									Log(
										$"Spustam rabat stavke {s.StavkaID} sa {s.Rabat.ToString("#,##0.00")} na 0 i azuriram u bazi..."
									);
									s.Rabat = 0;
									s.Update(con);
									Log($"Presabiram dokument {brDok}...");
									Komercijalno.Procedure.PresaberiDokument(15, brDok);
									Log($"Osvezavam podatke dokumenta u memoriji {brDok}...");
									dokument = Komercijalno.Dokument.Get(con, 15, brDok);
									double razlika = Math.Round(
										dokument.Potrazuje - pocetnoPotrazuje,
										2
									);
									Log(
										$"Razlika u odnosu na pocetak: {razlika.ToString("#,##0.00")}"
									);
									Log($"Ucitavam robu {s.RobaID}...");
									Komercijalno.Roba r = roba.FirstOrDefault(x =>
										x.ID == s.RobaID
									);
									Log($"Ucitavam tarifu robeid: {s.RobaID}...");
									Komercijalno.Tarife tarifa = _tarife.FirstOrDefault(x =>
										x.TarifaID == r.TarifaID
									);

									Log($"Ucitavam i preracunavam cene...");
									double prodajnaCenaNaDan = Math.Round(
										Komercijalno.Procedure.ProdajnaCenaNaDan(
											dokument.MagacinID,
											s.RobaID,
											dokument.Datum
										)
											* (
												1
												- (
													(double)tarifa.Stopa
													/ (100 + (double)tarifa.Stopa)
												)
											),
										2
									);
									double zeljenaPC = Math.Round(
										prodajnaCenaNaDan
											- (
												(razlika / s.Kolicina)
												* (
													1
													- (
														(double)tarifa.Stopa
														/ (100 + (double)tarifa.Stopa)
													)
												)
											),
										2
									);
									double nabavnaCenaNaDan =
										Komercijalno.Procedure.NabavnaCenaNaDan(
											dokument.MagacinID,
											s.RobaID,
											dokument.Datum
										);
									Log(
										$"Nabavna Cena Na Dan: {nabavnaCenaNaDan.ToString("#,##0.00")}\n"
											+ $"Prodajna Cena Na Dan: {prodajnaCenaNaDan.ToString("#,##0.00")}\n"
											+ $"Potrebna Prodajna Cena: {zeljenaPC.ToString("#,##0.00")}"
									);

									Log(
										$"Proveravam da nabavna cena nije veca od zeljene cene. Ukoliko jeste, zaobilazim ovu stavku i nastavljam dalje."
									);
									if (nabavnaCenaNaDan > zeljenaPC)
									{
										Log(
											$"Nabavna cena je nelogicna, vracam rabat na pocetnu vrednost i prelazim na sledecu stavku..."
										);
										s.Rabat = pocetniRabat;
										s.Update(con);
										continue;
									}

									Log($"Nabavna cena OK. Presabiram rabat...");
									double rabat =
										Math.Ceiling(
											Math.Round(
												(((zeljenaPC / prodajnaCenaNaDan) - 1) * -100),
												3
											) * 100
										) / 100;
									Log($"Novi Rabat: {rabat.ToString("#,##0.00")}");
									Log($"Azuriram rabat u bazi...");
									s.Rabat = rabat;
									s.Update(con);

									Log($"Presabiram dokument {brDok}...");
									Komercijalno.Procedure.PresaberiDokument(15, brDok);
									Log($"Osvezavam podatke dokumenta u memoriji {brDok}...");
									dokument = Komercijalno.Dokument.Get(con, 15, brDok);

									Log(
										$"Proveravam da li je dokument sravnjen u decimalu..."
											+ $"Pocetno potrazuje: {pocetnoPotrazuje.ToString("#,##0.00")}"
											+ $"Trenutno potrazuje: {dokument.Potrazuje.ToString("#,##0.00")}"
									);

									if (dokument.Potrazuje == pocetnoPotrazuje)
									{
										break;
									}
								}
								iTries++;
							}

							Log(
								$"Sada proveravam da li je dokument sravnjen. Ukoliko nije onda smanjujem rabat, i ubacujem robu koja ima nisku cenu, te je rabatom dovodim i pogadjam decimale..."
							);
							if (dokument.Potrazuje != pocetnoPotrazuje)
							{
								Log($"Dokument nije jos uvek sravnjen... Idemo na sitno...");
								Log($"Presabiram dokument {brDok}...");
								Komercijalno.Procedure.PresaberiDokument(15, brDok);
								Log($"Osvezavam podatke dokumenta u memoriji {brDok}...");
								dokument = Komercijalno.Dokument.Get(con, 15, brDok);
								Log($"Osvezavam stavke dokumenta {brDok}...");
								stavke = Komercijalno.Stavka.List(
									con,
									"VRDOK = 15 AND BRDOK = " + brDok
								);

								Log(
									$"Povecavam rabat na stavkama kako bi doveo potrazuje dokumenta na X ispod pocetnog potrazuje..."
								);
								while (dokument.Potrazuje > (pocetnoPotrazuje - 10)) // Ovih 10 je da bih ostavio luft da lakse unesem robu i dam rabat da sravnim
								{
									iSafeSwitch++;
									if (iSafeSwitch > 1000)
									{
										iSafeSwitch = 0;
										throw new Exception("Safe Switch Kill");
									}
									Log($"Nalazim stavku...");
									Komercijalno.Stavka st = stavke[Random.Next(stavke.Count)];
									Log($"Ucitavam robu {st.RobaID}...");
									Komercijalno.Roba r = roba.FirstOrDefault(x =>
										x.ID == st.RobaID
									);
									Log($"Ucitavam tarifu robeid: {st.RobaID}...");
									Komercijalno.Tarife tarifa = _tarife.FirstOrDefault(x =>
										x.TarifaID == r.TarifaID
									);
									Log("Ucitavam prodajnu cenu na dan nasumicno izabrane robe...");
									double prodajnaCenaNaDan = Math.Round(
										Komercijalno.Procedure.ProdajnaCenaNaDan(
											dokument.MagacinID,
											st.RobaID,
											dokument.Datum
										)
											* (
												1
												- (
													(double)tarifa.Stopa
													/ (100 + (double)tarifa.Stopa)
												)
											),
										2
									);
									Log("Ucitavam nabavnu cenu na dan nasumicno izabrane robe...");
									double nabavnaCenaNaDan =
										Komercijalno.Procedure.NabavnaCenaNaDan(
											dokument.MagacinID,
											st.RobaID,
											dokument.Datum
										);
									Log("Proracunavam max rabat...");
									double maxRabat =
										(1 - (nabavnaCenaNaDan / prodajnaCenaNaDan)) * 100;
									Log($"Povecavam i azuriram rabat stavke...");
									st.Rabat =
										st.Rabat + 1 > maxRabat - 0.05
											? maxRabat - 0.05
											: st.Rabat + 1;
									st.Update(con);
									Log($"Presabiram dokument {brDok}...");
									Komercijalno.Procedure.PresaberiDokument(15, brDok);
									Log($"Osvezavam podatke dokumenta u memoriji {brDok}...");
									dokument = Komercijalno.Dokument.Get(con, 15, brDok);
								}

								Log(
									$"Sada kada mi je potrazuje ispod zeljenog, hvatam razliku i ubacujem robu za tu istu razliku..."
								);
								Log($"Presabiram dokument {brDok}...");
								Komercijalno.Procedure.PresaberiDokument(15, brDok);
								Log($"Osvezavam podatke dokumenta u memoriji {brDok}...");
								dokument = Komercijalno.Dokument.Get(con, 15, brDok);
								double razlika = pocetnoPotrazuje - dokument.Potrazuje;
								Log($"Razlika: {razlika}");
								Log(
									$"Sada prolazim kroz robu u magacinu po ceni od najnize ka najvise, proveravam da li ce se uklopiti "
										+ $"prodajna cena da bude deljiva sa celokupnom razlikom i da ne moram rabat da dajem tako da se "
										+ $"uklopi u decimalu potrazuje..."
								);
								foreach (
									Komercijalno.RobaUMagacinu rum in robaUMagacinu.OrderBy(x =>
										x.ProdajnaCena
									)
								)
								{
									Log($"Ucitavam robu {rum.RobaID}...");
									Komercijalno.Roba r = roba.FirstOrDefault(x =>
										x.ID == rum.RobaID
									);
									Log($"Ucitavam tarifu robeid: {rum.RobaID}...");
									Komercijalno.Tarife tarifa = _tarife.FirstOrDefault(x =>
										x.TarifaID == r.TarifaID
									);
									Log($"Uzimam prodajnu cenu na dan...");
									double prodajnaCenaNaDan = Math.Round(
										Komercijalno.Procedure.ProdajnaCenaNaDan(
											dokument.MagacinID,
											rum.RobaID,
											dokument.Datum
										),
										2
									);
									Log($"Preracunavam potrebnu kolicinu...");
									double potrebnaKolicina = razlika / prodajnaCenaNaDan;
									Log($"Insertujem stavku...");
									Komercijalno.Stavka.Insert(
										con,
										dokument,
										r,
										rum,
										potrebnaKolicina,
										0
									);
									Log($"Presabiram dokument {brDok}...");
									Komercijalno.Procedure.PresaberiDokument(15, brDok);
									Log($"Osvezavam podatke dokumenta u memoriji {brDok}...");
									dokument = Komercijalno.Dokument.Get(con, 15, brDok);
									Log($"Proveravam je li sada okej...");
									if (dokument.Potrazuje < pocetnoPotrazuje)
									{
										Log(
											$"Nije okej.. Idalje je manje... Idem na sledeci RUM..."
										);
									}
									else if (dokument.Potrazuje > pocetnoPotrazuje)
									{
										Log($"NIje okej. Sada je vece... Prekidam...");
										break;
									}
									else
									{
										Log($"Okej je sada!");
										break;
									}
								}
							}
							else
							{
								Log($"Dokument je sravnjen!");
							}

							if (dokument.Potrazuje != pocetnoPotrazuje)
							{
								Log(
									$"Neuspesno sravnjen dokument {brDok}!"
										+ $"Dokument Potrazuje: {dokument.Potrazuje.ToString("#,##0.00")}"
										+ $"Pocetno Potrazuje: {pocetnoPotrazuje.ToString("#,##0.00")}"
								);
								neuspesnih++;
								this.Invoke(
									(MethodInvoker)
										delegate
										{
											label2.Text = "Neuspesnih: " + neuspesnih;
										}
								);
							}
							else
							{
								Log($"Dokument {brDok} je sredjen. Prelazim na sledeci!\n\n\n");
								uspesnih++;
								this.Invoke(
									(MethodInvoker)
										delegate
										{
											label1.Text = "Uspesnih: " + uspesnih;
										}
								);
							}
						}
						catch (Exception ex)
						{
							Task.Run(() =>
							{
								MessageBox.Show(ex.ToString());
							});
						}
						double doneIn = sw.ElapsedMilliseconds;
						Log($"Gotov dokument u: {doneIn.ToString("#,##0")} ms");
						totalTime += doneIn;
						this.Invoke(
							(MethodInvoker)
								delegate
								{
									label3.Text =
										$"Prosek ms: {totalTime / (uspesnih + neuspesnih)}";
								}
						);
					}
					MessageBox.Show("Gotovo!");
				}
			});
		}

		private void button2_Click(object sender, EventArgs e)
		{
			string text = richTextBox1.Text;
			System.IO.File.WriteAllText("D:\\AleksaR\\exLog.txt", text);
			MessageBox.Show("Exported");
		}
	}
}
