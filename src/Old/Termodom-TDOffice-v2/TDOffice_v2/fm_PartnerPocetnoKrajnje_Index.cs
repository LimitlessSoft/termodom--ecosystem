using System;
using System.Collections.Concurrent;
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

namespace TDOffice_v2
{
	public partial class fm_PartnerPocetnoKrajnje_Index : Form
	{
		DateTime dt1 = DateTime.Now;
		private Task<
			ConcurrentDictionary<int, List<Komercijalno.IstUpl>>
		> _istorijeUplata { get; set; }
		private Task<List<Komercijalno.Partner>> _partneri { get; set; } =
			Komercijalno.Partner.ListAsync(DateTime.Now.Year);
		private Task<
			ConcurrentDictionary<int, List<Komercijalno.Dokument>>
		> _dokumenti { get; set; }
		private Task<
			ConcurrentDictionary<int, Termodom.Data.Entities.Komercijalno.IzvodDictionary>
		> _izvodi { get; set; }
		private Task<ConcurrentDictionary<int, List<Komercijalno.Promena>>> _promene { get; set; }
		private Task _ucitavanjePodatakaUIAnimacija { get; set; }
		private Task _ucitavanjePodataka { get; set; }

		private DataTable _finalDataTable { get; set; }

		public fm_PartnerPocetnoKrajnje_Index()
		{
			InitializeComponent();
			UcitajPodatkeAsync();
		}

		private void fm_PartnerPocetnoKrajnje_Index_Load(object sender, EventArgs e) { }

		private void UcitajPodatkeAsync()
		{
			if (_ucitavanjePodataka != null && _ucitavanjePodataka.Status == TaskStatus.Running)
				return;

			Task.Run(() =>
			{
				ConcurrentDictionary<int, List<Komercijalno.IstUpl>> istorijeUplataDict =
					new ConcurrentDictionary<int, List<Komercijalno.IstUpl>>();
				ConcurrentDictionary<int, List<Komercijalno.Dokument>> dokumentiDict =
					new ConcurrentDictionary<int, List<Komercijalno.Dokument>>();
				ConcurrentDictionary<
					int,
					Termodom.Data.Entities.Komercijalno.IzvodDictionary
				> izvodiDict =
					new ConcurrentDictionary<
						int,
						Termodom.Data.Entities.Komercijalno.IzvodDictionary
					>();
				ConcurrentDictionary<int, List<Komercijalno.Promena>> promeneDict =
					new ConcurrentDictionary<int, List<Komercijalno.Promena>>();

				_ucitavanjePodataka = Task.Run(() =>
				{
					Parallel.ForEach(
						Komercijalno.Komercijalno.CONNECTION_STRING.Keys,
						godina =>
						{
							using (
								FbConnection con = new FbConnection(
									Komercijalno.Komercijalno.CONNECTION_STRING[godina]
								)
							)
							{
								con.Open();
								istorijeUplataDict[godina] = Komercijalno.IstUpl.List(con);
								dokumentiDict[godina] = Komercijalno.Dokument.List(
									con,
									"PPID IS NOT NULL AND VRDOK IN (10, 13, 14, 15, 22, 39, 40)"
								);
								izvodiDict[godina] = Komercijalno
									.IzvodManager.DictionaryAsync(150, godina)
									.GetAwaiter()
									.GetResult();
								promeneDict[godina] = Komercijalno.Promena.List(con);
							}
						}
					);
				});

				_istorijeUplata = Task.Run(() =>
				{
					_ucitavanjePodataka.Wait();
					return istorijeUplataDict;
				});
				_dokumenti = Task.Run(() =>
				{
					_ucitavanjePodataka.Wait();
					return dokumentiDict;
				});
				_izvodi = Task.Run(() =>
				{
					_ucitavanjePodataka.Wait();
					return izvodiDict;
				});
				_promene = Task.Run(() =>
				{
					_ucitavanjePodataka.Wait();
					return promeneDict;
				});
			});
		}

		private class DGVDataBuffer
		{
			public int PPID { get; set; }
			public string NazivPartnera { get; set; }
			public ConcurrentDictionary<int, double> PocetnoStanje { get; set; } =
				new ConcurrentDictionary<int, double>();
			public ConcurrentDictionary<int, double> KrajnjeStanje { get; set; } =
				new ConcurrentDictionary<int, double>();
			public string Izvor { get; set; }
		}

