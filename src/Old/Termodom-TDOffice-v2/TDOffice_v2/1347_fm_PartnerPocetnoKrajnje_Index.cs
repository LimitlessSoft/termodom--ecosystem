using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2
{
	public partial class _1347_fm_PartnerPocetnoKrajnje_Index : Form
	{
		private bool _working = false;
		private Task<fm_Help> _helpFrom { get; set; }

		private static int[] vrDokDokumenti = new int[] { -61, -59, 0, 10, 13, 14, 15, 22, 39, 40 };

		// Kreiram dictionary sa dokumentima. Key predstavlja godinu
		Dictionary<string, List<Dokument>> dokumenti { get; set; } =
			new Dictionary<string, List<Dokument>>();
		Dictionary<string, Termodom.Data.Entities.Komercijalno.IzvodDictionary> stavkeIzvoda =
			new Dictionary<string, Termodom.Data.Entities.Komercijalno.IzvodDictionary>();
		Dictionary<string, List<IstUpl>> istorijeUplata = new Dictionary<string, List<IstUpl>>();
		Dictionary<string, List<Promena>> promene = new Dictionary<string, List<Promena>>();
		Task<List<Partner>> sviPartneri = Partner.ListAsync(DateTime.Now.Year);

		private DataTable finalData = null;

		public _1347_fm_PartnerPocetnoKrajnje_Index()
		{
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.PartnerPocetnoKrajnje_Index);
		}

		private void _1347_fm_PartnerPocetnoKrajnje_Index_Load(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				sviPartneri.Wait();
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							doPPID_nud.Value = sviPartneri.Result.Count();
						}
				);
			});
			comboBox1.SelectedIndex = 0;
			OnemoguciFiltere();
		}

		public Task UcitajPodatkeAsync()
		{
			return Task.Run(() =>
			{
				dokumenti = new Dictionary<string, List<Dokument>>();
				stavkeIzvoda =
					new Dictionary<string, Termodom.Data.Entities.Komercijalno.IzvodDictionary>();
				istorijeUplata = new Dictionary<string, List<IstUpl>>();
				promene = new Dictionary<string, List<Promena>>();

				int i = 0;
				Task[] tsks = new Task[Komercijalno.Komercijalno.CONNECTION_STRING.Keys.Count];

				// Ucitavam sve ostale godine za koje mogu pronaci connection string
				foreach (int key in Komercijalno.Komercijalno.CONNECTION_STRING.Keys)
				{
					tsks[i] = Task.Run(() =>
					{
						using (
							FbConnection con = new FbConnection(
								Komercijalno.Komercijalno.CONNECTION_STRING[key]
							)
						)
						{
							con.Open();
							dokumenti.Add(
								key.ToString(),
								Dokument.List(
									con,
									"VRDOK IN ("
										+ string.Join(", ", vrDokDokumenti)
										+ ") AND PPID IS NOT NULL"
								)
							);
							stavkeIzvoda.Add(
								key.ToString(),
								IzvodManager.DictionaryAsync(150, key).GetAwaiter().GetResult()
							);
							istorijeUplata.Add(
								key.ToString(),
								IstUpl.List(con, "PPID IS NOT NULL AND PPID > 0")
							);
							promene.Add(
								key.ToString(),
								Promena.List(con, "KONTO LIKE '204%' OR KONTO LIKE '43%'")
							);
						}
					});
					i++;
				}

				Task.WaitAll(tsks);
			});
		}

		private void OnemoguciFiltere()
		{
			groupBox2.Visible = false;
			button1.Enabled = false;
			odPPID_nud.Enabled = false;
			doPPID_nud.Enabled = false;
		}

		private void OmoguciFiltere()
		{
			groupBox2.Visible = true;
			button1.Enabled = true;
			odPPID_nud.Enabled = true;
			doPPID_nud.Enabled = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if ((int)odPPID_nud.Value > (int)doPPID_nud.Value)
			{
				MessageBox.Show("Neispravan opseg PPID-a");
				return;
			}

			status_lbl.Text = "Ucitavam...";

			OnemoguciFiltere();

			// Skladistim ovu formu da mogu da invokujem metode nad njom
			Form mf = this;

			Thread mt = new Thread(() =>
			{
				_working = true;

				// Hvatam listu svih poslovnih partnera
				List<Partner> partneri = sviPartneri
					.Result.Where(x => x.PPID >= odPPID_nud.Value && x.PPID <= doPPID_nud.Value)
					.ToList();

				// Setupujem tabelu
				DataTable dt = new DataTable();
				dt.Columns.Add("PPID", typeof(int));
				dt.Columns.Add("Partner", typeof(string));
				dt.Columns.Add("PocetnoStanje2017", typeof(double));
				dt.Columns.Add("KrajnjeStanje2017", typeof(double));
				dt.Columns.Add("PocetnoStanje2018", typeof(double));
				dt.Columns.Add("KrajnjeStanje2018", typeof(double));
				dt.Columns.Add("PocetnoStanje2019", typeof(double));
				dt.Columns.Add("KrajnjeStanje2019", typeof(double));
				dt.Columns.Add("PocetnoStanje2020", typeof(double));
				dt.Columns.Add("KrajnjeStanje2020", typeof(double));
				dt.Columns.Add("PocetnoStanje2021", typeof(double));
				dt.Columns.Add("KrajnjeStanje2021", typeof(double));
				dt.Columns.Add("PocetnoStanje2022", typeof(double));
				dt.Columns.Add("KrajnjeStanje2022", typeof(double));
				dt.Columns.Add("PocetnoStanje2023", typeof(double));
				dt.Columns.Add("KrajnjeStanje2023", typeof(double));
				dt.Columns.Add("Izvor", typeof(string));

				// Lista svih godina
				List<string> godine = new List<string>()
				{
					"2017",
					"2018",
					"2019",
					"2020",
					"2021",
					"2022",
					"2023"
				};

				// Broj threadova koje cu startovati
				int partneraZaObradu = Math.Abs((int)odPPID_nud.Value - (int)doPPID_nud.Value) + 1;
				int workers = partneraZaObradu > 50 ? Math.Min(partneraZaObradu, 20) : 1;

				// Broj koliko threadova je zavrilo svoj posao
				int nFinished = 0;
				ManualResetEventSlim dataLoadMRE = new ManualResetEventSlim(false);
				dataLoadMRE.Reset();

				// Broj koliko je partnera zavrseno
				int nPartner = 0;

				// Koliko partnera uziam svaki worker
				int toTake = partneri.Count() / workers;

				List<DataTable> workersDTs = new List<DataTable>();

				// Koristim ovaj bool da ne dozvolim for loopu da nastavi pre nego sto thread uzme njegovo i
				// Desava se da for nastavi pre nego sto thread startuje i onda se desi da pogresno i zavrsi unutar thread-a
				ManualResetEventSlim workerStartedMRE = new ManualResetEventSlim(false);
				// Za svakog workera startujem thread
				for (int i = 0; i < workers; i++)
				{
					workerStartedMRE.Reset();
					Thread t1 = new Thread(() =>
					{
						// S obzirom da thread radi samo na odredjenom delu partnera, ja uzimam samo stvari vezane za te partnere
						// da ne bih svakim korakom prolazio kroz ceo loop
						Dictionary<string, List<Dokument>> Wdokumenti =
							new Dictionary<string, List<Dokument>>();

						// Kreiram datatable sa istom strukturom kao i glavna
						DataTable workerTable = dt.Clone();

						// Pocetni index parnera za ovog workera
						int toSkip = i * toTake;

						// Oglasavam da ovaj worker pocinje sa radom
						// Pustam sledeci for loop
						workerStartedMRE.Set();

						List<Partner> Wpartneri =
							i + 1 < workers
								? partneri.Skip(toSkip).Take(toTake).ToList()
								: partneri.Skip(toSkip).ToList();
						List<int> wPPID = Wpartneri.Select(x => x.PPID).ToList();

						int dokumentiLoaded = 0;

						ManualResetEventSlim dwMRE = new ManualResetEventSlim(false);
						ManualResetEventSlim dlMRE = new ManualResetEventSlim(false);
						foreach (string key in dokumenti.Keys)
						{
							dwMRE.Reset();
							Thread tl = new Thread(() =>
							{
								string dk = key;
								dwMRE.Set();
								Wdokumenti.Add(
									dk,
									dokumenti[dk]
										.Join(wPPID, d => d.PPID, p => p, (d, p) => d)
										.ToList()
								);
								dokumentiLoaded++;
								if (dokumentiLoaded == dokumenti.Keys.Count)
									dlMRE.Set();
							});
							tl.IsBackground = true;
							tl.Start();
							dwMRE.Wait();
						}

						dlMRE.Wait();

						// Ulazim u for loop partnera za ovog workera
						foreach (Partner p in Wpartneri)
						{
							for (int izv = 0; izv < 3; izv++)
							{
								DataRow dr = workerTable.NewRow();
								dr["PPID"] = p.PPID;
								dr["Partner"] = p.Naziv;
								dr["Izvor"] =
									izv == 0
										? "Finansijsko Kupac"
										: izv == 1
											? "Finansijsko Dobavljac"
											: "Komercijalno";

								foreach (string g in godine)
								{
									List<Dokument> dokumentiPartnera = Wdokumenti[g]
										.Where(x => x.PPID == p.PPID)
										.ToList();
									List<IstUpl> istorijeUplataPartnera = istorijeUplata[g]
										.Where(x => x.PPID == p.PPID)
										.ToList();

									double ps =
										izv == 0
											? GetPocetnoStanjeFinansijskoKupac(promene[g], p.PPID)
											: izv == 1
												? GetPocetnoStanjeFinansijskoDobavljac(
													promene[g],
													p.PPID
												)
												: GetRealnoPocetnoStanjePartnera(g, p.PPID);

									dr["PocetnoStanje" + g] = ps;
									dr["KrajnjeStanje" + g] =
										izv == 0
											? GetKrajnjeStanjeFinansijskoKupac(promene[g], p.PPID)
											: izv == 1
												? GetKrajnjeStanjeFinansijskoDobavljac(
													promene[g],
													p.PPID
												)
												: Math.Round(
													GetRealnoKrajnjeStanjePartnera(
														g,
														p.PPID,
														dokumentiPartnera,
														stavkeIzvoda[g]
															.Values.Where(x => x.PPID == p.PPID)
															.ToList()
													),
													2
												);
								}
								workerTable.Rows.Add(dr);
							}

							nPartner++;

							mf.Invoke(
								(MethodInvoker)
									delegate
									{
										status_lbl.Text = nPartner.ToString();
									}
							);
						}

						workersDTs.Add(workerTable);
						nFinished++;
						if (nFinished == workers)
							dataLoadMRE.Set();
					});

					t1.IsBackground = true;
					t1.Start();

					workerStartedMRE.Wait();
				}

				Thread ft = new Thread(() =>
				{
					dataLoadMRE.Wait();

					foreach (DataTable t in workersDTs)
						dt.Merge(t);

					mf.Invoke(
						(MethodInvoker)
							delegate
							{
								finalData = dt;
								dataGridView1.DataSource = finalData;

								if (dataGridView1.Rows.Count > 0)
								{
									dataGridView1.Columns["Partner"].Width = 300;
									dataGridView1.Columns["Izvor"].Width = 150;

									dataGridView1
										.Columns["PocetnoStanje2017"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["PocetnoStanje2017"].Width = 100;
									dataGridView1.Columns["PocetnoStanje2017"].HeaderText =
										"Pocetno 2017";
									dataGridView1
										.Columns["PocetnoStanje2017"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;
									dataGridView1
										.Columns["KrajnjeStanje2017"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["KrajnjeStanje2017"].Width = 100;
									dataGridView1.Columns["KrajnjeStanje2017"].HeaderText =
										"Krajnje 2017";
									dataGridView1
										.Columns["KrajnjeStanje2017"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;

									dataGridView1
										.Columns["PocetnoStanje2018"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["PocetnoStanje2018"].Width = 100;
									dataGridView1.Columns["PocetnoStanje2018"].HeaderText =
										"Pocetno 2018";
									dataGridView1
										.Columns["PocetnoStanje2018"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;
									dataGridView1
										.Columns["KrajnjeStanje2018"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["KrajnjeStanje2018"].Width = 100;
									dataGridView1.Columns["KrajnjeStanje2018"].HeaderText =
										"Krajnje 2018";
									dataGridView1
										.Columns["KrajnjeStanje2018"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;

									dataGridView1
										.Columns["PocetnoStanje2019"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["PocetnoStanje2019"].Width = 100;
									dataGridView1.Columns["PocetnoStanje2019"].HeaderText =
										"Pocetno 2019";
									dataGridView1
										.Columns["PocetnoStanje2019"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;
									dataGridView1
										.Columns["KrajnjeStanje2019"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["KrajnjeStanje2019"].Width = 100;
									dataGridView1.Columns["KrajnjeStanje2019"].HeaderText =
										"Krajnje 2019";
									dataGridView1
										.Columns["KrajnjeStanje2019"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;

									dataGridView1
										.Columns["PocetnoStanje2020"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["PocetnoStanje2020"].Width = 100;
									dataGridView1.Columns["PocetnoStanje2020"].HeaderText =
										"Pocetno 2020";
									dataGridView1
										.Columns["PocetnoStanje2020"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;
									dataGridView1
										.Columns["KrajnjeStanje2020"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["KrajnjeStanje2020"].Width = 100;
									dataGridView1.Columns["KrajnjeStanje2020"].HeaderText =
										"Krajnje 2020";
									dataGridView1
										.Columns["KrajnjeStanje2020"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;

									dataGridView1
										.Columns["PocetnoStanje2021"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["PocetnoStanje2021"].Width = 100;
									dataGridView1.Columns["PocetnoStanje2021"].HeaderText =
										"Pocetno 2021";
									dataGridView1
										.Columns["PocetnoStanje2021"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;
									dataGridView1
										.Columns["KrajnjeStanje2021"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["KrajnjeStanje2021"].Width = 100;
									dataGridView1.Columns["KrajnjeStanje2021"].HeaderText =
										"Krajnje 2021";
									dataGridView1
										.Columns["KrajnjeStanje2021"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;

									dataGridView1
										.Columns["PocetnoStanje2022"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["PocetnoStanje2022"].Width = 100;
									dataGridView1.Columns["PocetnoStanje2022"].HeaderText =
										"Pocetno 2022";
									dataGridView1
										.Columns["PocetnoStanje2022"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;
									dataGridView1
										.Columns["KrajnjeStanje2022"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["KrajnjeStanje2022"].Width = 100;
									dataGridView1.Columns["KrajnjeStanje2022"].HeaderText =
										"Krajnje 2022";

									dataGridView1
										.Columns["PocetnoStanje2023"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["PocetnoStanje2023"].Width = 100;
									dataGridView1.Columns["PocetnoStanje2023"].HeaderText =
										"Pocetno 2023";
									dataGridView1
										.Columns["PocetnoStanje2023"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;
									dataGridView1
										.Columns["KrajnjeStanje2023"]
										.DefaultCellStyle
										.Format = "#,##0.00";
									dataGridView1.Columns["KrajnjeStanje2023"].Width = 100;
									dataGridView1.Columns["KrajnjeStanje2023"].HeaderText =
										"Krajnje 2023";
									dataGridView1
										.Columns["KrajnjeStanje2023"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;
									dataGridView1
										.Columns["KrajnjeStanje2023"]
										.DefaultCellStyle
										.Alignment = DataGridViewContentAlignment.MiddleRight;

									ColorDGV();
								}
								OmoguciFiltere();
							}
					);
					_working = false;
				});
				ft.IsBackground = true;
				ft.Start();
			});
			mt.IsBackground = true;
			mt.Start();
		}

		private double GetPocetnoStanjeFinansijskoKupac(List<Promena> promene, int PPID)
		{
			throw new Exception("Ne radi trenutno!");
			//return promene.Where(x => x.PPID == PPID && (x.VrDok == -61 || x.VrDok == 0) && x.Konto.Substring(0, Math.Min(3, x.Konto.Length)) == "204" && x.DatNal.Day == 1 && x.DatNal.Month == 1).Sum(x => x.Potrazuje - x.Duguje);
		}

		private double GetPocetnoStanjeFinansijskoDobavljac(List<Promena> promene, int PPID)
		{
			throw new Exception("Ne radi trenutno!");
			//return promene.Where(x => x.PPID == PPID && (x.VrDok == -59 || x.VrDok == 0) && x.Konto.Substring(0, Math.Min(2, x.Konto.Length)) == "43" && x.DatNal.Day == 1 && x.DatNal.Month == 1).Sum(x => x.Potrazuje - x.Duguje);
		}

		private double GetKrajnjeStanjeFinansijskoKupac(List<Promena> promene, int PPID)
		{
			double stanje = 0;
			List<Promena> promeneKupac = promene
				.Where(x =>
					x.Konto.Substring(0, Math.Min(3, x.Konto.Length)) == "204" && x.PPID == PPID
				)
				.ToList();

			throw new Exception("Ne radi trenutno!");
			//stanje += promeneKupac.Sum(x => x.Potrazuje - x.Duguje);
			// stanje += promeneKupac.Where(x => x.PPID == PPID && x.VrDok != null && dokumentiKojiUticuNaPromene.Contains((int)x.VrDok)).Sum(x => x.Potrazuje - x.Duguje);

			return stanje;
		}

		private double GetKrajnjeStanjeFinansijskoDobavljac(List<Promena> promene, int PPID)
		{
			double stanje = 0;

			List<Promena> promeneDobavljac = promene
				.Where(x =>
					x.Konto.Substring(0, Math.Min(2, x.Konto.Length)) == "43" && x.PPID == PPID
				)
				.ToList();

			throw new Exception("Ne radi trenutno!");
			//stanje += promeneDobavljac.Sum(x => x.Potrazuje - x.Duguje);

			return stanje;
		}

		private void _1347_fm_PartnerPocetnoKrajnje_Index_FormClosing(
			object sender,
			FormClosingEventArgs e
		)
		{
			while (_working)
				Thread.Sleep(50);
		}

		private void FiltrirajDGV()
		{
			DataTable tempData = finalData.Copy();
			List<int> hidePPIDs = new List<int>();

			for (int i = tempData.Rows.Count - 1; i >= 0; i--)
			{
				string izvor = finalData.Rows[i]["Izvor"].ToString();

				Dictionary<string, double> pocetno = new Dictionary<string, double>();
				Dictionary<string, double> krajnje = new Dictionary<string, double>();

				double hPrag = (double)hPrag_nud.Value;
				double vPrag = (double)vPrag_nud.Value;
				pocetno.Add("2023", Convert.ToDouble(finalData.Rows[i]["PocetnoStanje2023"]));

				krajnje.Add("2022", Convert.ToDouble(finalData.Rows[i]["KrajnjeStanje2022"]));
				pocetno.Add("2022", Convert.ToDouble(finalData.Rows[i]["PocetnoStanje2022"]));

				krajnje.Add("2021", Convert.ToDouble(finalData.Rows[i]["KrajnjeStanje2021"]));
				pocetno.Add("2021", Convert.ToDouble(finalData.Rows[i]["PocetnoStanje2021"]));

				krajnje.Add("2020", Convert.ToDouble(finalData.Rows[i]["KrajnjeStanje2020"]));
				pocetno.Add("2020", Convert.ToDouble(finalData.Rows[i]["PocetnoStanje2020"]));

				krajnje.Add("2019", Convert.ToDouble(finalData.Rows[i]["KrajnjeStanje2019"]));
				pocetno.Add("2019", Convert.ToDouble(finalData.Rows[i]["PocetnoStanje2019"]));

				krajnje.Add("2018", Convert.ToDouble(finalData.Rows[i]["KrajnjeStanje2018"]));
				pocetno.Add("2018", Convert.ToDouble(finalData.Rows[i]["PocetnoStanje2018"]));

				krajnje.Add("2017", Convert.ToDouble(finalData.Rows[i]["KrajnjeStanje2017"]));

				Dictionary<int, CheckBox> cbs = new Dictionary<int, CheckBox>()
				{
					{ 2017, cb2017 },
					{ 2018, cb2018 },
					{ 2019, cb2019 },
					{ 2020, cb2020 },
					{ 2021, cb2021 },
					{ 2022, cb2022 }
				};

				if (comboBox1.SelectedIndex == 1)
				{
					bool toHide = true;
					if (hPrag > 0)
					{
						double maxRazlika = 0;

						for (int god = 2017; god <= 2022; god++)
						{
							if (!cbs[god].Checked)
								continue;

							double trenutnaRazlika = Math.Abs(
								krajnje[god.ToString()] - pocetno[(god + 1).ToString()]
							);
							if (maxRazlika < trenutnaRazlika)
								maxRazlika = trenutnaRazlika;
						}

						if (maxRazlika >= hPrag)
							toHide = false;
					}

					if (vPrag > 0)
					{
						cbs = new Dictionary<int, CheckBox>()
						{
							{ 2017, v2017 },
							{ 2018, v2018 },
							{ 2019, v2019 },
							{ 2020, v2020 },
							{ 2021, v2021 },
							{ 2022, v2022 },
							{ 2023, v2023 }
						};
						DataRow[] redovi = tempData.Select("PPID = " + tempData.Rows[i]["PPID"]);

						DataRow kupac = redovi
							.CopyToDataTable()
							.Select("Izvor = 'Finansijsko Kupac'")[0];
						DataRow dobavljac = redovi
							.CopyToDataTable()
							.Select("Izvor = 'Finansijsko Kupac'")[0];
						DataRow komercijalno = redovi
							.CopyToDataTable()
							.Select("Izvor = 'Komercijalno'")[0];

						if (
							redovi.Length != 3
							|| kupac == null
							|| dobavljac == null
							|| komercijalno == null
						)
						{
							Thread t5 = new Thread(() =>
							{
								MessageBox.Show(
									"Doslo je do greske prilikom selektovanja finansijskih podataka za ovog parnera. Filtriranje ce se nastaviti bez obzira na to!"
								);
							});
							t5.IsBackground = true;
							t5.Start();
							continue;
						}

						double maxRazlikaPocetnoStanje = 0;
						double maxRazlikaKrajnjeStanje = 0;

						for (int god = 2017; god <= 2023; god++)
						{
							if (!cbs[god].Checked)
								continue;

							double trenutnaRazlika = Math.Abs(
								Convert.ToDouble(komercijalno["PocetnoStanje" + god.ToString()])
									- Convert.ToDouble(kupac["PocetnoStanje" + god.ToString()])
									+ Convert.ToDouble(dobavljac["PocetnoStanje" + god.ToString()])
							);
							if (maxRazlikaPocetnoStanje < trenutnaRazlika)
								maxRazlikaPocetnoStanje = trenutnaRazlika;

							trenutnaRazlika = Math.Abs(
								Convert.ToDouble(komercijalno["PocetnoStanje" + god.ToString()])
									- Convert.ToDouble(kupac["KrajnjeStanje" + god.ToString()])
									+ Convert.ToDouble(dobavljac["KrajnjeStanje" + god.ToString()])
							);
							if (maxRazlikaKrajnjeStanje < trenutnaRazlika)
								maxRazlikaKrajnjeStanje = trenutnaRazlika;
						}

						if (maxRazlikaPocetnoStanje >= vPrag)
							toHide = false;
					}

					if (toHide && (hPrag > 0 || vPrag > 0))
						hidePPIDs.Add(Convert.ToInt32(tempData.Rows[i]["PPID"]));
				}
			}

			for (int i = tempData.Rows.Count - 1; i >= 0; i--)
				if (hidePPIDs.Count(x => x == Convert.ToInt32(tempData.Rows[i]["PPID"])) == 3)
					tempData.Rows.RemoveAt(i);

			dataGridView1.DataSource = tempData;
		}

		private void ColorDGV()
		{
			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				DataGridViewRow row = dataGridView1.Rows[i];
				if (
					Math.Round(Convert.ToDouble(row.Cells["KrajnjeStanje2017"].Value), 2)
					!= Math.Round(Convert.ToDouble(row.Cells["PocetnoStanje2018"].Value), 2)
				)
					dataGridView1.Rows[i].Cells["PocetnoStanje2018"].Style.BackColor = Color.Coral;

				if (
					Math.Round(Convert.ToDouble(row.Cells["KrajnjeStanje2018"].Value), 2)
					!= Math.Round(Convert.ToDouble(row.Cells["PocetnoStanje2019"].Value), 2)
				)
					dataGridView1.Rows[i].Cells["PocetnoStanje2019"].Style.BackColor = Color.Coral;

				if (
					Math.Round(Convert.ToDouble(row.Cells["KrajnjeStanje2019"].Value), 2)
					!= Math.Round(Convert.ToDouble(row.Cells["PocetnoStanje2020"].Value), 2)
				)
					dataGridView1.Rows[i].Cells["PocetnoStanje2020"].Style.BackColor = Color.Coral;

				if (
					Math.Round(Convert.ToDouble(row.Cells["KrajnjeStanje2020"].Value), 2)
					!= Math.Round(Convert.ToDouble(row.Cells["PocetnoStanje2021"].Value), 2)
				)
					dataGridView1.Rows[i].Cells["PocetnoStanje2021"].Style.BackColor = Color.Coral;

				if (
					Math.Round(Convert.ToDouble(row.Cells["KrajnjeStanje2021"].Value), 2)
					!= Math.Round(Convert.ToDouble(row.Cells["PocetnoStanje2022"].Value), 2)
				)
					dataGridView1.Rows[i].Cells["PocetnoStanje2022"].Style.BackColor = Color.Coral;

				if (
					Math.Round(Convert.ToDouble(row.Cells["KrajnjeStanje2022"].Value), 2)
					!= Math.Round(Convert.ToDouble(row.Cells["PocetnoStanje2023"].Value), 2)
				)
					dataGridView1.Rows[i].Cells["PocetnoStanje2023"].Style.BackColor = Color.Coral;

				if (row.Cells["Izvor"].Value.ToString() == "Komercijalno")
					dataGridView1.Rows[i].DefaultCellStyle.Font = new Font(
						dataGridView1.DefaultCellStyle.Font,
						FontStyle.Bold
					);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			FiltrirajDGV();
			ColorDGV();
		}

		private void dataGridView1_Sorted(object sender, EventArgs e)
		{
			ColorDGV();
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (comboBox1.SelectedIndex == 1)
			{
				label4.Visible = true;
				label3.Visible = true;
				hPrag_nud.Visible = true;
				vPrag_nud.Visible = true;
				cb2017.Visible = true;
				cb2018.Visible = true;
				cb2019.Visible = true;
				cb2020.Visible = true;
				cb2021.Visible = true;
				cb2022.Visible = true;
				v2017.Visible = true;
				v2018.Visible = true;
				v2019.Visible = true;
				v2020.Visible = true;
				v2021.Visible = true;
				v2022.Visible = true;
				v2023.Visible = true;
			}
			else
			{
				label4.Visible = false;
				label3.Visible = false;
				hPrag_nud.Visible = false;
				vPrag_nud.Visible = false;
				cb2017.Visible = false;
				cb2018.Visible = false;
				cb2019.Visible = false;
				cb2020.Visible = false;
				cb2021.Visible = false;
				cb2022.Visible = false;
				v2017.Visible = false;
				v2018.Visible = false;
				v2019.Visible = false;
				v2020.Visible = false;
				v2021.Visible = false;
				v2022.Visible = false;
				v2023.Visible = false;
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			Form fm = this;
			bool loaded = false;
			Task.Run(() =>
			{
				fm.Invoke(
					(MethodInvoker)
						delegate
						{
							button3.Enabled = false;
							OnemoguciFiltere();
						}
				);

				UcitajPodatkeAsync().Wait();

				loaded = true;

				fm.Invoke(
					(MethodInvoker)
						delegate
						{
							doPPID_nud.Enabled = true;
							odPPID_nud.Enabled = true;
							button1.Enabled = true;
							button3.Enabled = true;
						}
				);
			});

			Task.Run(() =>
			{
				double r = Random.Next(0, 256);
				double g = Random.Next(0, 256);
				double b = Random.Next(0, 256);
				while (!loaded)
				{
					fm.Invoke(
						(MethodInvoker)
							delegate
							{
								button3.BackColor = Color.FromArgb((int)r, (int)g, (int)b);
							}
					);
					r += 0.05;
					if (r > 255)
						r = 0;
					g += 0.05;
					if (g > 255)
						g = 0;
					b += 0.05;
					if (b > 255)
						b = 0;
				}
				button3.BackColor = Control.DefaultBackColor;
			});
		}

		private void karticaPartneraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dataGridView1.SelectedRows.Count == 0)
			{
				MessageBox.Show("Nije selektovan partner!");
				return;
			}
			int ppid = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["PPID"].Value);

			List<Termodom.Data.Entities.Komercijalno.Izvod> izvodiPartnera = stavkeIzvoda["2023"]
				.Values.Where(x => x.PPID == ppid)
				.ToList();
			List<IstUpl> istorijeUplataPartnera = istorijeUplata["2023"]
				.Where(x => x.PPID == ppid)
				.ToList();

			foreach (int key in Komercijalno.Komercijalno.CONNECTION_STRING.Keys)
			{
				izvodiPartnera = stavkeIzvoda[key.ToString()]
					.Values.Where(x => x.PPID == ppid)
					.ToList();
				istorijeUplataPartnera = istorijeUplata[key.ToString()]
					.Where(x => x.PPID == ppid)
					.ToList();

				Task.Run(async () =>
				{
					Partner p = await Partner.GetAsync(ppid);
					using (
						fm_KarticaPartnera kp = new fm_KarticaPartnera(
							dokumenti[key.ToString()],
							new Termodom.Data.Entities.Komercijalno.IzvodDictionary(
								izvodiPartnera.ToDictionary(x => x.IzvodId)
							),
							istorijeUplataPartnera,
							ppid
						)
					)
					{
						kp.Text = key + " - Kartica robe: " + p.Naziv;
						kp.ShowDialog();
					}
				});
			}
		}

		private double GetRealnoPocetnoStanjePartnera(string godina, int ppid)
		{
			// Sabiram sva pocetna stanja kupca (-61) ili dobavljaca (-59) za datog partnera
			// Radim kao sum jer moze imati vise pocetnih stanja
			// Hvatam samo na datum 01.01
			double psKupac = istorijeUplata[godina]
				.Where(x =>
					x.Datum.Day == 1 && x.Datum.Month == 1 && x.VrDok == -61 && x.PPID == ppid
				)
				.Sum(x => x.Iznos);
			double psDobavljac = istorijeUplata[godina]
				.Where(x =>
					x.Datum.Day == 1 && x.Datum.Month == 1 && x.VrDok == -59 && x.PPID == ppid
				)
				.Sum(x => x.Iznos);
			return psKupac - psDobavljac;
		}

		public double GetRealnoKrajnjeStanjePartnera(
			string godina,
			int ppid,
			List<Dokument> dokumentiPartnera,
			List<Termodom.Data.Entities.Komercijalno.Izvod> izvodiPartnera
		)
		{
			double krajnjeStanje = GetRealnoPocetnoStanjePartnera(godina, ppid);

			// Sada dodajem sva potrazivanja i dugovanja prema partneru
			foreach (
				Dokument dok in dokumentiPartnera.Where(x =>
					new int[] { 10, 13, 14, 15, 22, 39, 40 }.Contains(x.VrDok)
				)
			)
			{
				// 15, 14 = potrazuje = izlaz
				// 22 = potrazuje = ulaz
				// 39, 10 = duguje = ulaz
				// 13, 40 = duguje = izlaz

				if (new int[] { 13, 14, 15, 40 }.Contains(dok.VrDok))
				{
					krajnjeStanje -= new int[] { 13, 40 }.Contains(dok.VrDok)
						? dok.Duguje
						: dok.Potrazuje;
					krajnjeStanje += new NacinUplate[]
					{
						NacinUplate.Gotovina,
						NacinUplate.Kartica
					}.Contains(dok.NUID)
						? dok.Potrazuje
						: 0;
				}
				else
				{
					krajnjeStanje += new int[] { 10, 39 }.Contains(dok.VrDok)
						? dok.Duguje
						: dok.Potrazuje;
					krajnjeStanje -=
						new int[] { 10 }.Contains(dok.VrDok) && dok.NUID == NacinUplate.Gotovina
							? dok.Duguje
							: 0;
				}
			}

			// Sada dodajem sve uplate i islate

			foreach (Termodom.Data.Entities.Komercijalno.Izvod izv in izvodiPartnera)
			{
				krajnjeStanje -= izv.Duguje;
				krajnjeStanje += izv.Potrazuje;
			}

			return krajnjeStanje;
		}
	}
}
