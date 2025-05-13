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
	public partial class _1337_fm_Korisnici_List : Form
	{
		private Task<Termodom.Data.Entities.Komercijalno.MagacinDictionary> _magacini =
			Komercijalno.Magacin.DictionaryAsync();
		private Task<Termodom.Data.Entities.TDOffice_v2.GradDictionary> _gradovi =
			TDOffice.GradManager.DictionaryAsync();
		private Task<fm_Help> _helpFrom { get; set; }

		public _1337_fm_Korisnici_List()
		{
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.Korisnici_List);
		}

		private void _1337_fm_Korisnici_List_Load(object sender, EventArgs e)
		{
			_ = LoadUsersAsync();
		}

		private async Task LoadUsersAsync()
		{
			try
			{
				List<TDOffice.User> users = TDOffice.User.List();
				Termodom.Data.Entities.Komercijalno.MagacinDictionary magacini = await _magacini;
				Termodom.Data.Entities.TDOffice_v2.GradDictionary gradovi = await _gradovi;

				DataTable dt = new DataTable();
				dt.Columns.Add("ID", typeof(int));
				dt.Columns.Add("Ime", typeof(string));
				dt.Columns.Add("Magacin", typeof(string));
				dt.Columns.Add("Grad", typeof(string));

				foreach (TDOffice.User user in users)
				{
					Termodom.Data.Entities.Komercijalno.Magacin m = magacini.ContainsKey(
						user.MagacinID
					)
						? magacini[user.MagacinID]
						: null;
					Termodom.Data.Entities.TDOffice_v2.Grad g = gradovi.ContainsKey(user.Grad)
						? gradovi[user.Grad]
						: null;
					DataRow dr = dt.NewRow();
					dr["ID"] = user.ID;
					dr["Ime"] = user.Username;
					dr["Magacin"] = m == null ? "Unknown" : m.Naziv;
					dr["Grad"] = g.Naziv;
					dt.Rows.Add(dr);
				}

				dataGridView1.DataSource = dt;

				dataGridView1.Columns["ID"].ReadOnly = true;
				dataGridView1.Columns["ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
				dataGridView1.Columns["Ime"].ReadOnly = true;
				dataGridView1.Columns["Ime"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
				dataGridView1.Columns["Magacin"].ReadOnly = true;
				dataGridView1.Columns["Magacin"].AutoSizeMode =
					DataGridViewAutoSizeColumnMode.AllCells;
				dataGridView1.Columns["Grad"].ReadOnly = true;
				dataGridView1.Columns["Grad"].AutoSizeMode =
					DataGridViewAutoSizeColumnMode.AllCells;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void noviToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (_1337_fm_Korisnici_Novi n = new _1337_fm_Korisnici_Novi())
			{
				n.ShowDialog();
			}

			_ = LoadUsersAsync();
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			int userID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

			using (_1337_fm_Korisnici_Index ki = new _1337_fm_Korisnici_Index(userID))
				ki.ShowDialog();

			_ = LoadUsersAsync();
		}
	}
}
