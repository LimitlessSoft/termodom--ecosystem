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
	public partial class fm_Web_AnalizaPorudzbina_Index : Form
	{
		private FbConnection _conKomercijalno { get; set; }
		private Task<Dictionary<int, Komercijalno.Dokument>> _webProracuni { get; set; }
		private Task<Dictionary<int, Komercijalno.Magacin>> _magacini { get; set; }
		private DataTable _fullDTBuffer { get; set; }

		public fm_Web_AnalizaPorudzbina_Index()
		{
			InitializeComponent();
			//try
			//{
			//    string apiKey = Program.APIToken;
			//}
			//catch(Exception ex)
			//{
			//    MessageBox.Show(ex.Message);
			//    this.Close();
			//    return;
			//}
			panel2.DesniKlik_DatumRange();
			odDatuma_dtp.Value = new DateTime(DateTime.Now.Year, 1, 1);
			doDatuma_dtp.Value = DateTime.Now;
			magacin_cmb.Enabled = false;
			filtriraj_btn.Enabled = false;
			_conKomercijalno = new FbConnection(
				Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
			);
			_conKomercijalno.Open();
			UcitajMagacineAsync();
			UcitajWebProracuneAsync();
			Task.Run(() => {
				//var asd = TDWeb.Korisnik.List();
			});
		}

		private void fm_Web_AnalizaPorudzbina_Index_Load(object sender, EventArgs e) { }

		private void UcitajWebProracuneAsync()
		{
			_webProracuni = Task.Run(() =>
			{
				var dict = new Dictionary<int, Komercijalno.Dokument>();
				var list = Komercijalno.Dokument.List("VRDOK = 32 AND LOWER(INTBROJ) LIKE '%web%'");

				DataTable dt = new DataTable();
				dt.Columns.Add("Broj Proracuna", typeof(int));
				dt.Columns.Add("IntBroj", typeof(string));
				dt.Columns.Add("Datum", typeof(DateTime));
				dt.Columns.Add("MagacinID", typeof(string));
				dt.Columns.Add("Magacin", typeof(string));
				dt.Columns.Add("PPID", typeof(int));
				dt.Columns.Add("Potrazuje", typeof(double));
				dt.Columns.Add("VrDokOut", typeof(int));
				dt.Columns.Add("BrDokOut", typeof(int));

				foreach (var dok in list)
				{
					Komercijalno.Magacin mag = _magacini.Result[dok.MagacinID];
					dict.Add(dok.BrDok, dok);

					DataRow dr = dt.NewRow();
					dr["Broj Proracuna"] = dok.BrDok;
					dr["IntBroj"] = dok.IntBroj;
					dr["Datum"] = dok.Datum;
					dr["MagacinID"] = mag.ID;
					dr["Magacin"] = mag.Naziv;

					if (dok.PPID != null)
						dr["PPID"] = dok.PPID;

					dr["Potrazuje"] = dok.Potrazuje;

					if (dok.VrDokOut != null)
						dr["VrDokOut"] = dok.VrDokOut;

					if (dok.BrDokOut != null)
						dr["BrDokOut"] = dok.BrDokOut;

					dt.Rows.Add(dr);
				}

				_fullDTBuffer = dt.Copy();
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							dataGridView1.DataSource = dt;

							dataGridView1.Columns["Datum"].DefaultCellStyle.Format = "dd.MM.yyyy";
							dataGridView1.Columns["Potrazuje"].DefaultCellStyle.Format = "#,##0.00";
							dataGridView1.AutoSizeColumnsMode =
								DataGridViewAutoSizeColumnsMode.AllCells;
							dataGridView1.Sort(
								dataGridView1.Columns["Datum"],
								ListSortDirection.Descending
							);

							brojDokumenata_lbl.Text = "Broj Dokumenata: " + dict.Count;
							realizovano_lbl.Text =
								"Realizovano: "
								+ dict.Values.Count(x => x.VrDokOut != null).ToString();
						}
				);
				return dict;
			});
		}

		private void UcitajMagacineAsync()
		{
			_magacini = Task.Run(() =>
			{
				var list = Komercijalno.Magacin.Dict(_conKomercijalno);
				List<Komercijalno.Magacin> l = new List<Komercijalno.Magacin>(list.Values);
				l.Add(new Komercijalno.Magacin() { ID = -1, Naziv = "< svi magacini >" });
				l.Sort((x, y) => x.ID.CompareTo(y.ID));
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							magacin_cmb.ValueMember = "ID";
							magacin_cmb.DisplayMember = "Naziv";
							magacin_cmb.DataSource = l;
							magacin_cmb.Enabled = true;
							filtriraj_btn.Enabled = true;
						}
				);
				return list;
			});
		}

		private void filtriraj_btn_Click(object sender, EventArgs e)
		{
			int magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
			DateTime odDatuma = odDatuma_dtp.Value;
			DateTime doDatuma = doDatuma_dtp.Value;

			if (odDatuma > doDatuma)
			{
				MessageBox.Show("Neispravan datum!");
				return;
			}

			DataTable dt = _fullDTBuffer.Copy();

			for (int i = dt.Rows.Count - 1; i >= 0; i--)
			{
				DateTime dat = Convert.ToDateTime(dt.Rows[i]["Datum"]);
				int mag = Convert.ToInt32(dt.Rows[i]["MagacinID"]);
				if (dat < odDatuma || dat > doDatuma || (magacinID != mag && magacinID != -1))
					dt.Rows.RemoveAt(i);
			}

			dataGridView1.DataSource = dt;
			dataGridView1.Sort(dataGridView1.Columns["Datum"], ListSortDirection.Descending);
		}
	}
}
