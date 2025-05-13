using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TDOffice_v2
{
	public partial class _1370_fm_NacinIzracunavanjaRabata : Form
	{
		public int rbIzbor;

		public _1370_fm_NacinIzracunavanjaRabata()
		{
			InitializeComponent();
		}

		private void btn_OK_Click(object sender, EventArgs e)
		{
			if (rb_BezDecimala.Checked)
				rbIzbor = 1;
			else if (rb_DatiRabat.Checked)
				rbIzbor = 2;
			else if (rb_Standard.Checked)
				rbIzbor = 3;
			else if (rb_UprosecenRabat.Checked)
				rbIzbor = 4;
			this.Close();
		}
	}
}
