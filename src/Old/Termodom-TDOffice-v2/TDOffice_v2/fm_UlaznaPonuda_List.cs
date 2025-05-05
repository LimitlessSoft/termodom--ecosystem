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
	public partial class fm_UlaznaPonuda_List : Form
	{
		private Task<List<TDOffice.User>> _korisnici = TDOffice.User.ListAsync();

		public fm_UlaznaPonuda_List()
		{
			if (!Program.TrenutniKorisnik.ImaPravo(136580))
			{
				TDOffice.Pravo.NematePravoObavestenje(136580);
				this.Close();
				return;
			}

			InitializeComponent();

			odDatuma_dtp.Enabled = false;
			doDatuma_dtp.Enabled = false;

			odDatuma_dtp.Value = DateTime.Now;
			doDatuma_dtp.Value = DateTime.Now;

			UcitajDokumenta();

			odDatuma_dtp.Enabled = true;
			doDatuma_dtp.Enabled = true;

			UcitajDokumenta();
		}

		private void fm_UlaznaPonuda_List_Load(object sender, EventArgs e) { }

		private void UcitajDokumenta()
		{
			List<TDOffice.DokumentUlaznaPonuda> list = TDOffice.DokumentUlaznaPonuda.List(
				$"DATUM > '{odDatuma_dtp.Value.Date.ToString("dd.MM.yyyy")}' AND DATUM < '{doDatuma_dtp.Value.Date.AddDays(1).ToString("dd.MM.yyyy")}'"
			);

			DataTable dt = new DataTable();
			dt.Columns.Add("ID", typeof(int));
			dt.Columns.Add("Datum", typeof(DateTime));
			dt.Columns.Add("KorisnikID", typeof(int));
			dt.Columns.Add("Korisnik", typeof(string));
			dt.Columns.Add("VaziOd", typeof(DateTime));
			dt.Columns.Add("VaziDo", typeof(DateTime));

			foreach (TDOffice.DokumentUlaznaPonuda u in list)
			{
				TDOffice.User referent = _korisnici.Result.FirstOrDefault(x => x.ID == u.Korisnik);

				DataRow dr = dt.NewRow();
				dr["ID"] = u.ID;
				dr["Datum"] = u.Datum;
				dr["KorisnikID"] = u.Korisnik;
				dr["Korisnik"] = referent.Username;
				dr["VaziOd"] = u.DatumPocetkaVazenja;
				dr["VaziDo"] = u.DatumKrajaVazenja;
				dt.Rows.Add(dr);
			}

			dataGridView1.DataSource = dt;

			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

			dataGridView1.Columns["KorisnikID"].Visible = false;
			dataGridView1.Columns["Datum"].DefaultCellStyle.Format = "dd.MM.yyyy";
			dataGridView1.Columns["VaziOd"].DefaultCellStyle.Format = "dd.MM.yyyy";
			dataGridView1.Columns["VaziDo"].DefaultCellStyle.Format = "dd.MM.yyyy";
		}

		private void nova_btn_Click(object sender, EventArgs e)
		{
			using (
				fm_UlaznaPonuda_Index p = new fm_UlaznaPonuda_Index(
					TDOffice.DokumentUlaznaPonuda.Insert(
						Program.TrenutniKorisnik.ID,
						DateTime.Now,
						DateTime.Now.AddMonths(3)
					)
				)
			)
				if (!p.IsDisposed)
					p.ShowDialog();

			UcitajDokumenta();
		}

		private void doDatuma_dtp_ValueChanged(object sender, EventArgs e)
		{
			if (!odDatuma_dtp.Enabled)
				return;

			UcitajDokumenta();
		}

		private void odDatuma_dtp_ValueChanged(object sender, EventArgs e)
		{
			if (!doDatuma_dtp.Enabled)
				return;

			UcitajDokumenta();
		}

		private void osvezi_btn_Click(object sender, EventArgs e)
		{
			UcitajDokumenta();
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int id = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
			);

			using (fm_UlaznaPonuda_Index p = new fm_UlaznaPonuda_Index(id))
				if (!p.IsDisposed)
					p.ShowDialog();

			UcitajDokumenta();
		}
	}
}
