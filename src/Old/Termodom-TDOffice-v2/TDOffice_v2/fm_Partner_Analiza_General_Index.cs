using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2
{
	public partial class fm_Partner_Analiza_General_Index : Form
	{
		private static int[] _profiliHobiList { get; set; } =
			new int[12] { 6101, 6102, 6103, 6104, 6105, 6106, 6107, 6108, 6109, 6111, 6112, 6113 };
		private static int[] _profiliStandardList { get; set; } =
			new int[29]
			{
				90,
				91,
				570,
				572,
				573,
				574,
				577,
				579,
				580,
				582,
				583,
				588,
				590,
				592,
				597,
				1094,
				1096,
				1097,
				2034,
				2198,
				2281,
				2282,
				2284,
				2342,
				2343,
				2497,
				3213,
				3342,
				3486
			};
		private static int[] _profiliProfiList { get; set; } =
			new int[13]
			{
				836,
				862,
				1807,
				1808,
				2331,
				2496,
				3005,
				5286,
				6977,
				7397,
				7398,
				7399,
				7400
			};

		private static int[] _revizijeList { get; set; } =
			new int[7] { 167, 2953, 3410, 4741, 6303, 7020, 4136 };

		private static int[] _gkpList { get; set; } =
			new int[18]
			{
				30,
				32,
				34,
				38,
				39,
				42,
				51,
				56,
				62,
				65,
				66,
				838,
				1131,
				3120,
				5097,
				6968,
				7259,
				7273
			};
		private static int[] _gkpIzolacijaList { get; set; } =
			new int[21]
			{
				7017,
				7016,
				6185,
				6186,
				6144,
				6143,
				1919,
				2204,
				305,
				301,
				2185,
				294,
				2006,
				6972,
				2482,
				1725,
				398,
				400,
				1506,
				5957,
				6183
			};

		private static int[] _lepakFasadniHobiList { get; set; } = new int[1] { 453 };
		private static int[] _lepakFasadniStandardList { get; set; } =
			new int[4] { 437, 2732, 6817, 6543 };
		private static int[] _lepakFasadniProfiList { get; set; } = new int[1] { 3253 };

		private int? predefinedPPID { get; set; } = null;
		private Task<List<Komercijalno.Partner>> _komercijalnoPartneri { get; set; } =
			Komercijalno.Partner.ListAsync(DateTime.Now.Year);
		private _7_fm_Komercijalno_Roba_Kartica _fmKarticaRobe { get; set; } = null;

		private DataTable robaDT = new DataTable();
		private DataTable dataGridViewDataTable { get; set; } = null;

		public fm_Partner_Analiza_General_Index()
		{
			if (!Program.TrenutniKorisnik.ImaPravo(170200))
			{
				TDOffice.Pravo.NematePravoObavestenje(170200);
				this.Close();
				return;
			}
			InitializeComponent();

			partner_cmb.Enabled = false;
			partner_cmb.Items.Add("Ucitavanje...");
			partner_cmb.SelectedIndex = 0;
		}

		public fm_Partner_Analiza_General_Index(int ppid)
		{
			InitializeComponent();

			this.predefinedPPID = ppid;

			partner_cmb.Enabled = false;
			partner_cmb.Items.Add("Ucitavanje...");
			partner_cmb.SelectedIndex = 0;

			analiziraj_btn.Visible = false;
		}

		private void fm_Partner_Analiza_General_Index_Load(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				_komercijalnoPartneri.Wait();
				_komercijalnoPartneri.Result.Add(
					new Komercijalno.Partner() { PPID = -1, Naziv = " <<< Izaberi Partnera >>> " }
				);
				_komercijalnoPartneri.Result.RemoveAll(x => string.IsNullOrWhiteSpace(x.Naziv));
				_komercijalnoPartneri.Result.Sort((x, y) => x.PPID.CompareTo(y.PPID));

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							partner_cmb.DisplayMember = "Naziv";
							partner_cmb.ValueMember = "PPID";
							partner_cmb.DataSource = _komercijalnoPartneri.Result;

							if (predefinedPPID != null)
							{
								partner_cmb.SelectedValue = predefinedPPID;
								partner_cmb.Enabled = false;
								analiziraj_btn_Click(null, null);
							}
							else
							{
								partner_cmb.SelectedValue = -1;
								partner_cmb.Enabled = true;
							}
						}
				);
			});
		}

		private void FilterCtrlA()
		{
			string selectString = "";
			string input = textBox1.Text;
			string[] inputElemets = input.Split('+');

			foreach (object o in comboBox1.Items)
			{
				for (int i = 0; i < inputElemets.Length; i++)
					selectString +=
						"CONVERT("
						+ o.ToString()
						+ ", System.String) LIKE '%"
						+ inputElemets[i]
						+ "%' AND ";

				selectString = selectString.Remove(selectString.Length - 4);
				selectString += " OR ";
			}

			selectString = selectString.Remove(selectString.Length - 4);

			DataRow[] rows = robaDT.Copy().Select(selectString);
			dataGridViewDataTable =
				rows == null || rows.Count() == 0 ? null : rows.CopyToDataTable();

			UpdateDGV();
		}

		private void FilterEnter()
		{
			roba_dgv.ClearSelection();
			string kolona = comboBox1.SelectedItem.ToString();
			string input = textBox1.Text;

			if (string.IsNullOrWhiteSpace(input))
			{
				roba_dgv.FirstDisplayedScrollingRowIndex = 0;
				roba_dgv.Rows[0].Selected = true;
				roba_dgv.Focus();
				roba_dgv.CurrentCell = roba_dgv.Rows[0].Cells["NAZIV"];
				return;
			}

			foreach (DataGridViewRow row in roba_dgv.Rows)
			{
				string vrednostCelije = row.Cells[kolona].Value.ToString();
				if (vrednostCelije.ToLower().IndexOf(input.ToLower()) == 0)
				{
					roba_dgv.FirstDisplayedScrollingRowIndex = row.Index > 0 ? row.Index - 1 : 0;
					roba_dgv.Rows[row.Index].Selected = true;
					roba_dgv.Focus();
					roba_dgv.CurrentCell = roba_dgv.Rows[row.Index].Cells["Naziv"];
					return;
				}
			}
		}

		private void UpdateDGV()
		{
			roba_dgv.DataSource = dataGridViewDataTable;

			if (roba_dgv.Rows.Count == 0)
				return;

			roba_dgv.SuspendLayout();

			roba_dgv.Columns["ROBAID"].Visible = false;

			roba_dgv.Columns["NAZIV"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

			roba_dgv.Columns["KOLICINE"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			roba_dgv.Columns["KOLICINE"].DefaultCellStyle.Alignment =
				DataGridViewContentAlignment.MiddleRight;
			roba_dgv.Columns["KOLICINE"].DefaultCellStyle.Format = "#,##0.##";

			roba_dgv.Columns["JM"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

			roba_dgv.ResumeLayout();
		}

		public static Double OstvarenaRazlika(int godina, int ppid)
		{
			double R = 0;
			using (
				FbConnection con = new FbConnection(
					Komercijalno.Komercijalno.CONNECTION_STRING[godina]
				)
			)
			{
				con.Open();
				using (
					FbCommand cmd = new FbCommand(
						"Select SUM((s.PRODCENABP - s.NABAVNACENA) * s.KOLICINA) AS RAZLIKA from Dokument d join stavka s on d.brdok = s.brdok and d.vrdok = s.vrdok where d.VRDOK = 15 and D.PPID = @PPID",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@PPID", ppid);
					using (FbDataReader dr = cmd.ExecuteReader())
					{
						if (dr.Read())
							R =
								dr["RAZLIKA"] is DBNull
									? 0
									: (double)Convert.ToDouble(dr["RAZLIKA"]);
					}
				}
			}
			return R;
		}

		private void analiziraj_btn_Click(object sender, EventArgs e)
		{
			int PPID = Convert.ToInt32(partner_cmb.SelectedValue);
			if (PPID == -1)
			{
				MessageBox.Show("Niste izabrali partnera.Izaberite partnera");
				return;
			}
			List<string> output = new List<string>();
			//DataTable robaDT = new DataTable();
			using (
				FbConnection con = new FbConnection(
					Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				using (
					FbCommand cmd = new FbCommand(
						@"SELECT
s.ROBAID, r.NAZIV, SUM(s.KOLICINA) AS KOLICINE, r.JM
FROM STAVKA s
LEFT OUTER JOIN ROBA r ON r.ROBAID = s.ROBAID
LEFT OUTER JOIN DOKUMENT d ON s.VRDOK = d.VRDOK AND s.BRDOK = d.BRDOK
WHERE (s.VRDOK = 13 OR s.VRDOK = 15) AND d.PPID = @PPID
GROUP BY s.ROBAID, r.NAZIV, r.JM
ORDER BY s.ROBAID",
						con
					)
				)
				{
					cmd.Parameters.Add("@PPID", PPID);

					using (FbDataAdapter da = new FbDataAdapter(cmd))
					{
						da.Fill(robaDT);

						roba_dgv.SuspendLayout();
						roba_dgv.DataSource = robaDT;

						roba_dgv.Columns["ROBAID"].Visible = false;

						roba_dgv.Columns["NAZIV"].AutoSizeMode =
							DataGridViewAutoSizeColumnMode.AllCells;

						roba_dgv.Columns["KOLICINE"].AutoSizeMode =
							DataGridViewAutoSizeColumnMode.AllCells;
						roba_dgv.Columns["KOLICINE"].DefaultCellStyle.Alignment =
							DataGridViewContentAlignment.MiddleRight;
						roba_dgv.Columns["KOLICINE"].DefaultCellStyle.Format = "#,##0.##";

						roba_dgv.Columns["JM"].AutoSizeMode =
							DataGridViewAutoSizeColumnMode.AllCells;

						roba_dgv.ResumeLayout();
					}
				}
			}
			foreach (DataColumn col in robaDT.Columns)
				comboBox1.Items.Add(col.ColumnName);

			comboBox1.SelectedItem = "NAZIV";

			DataTable prometDT = new DataTable();
			prometDT.Columns.Add("PODATAK", typeof(string));
			foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x))
				prometDT.Columns.Add(godina.ToString(), typeof(string));

			DataRow prometDR = prometDT.NewRow();
			prometDR["PODATAK"] = "Promet";
			foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x))
			{
				using (
					FbConnection con = new FbConnection(
						Komercijalno.Komercijalno.CONNECTION_STRING[godina]
					)
				)
				{
					con.Open();
					using (
						FbCommand cmd = new FbCommand(
							"SELECT COALESCE(SUM(POTRAZUJE), 0) FROM DOKUMENT WHERE (VRDOK = 15 OR VRDOK = 13) AND PPID = @PPID",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@PPID", PPID);

						using (FbDataReader dr = cmd.ExecuteReader())
						//prometDR[godina.ToString()] = dr.Read() ? Convert.ToDouble(dr[0]) : 0;
						{
							double dd = dr.Read() ? Convert.ToDouble(dr[0]) : 0;
							string st = dd.ToString("N");
							prometDR[godina.ToString()] = st;
						}
					}
				}
			}
			prometDT.Rows.Add(prometDR);

			DataRow rabatDR = prometDT.NewRow();
			rabatDR["PODATAK"] = "Odobreni Rabat (vrednost)";
			foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x))
			{
				using (
					FbConnection con = new FbConnection(
						Komercijalno.Komercijalno.CONNECTION_STRING[godina]
					)
				)
				{
					con.Open();
					using (
						FbCommand cmd = new FbCommand(
							"SELECT COALESCE(SUM(DUGUJE - POTRAZUJE), 0) FROM DOKUMENT WHERE (VRDOK = 15 OR VRDOK = 13) AND PPID = @PPID",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@PPID", PPID);

						using (FbDataReader dr = cmd.ExecuteReader())
						//rabatDR[godina.ToString()] = dr.Read() ? Convert.ToDouble(dr[0]) : 0;
						{
							double OR = dr.Read() ? Convert.ToDouble(dr[0]) : 0;
							string stOR = OR.ToString("N");
							rabatDR[godina.ToString()] = stOR;
						}
					}
				}
			}
			prometDT.Rows.Add(rabatDR);

			promet_dgv.DataSource = prometDT;

			DataRow rabatDRp = prometDT.NewRow();
			rabatDRp["PODATAK"] = "Odobreni Rabat (procenat)";
			foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x))
			{
				double ORP =
					rabatDR[godina.ToString()] is DBNull || prometDR[godina.ToString()] is DBNull
						? 0
						: (
							(Convert.ToDouble(rabatDR[godina.ToString()]))
							/ Convert.ToDouble(prometDR[godina.ToString()])
						); // (((double)rabatDR[godina.ToString()]) / (double)prometDR[godina.ToString()]) * 100;
				string stORP = ORP.ToString("P");
				rabatDRp[godina.ToString()] = stORP;
			}
			//rabatDRp[godina.ToString()] = rabatDR[godina.ToString()] is DBNull || prometDR[godina.ToString()] is DBNull ? 0 : (((double)rabatDR[godina.ToString()]) / (double)prometDR[godina.ToString()]) * 100;
			//rabatDRp[godina.ToString()] = rabatDR[godina.ToString()] is DBNull || prometDR[godina.ToString()] is DBNull ? 0 : ((Convert.ToDouble(rabatDR[godina.ToString()])) / Convert.ToDouble(prometDR[godina.ToString()])) * 100;
			prometDT.Rows.Add(rabatDRp);

			DataRow brojKupovina = prometDT.NewRow();
			brojKupovina["PODATAK"] = "Broj kupovina";
			foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x))
			{
				using (
					FbConnection con = new FbConnection(
						Komercijalno.Komercijalno.CONNECTION_STRING[godina]
					)
				)
				{
					con.Open();
					using (
						FbCommand cmd = new FbCommand(
							"SELECT count(*) as brk FROM DOKUMENT WHERE VRDOK = 15 AND PPID = @PPID",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@PPID", PPID);

						using (FbDataReader dr = cmd.ExecuteReader())
							brojKupovina[godina.ToString()] = dr.Read()
								? Convert.ToInt32(dr["brk"])
								: 0;
					}
				}
			}

			prometDT.Rows.Add(brojKupovina);

			DataRow datumPoslednjeKupovine = prometDT.NewRow();
			datumPoslednjeKupovine["PODATAK"] = "Datum poslednje kupovine";
			foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x))
			{
				using (
					FbConnection con = new FbConnection(
						Komercijalno.Komercijalno.CONNECTION_STRING[godina]
					)
				)
				{
					con.Open();
					using (
						FbCommand cmd = new FbCommand(
							"SELECT MAX(DATUM) as dat FROM DOKUMENT WHERE VRDOK = 15 AND PPID = @PPID",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@PPID", PPID);

						using (FbDataReader dr = cmd.ExecuteReader())
							datumPoslednjeKupovine[godina.ToString()] = dr.Read() ? dr["dat"] : 0;
					}
				}
			}

			prometDT.Rows.Add(datumPoslednjeKupovine);

			DataRow ostvarenaRazlika = prometDT.NewRow();
			ostvarenaRazlika["PODATAK"] = "Ostvarena razlika partnera";
			foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x))
			{
				double OR = OstvarenaRazlika(godina, PPID);
				string stOR = OR.ToString("N");
				ostvarenaRazlika[godina.ToString()] = stOR;
			}

			prometDT.Rows.Add(ostvarenaRazlika);

			promet_dgv.Columns["PODATAK"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
			foreach (int godina in Komercijalno.Komercijalno.CONNECTION_STRING.Keys.OrderBy(x => x))
			{
				promet_dgv.Columns[godina.ToString()].DefaultCellStyle.Format = "#,##0.00";
				promet_dgv.Columns[godina.ToString()].DefaultCellStyle.Alignment =
					DataGridViewContentAlignment.MiddleRight;
				promet_dgv.Columns[godina.ToString()].AutoSizeMode =
					DataGridViewAutoSizeColumnMode.AllCells;
			}

			double ukupnoHobiProfila = robaDT
				.Select($"ROBAID IN ({string.Join(", ", _profiliHobiList)})")
				.AsEnumerable()
				.Sum(x => Convert.ToDouble(x["KOLICINE"]));
			double ukupnoStandardProfila = robaDT
				.Select($"ROBAID IN ({string.Join(", ", _profiliStandardList)})")
				.AsEnumerable()
				.Sum(x => Convert.ToDouble(x["KOLICINE"]));
			double ukupnoProfiProfila = robaDT
				.Select($"ROBAID IN ({string.Join(", ", _profiliProfiList)})")
				.AsEnumerable()
				.Sum(x => Convert.ToDouble(x["KOLICINE"]));
			double ukupnoProfila = ukupnoHobiProfila + ukupnoStandardProfila + ukupnoProfiProfila;

			if (ukupnoProfila > 0)
			{
				output.Add("======================");

				output.Add(
					"Udeo HOBI profila: " + (ukupnoHobiProfila / ukupnoProfila).ToString("0.00%")
				);
				output.Add(
					"Udeo STANDARD profila: "
						+ (ukupnoStandardProfila / ukupnoProfila).ToString("0.00%")
				);
				output.Add(
					"Udeo PROFI profila: " + (ukupnoProfiProfila / ukupnoProfila).ToString("0.00%")
				);
				output.Add("");
				output.Add("Minimalni odnos (Hobi / Standard / Profi) = 0% / 30% / 70%");
				output.Add("======================");
			}

			double ukupnoGKP = robaDT
				.Select($"ROBAID IN ({string.Join(", ", _gkpList)})")
				.AsEnumerable()
				.Sum(x => Convert.ToDouble(x["KOLICINE"]));
			double ukupnoRevizija = robaDT
				.Select($"ROBAID IN ({string.Join(", ", _revizijeList)})")
				.AsEnumerable()
				.Sum(x => Convert.ToDouble(x["KOLICINE"]));
			double ukupnoGKPSrafova =
				robaDT
					.Select($"NAZIV LIKE '%FOSFAT%'")
					.AsEnumerable()
					.Sum(x => Convert.ToDouble(x["KOLICINE"]))
				+ robaDT
					.Select($"NAZIV LIKE '%1000/1%'")
					.AsEnumerable()
					.Sum(x => Convert.ToDouble(x["KOLICINE"]) * 1000);

			if (ukupnoGKPSrafova / ukupnoGKP < 13)
			{
				output.Add("");
				output.Add("Prodaja srafova za gips je premala!");
				output.Add("");
			}
			if ((ukupnoGKP / 35) > ukupnoRevizija)
			{
				output.Add("");
				output.Add("Prodaja revizionih otvora za ovog partnera je premala!");
				output.Add("");
			}

			double ukupnoGKPIzolacije = robaDT
				.Select($"ROBAID IN ({string.Join(", ", _gkpIzolacijaList)})")
				.AsEnumerable()
				.Sum(x => Convert.ToDouble(x["KOLICINE"]));

			if (ukupnoGKP * 0.8 > ukupnoGKPIzolacije)
			{
				output.Add("");
				output.Add("Prodaja izolacije koja ide unutar GKP sisteme je premala!");
				output.Add("");
			}

			double ukupnoHobiFasadnogLepka = robaDT
				.Select($"ROBAID IN ({string.Join(", ", _lepakFasadniHobiList)})")
				.AsEnumerable()
				.Sum(x => Convert.ToDouble(x["KOLICINE"]));
			double ukupnoStandardFasadnogLepka = robaDT
				.Select($"ROBAID IN ({string.Join(", ", _lepakFasadniStandardList)})")
				.AsEnumerable()
				.Sum(x => Convert.ToDouble(x["KOLICINE"]));
			double ukupnoProfiFasadnogLepka = robaDT
				.Select($"ROBAID IN ({string.Join(", ", _lepakFasadniProfiList)})")
				.AsEnumerable()
				.Sum(x => Convert.ToDouble(x["KOLICINE"]));
			double ukupnoFasadnogLepka =
				ukupnoHobiFasadnogLepka + ukupnoStandardFasadnogLepka + ukupnoProfiFasadnogLepka;

			if (ukupnoFasadnogLepka > 0)
			{
				output.Add("======================");

				output.Add(
					"Udeo HOBI fasadnog lepka: "
						+ (ukupnoHobiFasadnogLepka / ukupnoFasadnogLepka).ToString("0.00%")
				);
				output.Add(
					"Udeo STANDARD fasadnog lepka: "
						+ (ukupnoStandardFasadnogLepka / ukupnoFasadnogLepka).ToString("0.00%")
				);
				output.Add(
					"Udeo PROFI fasadnog lepka: "
						+ (ukupnoProfiFasadnogLepka / ukupnoFasadnogLepka).ToString("0.00%")
				);
				output.Add("");
				output.Add("Minimalni odnos (Hobi / Standard / Profi) = 0% / 30% / 70%");
				output.Add("======================");
			}

			analiza_rtb.Text = string.Join(Environment.NewLine, output);

			Task.Run(() =>
			{
				/// Ovo koristim da bih azuzirao da je korisnik izvrsio ovaj zadatak
				/// Potrebno je da barem 10 sekundi gleda u izvestaj da bi se zadatak smatrao izvrsenim
				DateTime checkPoint = DateTime.Now;
				Thread.Sleep(TimeSpan.FromSeconds(10));

				if (!this.IsDisposed)
				{
					List<TDOffice.CheckListItem> korisnikZadaci =
						TDOffice.CheckListItem.ListByKorisnikID(Program.TrenutniKorisnik.ID);
					TDOffice.CheckListItem item = korisnikZadaci.FirstOrDefault(x =>
						x.Job == TDOffice.CheckList.Jobs.DetaljnaAnalizaPartnera
					);
					if (item != null)
					{
						item.DatumIzvrsenja = checkPoint;
						item.Update();
					}
				}
			});
		}

		private void podesavanjeSetovaProizvoda_btn_Click(object sender, EventArgs e)
		{
			MessageBox.Show(
				"Jos uvek nije uradjeno. Za sada su zakucani proizvodi sa kojima radim analizu ali ce biti u listama"
			);
		}

		private void karticaRobeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int robaID = Convert.ToInt32(
				roba_dgv.Rows[roba_dgv.SelectedCells[0].RowIndex].Cells["ROBAID"].Value
			);

			if (_fmKarticaRobe == null || _fmKarticaRobe.IsDisposed)
				_fmKarticaRobe = new _7_fm_Komercijalno_Roba_Kartica();

			_fmKarticaRobe.UcitajKarticu(robaID, 50);
			_fmKarticaRobe.TopMost = true;
			_fmKarticaRobe.Show();
		}

		private void roba_dgv_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
		{
			roba_dgv.ClearSelection();
			if (e.RowIndex >= 0)
				roba_dgv.Rows[e.RowIndex].Selected = true;

			if (_fmKarticaRobe != null && _fmKarticaRobe.Visible)
			{
				int robaID = Convert.ToInt32(roba_dgv.SelectedRows[0].Cells["ROBAID"].Value);
				_fmKarticaRobe.UcitajKarticu(robaID, 50);
			}
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
			{
				FilterEnter();
				roba_dgv.Focus();
			}
			else if (e.Control && e.KeyCode == Keys.A)
			{
				FilterCtrlA();
				roba_dgv.Focus();
			}
		}
	}
}