		private Task PopuniDGVAsync()
		{
			return Task.Run(() =>
			{
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							OnemoguciFiltere();
							status_lbl.Text = "Ucitavam podatke...";
						}
				);
				DataTable dt = new DataTable();
				dt.Columns.Add("PPID", typeof(int));
				dt.Columns.Add("Partner", typeof(string));
				foreach (
					int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x)
				)
				{
					dt.Columns.Add("PocetnoStanje" + godina, typeof(double));
					dt.Columns.Add("KrajnjeStanje" + godina, typeof(double));
				}
				dt.Columns.Add("Izvor", typeof(string));

				ConcurrentBag<DGVDataBuffer> buffer = new ConcurrentBag<DGVDataBuffer>();
				List<Komercijalno.Partner> partneriZaObradu = _partneri
					.Result.Where(x => x.PPID >= odPPID_nud.Value && x.PPID <= doPPID_nud.Value)
					.ToList();
				int obradjenoPartnera = 0;
				Parallel.ForEach(
					partneriZaObradu,
					partner =>
					{
						ConcurrentDictionary<int, List<Komercijalno.Promena>> promenePartnera =
							new ConcurrentDictionary<int, List<Komercijalno.Promena>>();
						foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys)
							promenePartnera[godina] = _promene
								.Result[godina]
								.Where(x => x.PPID == partner.PPID)
								.ToList();

						Parallel.For(
							0,
							3,
							(i) =>
							{
								// i = 0 > Finansijsko Kupac
								// i = 1 > Finansijsko Dobavljac
								// i = 2 > Komercijalno

								DGVDataBuffer dbi = new DGVDataBuffer();
								dbi.PPID = partner.PPID;
								dbi.NazivPartnera = partner.Naziv;
								Parallel.ForEach(
									Komercijalno.Komercijalno.CONNECTION_STRING.Keys,
									godina =>
									{
										double pocetnoStanaje =
											i == 0
												? GetPocetnoStanjePartneraFinansijskoKupac(
													promenePartnera[godina],
													partner.PPID
												)
												: i == 1
													? GetPocetnoStanjePartneraFinansijskoDobavljac(
														promenePartnera[godina],
														partner.PPID
													)
													: GetPocetnoStanjePartneraKomercijalno(
														godina,
														partner.PPID
													);

										dbi.PocetnoStanje[godina] = pocetnoStanaje;

										double krajnjeStanje =
											i == 0
												? GetKrajnjeStanjePartneraFinansijskoKupac(
													promenePartnera[godina],
													partner.PPID
												)
												: i == 1
													? GetKrajnjeStanjePartneraFinansijskoDobavljac(
														promenePartnera[godina],
														partner.PPID
													)
													: GetKrajnjeStanjePartneraKomercijalno(
														dbi.PocetnoStanje[godina],
														_dokumenti
															.Result[godina]
															.Where(x => x.PPID == partner.PPID)
															.ToList(),
														_izvodi
															.Result[godina]
															.Values.Where(x =>
																x.PPID == partner.PPID
															)
															.ToList()
													);

										dbi.KrajnjeStanje[godina] = krajnjeStanje;
									}
								);

								dbi.Izvor =
									i == 0
										? "Finansijsko Kupac"
										: i == 1
											? "Finansijsko Dobavljac"
											: "Komercijalno";
								buffer.Add(dbi);
							}
						);
						obradjenoPartnera++;
						this.Invoke(
							(MethodInvoker)
								delegate
								{
									status_lbl.Text =
										$"{obradjenoPartnera} / {partneriZaObradu.Count}";
								}
						);
					}
				);

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							status_lbl.Text = "Popunjavam...";
						}
				);

				foreach (DGVDataBuffer bufferItem in buffer)
				{
					DataRow dr = dt.NewRow();
					dr["PPID"] = bufferItem.PPID;
					dr["Partner"] = bufferItem.NazivPartnera;
					Parallel.ForEach(
						Komercijalno.Komercijalno.CONNECTION_STRING.Keys,
						godina =>
						{
							dr["PocetnoStanje" + godina] = bufferItem.PocetnoStanje.ContainsKey(
								godina
							)
								? bufferItem.PocetnoStanje[godina]
								: 0;
							dr["KrajnjeStanje" + godina] = bufferItem.KrajnjeStanje.ContainsKey(
								godina
							)
								? bufferItem.KrajnjeStanje[godina]
								: 0;
						}
					);

					dr["Izvor"] = bufferItem.Izvor;
					dt.Rows.Add(dr);
				}

				_finalDataTable = dt.Copy();

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							dataGridView1.DataSource = dt;
							status_lbl.Text = "Ucitano!";

							if (dataGridView1.Rows.Count > 0)
							{
								dataGridView1.Columns["Partner"].Width = 300;
								dataGridView1.Columns["Izvor"].Width = 150;

								dataGridView1.Columns["PocetnoStanje2017"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["PocetnoStanje2017"].Width = 100;
								dataGridView1.Columns["PocetnoStanje2017"].HeaderText =
									"Pocetno 2017";
								dataGridView1
									.Columns["PocetnoStanje2017"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;
								dataGridView1.Columns["KrajnjeStanje2017"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["KrajnjeStanje2017"].Width = 100;
								dataGridView1.Columns["KrajnjeStanje2017"].HeaderText =
									"Krajnje 2017";
								dataGridView1
									.Columns["KrajnjeStanje2017"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;

								dataGridView1.Columns["PocetnoStanje2018"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["PocetnoStanje2018"].Width = 100;
								dataGridView1.Columns["PocetnoStanje2018"].HeaderText =
									"Pocetno 2018";
								dataGridView1
									.Columns["PocetnoStanje2018"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;
								dataGridView1.Columns["KrajnjeStanje2018"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["KrajnjeStanje2018"].Width = 100;
								dataGridView1.Columns["KrajnjeStanje2018"].HeaderText =
									"Krajnje 2018";
								dataGridView1
									.Columns["KrajnjeStanje2018"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;

								dataGridView1.Columns["PocetnoStanje2019"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["PocetnoStanje2019"].Width = 100;
								dataGridView1.Columns["PocetnoStanje2019"].HeaderText =
									"Pocetno 2019";
								dataGridView1
									.Columns["PocetnoStanje2019"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;
								dataGridView1.Columns["KrajnjeStanje2019"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["KrajnjeStanje2019"].Width = 100;
								dataGridView1.Columns["KrajnjeStanje2019"].HeaderText =
									"Krajnje 2019";
								dataGridView1
									.Columns["KrajnjeStanje2019"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;

								dataGridView1.Columns["PocetnoStanje2020"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["PocetnoStanje2020"].Width = 100;
								dataGridView1.Columns["PocetnoStanje2020"].HeaderText =
									"Pocetno 2020";
								dataGridView1
									.Columns["PocetnoStanje2020"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;
								dataGridView1.Columns["KrajnjeStanje2020"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["KrajnjeStanje2020"].Width = 100;
								dataGridView1.Columns["KrajnjeStanje2020"].HeaderText =
									"Krajnje 2020";
								dataGridView1
									.Columns["KrajnjeStanje2020"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;

								dataGridView1.Columns["PocetnoStanje2021"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["PocetnoStanje2021"].Width = 100;
								dataGridView1.Columns["PocetnoStanje2021"].HeaderText =
									"Pocetno 2021";
								dataGridView1
									.Columns["PocetnoStanje2021"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;
								dataGridView1.Columns["KrajnjeStanje2021"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["KrajnjeStanje2021"].Width = 100;
								dataGridView1.Columns["KrajnjeStanje2021"].HeaderText =
									"Krajnje 2021";
								dataGridView1
									.Columns["KrajnjeStanje2021"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;

								dataGridView1.Columns["PocetnoStanje2022"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["PocetnoStanje2022"].Width = 100;
								dataGridView1.Columns["PocetnoStanje2022"].HeaderText =
									"Pocetno 2022";
								dataGridView1
									.Columns["PocetnoStanje2022"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;
								dataGridView1.Columns["KrajnjeStanje2022"].DefaultCellStyle.Format =
									"#,##0.00";
								dataGridView1.Columns["KrajnjeStanje2022"].Width = 100;
								dataGridView1.Columns["KrajnjeStanje2022"].HeaderText =
									"Krajnje 2022";
								dataGridView1
									.Columns["KrajnjeStanje2022"]
									.DefaultCellStyle
									.Alignment = DataGridViewContentAlignment.MiddleRight;

								ColorDGV();
							}

							OmoguciFiltere();
						}
				);
			});
		}

		private double GetPocetnoStanjePartneraKomercijalno(int godina, int ppid)
		{
			double psKupac = _istorijeUplata
				.Result[godina]
				.Where(x =>
					x.Datum.Day == 1 && x.Datum.Month == 1 && x.VrDok == -61 && x.PPID == ppid
				)
				.Sum(x => x.Iznos);
			double psDobavljac = _istorijeUplata
				.Result[godina]
				.Where(x =>
					x.Datum.Day == 1 && x.Datum.Month == 1 && x.VrDok == -59 && x.PPID == ppid
				)
				.Sum(x => x.Iznos);
			return psKupac - psDobavljac;
		}

		public double GetKrajnjeStanjePartneraKomercijalno(
			double pocetnoStanjePartnera,
			List<Komercijalno.Dokument> dokumentiPartnera,
			List<Termodom.Data.Entities.Komercijalno.Izvod> izvodiPartnera
		)
		{
			double krajnjeStanje = pocetnoStanjePartnera;

			// Sada dodajem sva potrazivanja i dugovanja prema partneru
			foreach (
				Komercijalno.Dokument dok in dokumentiPartnera.Where(x =>
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
					krajnjeStanje += new Komercijalno.NacinUplate[]
					{
						Komercijalno.NacinUplate.Gotovina,
						Komercijalno.NacinUplate.Kartica
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
						new int[] { 10 }.Contains(dok.VrDok)
						&& dok.NUID == Komercijalno.NacinUplate.Gotovina
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

		private double GetPocetnoStanjePartneraFinansijskoKupac(
			List<Komercijalno.Promena> promene,
			int PPID
		)
		{
			throw new Exception("Ne radi trenutno!");
			//return promene.Where(x => x.PPID == PPID && (x.VrDok == -61 || x.VrDok == 0) && x.Konto.Substring(0, Math.Min(3, x.Konto.Length)) == "204" && x.DatNal.Day == 1 && x.DatNal.Month == 1).Sum(x => x.Potrazuje - x.Duguje);
		}

		private double GetPocetnoStanjePartneraFinansijskoDobavljac(
			List<Komercijalno.Promena> promene,
			int PPID
		)
		{
			throw new Exception("Ne radi trenutno!");
			//return promene.Where(x => x.PPID == PPID && (x.VrDok == -59 || x.VrDok == 0) && x.Konto.Substring(0, Math.Min(2, x.Konto.Length)) == "43" && x.DatNal.Day == 1 && x.DatNal.Month == 1).Sum(x => x.Potrazuje - x.Duguje);
		}

		private double GetKrajnjeStanjePartneraFinansijskoKupac(
			List<Komercijalno.Promena> promene,
			int PPID
		)
		{
			double stanje = 0;
			List<Komercijalno.Promena> promeneKupac = promene
				.Where(x =>
					x.Konto.Substring(0, Math.Min(3, x.Konto.Length)) == "204" && x.PPID == PPID
				)
				.ToList();

			throw new Exception("Ne radi trenutno!");
			//stanje += promeneKupac.Sum(x => x.Potrazuje - x.Duguje);
			// stanje += promeneKupac.Where(x => x.PPID == PPID && x.VrDok != null && dokumentiKojiUticuNaPromene.Contains((int)x.VrDok)).Sum(x => x.Potrazuje - x.Duguje);

			return stanje;
		}

		private double GetKrajnjeStanjePartneraFinansijskoDobavljac(
			List<Komercijalno.Promena> promene,
			int PPID
		)
		{
			double stanje = 0;

			List<Komercijalno.Promena> promeneDobavljac = promene
				.Where(x =>
					x.Konto.Substring(0, Math.Min(2, x.Konto.Length)) == "43" && x.PPID == PPID
				)
				.ToList();

			stanje += promeneDobavljac.Sum(x => (double)x.Potrazuje - (double)x.Duguje);

			return stanje;
		}

		private void ColorDGV()
		{
			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				DataGridViewRow row = dataGridView1.Rows[i];
				foreach (
					int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x)
				)
				{
					if (godina == DateTime.Now.Year)
						break;

					if (
						Math.Round(Convert.ToDouble(row.Cells["KrajnjeStanje" + godina].Value), 2)
						!= Math.Round(
							Convert.ToDouble(row.Cells["PocetnoStanje" + (godina + 1)].Value),
							2
						)
					)
						dataGridView1
							.Rows[i]
							.Cells["PocetnoStanje" + (godina + 1)]
							.Style
							.BackColor = Color.Coral;
				}

				if (row.Cells["Izvor"].Value.ToString() == "Komercijalno")
					dataGridView1.Rows[i].DefaultCellStyle.Font = new Font(
						dataGridView1.DefaultCellStyle.Font,
						FontStyle.Bold
					);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			PopuniDGVAsync();
		}

		private void dataGridView1_Sorted(object sender, EventArgs e)
		{
			ColorDGV();
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

		private void button3_Click(object sender, EventArgs e)
		{
			UcitajPodatkeAsync();
		}

		private void fm_PartnerPocetnoKrajnje_Index_Shown(object sender, EventArgs e)
		{
			_ucitavanjePodatakaUIAnimacija = Task.Run(() =>
			{
				double r = Random.Next(0, 256);
				double g = Random.Next(0, 256);
				double b = Random.Next(0, 256);
				while (this.Visible)
				{
					while (
						_ucitavanjePodataka != null
						&& _ucitavanjePodataka.Status == TaskStatus.Running
					)
					{
						this.Invoke(
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
					Thread.Sleep(TimeSpan.FromMilliseconds(500));
				}
			});
		}
	}
}
