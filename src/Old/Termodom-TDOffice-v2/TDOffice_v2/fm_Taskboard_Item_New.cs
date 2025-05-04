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
	public partial class fm_Taskboard_Item_New : Form
	{
		private int _taskboardID { get; set; }

		public fm_Taskboard_Item_New(int taskboardID)
		{
			InitializeComponent();
			_taskboardID = taskboardID;
		}

		private void kreiraj_btn_Click(object sender, EventArgs e)
		{
			TDOffice.Taskboard.Item.Insert(
				_taskboardID,
				naslov_txt.Text,
				text_rtb.Text,
				Program.TrenutniKorisnik.ID
			);
			MessageBox.Show("Uspesno dodat task!");
			this.Close();
		}
	}
}
