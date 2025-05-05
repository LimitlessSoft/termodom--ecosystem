using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using TDOffice_v2.Komercijalno;
using TDOffice_v2.TDOffice;
using Termodom.Data.Entities.Komercijalno;
using Termodom.Data.Entities.TDOffice_v2;

namespace TDOffice_v2
{
	public partial class fm_StanjeRacuna_Index : Form
	{
		private Task<FirmaDictionary> _firme { get; set; } = FirmaManager.DictionaryAsync();
		private Task<BankaDictionary> _banke { get; set; } = BankaManager.DictionaryAsync();

		public fm_StanjeRacuna_Index()
		{
			InitializeComponent();
			status_lbl.Text = "Initialization...";
			checkedListBox1.Enabled = false;
			dataGridView1.Enabled = false;
			godina_cmb.Enabled = false;

			for (int i = 0; i <= 5; i++)
				godina_cmb.Items.Add(DateTime.Now.AddYears(-i).Year.ToString());

			godina_cmb.SelectedIndex = 0;
		}

		private void fm_StanjeRacuna_Index_Load(object sender, EventArgs e)
		{
			_firme.ContinueWith(
				async (prev) =>
				{
					await UcitajPodatkeAsync();
				}
			);

			_firme.ContinueWith(
				async (prev) =>
				{
					try
					{
						FirmaDictionary firme = await _firme;

						List<Tuple<string, int>> list = new List<Tuple<string, int>>();

						foreach (Firma f in firme.Values)
							list.Add(new Tuple<string, int>(f.Naziv, f.ID));

						this.Invoke(
							(MethodInvoker)
								delegate
								{
									checkedListBox1.DataSource = list;
									checkedListBox1.DisplayMember = "Item1";
									checkedListBox1.ValueMember = "Item2";

									for (int i = 0; i < checkedListBox1.Items.Count; i++)
										checkedListBox1.SetItemCheckState(i, CheckState.Checked);
								}
						);
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message);
					}
				}
			);
		}

		private async Task UcitajPodatkeAsync()
		{
			try
			{
				int godina = DateTime.Now.Year;
				DateTime dt1 = DateTime.UtcNow;
				DateTime dt2;
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							status_lbl.Text = "Ucitavanje podataka...";
							checkedListBox1.Enabled = false;
							dataGridView1.Enabled = false;
							godina_cmb.Enabled = false;
							godina = Convert.ToInt32(godina_cmb.SelectedItem);
						}
				);

				DataTable dt = new DataTable();
				dt.Columns.Add("FirmaId", typeof(int));
				dt.Columns.Add("Firma", typeof(string));
				dt.Columns.Add("Banka", typeof(string));
				dt.Columns.Add("Racun", typeof(string));
				dt.Columns.Add("Valuta", typeof(string));
				dt.Columns.Add("Poc. stanje Duguje", typeof(double));
				dt.Columns.Add("Poc. stanje Potrazuje", typeof(double));
				dt.Columns.Add("Duguje", typeof(double));
				dt.Columns.Add("Potrazuje", typeof(double));
				dt.Columns.Add("Stanje (iz baze)", typeof(double));
				dt.Columns.Add("Stanje (izracunato)", typeof(double));
				dt.Columns.Add("Nezakljucanih izvoda", typeof(int));

				FirmaDictionary firme = await _firme;
				BankaDictionary banke = await _banke;

