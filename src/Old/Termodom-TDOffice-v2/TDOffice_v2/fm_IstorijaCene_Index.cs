using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace TDOffice_v2
{
	public partial class fm_IstorijaCene_Index : Form
	{
		private Komercijalno.Roba _roba { get; set; }
		private static int[] _vrDokNabavke = new int[] { 0, 1, 2, 3, 36 };
		private Task<List<Komercijalno.Dokument>> _dokumentiNabavke { get; set; } =
			Task.Run<List<Komercijalno.Dokument>>(() =>
			{
				List<Komercijalno.Dokument> list = new List<Komercijalno.Dokument>();
				list.AddRange(
					Komercijalno
						.Dokument.ListByMagacinID(DateTime.Now.Year, 50)
						.Where(x => _vrDokNabavke.Contains(x.VrDok))
						.ToList()
				);
				return list;
			});

		private Task<List<Komercijalno.Dokument>> _dokumentiNivelacije { get; set; } =
			Task.Run<List<Komercijalno.Dokument>>(() =>
			{
				List<Komercijalno.Dokument> list = new List<Komercijalno.Dokument>();
				list.AddRange(
					Komercijalno
						.Dokument.ListByMagacinID(DateTime.Now.Year, 50)
						.Where(x => new[] { 0, 21 }.Contains(x.VrDok))
						.ToList()
				);
				return list;
			});
		private Task<List<Komercijalno.Dokument>> _dokumentiProdaje { get; set; } =
			Task.Run<List<Komercijalno.Dokument>>(() =>
			{
				List<Komercijalno.Dokument> list = new List<Komercijalno.Dokument>();
				list.AddRange(Komercijalno.Dokument.ListByVrDok(DateTime.Now.Year, 15));
				return list;
			});
		private Task<List<Komercijalno.Stavka>> stavkeNivelacija { get; set; }
		private Task<List<Komercijalno.Stavka>> stavkeNabavke { get; set; }
		private Task<List<Komercijalno.Stavka>> stavkeProdaje { get; set; }

		private Task<List<Komercijalno.Stavka>> _stavkeRobe { get; set; }

		private string[] _meseci = new string[]
		{
			"Januar",
			"Februar",
			"Mart",
			"April",
			"Maj",
			"Jun",
			"Jul",
			"Avgust",
			"Septembar",
			"Oktobar",
			"Novembar",
			"Decembar"
		};
		private bool _isNedeljniGrafikon { get; set; } = true;

		public fm_IstorijaCene_Index(int robaID)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(169000))
			{
				TDOffice.Pravo.NematePravoObavestenje(169000);
				this.Close();
				return;
			}
			InitializeComponent();

			_roba = Komercijalno.Roba.Get(DateTime.Now.Year, robaID);

			_stavkeRobe = Task.Run<List<Komercijalno.Stavka>>(() =>
			{
				return Komercijalno.Stavka.ListByRobaID(robaID);
			});

			stavkeNivelacija = Task.Run<List<Komercijalno.Stavka>>(() =>
			{
				return _stavkeRobe
					.Result.Where(x => x.MagacinID == 50 && new[] { 21, 0 }.Contains(x.VrDok))
					.ToList();
			});
			stavkeNabavke = Task.Run<List<Komercijalno.Stavka>>(() =>
			{
				return _stavkeRobe
					.Result.Where(x => x.MagacinID == 50 && _vrDokNabavke.Contains(x.VrDok))
					.ToList();
			});
			stavkeProdaje = Task.Run<List<Komercijalno.Stavka>>(() =>
			{
				return _stavkeRobe.Result.Where(x => new[] { 15 }.Contains(x.VrDok)).ToList();
			});
			UcitajPodatke();
		}

		private void UcitajPodatke()
		{
			Task<Dictionary<int, double>> prodajaUperiodu = Task.Run<Dictionary<int, double>>(() =>
			{
				Dictionary<int, double> prodajaUPeriodu = new Dictionary<int, double>();

				Parallel.ForEach(
					stavkeProdaje.Result,
					new ParallelOptions() { MaxDegreeOfParallelism = 100 },
					s =>
					{
						Komercijalno.Dokument dok = _dokumentiProdaje.Result.FirstOrDefault(x =>
							x.VrDok == s.VrDok && x.BrDok == s.BrDok
						);

						int nPer = _isNedeljniGrafikon
							? (int)(
								(dok.Datum.Date - new DateTime(DateTime.Now.Year, 1, 1)).TotalDays
								/ 7
							)
							: dok.Datum.Month;

						lock (prodajaUPeriodu)
						{
							if (prodajaUPeriodu.ContainsKey(nPer))
								prodajaUPeriodu[nPer] += s.Kolicina;
							else
								prodajaUPeriodu[nPer] = s.Kolicina;
						}
					}
				);

				return prodajaUPeriodu;
			});

			DataTable dt = new DataTable();
			dt.Columns.Add("ID", typeof(int));
			dt.Columns.Add("VrDok", typeof(int));
			dt.Columns.Add("BrDok", typeof(int));
			dt.Columns.Add("Datum", typeof(DateTime));
			dt.Columns.Add("Kolicina", typeof(double));
			dt.Columns.Add("NabavnaCena", typeof(double));

			chart1.Legends.Clear();
			chart1.Series.Clear();
			chart1.ChartAreas.Clear();

			chart1.ChartAreas.Add("Default");
			chart1.ChartAreas.Add("Prodaja");

			chart1.Series.Add("NabavnaCena");
			chart1.Series.Add("ProdajnaCena");
			chart1.Series.Add("ProdajaKolicina");

			chart1.Series["NabavnaCena"].IsVisibleInLegend = false;
			chart1.Series["NabavnaCena"].ChartType = System
				.Windows
				.Forms
				.DataVisualization
				.Charting
				.SeriesChartType
				.StepLine;
			chart1.Series["NabavnaCena"].BorderWidth = 5;
			chart1.Series["NabavnaCena"].IsValueShownAsLabel = true;
			chart1.Series["NabavnaCena"].Color = Color.Orange;
			chart1.Series["NabavnaCena"].LabelForeColor = Color.Orange;
			chart1.Series["NabavnaCena"].Font = new Font(
				chart1.Series["NabavnaCena"].Font,
				FontStyle.Bold
			);
			chart1.Series["NabavnaCena"].ChartArea = "Default";
			chart1.Series["NabavnaCena"].LabelFormat = "#,##0";

			chart1.Series["ProdajnaCena"].IsVisibleInLegend = false;
			chart1.Series["ProdajnaCena"].ChartType = System
				.Windows
				.Forms
				.DataVisualization
				.Charting
				.SeriesChartType
				.StepLine;
			chart1.Series["ProdajnaCena"].BorderWidth = 5;
			chart1.Series["ProdajnaCena"].IsValueShownAsLabel = true;
			chart1.Series["ProdajnaCena"].Color = Color.Green;
			chart1.Series["ProdajnaCena"].LabelForeColor = Color.Green;
			chart1.Series["ProdajnaCena"].Font = new Font(
				chart1.Series["NabavnaCena"].Font,
				FontStyle.Bold
			);
			chart1.Series["ProdajnaCena"].ChartArea = "Default";
			chart1.Series["ProdajnaCena"].LabelFormat = "#,##0";

			chart1.Series["ProdajaKolicina"].IsVisibleInLegend = false;
			chart1.Series["ProdajaKolicina"].ChartType = System
				.Windows
				.Forms
				.DataVisualization
				.Charting
				.SeriesChartType
				.Column;
			chart1.Series["ProdajaKolicina"].BorderWidth = 5;
			chart1.Series["ProdajaKolicina"].IsValueShownAsLabel = true;
			chart1.Series["ProdajaKolicina"].Color = Color.Green;
			chart1.Series["ProdajaKolicina"].LabelForeColor = Color.Green;
			chart1.Series["ProdajaKolicina"].Font = new Font(
				chart1.Series["ProdajaKolicina"].Font,
				FontStyle.Bold
			);
			chart1.Series["ProdajaKolicina"].ChartArea = "Prodaja";
			chart1.Series["ProdajaKolicina"].LabelFormat = "#,##0";

			chart1.ChartAreas["Default"].AxisX.ScaleView.Zoomable = true;
			chart1.ChartAreas["Default"].AxisX.ScaleView.SizeType = System
				.Windows
				.Forms
				.DataVisualization
				.Charting
				.DateTimeIntervalType
				.Number;
			chart1.ChartAreas["Default"].AxisX.Minimum = 0;
			chart1.ChartAreas["Default"].AxisX.Maximum = _isNedeljniGrafikon ? 53 : 13;
			Task.Run(async () =>
			{
				await stavkeNabavke;
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							chart1.ChartAreas["Default"].AxisY.Maximum =
								(int)stavkeNivelacija.Result.Max(x => x.ProdajnaCena) * 1.2;
						}
				);
			});
			chart1.ChartAreas["Default"].CursorX.AutoScroll = true;
			chart1.ChartAreas["Default"].AxisX.ScaleView.Zoom(0, (int)numericUpDown1.Value);
			chart1.ChartAreas["Default"].AxisX.Interval = 1;
			chart1.ChartAreas["Default"].AxisY.Title = "Nabavna cena";

			chart1.ChartAreas["Prodaja"].AxisX.ScaleView.Zoomable = true;
			chart1.ChartAreas["Prodaja"].AxisX.ScaleView.SizeType = System
				.Windows
				.Forms
				.DataVisualization
				.Charting
				.DateTimeIntervalType
				.Number;
			chart1.ChartAreas["Prodaja"].AxisX.Minimum = 0;
			chart1.ChartAreas["Prodaja"].AxisX.Maximum = _isNedeljniGrafikon ? 53 : 13;
			chart1.ChartAreas["Prodaja"].CursorX.AutoScroll = true;
			chart1.ChartAreas["Prodaja"].AxisX.ScaleView.Zoom(0, (int)numericUpDown1.Value);
			chart1.ChartAreas["Prodaja"].AxisX.Interval = 1;
			chart1.ChartAreas["Prodaja"].AxisY.Title =
				"Ukupno prodato kolicinski VRDOK 15 (" + _roba.JM + ")";

			int nNedelja = (int)(
				(
					new DateTime(DateTime.Now.Year, 12, 31) - new DateTime(DateTime.Now.Year, 1, 1)
				).TotalDays / 7
			);

			for (int i = 0; i < (_isNedeljniGrafikon ? nNedelja : 12); i++)
			{
				DateTime end = new DateTime(TimeSpan.FromDays((i + 1) * 7).Ticks);
				DateTime start = new DateTime(TimeSpan.FromDays(i * 7).Ticks);

				chart1
					.ChartAreas[0]
					.AxisX.CustomLabels.Add(
						new CustomLabel(
							i,
							i + 1,
							_isNedeljniGrafikon
								? "Nedelja "
									+ (i + 1)
									+ Environment.NewLine
									+ start.ToString("dd.MM")
									+ " - "
									+ end.ToString("dd.MM")
								: _meseci[i],
							0,
							LabelMarkStyle.Box
						)
					);
				chart1
					.ChartAreas[1]
					.AxisX.CustomLabels.Add(
						new CustomLabel(
							i,
							i + 1,
							_isNedeljniGrafikon
								? "Nedelja "
									+ (i + 1)
									+ Environment.NewLine
									+ start.ToString("dd.MM")
									+ " - "
									+ end.ToString("dd.MM")
								: _meseci[i],
							0,
							LabelMarkStyle.Box
						)
					);
			}

			double poslednjaCena = 0;
			double realnaPoslednjaNabavna = 0;
			double realnaPoslednjaProdajna = 0;
			List<Tuple<double, double>> items = new List<Tuple<double, double>>();
			List<Tuple<double, double>> items1 = new List<Tuple<double, double>>();
			foreach (Komercijalno.Stavka s in stavkeNabavke.Result)
			{
				Komercijalno.Dokument dok = _dokumentiNabavke.Result.FirstOrDefault(x =>
					x.VrDok == s.VrDok && x.BrDok == s.BrDok
				);

				DataRow dr = dt.NewRow();
				dr["ID"] = s.StavkaID;
				dr["VrDok"] = s.VrDok;
				dr["BrDok"] = s.BrDok;
				dr["Datum"] = dok.Datum;
				dr["Kolicina"] = s.Kolicina;
				dr["NabavnaCena"] = s.NabavnaCena;
				dt.Rows.Add(dr);

				double minCen = s.NabavnaCena * 0.91;
				double maxCena = s.NabavnaCena * 1.01;
				realnaPoslednjaNabavna = s.NabavnaCena;
				if (poslednjaCena < minCen || poslednjaCena > maxCena)
				{
					items.Add(
						new Tuple<double, double>(
							(dok.Datum.Date - new DateTime(DateTime.Now.Year, 1, 1)).TotalDays
								/ (_isNedeljniGrafikon ? 7 : 30.5),
							s.NabavnaCena
						)
					);
					poslednjaCena = s.NabavnaCena;
				}
			}

			foreach (Komercijalno.Stavka s in stavkeNivelacija.Result)
			{
				Komercijalno.Dokument dok = _dokumentiNivelacije.Result.FirstOrDefault(x =>
					x.BrDok == s.BrDok
				);

				double minCen = s.ProdajnaCena * 0.99;
				double maxCena = s.ProdajnaCena * 1.01;
				realnaPoslednjaProdajna = s.ProdajnaCena;
				if (poslednjaCena < minCen || poslednjaCena > maxCena)
				{
					items1.Add(
						new Tuple<double, double>(
							(dok.Datum.Date - new DateTime(DateTime.Now.Year, 1, 1)).TotalDays
								/ (_isNedeljniGrafikon ? 7 : 30.5),
							s.ProdajnaCena
						)
					);
					poslednjaCena = s.NabavnaCena;
				}
			}

			chart1.ChartAreas["Prodaja"].AxisY.Maximum =
				prodajaUperiodu.Result.Count == 0
					? 0
					: (int)(prodajaUperiodu.Result.Max(x => x.Value) * 1.5);

			for (int i = 0; i < nNedelja; i++)
				chart1
					.Series["ProdajaKolicina"]
					.Points.AddXY(
						i - 0.5,
						prodajaUperiodu.Result.ContainsKey(i) ? prodajaUperiodu.Result[i] : 0
					);

			foreach (Tuple<double, double> it in items.OrderBy(x => x.Item1))
				chart1.Series["NabavnaCena"].Points.AddXY(it.Item1, it.Item2);
			chart1
				.Series["NabavnaCena"]
				.Points.AddXY(
					(DateTime.Now - new DateTime(DateTime.Now.Year, 1, 1)).TotalDays
						/ (_isNedeljniGrafikon ? 7 : 30.5),
					realnaPoslednjaNabavna
				);

			foreach (Tuple<double, double> it in items1.OrderBy(x => x.Item1))
				chart1.Series["ProdajnaCena"].Points.AddXY(it.Item1, it.Item2);
			chart1
				.Series["ProdajnaCena"]
				.Points.AddXY(
					(DateTime.Now - new DateTime(DateTime.Now.Year, 1, 1)).TotalDays
						/ (_isNedeljniGrafikon ? 7 : 30.5),
					realnaPoslednjaProdajna
				);

			dataGridView1.DataSource = dt;

			dataGridView1.Columns["ID"].Visible = false;

			dataGridView1.Columns["VrDok"].Width = 50;
			dataGridView1.Columns["VrDok"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleCenter;

			dataGridView1.Columns["BrDok"].Width = 50;
			dataGridView1.Columns["BrDok"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleCenter;

			dataGridView1.Columns["Datum"].DefaultCellStyle.Format = "dd.MM.yyyy";
			dataGridView1.Columns["Datum"].Width = 70;

			dataGridView1.Columns["Kolicina"].HeaderText = "Kolicina";
			dataGridView1.Columns["Kolicina"].DefaultCellStyle.Format = "#,##0";
			dataGridView1.Columns["Kolicina"].Width = 80;
			dataGridView1.Columns["Kolicina"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;

			dataGridView1.Columns["NabavnaCena"].HeaderText = "Nabavna Cena";
			dataGridView1.Columns["NabavnaCena"].DefaultCellStyle.Format = "#,##0.00 RSD";
			dataGridView1.Columns["NabavnaCena"].Width = 80;
			dataGridView1.Columns["NabavnaCena"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;

			this.dataGridView1.Sort(
				this.dataGridView1.Columns["Datum"],
				ListSortDirection.Ascending
			);

			this.id_txt.Text = _roba.ID.ToString();
			this.katbr_txt.Text = _roba.KatBr;
			this.naziv_txt.Text = _roba.Naziv;
		}

		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			chart1.ChartAreas["Default"].AxisX.ScaleView.Zoom(0, (int)numericUpDown1.Value);
			chart1.ChartAreas["Prodaja"].AxisX.ScaleView.Zoom(0, (int)numericUpDown1.Value);
		}

		private void modGrafikona_btn_Click(object sender, EventArgs e)
		{
			_isNedeljniGrafikon = !_isNedeljniGrafikon;

			modGrafikona_btn.Text = _isNedeljniGrafikon
				? "Promeni na mesecni prikaz"
				: "Promeni na nedeljni prikaz";
			UcitajPodatke();
		}

		private void label1_Click(object sender, EventArgs e) { }
	}
}
