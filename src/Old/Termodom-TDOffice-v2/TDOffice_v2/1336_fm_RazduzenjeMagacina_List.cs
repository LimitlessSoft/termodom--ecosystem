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
	public partial class _1336_fm_RazduzenjeMagacina_List : Form
	{
		private DataTable _sviDokumenti = null;
		private DataTable _dgvDT = null;
		private bool _loaded = false;
		private Task<fm_Help> _helpFrom { get; set; }

		private Task<List<Komercijalno.Magacin>> _magacini = Task.Run(async () =>
		{
			List<Komercijalno.Magacin> magacini = await Komercijalno.Magacin.ListAsync();
			magacini.Add(new Komercijalno.Magacin() { ID = -1, Naziv = "Svi magacini" });

			return magacini.OrderBy(x => x.ID).ToList();
		});
		private Task<List<TDOffice.User>> _korisnici = Task.Run(() =>
		{
			return TDOffice.User.List();
		});

		public _1336_fm_RazduzenjeMagacina_List()
		{
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.RazduzenjeMagacina_List);
			panel2.DesniKlik_DatumRange(odDatuma_dtp_CloseUp);
		}

		private void _1336_fm_RazduzenjeMagacina_List_Load(object sender, EventArgs e)
		{
			SetUI();
			SetDGV();
			UcitajRazduzenja();
			PopulateDGV();
			_loaded = true;
		}

		private void SetUI()
		{
			magacin_cmb.DataSource = _magacini.Result;
			magacin_cmb.DisplayMember = "Naziv";
			magacin_cmb.ValueMember = "ID";

			magacin_cmb.SelectedValue = Program.TrenutniKorisnik.MagacinID;
			magacin_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(133604);

			odDatuma_dtp.Value = DateTime.Now;
			doDatuma_dtp.Value = DateTime.Now;
		}

		private void SetDGV()
		{
			_sviDokumenti = new DataTable();
			_sviDokumenti.Columns.Add("ID", typeof(int));
			_sviDokumenti.Columns.Add("Datum", typeof(DateTime));
			_sviDokumenti.Columns.Add("MagacinID", typeof(int));
			_sviDokumenti.Columns.Add("Magacin", typeof(string));
			_sviDokumenti.Columns.Add("KorisnikID", typeof(int));
			_sviDokumenti.Columns.Add("Referent", typeof(string));
			_sviDokumenti.Columns.Add("Status", typeof(int));
			_sviDokumenti.Columns.Add("Komentar", typeof(string));
			_sviDokumenti.Columns.Add("InterniKomentar", typeof(string));

			_dgvDT = _sviDokumenti;
			dataGridView1.DataSource = _dgvDT;

			dataGridView1.Columns["ID"].Visible = false;

			dataGridView1.Columns["Datum"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dataGridView1.Columns["Datum"].DefaultCellStyle.Format = "dd.MM.yyyy";

			dataGridView1.Columns["Magacin"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

			dataGridView1.Columns["MagacinID"].Visible = false;

			dataGridView1.Columns["Referent"].AutoSizeMode =
				DataGridViewAutoSizeColumnMode.AllCells;

			dataGridView1.Columns["KorisnikID"].Visible = false;

			dataGridView1.Columns["Status"].Visible = false;

			dataGridView1.Columns["Komentar"].AutoSizeMode =
				DataGridViewAutoSizeColumnMode.AllCells;
			dataGridView1.Columns["Komentar"].Visible = false;

			dataGridView1.Columns["InterniKomentar"].AutoSizeMode =
				DataGridViewAutoSizeColumnMode.AllCells;
			dataGridView1.Columns["InterniKomentar"].Visible = false;
		}

		private void PopulateDGV()
		{
			DateTime pocetak = odDatuma_dtp.Value;
			DateTime kraj = doDatuma_dtp.Value;

			string _sqlWhere =
				"Datum > '"
				+ new DateTime(pocetak.Year, pocetak.Month, pocetak.Day, 0, 0, 0)
				+ "' AND Datum < '"
				+ new DateTime(kraj.Year, kraj.Month, kraj.Day, 23, 59, 59)
				+ "'";

			if ((int)magacin_cmb.SelectedValue != -1)
				_sqlWhere += " AND MAGACINID = '" + (int)magacin_cmb.SelectedValue + "'";

			string _sqlOrder = "ID ASC";

			DataRow[] rows = _sviDokumenti.Select(_sqlWhere, _sqlOrder);

			if (rows != null && rows.Length > 0)
				_dgvDT = rows.CopyToDataTable();
			else
				_dgvDT = _sviDokumenti.Clone();

			dataGridView1.DataSource = _dgvDT;

			foreach (DataGridViewRow r in dataGridView1.Rows)
			{
				if (Convert.ToInt32(r.Cells["Status"].Value) == 0)
				{
					r.DefaultCellStyle.ForeColor = Color.Black;
					r.DefaultCellStyle.Font = new Font(
						dataGridView1.DefaultCellStyle.Font,
						FontStyle.Regular
					);
				}
				else if (Convert.ToInt32(r.Cells["Status"].Value) == 1)
				{
					r.DefaultCellStyle.ForeColor = Color.Red;
					r.DefaultCellStyle.Font = new Font(
						dataGridView1.DefaultCellStyle.Font,
						FontStyle.Regular
					);
				}
				else if (Convert.ToInt32(r.Cells["Status"].Value) == 2)
				{
					r.DefaultCellStyle.ForeColor = Color.OrangeRed;
					r.DefaultCellStyle.Font = new Font(
						dataGridView1.DefaultCellStyle.Font,
						FontStyle.Bold
					);
				}
			}
		}

		private void UcitajRazduzenja()
		{
			int magID = Convert.ToInt32(magacin_cmb.SelectedValue);

			List<TDOffice.DokumentRazduzenjaMagacina> dokumenti =
				magID == -1
					? TDOffice.DokumentRazduzenjaMagacina.List()
					: TDOffice.DokumentRazduzenjaMagacina.ListByMagacinID(magID);

			if (_sviDokumenti != null)
				_sviDokumenti.Clear();

			foreach (TDOffice.DokumentRazduzenjaMagacina drz in dokumenti)
			{
				Komercijalno.Magacin m = _magacini
					.Result.Where(x => x.ID == drz.MagacinID)
					.FirstOrDefault();
				TDOffice.User k = _korisnici
					.Result.Where(x => x.ID == drz.KorisnikID)
					.FirstOrDefault();

				DataRow dr = _sviDokumenti.NewRow();
				dr["ID"] = drz.ID;
				dr["Datum"] = drz.Datum;
				dr["MagacinID"] = drz.MagacinID;
				dr["Magacin"] = m == null ? "Undefined" : m.Naziv;
				dr["Referent"] = k.Username;
				dr["Status"] = (int)drz.Status;
				dr["Komentar"] = drz.Komentar;
				dr["InterniKomentar"] = drz.InterniKomentar;

				_sviDokumenti.Rows.Add(dr);
			}
		}

		private void nova_btn_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(133601))
			{
				TDOffice.Pravo.NematePravoObavestenje(133601);
				return;
			}

			int magID = Convert.ToInt32(magacin_cmb.SelectedValue);

			if (magID < 0)
			{
				MessageBox.Show("Neispravan magacin!");
				return;
			}

			int newID = TDOffice.DokumentRazduzenjaMagacina.Insert(
				magID,
				DateTime.Now,
				Program.TrenutniKorisnik.ID,
				TDOffice.DokumentRazduzenjaMagacinaStatus.Otkljucan
			);

			using (
				_1336_fm_RazduzenjeMagacina_Index rm = new _1336_fm_RazduzenjeMagacina_Index(
					TDOffice.DokumentRazduzenjaMagacina.Get(newID)
				)
			)
				rm.ShowDialog();

			UcitajRazduzenja();
			PopulateDGV();
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int brDok = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

			TDOffice.DokumentRazduzenjaMagacina dokument = TDOffice.DokumentRazduzenjaMagacina.Get(
				brDok
			);

			using (
				_1336_fm_RazduzenjeMagacina_Index rm = new _1336_fm_RazduzenjeMagacina_Index(
					dokument
				)
			)
			{
				rm.ShowDialog();

				UcitajRazduzenja();
				PopulateDGV();
			}
		}

		private void odDatuma_dtp_CloseUp(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			PopulateDGV();
		}

		private void doDatuma_dtp_CloseUp(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			PopulateDGV();
		}

		private void magacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			UcitajRazduzenja();
			PopulateDGV();
		}
	}
}
