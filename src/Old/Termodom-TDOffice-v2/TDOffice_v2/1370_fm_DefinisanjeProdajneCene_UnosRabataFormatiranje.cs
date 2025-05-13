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
	public partial class _1370_fm_DefinisanjeProdajneCene_UnosRabataFormatiranje : Form
	{
		public enum FormatRabata
		{
			Standard = 0,
			BezDecimala = 1
		}

		public FormatRabata returnValue { get; set; } = 0;

		public _1370_fm_DefinisanjeProdajneCene_UnosRabataFormatiranje()
		{
			InitializeComponent();
		}

		private void _1370_fm_DefinisanjeProdajneCene_UnosRabataFormatiranje_FormClosing(
			object sender,
			FormClosingEventArgs e
		)
		{
			returnValue = radioButton3.Checked ? FormatRabata.BezDecimala : FormatRabata.Standard;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
