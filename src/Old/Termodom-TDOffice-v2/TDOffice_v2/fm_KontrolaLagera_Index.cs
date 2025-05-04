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

namespace TDOffice_v2
{
	public partial class fm_KontrolaLagera_Index : Form
	{
		private int _trenutniMagacin { get; set; } = -1;
		private Task<List<Komercijalno.Magacin>> _magacini { get; set; } =
			Komercijalno.Magacin.ListAsync(DateTime.Now.Year);
		private Task<List<Komercijalno.Roba>> _roba { get; set; } =
			Komercijalno.Roba.ListAsync(DateTime.Now.Year);
		private Task<fm_Help> _helpFrom { get; set; }
		private DataTable _allData { get; set; }
		private DataTable _shownData { get; set; }

		private List<int> _listaRobe { get; set; } = null;
		private bool _prikaziOdmah { get; set; } = false;

		private bool _noPermission { get; set; } = false;

		public fm_KontrolaLagera_Index()
		{
			if (!Program.TrenutniKorisnik.ImaPravo(133500))
			{
				TDOffice.Pravo.NematePravoObavestenje(133500);
				this.Close();
				return;
			}
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.KontrolaLagera_Index);
			groupBox1.DesniKlik_DatumRange(null);
			SetBaseDataTable();
			SetUI();
		}

