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

namespace TDOffice_v2
{
	public partial class fm_Partner_List : Form
	{
		private Task<List<TDOffice.Partner>> _tdOfficePartneri = TDOffice.Partner.ListAsync(
			"NAZIV NOT LIKE '%undefined%' AND NAZIV NOT LIKE '%BLOKIRAN%'"
		);
		private Task<List<TDOffice.Grad>> _tdGradovi = TDOffice.Grad.ListAsync();
		private Task<List<TDOffice.PartnerGrupa>> _partnerGrupe = TDOffice.PartnerGrupa.ListAsync();

		private Task<List<Komercijalno.Magacin>> _magaciniKomercijalno = Task.Run(() =>
		{
			List<Komercijalno.Magacin> list = new List<Komercijalno.Magacin>();
			list.AddRange(Komercijalno.Magacin.ListAsync().Result);
			list.Add(new Komercijalno.Magacin() { ID = -2, Naziv = "Greska" });
			list.Add(new Komercijalno.Magacin() { ID = -1, Naziv = "Nedefinisano" });
			list.Add(new Komercijalno.Magacin() { ID = -5, Naziv = "Dostava" });
			list.Sort((x, y) => x.ID.CompareTo(y.ID));
			return list;
		});
		private DataTable dt { get; set; } = new DataTable();

		public fm_Partner_List()
		{
			InitializeComponent();

			grupe_clb.DataSource = _partnerGrupe.Result;
			grupe_clb.DisplayMember = "Naziv";
			grupe_clb.ValueMember = "ID";

			magacin_clb.DataSource = _magaciniKomercijalno.Result;
			magacin_clb.DisplayMember = "Naziv";
			magacin_clb.ValueMember = "ID";

			for (int i = 0; i < grupe_clb.Items.Count; i++)
				grupe_clb.SetItemChecked(i, true);

			for (int i = 0; i < magacin_clb.Items.Count; i++)
				magacin_clb.SetItemChecked(i, true);

			toolStripStatusLabel1.Text = "Ucitavanje...";
			PrikaziPartnereAsync();
		}

		private void fm_Partner_List_Load(object sender, EventArgs e) { }

		private Task PrikaziPartnereAsync()
		{
			return Task.Run(() =>
			{
				try
				{
					Task.WaitAll(_tdOfficePartneri);

					dt = new DataTable();

					dt.Columns.Add("PPID", typeof(int));
					dt.Columns.Add("Naziv", typeof(string));
					dt.Columns.Add("Mobilni", typeof(string));
					dt.Columns.Add("MestoID", typeof(int));
					dt.Columns.Add("Mesto", typeof(string));
					dt.Columns.Add("Mejl", typeof(string));
					dt.Columns.Add("Komentar", typeof(string));
					dt.Columns.Add("Grupe", typeof(string));
					dt.Columns.Add("PripadaGrupama", typeof(string));
					dt.Columns.Add("MagacinID", typeof(int));
					dt.Columns.Add("Magacin", typeof(string));

					foreach (TDOffice.Partner p in _tdOfficePartneri.Result.OrderBy(x => x.ID))
					{
						Komercijalno.Magacin m =
							p.MagacinID == null
								? _magaciniKomercijalno.Result.FirstOrDefault(x => x.ID == -1)
								: _magaciniKomercijalno.Result.FirstOrDefault(x =>
									x.ID == (int)p.MagacinID
								);
						DataRow dr = dt.NewRow();
						dr["PPID"] = p.ID;
						dr["Naziv"] = p.Naziv;
						dr["Mobilni"] = MobileNumber.IsValid(p.Mobilni)
							? MobileNumber.Collate(p.Mobilni)
							: "";
						dr["MestoID"] = p.Grad;
						dr["Mesto"] = _tdGradovi.Result.FirstOrDefault(x => x.ID == p.Grad).Naziv;
						dr["Mejl"] = p.Mejl;
						dr["Komentar"] = p.Komentar;
						dr["Grupe"] = p.Grupe == null ? "" : string.Join(",", p.Grupe);
						dr["PripadaGrupama"] =
							p.Grupe == null
								? ""
								: string.Join(
									", ",
									_partnerGrupe
										.Result.Where(x => p.Grupe.Any(y => y == x.ID))
										.Select(x => x.Naziv)
								);
						dr["MagacinID"] = p.MagacinID == null ? -1 : (int)p.MagacinID;
						dr["Magacin"] = m == null ? "Greska" : m.Naziv;
						dt.Rows.Add(dr);
					}

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								this.dataGridView1.DataSource = dt;

								this.dataGridView1.Columns["PPID"].Width = 50;
								this.dataGridView1.Columns["Naziv"].Width = 300;
								this.dataGridView1.Columns["Mobilni"].Width = 100;
								this.dataGridView1.Columns["MestoID"].Visible = false;
								this.dataGridView1.Columns["Mesto"].Width = 150;
								this.dataGridView1.Columns["Mejl"].Width = 150;
								this.dataGridView1.Columns["Komentar"].Width = 500;
								this.dataGridView1.Columns["Grupe"].Visible = false;
								this.dataGridView1.Columns["MagacinID"].Visible = false;
								this.dataGridView1.Columns["PripadaGrupama"].Width = 500;
								this.dataGridView1.Columns["Magacin"].Width = 150;

								toolStripStatusLabel1.Text = "Ucitano!";
							}
					);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			});
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int partnerID = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["PPID"].Value
			);

