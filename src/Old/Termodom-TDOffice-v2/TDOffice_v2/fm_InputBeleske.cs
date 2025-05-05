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
	public partial class fm_InputBeleske : Form
	{
		public string returnData;
		public string beleska;

		public fm_InputBeleske(string Title, string Text, string TextBeleska)
		{
			InitializeComponent();
			this.TopMost = true;
			this.Text = Title;
			this.textBox2.Text = Text;
			this.textBox1.Text = TextBeleska;
			this.beleska = TextBeleska;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			beleska = textBox1.Text;
			this.Close();
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				button1.PerformClick();
				e.Handled = true;
			}
		}

		private void fm_InputBeleske_FormClosing(object sender, FormClosingEventArgs e)
		{
			returnData = beleska;
		}
	}
}
