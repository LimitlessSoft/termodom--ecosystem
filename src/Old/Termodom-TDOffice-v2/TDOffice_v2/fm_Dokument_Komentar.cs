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
	public enum DokumentKomentarType
	{
		Default = 0,
		Interni = 1
	}

	public partial class fm_Dokument_Komentar : Form
	{
		public delegate void OnKomentarSacuvan(string noviKomentar);
		public OnKomentarSacuvan KomentarSacuvan;
		public bool DozvoliEdit
		{
			get { return _dozvoliEdit; }
			set
			{
				_dozvoliEdit = value;
				button1.Enabled = _dozvoliEdit;
				richTextBox1.ReadOnly = !_dozvoliEdit;
			}
		}
		private bool _dozvoliEdit { get; set; } = false;
		private string _osnova = null;
		private bool _imaIzmena = false;

		public fm_Dokument_Komentar(string osnova)
		{
			InitializeComponent();
			_osnova = osnova;
		}

		private void fm_Dokument_Komentar_Load(object sender, EventArgs e)
		{
			richTextBox1.Text = _osnova;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (KomentarSacuvan != null)
				KomentarSacuvan(richTextBox1.Text);

			_imaIzmena = false;
		}

		private void richTextBox1_KeyDown(object sender, KeyEventArgs e)
		{
			_imaIzmena = true;
		}

		private void fm_Dokument_Komentar_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (_imaIzmena && _dozvoliEdit)
				if (
					MessageBox.Show(
						"Da li zelite sacuvati komentar pre izlaska?",
						"Sacuvati?",
						MessageBoxButtons.YesNo
					) == DialogResult.Yes
				)
					button1.PerformClick();
		}
	}
}
