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
	public partial class _1370_fm_DefinisanjeProdajneCene_UnosProcentaZelejeneCene : Form
	{
		public double returnValue = 0;
		public PraviloDefinisanjaCene returnPravilo = PraviloDefinisanjaCene.PlusZeljenaMarza;

		public enum PraviloDefinisanjaCene
		{
			PlusZeljenaMarza,
			ProcenatOdMaxRabata
		}

		public _1370_fm_DefinisanjeProdajneCene_UnosProcentaZelejeneCene()
		{
			InitializeComponent();
		}

		private void btn_Unesi_Click(object sender, EventArgs e)
		{
			returnValue = rb_ProcenatOdMaxRabata.Checked
				? Convert.ToDouble(tb_ProcenatOdMaxRabata.Text)
				: Convert.ToDouble(tb_ZeljenaMarza.Text);
			returnPravilo = rb_ProcenatOdMaxRabata.Checked
				? PraviloDefinisanjaCene.ProcenatOdMaxRabata
				: PraviloDefinisanjaCene.PlusZeljenaMarza;
			this.Close();
		}
	}
}
