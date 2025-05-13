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
	public partial class fm_Taskboard_Item_Index : Form
	{
		private TDOffice.Taskboard.Item _item { get; set; }
		private bool _loaded = false;

		public fm_Taskboard_Item_Index(TDOffice.Taskboard.Item item)
		{
			InitializeComponent();
			_item = item;

			List<Tuple<int, string>> itemStatuses = new List<Tuple<int, string>>()
			{
				new Tuple<int, string>(0, "Zatrazeno"),
				new Tuple<int, string>(1, "Na cekanju"),
				new Tuple<int, string>(2, "Trenutno se radi"),
				new Tuple<int, string>(3, "Zavrseno")
			};
			itemStatus_cmb.DataSource = itemStatuses;
			itemStatus_cmb.ValueMember = "Item1";
			itemStatus_cmb.DisplayMember = "Item2";

			itemStatus_cmb.SelectedValue = (int)_item.Status;

			itemStatus_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(1845003);
		}

		private void fm_Taskboard_Item_Index_Load(object sender, EventArgs e)
		{
			naslov_txt.ReadOnly = true;
			naslov_txt.BackColor = Color.LightYellow;

			text_rtb.ReadOnly = true;
			text_rtb.BackColor = Color.LightYellow;

			btn_Sacuvaj.Enabled = false;
			btn_Sacuvaj.BackColor = Color.Gray;

			naslov_txt.Text = _item.Text;
			text_rtb.Text = _item.Text;
			UcitajKomentare();

			_loaded = true;
		}

		private void itemStatus_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			btn_Sacuvaj.Enabled = true;
			btn_Sacuvaj.BackColor = Control.DefaultBackColor;
		}

		private void UcitajKomentare()
		{
			rtb_Komentari.Clear();
			rtb_Komentari.Text =
				"*******************************\n******  KOMENTARI  ******\n*******************************";

			List<TDOffice.TaskboardTaskKomentar> komentari = TDOffice
				.TaskboardTaskKomentar.ListByTaskID(_item.ID)
				.OrderBy(x => x.Datum)
				.ToList();
			foreach (TDOffice.TaskboardTaskKomentar k in komentari)
			{
				//rtb_Komentari.AppendText("\n" + k.Datum.ToString() + "  Komentar korisnika  <" + TDOffice.User.Get(k.KorisnikID).Username + ">   \n<<"+k.Text+">>\n");
				rtb_Komentari.AppendText(
					k.Datum.ToString()
						+ "  Komentar korisnika  <"
						+ TDOffice.User.Get(k.KorisnikID).Username
						+ ">   <<"
						+ k.Text
						+ ">>\n"
				);
			}
		}

		private void btn_Sacuvaj_Click(object sender, EventArgs e)
		{
			_item.Status = (TDOffice.Taskboard.ItemStatus)
				Convert.ToInt32(itemStatus_cmb.SelectedValue);
			_item.Update();

			btn_Sacuvaj.Enabled = false;
			btn_Sacuvaj.BackColor = Color.Gray;
		}

		private void btn_Komentarisi_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show(
					"Sigurno zelite da dodate komentar ",
					"Dodavanje komentara",
					MessageBoxButtons.YesNo
				) == DialogResult.No
			)
			{
				tb_NoviKomentar.Clear();
				return;
			}
			int newID = TDOffice.TaskboardTaskKomentar.Insert(
				Program.TrenutniKorisnik.ID,
				_item.ID,
				DateTime.Now,
				tb_NoviKomentar.Text.ToString()
			);
			UcitajKomentare();
			tb_NoviKomentar.Clear();
		}
	}
}
