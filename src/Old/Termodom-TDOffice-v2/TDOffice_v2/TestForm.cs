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
	public partial class TestForm : Form
	{
		private Task<List<Tuple<string, int>>> data { get; set; }

		public TestForm()
		{
			InitializeComponent();

			data = UcitajDataAsync();
		}

		private void TestForm_Load(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				data.Wait();
				if (data.IsCompleted)
					OsveziChart();
			});
		}

		private Task<List<Tuple<string, int>>> UcitajDataAsync()
		{
			return Task.Run(() =>
			{
				List<Tuple<string, int>> list = new List<Tuple<string, int>>();
				List<Komercijalno.Dokument> doks = Komercijalno.Dokument.List();

				foreach (int vrDok in doks.Where(x => x.VrDok <= 5).Select(x => x.VrDok).Distinct())
					list.Add(
						new Tuple<string, int>(vrDok.ToString(), doks.Count(x => x.VrDok == vrDok))
					);

				return list;
			});
		}

		private void OsveziChart()
		{
			this.Invoke(
				(MethodInvoker)
					delegate
					{
						chart1.DataSource = data.Result;
						chart1.Series.First().XValueMember = "Item1";
						chart1.Series.First().YValueMembers = "Item2";

						chart1.Series.First().IsValueShownAsLabel = true;

						chart1.DataBind();
					}
			);
		}
	}
}
