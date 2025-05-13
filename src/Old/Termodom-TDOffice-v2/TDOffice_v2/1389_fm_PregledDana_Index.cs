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
	public partial class _1389_fm_PregledDana_Index : Form
	{
		private bool _loaded = false;
		private Task<fm_Help> _helpFrom { get; set; }

		public _1389_fm_PregledDana_Index()
		{
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.PregledDana_Index);

			magacin_cmb.DataSource = Komercijalno.Magacin.ListAsync().Result;
			magacin_cmb.DisplayMember = "Naziv";
			magacin_cmb.ValueMember = "ID";

			NamestiGrafikon();
		}

		private void _1389_fm_PregledDana_Index_Load(object sender, EventArgs e)
		{
			odDatuma_dtp.Value = DateTime.Now;
			magacin_cmb.SelectedValue = Program.TrenutniKorisnik.MagacinID;
			NamestiGrafikon();
			UcitajPodatke();
			_loaded = true;
		}

		private void NamestiGrafikon()
		{
			List<Tuple<int, double>> list = Komercijalno.Magacin.PrometList(
				new DateTime(
					odDatuma_dtp.Value.Year,
					odDatuma_dtp.Value.Month,
					odDatuma_dtp.Value.Day
				)
			);
			List<Tuple<string, double>> list1 = new List<Tuple<string, double>>();

			foreach (var i in list)
				list1.Add(new Tuple<string, double>(string.Format("M{0}", i.Item1), i.Item2));

			chart1.DataSource = list1;
			chart1.Series.First().XValueMember = "Item1";
			chart1.Series.First().YValueMembers = "Item2";

			chart1.Series[0].IsVisibleInLegend = false;

			chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
			chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

			chart1.ChartAreas[0].AxisY.LabelStyle.Enabled = false;

			chart1.ChartAreas[0].AxisX.Interval = 1;
			chart1.ChartAreas[0].AxisX.Maximum = 18;

			chart1.ChartAreas[0].AxisX.CustomLabels.Add(0.5, 1.5, "M12");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(1.5, 2.5, "M13");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(2.5, 3.5, "M14");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(3.5, 4.5, "M15");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(4.5, 5.5, "M16");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(5.5, 6.5, "M17");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(6.5, 7.5, "M18");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(7.5, 8.5, "M19");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(8.5, 9.5, "M20");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(9.5, 10.5, "M21");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(10.5, 11.5, "M22");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(11.5, 12.5, "M23");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(12.5, 13.5, "M24");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(13.5, 14.5, "M25");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(14.5, 15.5, "M26");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(15.5, 16.5, "M27");
			chart1.ChartAreas[0].AxisX.CustomLabels.Add(16.5, 17.5, "M28");
		}

		private void UcitajPodatke()
		{
			Tuple<int, double> mesecniPromet = Komercijalno.Magacin.Promet(
				Convert.ToInt32(magacin_cmb.SelectedValue),
				new DateTime(odDatuma_dtp.Value.Year, odDatuma_dtp.Value.Month, 1),
				new DateTime(
					odDatuma_dtp.Value.Year,
					odDatuma_dtp.Value.Month,
					DateTime.DaysInMonth(odDatuma_dtp.Value.Year, odDatuma_dtp.Value.Month)
				)
			);
			Tuple<int, double> danasnjiPromet = Komercijalno.Magacin.Promet(
				Convert.ToInt32(magacin_cmb.SelectedValue),
				odDatuma_dtp.Value
			);
			Tuple<int, double> prometGotovina = Komercijalno.Magacin.Promet(
				Convert.ToInt32(magacin_cmb.SelectedValue),
				odDatuma_dtp.Value,
				Komercijalno.NacinUplate.Gotovina
			);
			Tuple<int, double> prometVirman = Komercijalno.Magacin.Promet(
				Convert.ToInt32(magacin_cmb.SelectedValue),
				odDatuma_dtp.Value,
				Komercijalno.NacinUplate.Virman
			);
			Tuple<int, double> prometKartica = Komercijalno.Magacin.Promet(
				Convert.ToInt32(magacin_cmb.SelectedValue),
				odDatuma_dtp.Value,
				Komercijalno.NacinUplate.Kartica
			);
			Tuple<int, double> prometOdlozeno = Komercijalno.Magacin.Promet(
				Convert.ToInt32(magacin_cmb.SelectedValue),
				odDatuma_dtp.Value,
				Komercijalno.NacinUplate.Odlozeo
			);

			ukupanPrometM_txt.Text = mesecniPromet.Item2.ToString("#,##0.00 RSD");
			ukupanPromet_txt.Text = danasnjiPromet.Item2.ToString("#,##0.00 RSD");
			gotovina_txt.Text = prometGotovina.Item2.ToString("#,##0.00 RSD");
			virman_txt.Text = prometVirman.Item2.ToString("#,##0.00 RSD");
			kartica_txt.Text = prometKartica.Item2.ToString("#,##0.00 RSD");
			odlozeno_txt.Text = prometOdlozeno.Item2.ToString("#,##0.00 RSD");

			ucitaj_btn.Visible = false;
		}

		private void magacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			ucitaj_btn.Visible = true;
		}

		private void odDatuma_dtp_ValueChanged(object sender, EventArgs e)
		{
			if (!_loaded)
				return;

			ucitaj_btn.Visible = true;
		}

		private void ucitaj_btn_Click(object sender, EventArgs e)
		{
			NamestiGrafikon();
			UcitajPodatke();
		}
	}
}