			using (fm_Partner_Index pi = new fm_Partner_Index(partnerID))
			{
				pi.PartnerIzmenjen += () =>
				{
					_tdOfficePartneri = TDOffice.Partner.ListAsync(
						"NAZIV NOT LIKE '%undefined%' AND NAZIV NOT LIKE '%BLOKIRAN%'"
					);
					_tdOfficePartneri.Wait();
					PrikaziPartnereAsync();
				};
				pi.ShowDialog();
			}
		}

		private void nova_btn_Click(object sender, EventArgs e)
		{
			using (fm_Partner_Index pi = new fm_Partner_Index())
			{
				pi.NoviPartnerKreiran += () =>
				{
					_tdOfficePartneri = TDOffice.Partner.ListAsync(
						"NAZIV NOT LIKE '%undefined%' AND NAZIV NOT LIKE '%BLOKIRAN%'"
					);
					_tdOfficePartneri.Wait();
					PrikaziPartnereAsync();
				};
				pi.ShowDialog();
			}
		}

		private void ukloniPartneraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
			int partnerID = Convert.ToInt32(dataGridView1.Rows[rowIndex].Cells["PPID"].Value);
			TDOffice.Partner p = TDOffice.Partner.Get(partnerID);
			p.Naziv = "***undefined***";
			p.Update();
			_tdOfficePartneri = TDOffice.Partner.ListAsync(
				"NAZIV NOT LIKE '%undefined%' AND NAZIV NOT LIKE '%BLOKIRAN%'"
			);
			_tdOfficePartneri.Wait();
			PrikaziPartnereAsync();
			MessageBox.Show("Partner uspesno uklonjen!");
		}

		private void posaljiSMSPorukeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (
				InputBox ib = new InputBox(
					"Tekst SMS poruke",
					"Unesite tekst SMS poruke (max 140 karaktera). Ukoliko zelite odustati pritisnite 'odustani'."
				)
			)
			{
				ib.ShowDialog();
				if (ib.DialogResult != DialogResult.OK)
					return;

				if (string.IsNullOrWhiteSpace(ib.returnData) || ib.returnData.Length > 140)
				{
					MessageBox.Show("Neispravan tekst poruke!");
					return;
				}

				DataTable izlistani = (dataGridView1.DataSource as DataTable).Copy();

				int nPoslato = 0;
				try
				{
					for (int i = 0; i < izlistani.Rows.Count; i++)
					{
						SMSGateway.PosaljiSms(
							izlistani.Rows[i]["Mobilni"].ToString(),
							ib.returnData
						);
						nPoslato++;
					}

					MessageBox.Show("SMS poruke uspesno poslate!");
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						"Doslo je do greske prilikom komunikacije sa SMSGatewayom. Poslatih poruka: "
							+ nPoslato
					);
					MessageBox.Show(ex.ToString());
				}
			}
		}

		private void filtriraj_btn_Click(object sender, EventArgs e)
		{
			DataTable newDt = dt.Copy();

			List<string> cekiraneGrupe = grupe_clb
				.CheckedItems.OfType<TDOffice.PartnerGrupa>()
				.Select(x => x.ID.ToString())
				.ToList();

			for (int i = newDt.Rows.Count - 1; i >= 0; i--)
				if (
					cekiraneGrupe.Any(x => newDt.Rows[i]["Grupe"].ToString().Split(',').Contains(x))
				)
					continue;
				else
					newDt.Rows.RemoveAt(i);

			List<int> cekiraniMagacini = magacin_clb
				.CheckedItems.OfType<Komercijalno.Magacin>()
				.Select(x => x.ID)
				.ToList();

			for (int i = newDt.Rows.Count - 1; i >= 0; i--)
				if (cekiraniMagacini.Any(x => x == Convert.ToInt32(newDt.Rows[i]["MagacinID"])))
					continue;
				else
					newDt.Rows.RemoveAt(i);

			dataGridView1.DataSource = newDt;
		}
	}
}
