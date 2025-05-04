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
	public partial class fm_PatchLog_Index : Form
	{
		private TDOffice.Config<Dictionary<DateTime, string>> _patchLog = TDOffice.Config<
			Dictionary<DateTime, string>
		>.Get(TDOffice.ConfigParameter.PatchLog);

		public fm_PatchLog_Index()
		{
			InitializeComponent();
		}

		private void fm_PatchLog_Index_Load(object sender, EventArgs e)
		{
			if (_patchLog == null || _patchLog.Tag == null)
			{
				if (_patchLog == null)
					_patchLog = new TDOffice.Config<Dictionary<DateTime, string>>();

				if (_patchLog.Tag == null)
					_patchLog.Tag = new Dictionary<DateTime, string>();

				_patchLog.UpdateOrInsert();
			}

			List<string> output = new List<string>();

			foreach (DateTime key in _patchLog.Tag.Keys.OrderByDescending(x => x))
			{
				output.Add("");
				output.Add("==================");
				output.Add(key.ToString("dd.MM.yyyy [ HH:mm ]"));
				output.Add("==================");
				output.Add(_patchLog.Tag[key]);
				output.Add("==================");
				output.Add("");
			}

			richTextBox1.Text = string.Join(Environment.NewLine, output);

			LocalSettings.Settings.LastPatchLogSeen = DateTime.Now;
			LocalSettings.Update();
		}

		private void noviToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Program.TrenutniKorisnik.ID != 1)
			{
				MessageBox.Show("Nemate pravo!");
				return;
			}

			using (InputRichTextBox inp = new InputRichTextBox("Novi log", "Telo:"))
			{
				inp.ShowDialog();

				if (inp.DialogResult != DialogResult.OK)
				{
					MessageBox.Show("Obustavljam!");
					return;
				}

				if (string.IsNullOrWhiteSpace(inp.ReturnData))
				{
					MessageBox.Show("Obustavljam!");
					return;
				}

				_patchLog.Tag.Add(DateTime.Now, inp.ReturnData);
				_patchLog.UpdateOrInsert();

				MessageBox.Show("Insertovano!");
			}
		}
	}
}
