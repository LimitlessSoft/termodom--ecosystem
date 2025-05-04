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
	public partial class _1332_fm_Proracun_List : Form
	{
		private Task<FbConnection> _conTDOfficeTask { get; set; } =
			Task.Run(() =>
			{
				FbConnection con = new FbConnection(TDOffice.TDOffice.connectionString);
				con.Open();
				return con;
			});
		private FbConnection _conTDOffice
		{
			get => _conTDOfficeTask.Result;
		}
		private Task<FbConnection> _conKomercijalnoTask { get; set; } =
			Task.Run(() =>
			{
				FbConnection con = new FbConnection(
					Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				);
				con.Open();
				return con;
			});
		private FbConnection _conKomercijalno
		{
			get => _conKomercijalnoTask.Result;
		}

		private Dictionary<int, TDOffice.User> _korisnici { get; set; }
		private Task<fm_Help> _helpFrom { get; set; }

		private DataTable _sviDokumenti = null;
		private DataTable _dgvDT = null;
		private bool _uiLoaded = false;

		public _1332_fm_Proracun_List()
		{
			InitializeComponent();
			_korisnici = TDOffice.User.Dict(_conTDOffice);
			_helpFrom = this.InitializeHelpModulAsync(Modul.Proracun_List);
			panel2.DesniKlik_DatumRange(odDatuma_dtp_CloseUp);
		}

		private void _1332_fm_PredlogProracuna_List_Load(object sender, EventArgs e)
		{
			SetUI();
			SetDGV();
			UcitajProracune();
			PopulateDGV();
		}

		private void SetUI()
		{
			var magacini = Komercijalno.Magacin.List(_conKomercijalno);
			magacini.Add(new Komercijalno.Magacin() { ID = -1, Naziv = "Svi magacini" });

			magacin_cmb.DataSource = magacini.OrderBy(x => x.ID).ToList();
			magacin_cmb.DisplayMember = "Naziv";
			magacin_cmb.ValueMember = "ID";

			if (Program.TrenutniKorisnik.ImaPravo(133204))
			{
				magacin_cmb.SelectedValue = -1;
				magacin_cmb.Enabled = true;
			}
			else
			{
				magacin_cmb.SelectedValue = Program.TrenutniKorisnik.MagacinID;
				magacin_cmb.Enabled = false;
			}

			odDatuma_dtp.Value = DateTime.Now;
			doDatuma_dtp.Value = DateTime.Now;

			_uiLoaded = true;
		}

		private void SetDGV()
		{
			_sviDokumenti = new DataTable();
			_sviDokumenti.Columns.Add("BrDok", typeof(int));
			_sviDokumenti.Columns.Add("Datum", typeof(DateTime));
			_sviDokumenti.Columns.Add("MagacinID", typeof(int));
			_sviDokumenti.Columns.Add("Magacin", typeof(string));
			_sviDokumenti.Columns.Add("Referent", typeof(string));
			_sviDokumenti.Columns.Add("Komercijalno", typeof(int));
			_sviDokumenti.Columns.Add("Status", typeof(int));
			_sviDokumenti.Columns.Add("KomProracun", typeof(string));
			_sviDokumenti.Columns.Add("MPRacun", typeof(string));

			_dgvDT = _sviDokumenti;
			dataGridView1.DataSource = _dgvDT;

			dataGridView1.Columns["MagacinID"].Visible = false;
			dataGridView1.Columns["BrDok"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dataGridView1.Columns["Datum"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dataGridView1.Columns["Magacin"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			dataGridView1.Columns["Referent"].AutoSizeMode =
				DataGridViewAutoSizeColumnMode.AllCells;
			dataGridView1.Columns["Komercijalno"].Visible = false;
			dataGridView1.Columns["Status"].Visible = false;
			dataGridView1.Columns["KomProracun"].AutoSizeMode =
				DataGridViewAutoSizeColumnMode.AllCells;
			dataGridView1.Columns["MPRacun"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
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

			string _sqlOrder = "BrDok ASC";

			DataRow[] rows = _sviDokumenti.Select(_sqlWhere, _sqlOrder);
			if (rows != null && rows.Length > 0)
			{
				_dgvDT = rows.CopyToDataTable();
			}
			else
				_dgvDT = _sviDokumenti.Clone();

			dataGridView1.DataSource = _dgvDT;

			foreach (DataGridViewRow r in dataGridView1.Rows)
			{
				int status = Convert.ToInt32(r.Cells["Status"].Value);
				int komercijalno = Convert.ToInt32(r.Cells["Komercijalno"].Value);

				r.DefaultCellStyle.ForeColor = status == 0 ? Color.Green : Color.Red;

				r.DefaultCellStyle.ForeColor =
					status == 1 && komercijalno > 0 ? Color.Red : Color.FromArgb(255, 125, 25);
			}
		}

		private void UcitajProracune()
		{
			List<TDOffice.DokumentProracun> dokumenti = TDOffice.DokumentProracun.List(
				_conTDOffice
			);

			Task<Dictionary<int, List<Komercijalno.Dokument>>> komDokBuffer = Task.Run(() =>
			{
				Dictionary<int, List<Komercijalno.Dokument>> dict =
					new Dictionary<int, List<Komercijalno.Dokument>>();

				foreach (int godina in dokumenti.Select(x => x.Datum.Year).Distinct())
				{
					using (
						FbConnection con = new FbConnection(
							Komercijalno.Komercijalno.CONNECTION_STRING[godina]
						)
					)
					{
						con.Open();
						dict.Add(godina, Komercijalno.Dokument.List(con, "VRDOK = 32"));
					}
				}
				return dict;
			});

			Dictionary<int, Komercijalno.Magacin> magacini = Komercijalno.Magacin.Dict(
				_conKomercijalno
			);

			if (_sviDokumenti != null)
				_sviDokumenti.Clear();

			foreach (TDOffice.DokumentProracun p in dokumenti)
			{
				if (!magacini.ContainsKey(p.MagacinID))
				{
					Task.Run(() =>
					{
						MessageBox.Show(
							$"Greska prilikom ucitavanja magacina za dokument TD Proracuna broj {p.ID}, magacin {p.MagacinID}"
						);
					});
					continue;
				}
				Komercijalno.Magacin m = magacini[p.MagacinID];
				TDOffice.User k = _korisnici[p.UserID];
				DataRow dr = _sviDokumenti.NewRow();
				dr["BrDok"] = p.ID;
				dr["Datum"] = p.Datum;
				dr["MagacinID"] = p.MagacinID;
				dr["Magacin"] = m == null ? "Undefined" : m.Naziv;
				dr["Referent"] = k.Username;
				dr["Komercijalno"] =
					p.KomercijalnoProracunBroj == null ? -1 : p.KomercijalnoProracunBroj;
				dr["Status"] = (int)p.Status;

				if (p.KomercijalnoProracunBroj == null)
				{
					dr["KomProracun"] = " ";
					dr["MPRacun"] = " ";
				}
				else
				{
					Komercijalno.Dokument proracunKom = komDokBuffer
						.Result[p.Datum.Year]
						.FirstOrDefault(x => x.BrDok == p.KomercijalnoProracunBroj);
					dr["KomProracun"] =
						proracunKom == null ? "ERROR" : proracunKom.BrDok.ToString();
					dr["MPRacun"] =
						proracunKom == null ? "ERROR" : proracunKom.BrDokOut.ToStringOrDefault();
				}
				_sviDokumenti.Rows.Add(dr);
			}
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int brDok = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["BrDok"].Value);

			TDOffice.DokumentProracun dokument = TDOffice.DokumentProracun.Get(brDok);

			Task.Run(() =>
			{
				using (_1332_fm_Proracun_Index p = new _1332_fm_Proracun_Index(dokument))
					if (!p.IsDisposed)
						p.ShowDialog();
			});
		}

		private void nova_btn_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(133201))
			{
				TDOffice.Pravo.NematePravoObavestenje(133201);
				return;
			}

			if (Convert.ToInt32(magacin_cmb.SelectedValue) <= 0)
			{
				MessageBox.Show("Neispravan magacin!");
				return;
			}

			if (Program.TrenutniKorisnik.KomercijalnoUserID == null)
			{
				MessageBox.Show(
					"Nemate povezan nalog komercijalnog poslovanja pa ne mozete kreirati proracun!"
				);
				return;
			}

			int newID = TDOffice.DokumentProracun.Insert(
				DateTime.Now,
				null,
				Convert.ToInt32(magacin_cmb.SelectedValue),
				Program.TrenutniKorisnik.ID,
				"Generisao: TDOffice_v2",
				null,
				-1
			);
			TDOffice.DokumentProracun dok = TDOffice.DokumentProracun.Get(newID);
			UcitajProracune();
			PopulateDGV();

			Task.Run(() =>
			{
				using (_1332_fm_Proracun_Index p = new _1332_fm_Proracun_Index(dok))
					p.ShowDialog();
			});
		}

		private void odDatuma_dtp_CloseUp(object sender, EventArgs e)
		{
			if (!_uiLoaded)
				return;
			PopulateDGV();
		}

		private void doDatuma_dtp_CloseUp(object sender, EventArgs e)
		{
			if (!_uiLoaded)
				return;
			PopulateDGV();
		}

		private void help_btn_Click(object sender, EventArgs e)
		{
			_helpFrom.Result.ShowDialog();
		}
	}
}
