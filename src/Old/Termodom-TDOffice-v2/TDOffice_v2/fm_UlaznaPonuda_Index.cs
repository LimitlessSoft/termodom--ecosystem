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
	public partial class fm_UlaznaPonuda_Index : Form
	{
		private TDOffice.DokumentUlaznaPonuda _dokument { get; set; }
		private List<TDOffice.StavkaUlaznaPonuda> _stavke { get; set; }

		private Task<List<Komercijalno.Roba>> _komercijalnoRoba = Komercijalno.Roba.ListAsync(
			DateTime.Now.Year
		);

		public fm_UlaznaPonuda_Index(int dokumentID)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(136580))
			{
				TDOffice.Pravo.NematePravoObavestenje(136580);
				this.Close();
				return;
			}

			InitializeComponent();

			UcitajDokument(dokumentID);
		}

		private void fm_UlaznaPonuda_Index_Load(object sender, EventArgs e) { }

		private void UcitajDokument(int id)
		{
			_dokument = TDOffice.DokumentUlaznaPonuda.Get(id);

			this.Text = "Ulazna Ponuda - " + _dokument.ID.ToString();

			datumDokumenta_dtp.Enabled = false;
			pocetakVazenja_dtp.Enabled = false;
			krajVazenja_dtp.Enabled = false;
			btn_Sacuvaj.Enabled = false;
			btn_OdbaciIzmene.Enabled = false;

			nova_btn.BackColor = TDColor.ControlaActive;
			osvezi_btn.BackColor = TDColor.ControlaActive;

			datumDokumenta_dtp.Value = _dokument.Datum;
			pocetakVazenja_dtp.Value = _dokument.DatumPocetkaVazenja;
			krajVazenja_dtp.Value = _dokument.DatumKrajaVazenja;

			pocetakVazenja_dtp.Enabled = true;
			krajVazenja_dtp.Enabled = true;

			UcitajStavke();
		}

		private void UcitajStavke()
		{
			_stavke = TDOffice.StavkaUlaznaPonuda.List($"BRDOK = {_dokument.ID}");

			DataTable dt = new DataTable();
			dt.Columns.Add("ID", typeof(int));
			dt.Columns.Add("RobaID", typeof(int));
			dt.Columns.Add("Proizvod", typeof(string));
			dt.Columns.Add("JM", typeof(string));
			dt.Columns.Add("NabavnaCena", typeof(double));

			foreach (var s in _stavke)
			{
				Komercijalno.Roba r = _komercijalnoRoba.Result.FirstOrDefault(x =>
					x.ID == s.RobaID
				);

				DataRow dr = dt.NewRow();
				dr["ID"] = s.ID;
				dr["RobaID"] = s.RobaID;
				dr["Proizvod"] = r == null ? "UNKNOWN" : r.Naziv;
				dr["JM"] = r == null ? "UNKNOWN" : r.JM;
				dr["NabavnaCena"] = s.NabavnaCena;
				dt.Rows.Add(dr);
			}

			dataGridView1.DataSource = dt;

			dataGridView1.ReadOnly = false;
			dataGridView1.SelectionMode = DataGridViewSelectionMode.CellSelect;

			dataGridView1.Columns["ID"].Visible = false;
			dataGridView1.Columns["RobaID"].Visible = false;
			dataGridView1.Columns["Proizvod"].ReadOnly = true;
			dataGridView1.Columns["JM"].ReadOnly = true;

			dataGridView1.Columns["NabavnaCena"].HeaderText = "Nabavna Cena";
			dataGridView1.Columns["NabavnaCena"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			btn_Sacuvaj.Enabled = false;
			btn_OdbaciIzmene.Enabled = false;
		}

		private void nova_btn_Click(object sender, EventArgs e)
		{
			int noviID = TDOffice.DokumentUlaznaPonuda.Insert(
				Program.TrenutniKorisnik.ID,
				DateTime.Now,
				DateTime.Now.AddMonths(3)
			);
			UcitajDokument(noviID);
		}

		private void osvezi_btn_Click(object sender, EventArgs e)
		{
			UcitajStavke();
		}

		private void datumDokumenta_dtp_ValueChanged(object sender, EventArgs e) { }

		private void pocetakVazenja_dtp_ValueChanged(object sender, EventArgs e)
		{
			_dokument.DatumPocetkaVazenja = pocetakVazenja_dtp.Value;

			btn_Sacuvaj.Enabled = true;
			btn_OdbaciIzmene.Enabled = true;
		}

		private void krajVazenja_dtp_ValueChanged(object sender, EventArgs e)
		{
			_dokument.DatumKrajaVazenja = krajVazenja_dtp.Value;

			btn_Sacuvaj.Enabled = true;
			btn_OdbaciIzmene.Enabled = true;
		}

		private void btn_OdbaciIzmene_Click(object sender, EventArgs e)
		{
			UcitajDokument(_dokument.ID);
		}

		private void btn_Sacuvaj_Click(object sender, EventArgs e)
		{
			_dokument.Update();
			UcitajDokument(_dokument.ID);
		}

		private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			using (IzborRobe ir = new IzborRobe(50))
			{
				ir.OnRobaClickHandler += (Komercijalno.RobaUMagacinu[] args) =>
				{
					foreach (Komercijalno.RobaUMagacinu r in args)
					{
						if (_stavke.Any(x => x.RobaID == r.RobaID))
							MessageBox.Show(
								"Ulazna ponuda vec sadrzi robu "
									+ _komercijalnoRoba
										.Result.FirstOrDefault(x => x.ID == r.RobaID)
										.Naziv
							);
						else
						{
							TDOffice.StavkaUlaznaPonuda.Get(
								TDOffice.StavkaUlaznaPonuda.Insert(_dokument.ID, r.RobaID, 0)
							);
							UcitajStavke();
						}
					}
				};
				ir.ShowDialog();
			}
		}

		private void btn_OdbaciIzmene_EnabledChanged(object sender, EventArgs e)
		{
			(sender as Control).BackColor = (sender as Control).Enabled
				? TDColor.ControlaActive
				: TDColor.ControlInactive;
		}

		private void dataGridView1_CellValidating(
			object sender,
			DataGridViewCellValidatingEventArgs e
		)
		{
			if (e.ColumnIndex != dataGridView1.Columns["NabavnaCena"].Index)
				return;

			int idStavke = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

			var stavka = _stavke.FirstOrDefault(x => x.ID == idStavke);

			_stavke.RemoveAll(x => x.ID == idStavke);
			stavka.NabavnaCena = Convert.ToDouble(e.FormattedValue);
			stavka.Update();
			_stavke.Add(stavka);
		}
	}
}
