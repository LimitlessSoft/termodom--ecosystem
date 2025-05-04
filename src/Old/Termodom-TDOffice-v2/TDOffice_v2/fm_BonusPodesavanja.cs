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
	public partial class fm_BonusPodesavanja : Form
	{
		private DataTable _sviKorisnici = null;
		private DataTable _dgvDT = null;

		public fm_BonusPodesavanja()
		{
			InitializeComponent();
		}

		private void SetDGV()
		{
			_sviKorisnici = new DataTable();
			_sviKorisnici.Columns.Add("Korisnik", typeof(string));
			_sviKorisnici.Columns.Add("TrenutnoBonusa", typeof(int));
			_sviKorisnici.Columns.Add("LimitBonusa", typeof(double));
			_sviKorisnici.Columns.Add("ID", typeof(int));

			_dgvDT = _sviKorisnici;
			dataGridView1.DataSource = _dgvDT;

			dataGridView1.Columns["ID"].Visible = false;
			dataGridView1.Columns["Korisnik"].Visible = true;
			dataGridView1.Columns["Korisnik"].ReadOnly = false;

			dataGridView1.Columns["TrenutnoBonusa"].Visible = true;
			dataGridView1.Columns["LimitBonusa"].Visible = true;
			dataGridView1.Columns["LimitBonusa"].DefaultCellStyle.Format = "#,##0.00";
		}

		private void UcitajKorisnike()
		{
			Task<List<TDOffice.User>> _korisnici = Task.Run(() =>
			{
				return TDOffice.User.List();
			});

			foreach (TDOffice.User k in _korisnici.Result)
			{
				DataRow dr = _sviKorisnici.NewRow();
				dr["ID"] = k.ID;
				dr["Korisnik"] = k.Username;
				dr["TrenutnoBonusa"] = k.BonusZakljucavanjaCount;
				dr["LimitBonusa"] = k.BonusZakljucavanjaLimit;

				_sviKorisnici.Rows.Add(dr);
			}
		}

		private void dataGridView1_CellValidating(
			object sender,
			DataGridViewCellValidatingEventArgs e
		)
		{
			if (dataGridView1.Columns[e.ColumnIndex].Name == "TrenutnoBonusa")
			{
				int id = Convert.ToInt32(
					dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
				);
				TDOffice.User _user = TDOffice.User.Get(id);

				int noviBrBonusa = Convert.ToInt32(e.FormattedValue);
				if (noviBrBonusa < 0)
				{
					MessageBox.Show("Uneti broj bonusa mora biti pozitivan");
					e.Cancel = true;
					return;
				}
				_user.BonusZakljucavanjaCount = noviBrBonusa;
				_user.Update();
			}
			else if (dataGridView1.Columns[e.ColumnIndex].Name == "LimitBonusa")
			{
				int id = Convert.ToInt32(
					dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value
				);
				TDOffice.User _user = TDOffice.User.Get(id);

				double noviLimitBonusa = Convert.ToDouble(e.FormattedValue);
				if (noviLimitBonusa < 0)
				{
					MessageBox.Show("Uneti limit bonusa mora biti pozitivan");
					e.Cancel = true;
					return;
				}
				_user.BonusZakljucavanjaLimit = noviLimitBonusa;
				_user.Update();
			}
		}

		private void fm_BonusPodesavanja_Load(object sender, EventArgs e)
		{
			SetDGV();
			UcitajKorisnike();
		}
	}
}
