using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
	public partial class fm_GrupaTroskova_List : Form
	{
		private TDOffice.Config<Dictionary<string, List<int>>> _grupeTroskova = TDOffice.Config<
			Dictionary<string, List<int>>
		>.Get(TDOffice.ConfigParameter.GrupeTroskova);
		private Task<List<Komercijalno.Roba>> _roba = Task.Run(() =>
		{
			return Komercijalno.Roba.List(DateTime.Now.Year);
		});

		public fm_GrupaTroskova_List()
		{
			InitializeComponent();

			if (_grupeTroskova == null || _grupeTroskova.Tag == null)
			{
				if (_grupeTroskova == null)
					_grupeTroskova = new TDOffice.Config<Dictionary<string, List<int>>>();

				if (_grupeTroskova.Tag == null)
					_grupeTroskova.Tag = new Dictionary<string, List<int>>();

				_grupeTroskova.UpdateOrInsert();
			}
		}

		private void fm_GrupaTroskova_List_Load(object sender, EventArgs e)
		{
			UcitajGrupe();
		}

		private void UcitajGrupe()
		{
			grupa_cmb.Enabled = false;
			List<Tuple<string, string>> list = new List<Tuple<string, string>>();

			foreach (string key in _grupeTroskova.Tag.Keys)
				list.Add(new Tuple<string, string>(key, key));

			list.Add(new Tuple<string, string>(" ", "< Izaberi Grupu >"));

			list.Sort((y, x) => x.Item1.CompareTo(x.Item2));

			grupa_cmb.DataSource = list;
			grupa_cmb.DisplayMember = "Item2";
			grupa_cmb.ValueMember = "Item1";

			grupa_cmb.SelectedValue = " ";

			grupa_cmb.Enabled = true;
			dataGridView1.DataSource = null;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			using (
				InputBox ib = new InputBox(
					"Nova Grupa Troskova",
					"Unesite naziv nove grupe troskova"
				)
			)
			{
				ib.ShowDialog();

				if (ib.DialogResult != DialogResult.OK)
				{
					MessageBox.Show("Obustavljam!");
					return;
				}

				string noviNaziv = ib.returnData.ToUpper().Trim();
				RegexOptions options = RegexOptions.None;
				Regex regex = new Regex("[ ]{2,}", options);
				noviNaziv = regex.Replace(noviNaziv, " ");

				if (_grupeTroskova.Tag.Keys.Contains(noviNaziv))
				{
					MessageBox.Show("Grupa sa zadatim nazivom vec postoji!");
					return;
				}

				_grupeTroskova.Tag.Add(noviNaziv, new List<int>());
				_grupeTroskova.UpdateOrInsert();

				UcitajGrupe();

				MessageBox.Show("Grupa kreirana!");
			}
		}

		private void grupa_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!grupa_cmb.Enabled)
				return;

			UcitajPodatke();
		}

		private void UcitajPodatke()
		{
			string grupa = grupa_cmb.SelectedValue.ToString();
			if (grupa == " ")
			{
				dataGridView1.DataSource = null;
				return;
			}

			List<int> proizvodi = _grupeTroskova.Tag[grupa];

			DataTable dt = new DataTable();
			dt.Columns.Add("RobaID", typeof(int));
			dt.Columns.Add("Naziv", typeof(string));

			foreach (int i in proizvodi)
			{
				Komercijalno.Roba r = _roba.Result.FirstOrDefault(x => x.ID == i);

				DataRow dr = dt.NewRow();
				dr["RobaID"] = i;
				dr["Naziv"] = r.Naziv;
				dt.Rows.Add(dr);
			}

			dataGridView1.DataSource = dt;
			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
		}

		private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("ID", typeof(int));
			dt.Columns.Add("Naziv", typeof(string));

			foreach (
				Komercijalno.Roba r in _roba.Result.Where(x =>
					x.Vrsta != 1 && x.Naziv.ToLower().Contains("trosak")
				)
			)
			{
				DataRow dr = dt.NewRow();
				dr["ID"] = r.ID;
				dr["Naziv"] = r.Naziv;
				dt.Rows.Add(dr);
			}

			using (DataGridViewSelectBox sb = new DataGridViewSelectBox(dt))
			{
				sb.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
				sb.RowHeaderVisible = false;
				sb.CloseOnSelect = false;
				sb.OnRowSelected += (DataGridViewSelectBox.RowSelectEventArgs args) =>
				{
					int noviRobaID = Convert.ToInt32(args.SelectedRow["ID"]);
					string trenutnaGrupa = grupa_cmb.SelectedValue.ToString();
					if (_grupeTroskova.Tag[trenutnaGrupa].Contains(noviRobaID))
					{
						MessageBox.Show("Ova grupa vec sadrzi ovaj trosak!");
						return;
					}

					_grupeTroskova.Tag[trenutnaGrupa].Add(noviRobaID);
					_grupeTroskova.UpdateOrInsert();

					UcitajPodatke();
					MessageBox.Show("Grupa uspesno azurirana!");
				};
				sb.ShowDialog();
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			string trenutnaGrupa = grupa_cmb.SelectedValue.ToString();
			if (
				MessageBox.Show(
					"Da li sigurno zelite obrisati grupu: " + trenutnaGrupa,
					"Potvrdi!",
					MessageBoxButtons.YesNo
				) != DialogResult.Yes
			)
				return;

			_grupeTroskova.Tag.Remove(trenutnaGrupa);
			_grupeTroskova.UpdateOrInsert();

			UcitajGrupe();
			MessageBox.Show("Grupa uspesno uklonjena!");
		}

		private void ukloniTrosakToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string trenutnaGrupa = grupa_cmb.SelectedValue.ToString();
			int robaID = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["RobaID"].Value
			);

			_grupeTroskova.Tag[trenutnaGrupa].Remove(robaID);

			_grupeTroskova.UpdateOrInsert();
			UcitajPodatke();
		}
	}
}
