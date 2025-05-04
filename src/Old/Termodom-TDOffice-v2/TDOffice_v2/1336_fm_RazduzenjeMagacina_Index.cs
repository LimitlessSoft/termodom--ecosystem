using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2
{
	public partial class _1336_fm_RazduzenjeMagacina_Index : Form
	{
		private TDOffice.DokumentRazduzenjaMagacina _dokument = null;
		private List<TDOffice.StavkaRazduzenjaMagacina> _stavke = null;
		private Task<TDOffice.Magacin> _tdMagacin { get; set; }
		private DataTable _dgvDTStavke = null;
		private Task<fm_Help> _helpFrom { get; set; }
		private Task<List<TDOffice.User>> _korisnici = Task.Run(() =>
		{
			return TDOffice.User.List();
		});
		private Task<List<Komercijalno.Magacin>> _magacini = Komercijalno.Magacin.ListAsync(
			DateTime.Now.Year
		);

		public _1336_fm_RazduzenjeMagacina_Index(TDOffice.DokumentRazduzenjaMagacina dokument)
		{
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.RazduzenjeMagacina_Index);
			_dokument = dokument;

			Task.Run(() =>
			{
				_tdMagacin = TDOffice.Magacin.GetAsync(_dokument.MagacinID);
			});
		}

		private void _1336_fm_RazduzenjeMagacina_Index_Load(object sender, EventArgs e)
		{
			SetUI();
			SetDGV();
			ReloadStavke();
			PopulateDGV();
		}

		private void SetUI()
		{
			TDOffice.User k = _korisnici
				.Result.Where(x => x.ID == _dokument.KorisnikID)
				.FirstOrDefault();

			Komercijalno.Magacin m = _magacini
				.Result.Where(x => x.ID == _dokument.MagacinID)
				.FirstOrDefault();
			Komercijalno.Dokument d = Komercijalno
				.Dokument.ListByMagacinID(_dokument.Datum.Year, _dokument.MagacinID)
				.ToList()
				.Where(x => x.VrDok == 19 && x.BrDok == _dokument.KomercijalnoBrDokOut)
				.FirstOrDefault();

			this.brojDokumenta_txt.Text = _dokument.ID.ToString();

			this.datum_txt.Text = _dokument.Datum.ToString();

			this.referent_txt.Text = k.Username;

			this.tdofficeMagacin_txt.Text = m.Naziv;

			this.izvrsiRazduzenje_btn.Enabled = !(
				_dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Izvrsen
			);

			this.dataGridView1.Enabled = !(
				_dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Izvrsen
			);

			this.BackColor =
				_dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Zakljucan
					? Color.Red
					: _dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Izvrsen
						? Color.OrangeRed
						: Color.Green;

			this.status_btn.BackgroundImage =
				_dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Zakljucan
					? TDOffice_v2.Properties.Resources.key_green
					: TDOffice_v2.Properties.Resources.key_red;
			this.status_btn.Enabled =
				_dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Izvrsen
					? false
					: _dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Otkljucan
						? Program.TrenutniKorisnik.ImaPravo(133602)
						: Program.TrenutniKorisnik.ImaPravo(133603);

			this.tb_Otpremnica.Text = "[19]  " + _dokument.KomercijalnoBrDokOut.ToString();
			if (d != null)
				this.tb_Kalkulacija.Text = "[18]  " + d.BrDokOut.ToString();
			else
				this.tb_Kalkulacija.Text = "0";

			this.komentar_btn.Enabled =
				_dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Otkljucan
					? true
					: false;
			this.interniKomentar_btn.Enabled =
				_dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Otkljucan
					? true
					: false;
		}

		private void SetDGV()
		{
			_dgvDTStavke = new DataTable();
			_dgvDTStavke.Columns.Add("ID", typeof(int));
			_dgvDTStavke.Columns.Add("RobaID", typeof(int));
			_dgvDTStavke.Columns.Add("Roba", typeof(string));
			_dgvDTStavke.Columns.Add("Kolicina", typeof(double));

			dataGridView1.DataSource = _dgvDTStavke;

			dataGridView1.Columns["ID"].Visible = false;
			dataGridView1.Columns["RobaID"].Visible = false;

			dataGridView1.Columns["Roba"].Width = 250;

			dataGridView1.Columns["Kolicina"].Width = 50;
			//dataGridView1.Columns["Kolicina"].ReadOnly = false;
			dataGridView1.Columns["Kolicina"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.Columns["Kolicina"].DefaultCellStyle.BackColor = Color.LightYellow;
			dataGridView1.Columns["Kolicina"].DefaultCellStyle.Format = "#,##0.00";
		}

		private void ReloadStavke()
		{
			_stavke = TDOffice.StavkaRazduzenjaMagacina.ListByDokumentID(_dokument.ID);
		}

		private void PopulateDGV()
		{
			List<Komercijalno.Roba> roba = Komercijalno.Roba.List(DateTime.Now.Year);
			_dgvDTStavke.Clear();
			foreach (TDOffice.StavkaRazduzenjaMagacina s in _stavke)
			{
				Komercijalno.Roba r = roba.Where(x => x.ID == s.RobaID).FirstOrDefault();
				DataRow dr = _dgvDTStavke.NewRow();

				dr["ID"] = s.ID;
				dr["RobaID"] = s.RobaID;
				dr["Roba"] = r.Naziv;
				dr["Kolicina"] = s.Kolicina;

				_dgvDTStavke.Rows.Add(dr);
			}

			dataGridView1.DataSource = _dgvDTStavke;
		}

		private void dataGridView1_DoubleClick(object sender, EventArgs e)
		{
			if (_dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Zakljucan)
			{
				MessageBox.Show("Dokument je zakljucan!");
				return;
			}

			using (IzborRobe ir = new IzborRobe(_dokument.MagacinID))
			{
				ir.DozvoliMultiSelect = false;
				ir.OnRobaClickHandler += OnIzborRobeTrigger;
				ir.ShowDialog();
			}
		}

		private void OnIzborRobeTrigger(Komercijalno.RobaUMagacinu[] args)
		{
			Komercijalno.RobaUMagacinu r = args[0];

			double kolicina = 0;
			using (InputBox ib = new InputBox("Kolicina", "Unesite kolicinu"))
			{
				ib.ShowDialog();

				if (ib.DialogResult != DialogResult.OK)
					return;

				try
				{
					kolicina = Convert.ToDouble(ib.returnData);
				}
				catch
				{
					MessageBox.Show("Neispravna kolicina!");
					return;
				}
			}

			if (kolicina <= 0)
			{
				MessageBox.Show("Kolicina mora biti veca od 0!");
				return;
			}

			TDOffice.StavkaRazduzenjaMagacina ss = _stavke
				.Where(x => x.RobaID == r.RobaID)
				.FirstOrDefault();

			if (ss != null)
			{
				MessageBox.Show("Stavka vec postoji u radzuzenju!");
				return;
			}

			TDOffice.StavkaRazduzenjaMagacina.Insert(r.RobaID, kolicina, _dokument.ID);

			ReloadStavke();
			PopulateDGV();
		}

		private void status_btn_Click(object sender, EventArgs e)
		{
			if (_dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Izvrsen)
				return;

			_dokument.Status =
				_dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Otkljucan
					? TDOffice.DokumentRazduzenjaMagacinaStatus.Zakljucan
					: TDOffice.DokumentRazduzenjaMagacinaStatus.Otkljucan;
			_dokument.Update();

			SetUI();
		}

		private void izvrsiRazduzenje_btn_Click(object sender, EventArgs e)
		{
			if (_tdMagacin.Result == null)
			{
				MessageBox.Show(
					"Magacin nema definisan svoj Magacin Razduzenja!\nPodesavanja > Magacini > Magacin > Magacin Razduzenja"
				);
				return;
			}

			if (dataGridView1.Rows.Count == 0)
			{
				MessageBox.Show("Razduzenje mora imati stavke!");
				return;
			}

			if (_dokument.Status == TDOffice.DokumentRazduzenjaMagacinaStatus.Otkljucan)
			{
				MessageBox.Show("Dokument mora biti zakljucan");
				return;
			}

			if (!Program.TrenutniKorisnik.ImaPravo(133605))
			{
				TDOffice.Pravo.NematePravoObavestenje(133605);
				return;
			}

			if (
				MessageBox.Show(
					"Sigurno zelite da izvrsite razduzenje u magacin "
						+ _magacini.Result.FirstOrDefault(x => x.ID == _tdMagacin.Result.ID).Naziv
						+ "?",
					"Razduzenje magacina",
					MessageBoxButtons.YesNo
				) != DialogResult.Yes
			)
				return;

			using (
				FbConnection con = new FbConnection(
					Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				int komercijalnoBrDok19 = Komercijalno.Dokument.Insert(
					con,
					19,
					null,
					null,
					"TDOffice_v2",
					1,
					_dokument.MagacinID,
					Program.TrenutniKorisnik.KomercijalnoUserID,
					_tdMagacin.Result.MagacinRazduzenja
				);
				Komercijalno.Dokument dok19 = Komercijalno.Dokument.Get(
					con,
					19,
					komercijalnoBrDok19
				);

				int komercijalnoBrDok18 = Komercijalno.Dokument.Insert(
					con,
					18,
					null,
					null,
					"TDOffice_v2",
					1,
					_tdMagacin.Result.MagacinRazduzenja,
					Program.TrenutniKorisnik.KomercijalnoUserID,
					_dokument.MagacinID
				);
				Komercijalno.Dokument dok18 = Komercijalno.Dokument.Get(
					con,
					18,
					komercijalnoBrDok18
				);

				dok19.VrDokOut = 18;
				dok19.BrDokOut = komercijalnoBrDok18;
				dok19.Update();

				dok18.VrDokIn = 19;
				dok18.BrDokIn = komercijalnoBrDok19;
				dok18.Update();

				Komercijalno.Dokument.SetKomentar(
					dok19.Datum.Year,
					19,
					dok19.BrDok,
					_dokument.Komentar
				);
				Komercijalno.Dokument.SetInterniKomentar(
					dok19.Datum.Year,
					19,
					dok19.BrDok,
					_dokument.InterniKomentar
				);
				Komercijalno.Dokument.SetKomentar(
					dok19.Datum.Year,
					18,
					dok18.BrDok,
					_dokument.Komentar
				);
				Komercijalno.Dokument.SetInterniKomentar(
					dok19.Datum.Year,
					18,
					dok18.BrDok,
					_dokument.InterniKomentar
				);

				_dokument.KomercijalnoBrDokOut = komercijalnoBrDok19;
				_dokument.Update();

				Task<List<Komercijalno.Roba>> robaKom = Komercijalno.Roba.ListAsync(
					DateTime.Now.Year
				);
				Task<List<Komercijalno.RobaUMagacinu>> rumKom =
					Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(_dokument.MagacinID);

				foreach (DataGridViewRow row in dataGridView1.Rows)
				{
					int robaID = Convert.ToInt32(row.Cells["Robaid"].Value);

					Komercijalno.Stavka.Insert(
						DateTime.Now.Year,
						dok19,
						robaKom.Result.Where(x => x.ID == robaID).FirstOrDefault(),
						rumKom.Result.Where(x => x.RobaID == robaID).FirstOrDefault(),
						Convert.ToDouble(row.Cells["KOLICINA"].Value),
						0
					);
					Komercijalno.Stavka.Insert(
						DateTime.Now.Year,
						dok18,
						robaKom.Result.Where(x => x.ID == robaID).FirstOrDefault(),
						rumKom.Result.Where(x => x.RobaID == robaID).FirstOrDefault(),
						Convert.ToDouble(row.Cells["KOLICINA"].Value),
						0
					);
				}

				_dokument.Status = TDOffice.DokumentRazduzenjaMagacinaStatus.Izvrsen;
				_dokument.Update();

				// Slanje poruka
				string tekstPoruke =
					"Korisnik "
					+ Program.TrenutniKorisnik.Username
					+ " je razduzio lager magacina "
					+ _dokument.MagacinID
					+ " po dokumentu br."
					+ _dokument.ID
					+ " cija je vrednost \nKreirana je interna otpremnica br."
					+ dok18.BrDokIn;
				string naslovPoruke =
					"Razduzenje magacina"
					+ _dokument.MagacinID
					+ " po dokumentu broj "
					+ _dokument.ID;
				TDOffice.Poruka.Insert(
					TDOffice
						.User.List()
						.Where(x =>
							x.Tag != null
							&& x.Tag.PrimaObavestenja[
								TDOffice.User.TipAutomatskogObavestenja.NakonRazduzenjaRobe
							] == true
						)
						.Select(x => x.ID)
						.ToArray(),
					naslovPoruke,
					tekstPoruke,
					new TDOffice.PorukaAdditionalInfo()
					{
						Action = TDOffice.PorukaAction.RealizovanoRazduzenjeMagacina,
						AdditionalInfo = _dokument.ID
					}
				);
			}

			SetUI();
		}

		private void brisiSelektovanuStavkuToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int id = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
			);

			TDOffice.StavkaRazduzenjaMagacina.Delete(id);

			ReloadStavke();
			PopulateDGV();
		}

		private void dataGridView1_CellValidating(
			object sender,
			DataGridViewCellValidatingEventArgs e
		)
		{
			if (dataGridView1.Columns[e.ColumnIndex].Name == "Kolicina")
			{
				int id = Convert.ToInt32(
					dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
				);
				TDOffice.StavkaRazduzenjaMagacina _stavka = TDOffice.StavkaRazduzenjaMagacina.Get(
					id
				);

				double novaKolicina = Convert.ToDouble(e.FormattedValue);
				if (novaKolicina <= 0)
				{
					MessageBox.Show("Uneta kolicina mora biti pozitivna");
					e.Cancel = true;
					return;
				}
				_stavka.Kolicina = novaKolicina;
				_stavka.Update();
			}
		}

		private void komentar_btn_Click(object sender, EventArgs e)
		{
			using (fm_Dokument_Komentar kom = new fm_Dokument_Komentar(_dokument.Komentar))
			{
				kom.KomentarSacuvan += (noviKomentar) =>
				{
					_dokument.Komentar = noviKomentar;
					_dokument.Update();

					MessageBox.Show("Dokument sacuvan!");
				};
				kom.ShowDialog();
			}
		}

		private void interniKomentar_btn_Click(object sender, EventArgs e)
		{
			using (fm_Dokument_Komentar kom = new fm_Dokument_Komentar(_dokument.InterniKomentar))
			{
				kom.KomentarSacuvan += (noviKomentar) =>
				{
					_dokument.InterniKomentar = noviKomentar;
					_dokument.Update();

					MessageBox.Show("Dokument sacuvan!");
				};
				kom.ShowDialog();
			}
		}

		private void help_btn_Click(object sender, EventArgs e)
		{
			_helpFrom.Result.ShowDialog();
		}
	}
}
