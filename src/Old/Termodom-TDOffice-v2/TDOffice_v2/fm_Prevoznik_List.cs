using System;
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
	public partial class fm_Prevoznik_List : Form
	{
		public class PartnerSelectEventtArgs
		{
			public int IDPrevoznika { get; set; }
		}

		private delegate void PartnerSelectEvent(PartnerSelectEventtArgs args);
		private PartnerSelectEvent OnPartnerSelect { get; set; }

		private Task<List<TDOffice.Partner>> _tdOfficePartneri = TDOffice.Partner.ListAsync();
		private Task<TDOffice.Config<List<int>>> _prevoznici = TDOffice.Config<List<int>>.GetAsync(
			(int)TDOffice.ConfigParameter.ListaIDevaPrevoznika
		);

		public fm_Prevoznik_List()
		{
			InitializeComponent();
			dataGridView1.Enabled = false;
			nova_btn.Enabled = false;
			status_lbl.Text = "Ucitavanje partnera...";
		}

		private void fm_Prevoznik_List_Load(object sender, EventArgs e)
		{
			OsveziDGVAsync();
		}

		private Task OsveziDGVAsync()
		{
			return Task.Run(() =>
			{
				Task.WaitAll(_prevoznici, _tdOfficePartneri);

				DataTable dt = new DataTable();
				dt.Columns.Add("ID", typeof(int));
				dt.Columns.Add("Naziv", typeof(string));
				dt.Columns.Add("Kontakt", typeof(string));

				if (_prevoznici.Result != null && _prevoznici.Result.Tag != null)
				{
					foreach (int partnerID in _prevoznici.Result.Tag)
					{
						TDOffice.Partner p = _tdOfficePartneri.Result.FirstOrDefault(x =>
							x.ID == partnerID
						);

						if (p != null)
						{
							DataRow dr = dt.NewRow();
							dr["ID"] = p.ID;
							dr["Naziv"] = p.Naziv;
							dr["Kontakt"] = p.Mobilni;
							dt.Rows.Add(dr);
						}
					}
				}

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							dataGridView1.DataSource = dt;

							//dataGridView1.Columns["ID"].Visible = false;
							dataGridView1.Columns["Naziv"].AutoSizeMode =
								DataGridViewAutoSizeColumnMode.AllCells;
							dataGridView1.Columns["Kontakt"].AutoSizeMode =
								DataGridViewAutoSizeColumnMode.AllCells;
							status_lbl.Text = "";

							nova_btn.Enabled = true;
							dataGridView1.Enabled = true;
						}
				);
			});
		}

		private void nova_btn_Click(object sender, EventArgs e)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("ID", typeof(int));
			dt.Columns.Add("Naziv", typeof(string));
			dt.Columns.Add("Mobilni", typeof(string));

			foreach (TDOffice.Partner p in _tdOfficePartneri.Result)
			{
				DataRow dr = dt.NewRow();
				dr["ID"] = p.ID;
				dr["Naziv"] = p.Naziv;
				dr["Mobilni"] = p.Mobilni;
				dt.Rows.Add(dr);
			}

			using (DataGridViewSelectBox sb = new DataGridViewSelectBox(dt))
			{
				sb.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				sb.RowHeaderVisible = false;
				sb.CloseOnSelect = true;
				sb.Text = "Izbor novog prevoznika";
				sb.OnRowSelected += OnRowSelectedHanlder;
				sb.ShowDialog();
			}
		}

		private void OnRowSelectedHanlder(DataGridViewSelectBox.RowSelectEventArgs args)
		{
			DataRow row = args.SelectedRow;

			if (row == null)
			{
				MessageBox.Show("Greska!");
				return;
			}

			int idPartnera = Convert.ToInt32(row["ID"]);

			if (_prevoznici.Result.Tag == null)
				_prevoznici.Result.Tag = new List<int>();

			if (_prevoznici.Result.Tag.Contains(idPartnera))
			{
				MessageBox.Show("Ovaj partner je vec prevoznik!");
				return;
			}

			_prevoznici.Result.Tag.Add(idPartnera);

			_prevoznici.Result.UpdateOrInsert();

			OsveziDGVAsync();

			MessageBox.Show("Partner uspesno dodat u prevoznike!");
		}

		private void ukloniIzPrevoznikaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				int idPartnera = Convert.ToInt32(
					dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
				);

				_prevoznici.Result.Tag.RemoveAll(x => x == idPartnera);
				_prevoznici.Result.UpdateOrInsert();

				OsveziDGVAsync();

				MessageBox.Show("Partner uspesno uklonjen iz prevoznika!");
			}
			catch
			{
				MessageBox.Show("Greska!");
			}
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			try
			{
				int idPartnera = Convert.ToInt32(
					dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
				);

				OnPartnerSelect(new PartnerSelectEventtArgs() { IDPrevoznika = idPartnera });
			}
			catch
			{
				MessageBox.Show("Greska!");
			}
		}
	}
}