		public fm_KontrolaLagera_Index(int magacinID, List<int> listaRobe)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(133500))
			{
				TDOffice.Pravo.NematePravoObavestenje(133500);
				this.Close();
				return;
			}
			_trenutniMagacin = magacinID;
			InitializeComponent();
			groupBox1.DesniKlik_DatumRange(null);
			SetBaseDataTable();
			SetUI();
			_listaRobe = new List<int>(listaRobe);
			listaRobe_cb.Checked = true;
			_prikaziOdmah = true;
			listaRobe_btn.Text = $"Lista Robe ({_listaRobe.Count})";
		}

		private void fm_KontrolaLagera_Index_Load(object sender, EventArgs e) { }

		private void SetBaseDataTable()
		{
			_allData = new DataTable();
			_allData.Columns.Add("RobaID", typeof(int));
			_allData.Columns.Add("KatBr", typeof(string));
			_allData.Columns.Add("KatBrPro", typeof(string));
			_allData.Columns.Add("Naziv", typeof(string));
			_allData.Columns.Add("Stanje", typeof(double));
			_allData.Columns.Add("JM", typeof(string));
			_allData.Columns.Add("OptZal", typeof(double));
			_allData.Columns.Add("KritZal", typeof(double));
			_allData.Columns.Add("IzasloMes", typeof(double));
			_allData.Columns.Add("ProdatoMes", typeof(double));
			_allData.Columns.Add("ProdajnaCena", typeof(double));
			_allData.Columns.Add("VisakZaliha", typeof(double));
			_allData.Columns.Add("Vrednost", typeof(double));
			_allData.Columns.Add("Valuta", typeof(string));

			_shownData = _allData.Copy();

			foreach (DataColumn col in _allData.Columns)
				kolone_cmb.Items.Add(col.ColumnName);

			kolone_cmb.SelectedItem = "Naziv";

			UpdateDGV();

			dataGridView1.Columns["RobaID"].Visible = false;

			dataGridView1.Columns["KatBr"].Width = 150;

			dataGridView1.Columns["KatBrPro"].Width = 150;

			dataGridView1.Columns["Naziv"].Width = 300;

			dataGridView1.Columns["Stanje"].Width = 100;
			dataGridView1.Columns["Stanje"].DefaultCellStyle.Format = "#,##0.00";
			dataGridView1.Columns["Stanje"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;

			dataGridView1.Columns["JM"].Width = 50;

			dataGridView1.Columns["OptZal"].Width = 100;
			dataGridView1.Columns["OptZal"].DefaultCellStyle.Format = "#,##0.00";
			dataGridView1.Columns["OptZal"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;

			dataGridView1.Columns["KritZal"].Width = 100;
			dataGridView1.Columns["KritZal"].DefaultCellStyle.Format = "#,##0.00";
			dataGridView1.Columns["KritZal"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;

			dataGridView1.Columns["IzasloMes"].Width = 100;
			dataGridView1.Columns["IzasloMes"].DefaultCellStyle.Format = "#,##0.00";
			dataGridView1.Columns["IzasloMes"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;

			dataGridView1.Columns["ProdatoMes"].Width = 100;
			dataGridView1.Columns["ProdatoMes"].DefaultCellStyle.Format = "#,##0.00";
			dataGridView1.Columns["ProdatoMes"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;

			dataGridView1.Columns["VisakZaliha"].Width = 100;
			dataGridView1.Columns["VisakZaliha"].DefaultCellStyle.Format = "#,##0.00";
			dataGridView1.Columns["VisakZaliha"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["VisakZaliha"].HeaderText = "Visak Zaliha (Stanje - ProdatoMes)";

			dataGridView1.Columns["ProdajnaCena"].Width = 100;
			dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Format = "#,##0.00";
			dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;

			dataGridView1.Columns["Vrednost"].Width = 150;
			dataGridView1.Columns["Vrednost"].DefaultCellStyle.Format = "#,##0.00";
			dataGridView1.Columns["Vrednost"].HeaderText = "Vrednost (Stanje * Prodajna Cena)";
			dataGridView1.Columns["Vrednost"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;

			dataGridView1.Columns["Valuta"].Width = 50;

			kolonaVrednsot_cmb.SelectedIndex = 0;
		}

		private void SetUI()
		{
			odDatuma_dtp.Value = new DateTime(DateTime.Now.Year, 1, 1);
			doDatuma_dtp.Value = DateTime.Now;

			_magacini.Result.Add(
				new Komercijalno.Magacin() { ID = -1, Naziv = " < izaberi magacin > " }
			);
			_magacini.Result.Sort((x, y) => x.ID.CompareTo(y.ID));

			magacin_cmb.DisplayMember = "Naziv";
			magacin_cmb.ValueMember = "ID";

			magacin_cmb.DataSource = _magacini.Result;
			magacin_cmb.SelectedValue = _trenutniMagacin;
			magacin_cmb.Enabled = true;
		}

		private void UpdateDGV()
		{
			dataGridView1.DataSource = _shownData;
			slogova_lbl.Text =
				"Slogova: " + (_shownData == null ? 0 : _shownData.Rows.Count).ToString();

			if (
				dataGridView1.DataSource == null
				|| _shownData == null
				|| _shownData.Rows.Count == 0
			)
				return;

			this.dataGridView1.Sort(
				this.dataGridView1.Columns["Naziv"],
				ListSortDirection.Ascending
			);
		}

		private void prikazi_btn_Click(object sender, EventArgs e)
		{
			if (listaRobe_cb.Checked)
			{
				if (_listaRobe == null || _listaRobe.Count == 0)
				{
					MessageBox.Show("Cekirali ste da koristite listu robe ali ona je prazna!");
					return;
				}
			}

			_allData.Clear();
			int magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
			if (magacinID <= 0)
			{
				MessageBox.Show("Neispravan magacin!");
				return;
			}

			if (odDatuma_dtp.Value.Date >= doDatuma_dtp.Value.Date)
			{
				MessageBox.Show("Neispravan datum!");
				return;
			}

			_trenutniMagacin = magacinID;

			filterStanjeOperacija_cmb.Enabled = false;
			odDatuma_dtp.Enabled = false;
			doDatuma_dtp.Enabled = false;
			pretraga_txt.Enabled = false;
			filtriraj_btn.Enabled = false;
			listaRobe_btn.Enabled = false;
			prikazi_btn.Enabled = false;
			listaRobe_cb.Enabled = false;
			filterStanjeUslov_cmb.Enabled = false;
			kolone_cmb.Enabled = false;
			magacin_cmb.Enabled = false;
			kolonaVrednsot_cmb.Enabled = false;
			filterStanjeOperacija_cmb.SelectedIndex = 0;
			filterStanjeUslov_cmb.SelectedIndex = 0;

			Task.Run(() =>
			{
				List<int> godineZaKojeStavkeNisuUcitane = new List<int>();
				Task<List<Komercijalno.Stavka>> stavke = Task.Run<List<Komercijalno.Stavka>>(() =>
				{
					int yearsDiff = doDatuma_dtp.Value.Year - odDatuma_dtp.Value.Year;

					List<Komercijalno.Stavka> list = new List<Komercijalno.Stavka>();

					Task[] tasks = new Task[yearsDiff + 1];

					ManualResetEventSlim mre = new ManualResetEventSlim(true);
					for (int y = 0; y <= yearsDiff; y++)
					{
						mre.Wait();
						mre.Reset();
						tasks[y] = Task.Run(() =>
						{
							int i = y;
							mre.Set();
							int godina = doDatuma_dtp.Value.Year - i;

							if (
								i != 0
								&& !Komercijalno.Komercijalno.CONNECTION_STRING.ContainsKey(godina)
							)
							{
								godineZaKojeStavkeNisuUcitane.Add(godina);
								return;
							}

							using (
								FbConnection con = new FbConnection(
									Komercijalno.Komercijalno.CONNECTION_STRING[godina]
								)
							)
							{
								con.Open();
								List<Komercijalno.Stavka> mainList =
									Komercijalno.Stavka.ListByMagacinID(con, _trenutniMagacin);
								lock (list)
								{
									list.AddRange(mainList);
								}
							}
						});
					}

					for (int i = 0; i < tasks.Length; i++)
						tasks[i].Wait();

					return list;
				});
				Task<List<Komercijalno.RobaUMagacinu>> robaUMagacinu = Task.Run<
					List<Komercijalno.RobaUMagacinu>
				>(() =>
				{
					List<Komercijalno.RobaUMagacinu> rum =
						Komercijalno.RobaUMagacinu.ListByMagacinID(_trenutniMagacin);
					if (listaRobe_cb.Checked)
					{
						rum.RemoveAll(x => !_listaRobe.Contains(x.RobaID));
					}

					return rum;
				});
				Task<List<Komercijalno.Stavka>> stavkeProdajeMagacina = Task.Run<
					List<Komercijalno.Stavka>
				>(() =>
				{
					return stavke
						.Result.Where(x =>
							x.MagacinID == _trenutniMagacin && new[] { 15, 13 }.Contains(x.VrDok)
						)
						.ToList();
				});
				Task<List<Komercijalno.Stavka>> stavkeIzlazaMagacina = Task.Run<
					List<Komercijalno.Stavka>
				>(() =>
				{
					return stavke
						.Result.Where(x =>
							x.MagacinID == _trenutniMagacin
							&& Komercijalno.Komercijalno.izlazniVrDok.Contains(x.VrDok)
						)
						.ToList();
				});
				Task<List<Komercijalno.Dokument>> prodajniDokumentiPerioda = Task.Run<
					List<Komercijalno.Dokument>
				>(() =>
				{
					List<Komercijalno.Dokument> doks = Komercijalno.Dokument.ListByVrDok(
						DateTime.Now.Year,
						15
					);
					doks.AddRange(Komercijalno.Dokument.ListByVrDok(DateTime.Now.Year, 13));
					doks.RemoveAll(x =>
						x.MagacinID != _trenutniMagacin
						|| x.Datum.Date < odDatuma_dtp.Value.Date
						|| x.Datum.Date > doDatuma_dtp.Value.Date
					);
					return doks;
				});
				Task<List<Komercijalno.Dokument>> izlazniDokumentiPerioda = Task.Run<
					List<Komercijalno.Dokument>
				>(() =>
				{
					List<Komercijalno.Dokument> doks = new List<Komercijalno.Dokument>();

					foreach (int vrDok in Komercijalno.Komercijalno.izlazniVrDok)
						doks.AddRange(Komercijalno.Dokument.ListByVrDok(DateTime.Now.Year, vrDok));

					doks.RemoveAll(x =>
						x.MagacinID != _trenutniMagacin
						|| x.Datum.Date < odDatuma_dtp.Value.Date
						|| x.Datum.Date > doDatuma_dtp.Value.Date
					);
					return doks;
				});
				Task<Dictionary<int, double>> ukupnoProdato = Task.Run<Dictionary<int, double>>(
					() =>
					{
						Dictionary<int, double> dict = new Dictionary<int, double>();

						Parallel.ForEach(
							robaUMagacinu.Result,
							r =>
							{
								double res = stavkeProdajeMagacina
									.Result.Where(x =>
										x.RobaID == r.RobaID
										&& prodajniDokumentiPerioda.Result.Any(y =>
											y.VrDok == x.VrDok && y.BrDok == x.BrDok
										)
									)
									.Sum(x => x.Kolicina);
								lock (dict)
								{
									dict[r.RobaID] = res;
								}
							}
						);

						return dict;
					}
				);
				Task<Dictionary<int, double>> ukupnoIzaslo = Task.Run<Dictionary<int, double>>(() =>
				{
					Dictionary<int, double> dict = new Dictionary<int, double>();

					Parallel.ForEach(
						robaUMagacinu.Result,
						r =>
						{
							double res = stavkeIzlazaMagacina
								.Result.Where(x =>
									x.RobaID == r.RobaID
									&& izlazniDokumentiPerioda.Result.Any(y =>
										y.VrDok == x.VrDok && y.BrDok == x.BrDok
									)
								)
								.Sum(x => x.Kolicina);
							lock (dict)
							{
								dict[r.RobaID] = res;
							}
						}
					);

					return dict;
				});

				double nMeseci =
					(doDatuma_dtp.Value.Date - odDatuma_dtp.Value.Date).TotalDays / 30.5;
				double ukupnaVrednostLagera = 0;

				foreach (Komercijalno.RobaUMagacinu rum in robaUMagacinu.Result)
				{
					Komercijalno.Roba r = _roba
						.Result.Where(x => x.ID == rum.RobaID)
						.FirstOrDefault();

					DataRow dr = _allData.NewRow();

					dr["RobaID"] = rum.RobaID;
					dr["KatBr"] = r == null ? "UNKNOWN" : r.KatBr;
					dr["KatBrPro"] = r == null ? "UNKNOWN" : r.KatBrPro;
					dr["Naziv"] = r == null ? "UNKNOWN" : r.Naziv;
					dr["Stanje"] = rum.Stanje;
					dr["JM"] = r.JM;
					dr["OptZal"] = rum.OptimalneZalihe;
					dr["KritZal"] = rum.KriticneZalihe;
					dr["IzasloMes"] = ukupnoIzaslo.Result[rum.RobaID] / nMeseci;
					dr["ProdatoMes"] = ukupnoProdato.Result[rum.RobaID] / nMeseci;
					dr["ProdajnaCena"] = rum.ProdajnaCena;
					dr["VisakZaliha"] = rum.Stanje - (ukupnoProdato.Result[rum.RobaID] / nMeseci);
					dr["Vrednost"] = rum.ProdajnaCena * rum.Stanje;
					dr["Valuta"] = "RSD";

					ukupnaVrednostLagera += rum.ProdajnaCena * rum.Stanje;
					_allData.Rows.Add(dr);
				}

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							ukupnaVrednostLagera_txt.Text = ukupnaVrednostLagera.ToString(
								"#,##0.00 RSD"
							);
							dataGridView1.Columns["Vrednost"].HeaderText =
								"Vrednost (Stanje * Prodajna Cena)";

							_shownData = _allData.Copy();
							UpdateDGV();

							if (godineZaKojeStavkeNisuUcitane.Count > 0)
								MessageBox.Show(
									"Za godine "
										+ string.Join(", ", godineZaKojeStavkeNisuUcitane)
										+ " nisu ucitani podaci!"
								);

							filterStanjeOperacija_cmb.Enabled = true;
							odDatuma_dtp.Enabled = true;
							doDatuma_dtp.Enabled = true;
							pretraga_txt.Enabled = true;
							filterStanjeUslov_cmb.Enabled = true;
							filtriraj_btn.Enabled = true;
							listaRobe_btn.Enabled = true;
							prikazi_btn.Enabled = true;
							listaRobe_cb.Enabled = true;
							kolone_cmb.Enabled = true;
							magacin_cmb.Enabled = true;
							kolonaVrednsot_cmb.Enabled = true;

							MessageBox.Show("Podaci su uspesno ucitani!");
						}
				);
			});
		}

		private void pretraga_txt_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				FilterEnter();
				dataGridView1.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.A)
			{
				FilterCtrlA();
				dataGridView1.Focus();
			}
		}

		private void FilterCtrlA()
		{
			string selectString = "";
			string input = pretraga_txt.Text;
			string[] inputElemets = input.Split('+');

			foreach (object o in kolone_cmb.Items)
			{
				for (int i = 0; i < inputElemets.Length; i++)
					selectString +=
						"CONVERT("
						+ o.ToString()
						+ ", System.String) LIKE '%"
						+ inputElemets[i]
						+ "%' AND ";

				selectString = selectString.Remove(selectString.Length - 4);
				selectString += " OR ";
			}

			selectString = selectString.Remove(selectString.Length - 4);

			DataRow[] rows = _allData.Copy().Select(selectString);
			_shownData = rows == null || rows.Count() == 0 ? null : rows.CopyToDataTable();
			UpdateDGV();
		}

		private void FilterEnter()
		{
			dataGridView1.ClearSelection();
			string kolona = kolone_cmb.SelectedItem.ToString();
			string input = pretraga_txt.Text;

			if (string.IsNullOrWhiteSpace(input))
			{
				dataGridView1.FirstDisplayedScrollingRowIndex = 0;
				dataGridView1.Rows[0].Selected = true;
				dataGridView1.Focus();
				dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["Naziv"];
				return;
			}

			foreach (DataGridViewRow row in dataGridView1.Rows)
			{
				string vrednostCelije = row.Cells[kolona].Value.ToString();
				if (vrednostCelije.ToLower().IndexOf(input.ToLower()) == 0)
				{
					dataGridView1.FirstDisplayedScrollingRowIndex =
						row.Index > 0 ? row.Index - 1 : 0;
					dataGridView1.Rows[row.Index].Selected = true;
					dataGridView1.Focus();
					dataGridView1.CurrentCell = dataGridView1.Rows[row.Index].Cells["Naziv"];
					return;
				}
			}
		}

		private void filtriraj_btn_Click(object sender, EventArgs e)
		{
			if (
				filterStanjeOperacija_cmb.SelectedIndex == 0
				|| filterStanjeUslov_cmb.SelectedIndex == 0
			)
			{
				MessageBox.Show("Neispravan filter!");
				return;
			}
			if (_shownData == null || _shownData.Rows.Count == 0)
				return;

			string stanjeOperacija =
				filterStanjeOperacija_cmb.SelectedIndex == 1
					? "="
					: filterStanjeOperacija_cmb.SelectedIndex == 2
						? ">"
						: "<";
			string stanjeUporednaKolona = "";
			string selectString = "";

			switch (filterStanjeUslov_cmb.SelectedIndex)
			{
				case 0:
					break;
				case 1:
					stanjeUporednaKolona = "OptZal";
					break;
				case 2:
					stanjeUporednaKolona = "KritZal";
					break;
				case 3:
					stanjeUporednaKolona = "IzasloMes";
					break;
				case 4:
					stanjeUporednaKolona = "ProdatoMes";
					break;
				default:
					MessageBox.Show("Greska filter stanja uslov.");
					return;
			}

			selectString += $"STANJE {stanjeOperacija} {stanjeUporednaKolona}";

			DataRow[] rows = _allData.Copy().Select(selectString);
			_shownData = rows == null || rows.Count() == 0 ? null : rows.CopyToDataTable();
			UpdateDGV();
			kolonaVrednsot_cmb.SelectedIndex = 0;
		}

		private void filterStanjeOperacija_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterStanjeOperacija_cmb.SelectedIndex == 0)
				filterStanjeUslov_cmb.SelectedIndex = 0;
		}

		private void filterStanjeUslov_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (filterStanjeUslov_cmb.SelectedIndex == 0)
				filterStanjeOperacija_cmb.SelectedIndex = 0;
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			foreach (DataRow dr in _shownData.Rows)
			{
				double vrednost = 0;

				switch (kolonaVrednsot_cmb.SelectedIndex)
				{
					case 0:
						vrednost = (double)dr["Stanje"] * (double)dr["ProdajnaCena"];
						dr["Valuta"] = "RSD";
						break;
					case 1:
						vrednost = (double)dr["Stanje"] - (double)dr["IzasloMes"];
						dr["Valuta"] = dr["JM"].ToString();
						break;
					case 2:
						vrednost = (double)dr["Stanje"] - (double)dr["OptZal"];
						dr["Valuta"] = dr["JM"].ToString();
						break;
					case 3:
						vrednost = (double)dr["OptZal"] - (double)dr["IzasloMes"];
						dr["Valuta"] = dr["JM"].ToString();
						break;
					case 4:
						vrednost = (double)dr["ProdatoMes"] * (double)dr["ProdajnaCena"];
						dr["Valuta"] = "RSD";
						break;
					case 5:
						vrednost = (double)dr["Stanje"] - (double)dr["ProdatoMes"];
						dr["Valuta"] = dr["JM"].ToString();
						break;
					case 6:
						vrednost = (double)dr["OptZal"] - (double)dr["ProdatoMes"];
						dr["Valuta"] = dr["JM"].ToString();
						break;
					case 7:
						vrednost = (double)dr["VisakZaliha"] * (double)dr["ProdajnaCena"];
						dr["Valuta"] = "RSD";
						break;
					default:
						vrednost = 0;
						break;
				}

				dr["Vrednost"] = vrednost;
			}

			switch (kolonaVrednsot_cmb.SelectedIndex)
			{
				case 0:
					dataGridView1.Columns["Vrednost"].HeaderText =
						"Vrednost (Stanje * Prodajna Cena)";
					break;
				case 1:
					dataGridView1.Columns["Vrednost"].HeaderText =
						"Vrednost (Stanje - Izaslo mesecno)";
					break;
				case 2:
					dataGridView1.Columns["Vrednost"].HeaderText = "Vrednost (Stanje - Opt Zalihe)";
					break;
				case 3:
					dataGridView1.Columns["Vrednost"].HeaderText =
						"Vrednost (Opt Zalihe - Izaslo Mesecno)";
					break;
				case 4:
					dataGridView1.Columns["Vrednost"].HeaderText =
						"Vrednost (Prodato Mesecno * Prodajna cena)";
					break;
				case 5:
					dataGridView1.Columns["Vrednost"].HeaderText =
						"Vrednost (Stanje - Prodato Mesecno)";
					break;
				case 6:
					dataGridView1.Columns["Vrednost"].HeaderText =
						"Vrednost (Opt Zalihe - Prodato Mesecno)";
					break;
				case 7:
					dataGridView1.Columns["Vrednost"].HeaderText =
						"Vrednost (Visak Zaliha * Prodajna Cena)";
					break;
				default:
					MessageBox.Show("Greska pri imenovanju kolone vrednsot");
					break;
			}
		}

		private void listaRobe_btn_Click(object sender, EventArgs e)
		{
			using (fm_ListaRobe lr = new fm_ListaRobe())
			{
				lr.robaUListi = _listaRobe == null ? new List<int>() : _listaRobe;
				lr.MagacinID = _trenutniMagacin;
				lr.ShowDialog();
				_listaRobe = new List<int>(lr.robaUListi);
				listaRobe_btn.Text = $"Lista Robe ({_listaRobe.Count})";
			}
		}

		private void fm_KontrolaLagera_Index_Shown(object sender, EventArgs e)
		{
			if (_prikaziOdmah)
				prikazi_btn.PerformClick();
		}
	}
}