				foreach (Firma f in firme.Values)
				{
					TekuciRacunList tekuciRacuni = await Komercijalno.TekuciRacun.ListAsync(
						f.GlavniMagacin,
						godina
					);
					DokumentDictionary dokumenti =
						await Komercijalno.DokumentManager.DictionaryAsync(
							f.GlavniMagacin,
							godina,
							new int[] { 90 }
						);
					foreach (
						Termodom.Data.Entities.Komercijalno.TekuciRacun tr in tekuciRacuni.Where(
							x => x.PPID == f.PPID
						)
					)
					{
						StanjeRacuna stanjeRacuna = await StanjeRacunaManager.GetAsync(
							f.GlavniMagacin,
							godina,
							tr.Racun
						);
						double duguje = await IzvodManager.DugujeSumAsync(
							f.GlavniMagacin,
							godina,
							tr.Racun
						);
						double potrazuje = await IzvodManager.PotrazujeSumAsync(
							f.GlavniMagacin,
							godina,
							tr.Racun
						);
						DataRow dr = dt.NewRow();
						dr["FirmaId"] = f.ID;
						dr["Firma"] = f.Naziv;
						dr["Banka"] = banke[tr.BankaID].Naziv;
						dr["Racun"] = tr.Racun;
						dr["Valuta"] = tr.Valuta;
						dr["Poc. stanje Duguje"] =
							stanjeRacuna == null ? 0 : stanjeRacuna.PocDuguje;
						dr["Poc. stanje Potrazuje"] =
							stanjeRacuna == null ? 0 : stanjeRacuna.PocPotrazuje;
						dr["Duguje"] = duguje;
						dr["Potrazuje"] = potrazuje;
						dr["Stanje (iz baze)"] = tr.Stanje;
						dr["Stanje (izracunato)"] =
							((stanjeRacuna == null ? 0 : stanjeRacuna.PocPotrazuje) + potrazuje)
							- ((stanjeRacuna == null ? 0 : stanjeRacuna.PocDuguje) + duguje);
						dr["Nezakljucanih izvoda"] = dokumenti.ContainsKey(90)
							? dokumenti[90].Values.Count(x => x.OpisUpl == tr.Racun && x.Flag == 0)
							: 0;
						dt.Rows.Add(dr);
					}
				}

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							dataGridView1.DataSource = dt;
							dataGridView1.AutoSizeColumnsMode =
								DataGridViewAutoSizeColumnsMode.AllCells;

							dataGridView1.Columns["FirmaId"].Visible = false;

							dataGridView1.Columns["Poc. stanje Duguje"].DefaultCellStyle.Format =
								"#,##0.00";
							dataGridView1.Columns["Poc. stanje Duguje"].DefaultCellStyle.Alignment =
								DataGridViewContentAlignment.MiddleRight;

							dataGridView1.Columns["Poc. stanje Potrazuje"].DefaultCellStyle.Format =
								"#,##0.00";
							dataGridView1
								.Columns["Poc. stanje Potrazuje"]
								.DefaultCellStyle
								.Alignment = DataGridViewContentAlignment.MiddleRight;

							dataGridView1.Columns["Duguje"].DefaultCellStyle.Format = "#,##0.00";
							dataGridView1.Columns["Duguje"].DefaultCellStyle.Alignment =
								DataGridViewContentAlignment.MiddleRight;

							dataGridView1.Columns["Potrazuje"].DefaultCellStyle.Format = "#,##0.00";
							dataGridView1.Columns["Potrazuje"].DefaultCellStyle.Alignment =
								DataGridViewContentAlignment.MiddleRight;

							dataGridView1.Columns["Stanje (iz baze)"].DefaultCellStyle.Format =
								"#,##0.00";
							dataGridView1.Columns["Stanje (iz baze)"].DefaultCellStyle.Alignment =
								DataGridViewContentAlignment.MiddleRight;

							dataGridView1.Columns["Stanje (izracunato)"].DefaultCellStyle.Format =
								"#,##0.00";
							dataGridView1
								.Columns["Stanje (izracunato)"]
								.DefaultCellStyle
								.Alignment = DataGridViewContentAlignment.MiddleRight;

							checkedListBox1.Enabled = true;
							dataGridView1.Enabled = true;
							godina_cmb.Enabled = true;

							status_lbl.Text = "Podaci ucitani!";
							dt2 = DateTime.UtcNow;

							MessageBox.Show((dt2 - dt1).TotalSeconds.ToString());
						}
				);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!checkedListBox1.Enabled)
				return;

			dataGridView1.Visible = false;

			List<int> visibleFirmaIds = new List<int>();
			foreach (Tuple<string, int> item in checkedListBox1.CheckedItems)
				visibleFirmaIds.Add(item.Item2);

			for (int i = 0; i < dataGridView1.Rows.Count; i++)
				dataGridView1.Rows[i].Visible = visibleFirmaIds.Contains(
					Convert.ToInt32(dataGridView1.Rows[i].Cells["FirmaId"].Value)
				);

			dataGridView1.Visible = true;
		}

		private void godina_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!godina_cmb.Enabled)
				return;

			_ = UcitajPodatkeAsync();
		}
	}
}
