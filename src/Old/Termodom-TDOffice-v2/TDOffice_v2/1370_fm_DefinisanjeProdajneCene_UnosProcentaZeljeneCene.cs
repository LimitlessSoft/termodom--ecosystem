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
	public partial class _1370_fm_DefinisanjeProdajneCene_UnosProcentaZeljeneCene : Form
	{
		public double returnValue = 0;
		public PraviloDefinisanjaCene returnPravilo = PraviloDefinisanjaCene.PlusZeljenaMarza;

		public enum PraviloDefinisanjaCene
		{
			PlusZeljenaMarza,
			ProcenatOdMaxRabata
		}

		public _1370_fm_DefinisanjeProdajneCene_UnosProcentaZeljeneCene()
		{
			InitializeComponent();
			maxRabat_txt.Text =
				LocalSettings.Settings.DefinisanjeProdajneCene_MaximalniRabat.ToString();
		}

		private void btn_Unesi_Click(object sender, EventArgs e)
		{
			try
			{
				LocalSettings.Settings.DefinisanjeProdajneCene_MaximalniRabat = Convert.ToDouble(
					maxRabat_txt.Text
				);
				LocalSettings.Update();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
				MessageBox.Show("Neispravna vrednost maksimalnog rabata!");
				return;
			}
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
