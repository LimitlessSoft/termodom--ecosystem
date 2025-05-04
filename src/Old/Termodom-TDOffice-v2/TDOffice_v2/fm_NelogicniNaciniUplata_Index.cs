using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
	public partial class fm_NelogicniNaciniUplata_Index : Form
	{
		private Task<List<Komercijalno.Dokument>> _dokument { get; set; } =
			Komercijalno.Dokument.ListAsync(null, new int[] { 15 });
		private Task<List<Komercijalno.IstUpl>> _istUplate { get; set; } =
			Komercijalno.IstUpl.ListAsync("VRDOK = 15");

		private bool _loaded = false;
		private DataTable _sveNelogicneUplate;

		public fm_NelogicniNaciniUplata_Index()
		{
			InitializeComponent();

			_loaded = true;
			for (int i = 0; i < clb_Filter.Items.Count; i++)
				clb_Filter.SetItemChecked(i, true);
		}

		private void btn_ProveriNelogicnostiNacinaPlacanja_Click(object sender, EventArgs e)
		{
			slogova_lbl.Text = "Ucitavanje...";
			this.Enabled = false;

			Task.Run(() =>
			{
				double nStep = _dokument.Result.Count;
				double cStep = 0;
				DataTable dt = new DataTable();
				dt.Columns.Add("NUID", typeof(string));
				dt.Columns.Add("NUID_Dokumenta", typeof(int));
				dt.Columns.Add("Iznos", typeof(double));
				dt.Columns.Add("IznosDokumenta", typeof(double));
				dt.Columns.Add("BrDok", typeof(int));
				dt.Columns.Add("VrDok", typeof(int));
				dt.Columns.Add("NacinPlacanjaDokumenta", typeof(string));
				dt.Columns.Add("NacinPlacanjaKupac", typeof(string));
				dt.Columns.Add("PPID", typeof(int));
				dt.Columns.Add("MAGACINID", typeof(int));
				dt.Columns.Add("Partner", typeof(string));
				dt.Columns.Add("Magacin", typeof(string));

				List<Komercijalno.Partner> par = Komercijalno
					.Partner.ListAsync(DateTime.Now.Year)
					.Result;
				List<Komercijalno.Magacin> mag = Komercijalno
					.Magacin.ListAsync(DateTime.Now.Year)
					.Result;

				ConcurrentBag<DataDTO> bag = new ConcurrentBag<DataDTO>();

				Parallel.ForEach(
					_dokument.Result,
					d =>
					{
						double p = 0;
						cStep++;

						this.Invoke(
							(MethodInvoker)
								delegate
								{
									slogova_lbl.Text = $"{cStep}/{nStep}";
								}
						);

						if (d.Potrazuje == 0)
							return;

						List<Komercijalno.IstUpl> istUpls = _istUplate
							.Result.Where(x => x.BrDok == d.BrDok)
							.ToList();
						List<string> naciniPlacanjaText = new List<string>();

						if (istUpls.Select(x => x.NUID).Count() == 0)
							naciniPlacanjaText.Add("Bez uplata!");
						else
							foreach (int nu in istUpls.Select(x => x.NUID).Distinct())
								naciniPlacanjaText.Add(((Komercijalno.NacinUplate)nu).ToString());

						DataDTO dto = new DataDTO();
						p = 0;
						p = istUpls.Sum(x => x.Iznos);
						dto.NUID = string.Join(", ", istUpls.Select(x => x.NUID));
						dto.DokumentNUID = Convert.ToInt32(d.NUID);
						dto.Iznos = p;
						dto.DokumentIznos = d.Potrazuje;
						dto.BrDok = d.BrDok;
						dto.VrDok = d.VrDok;
						dto.NacinPlacanjaDokumenta = ((Komercijalno.NacinUplate)d.NUID).ToString();
						dto.NacinPlacanjaKupac = string.Join(", ", naciniPlacanjaText);
						dto.MagacinID = d.MagacinID;
						dto.PPID = d.PPID is null ? 0 : (int)d.PPID;
						dto.Partner = d.PPID is null
							? ""
							: par.FirstOrDefault(x => x.PPID == d.PPID).Naziv;
						dto.Magacin = mag.FirstOrDefault(x => x.ID == d.MagacinID).Naziv;
						bag.Add(dto);
					}
				);

				cStep = 0;
				nStep = bag.Count;
				foreach (DataDTO dto in bag)
				{
					cStep++;

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								slogova_lbl.Text = $"{cStep}/{nStep}";
							}
					);
					DataRow dr = dt.NewRow();
					dr["NUID"] = dto.NUID;
					dr["NUID_Dokumenta"] = dto.DokumentNUID;
					dr["Iznos"] = dto.Iznos;
					dr["IznosDokumenta"] = dto.DokumentIznos;
					dr["BrDok"] = dto.BrDok;
					dr["VrDok"] = dto.VrDok;
					dr["NacinPlacanjaDokumenta"] = dto.NacinPlacanjaDokumenta;
					dr["NacinPlacanjaKupac"] = dto.NacinPlacanjaKupac;
					dr["MAGACINID"] = dto.MagacinID;
					dr["PPID"] = dto.PPID;
					dr["Partner"] = dto.Partner;
					dr["Magacin"] = dto.Magacin;
					dt.Rows.Add(dr);
				}
				_sveNelogicneUplate = dt;

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							slogova_lbl.Text = "Slogova: " + dataGridView1.Rows.Count;
							dataGridView1.DataSource = _sveNelogicneUplate;
							dataGridView1.Columns["Iznos"].DefaultCellStyle.Format = "#,##0.00 RSD";
							dataGridView1.Columns["Iznos"].DefaultCellStyle.Alignment =
								DataGridViewContentAlignment.MiddleRight;
							dataGridView1.Columns["Iznos"].HeaderText = "Zbir uplata za dokument";
							dataGridView1.Columns["Iznos"].Width = 100;
							dataGridView1.Columns["IznosDokumenta"].DefaultCellStyle.Format =
								"#,##0.00 RSD";
							dataGridView1.Columns["IznosDokumenta"].DefaultCellStyle.Alignment =
								DataGridViewContentAlignment.MiddleRight;
							dataGridView1.Columns["IznosDokumenta"].HeaderText =
								"Vrednost dokumenta";
							dataGridView1.Columns["IznosDokumenta"].Width = 100;
							dataGridView1.Columns["NacinPlacanjaDokumenta"].HeaderText =
								"Nacin placanja Dokument";
							dataGridView1.Columns["NacinPlacanjaDokumenta"].Width = 100;
							dataGridView1.Columns["VrDok"].Visible = false;
							dataGridView1.Columns["NUID"].Visible = false;
							dataGridView1.Columns["NUID_Dokumenta"].Visible = false;
							dataGridView1.Columns["MAGACINID"].Visible = false;
							dataGridView1.Columns["PPID"].Visible = false;
							dataGridView1.Columns["Partner"].Width = 200;
							dataGridView1.Columns["Magacin"].Width = 200;
							dataGridView1.Sort(
								dataGridView1.Columns["BrDok"],
								ListSortDirection.Descending
							);
							this.Enabled = true;
							button1.PerformClick();
						}
				);
				slogova_lbl.Text = "Ukupno slogova:" + dataGridView1.Rows.Count.ToString();
			});
		}

		private void button1_Click(object sender, EventArgs e)
		{
			try
			{
				DataTable dt = _sveNelogicneUplate.Copy();

				for (int y = dt.Rows.Count - 1; y >= 0; y--)
				{
					int nuDok = Convert.ToInt32(dt.Rows[y]["NUID_Dokumenta"]);
					int[] nups = string.IsNullOrWhiteSpace(dt.Rows[y]["NUID"].ToString())
						? null
						: dt.Rows[y]["NUID"]
							.ToString()
							.Replace(" ", String.Empty)
							.Split(',')
							.Select(x => Convert.ToInt32(x))
							.ToArray();

					if (clb_Filter.GetItemChecked(0))
						if (nups != null && nups.Where(x => x == nuDok).Count() != nups.Length)
							continue;

					if (clb_Filter.GetItemChecked(2))
					{
						if (
							nuDok == (int)Komercijalno.NacinUplate.Virman
							&& Convert.ToDouble(dt.Rows[y]["Iznos"])
								< Convert.ToDouble(dt.Rows[y]["IznosDokumenta"])
							&& nups != null
							&& nups.Where(x => x == 1).Count() == nups.Length
						)
							continue;
					}

					int iznosDokumenta = (int)(
						Convert.ToDouble(dt.Rows[y]["IznosDokumenta"]) * 100
					);
					int zbirIstUpl = (int)(Convert.ToDouble(dt.Rows[y]["Iznos"]) * 100);

					if (clb_Filter.GetItemChecked(1))
					{
						if (
							nuDok == 1
							&& !clb_Filter.GetItemChecked(2)
							&& zbirIstUpl < iznosDokumenta
						) { }
						else if (iznosDokumenta != zbirIstUpl)
							continue;
					}

					dt.Rows.RemoveAt(y);
				}

				dataGridView1.DataSource = dt;
				slogova_lbl.Text = "Ukupno slogova:" + dataGridView1.Rows.Count.ToString();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private class DataDTO
		{
			public string NUID { get; set; }
			public int DokumentNUID { get; set; }
			public double Iznos { get; set; }
			public double DokumentIznos { get; set; }
			public int BrDok { get; set; }
			public int VrDok { get; set; }
			public string NacinPlacanjaDokumenta { get; set; }
			public string NacinPlacanjaKupac { get; set; }
			public int PPID { get; set; }
			public int MagacinID { get; set; }
			public string Partner { get; set; }
			public string Magacin { get; set; }
		}
	}
}
