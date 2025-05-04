using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2
{
	public partial class fm_MojKupac_Index : Form
	{
		private Task<List<Komercijalno.Partner>> _partneriKomercijalno =
			Komercijalno.Partner.ListAsync(DateTime.Now.Year);
		private Task<TDOffice.Config<Dictionary<int, int>>> _okupiraniKupci = Task.Run(() =>
		{
			var c = TDOffice.Config<Dictionary<int, int>>.Get(
				TDOffice.ConfigParameter.ListaOkupiranihKupaca_MojKupac
			);
			if (c.Tag == null)
			{
				c.Tag = new Dictionary<int, int>();
				c.UpdateOrInsert();
			}
			return c;
		});

		private Task<TDOffice.Config<Dictionary<int, string>>> _beleskeKupaca = Task.Run(() =>
		{
			var c = TDOffice.Config<Dictionary<int, string>>.Get(
				TDOffice.ConfigParameter.BelekseKupaca_MojKupac
			);
			if (c.Tag == null)
			{
				c.Tag = new Dictionary<int, string>();
				c.UpdateOrInsert();
			}
			return c;
		});

		private int _trenutniPPID = 0;
		private int _trenutniKorisnik = Program.TrenutniKorisnik.ID;

		public fm_MojKupac_Index()
		{
			if (!Program.TrenutniKorisnik.ImaPravo(168000))
			{
				TDOffice.Pravo.NematePravoObavestenje(168000);
				this.Close();
				return;
			}
			InitializeComponent();
			List<Tuple<int, string>> korisnici = new List<Tuple<int, string>>();
			foreach (TDOffice.User u in TDOffice.User.List())
				korisnici.Add(new Tuple<int, string>(u.ID, u.Username));

			korisnik_cmb.DataSource = korisnici;
			korisnik_cmb.ValueMember = "Item1";
			korisnik_cmb.DisplayMember = "Item2";

			korisnik_cmb.SelectedValue = Program.TrenutniKorisnik.ID;

			korisnik_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(168001);
		}

		private void fm_MojKupac_Index_Load(object sender, EventArgs e)
		{
			UcitajMojeKupce();
		}

		private void UcitajMojeKupce()
		{
			DataTable dt = new DataTable();
			dt.Columns.Add("PPID", typeof(int));
			dt.Columns.Add("PIB", typeof(string));
			dt.Columns.Add("Naziv", typeof(string));

			foreach (
				int ppid in _okupiraniKupci
					.Result.Tag.Where(x => x.Value == _trenutniKorisnik)
					.Select(x => x.Key)
			)
			{
				Komercijalno.Partner par = _partneriKomercijalno.Result.FirstOrDefault(x =>
					x.PPID == ppid
				);
				DataRow dr = dt.NewRow();
				dr["PPID"] = ppid;
				dr["PIB"] = par == null ? "UNDEFINED" : par.PIB;
				dr["Naziv"] = par == null ? "UNDEFINED" : par.Naziv;
				dt.Rows.Add(dr);
			}

			dataGridView1.DataSource = dt;

			dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
			dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

			prometUOdnosuNaPrethodnuGodinu_txt.Text = "";
			cagr_txt.Text = "";
			beleska_txt.Text = "";
		}

		private void uvrstiNovogKupcaToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			using (
				InputBox ib = new InputBox(
					"Uvrsti Novog Kupca",
					"Unesite PIB kupca kog zelite da uvrstite medju svoje:"
				)
			)
			{
				ib.ShowDialog();

				if (ib.DialogResult != DialogResult.OK)
					return;

				if (string.IsNullOrWhiteSpace(ib.returnData))
				{
					MessageBox.Show("Neispravan PIB");
					return;
				}

				Komercijalno.Partner part = _partneriKomercijalno.Result.FirstOrDefault(x =>
					x.PIB == ib.returnData
				);

				if (part == null)
				{
					MessageBox.Show("Partner sa datim PIBom nije pronadjen u bazi!");
					return;
				}

				if (_okupiraniKupci.Result.Tag.ContainsKey(part.PPID))
				{
					MessageBox.Show(
						"Ovog kupca je neko vec uvrstao kao svog! Kontaktiraj administratora!"
					);
					return;
				}

				_okupiraniKupci.Result.Tag[part.PPID] = _trenutniKorisnik;
				_okupiraniKupci.Result.UpdateOrInsert();

				MessageBox.Show($"Uspesno ste uvrstali {part.Naziv} medju svoje!");

				UcitajMojeKupce();
			}
		}

		private void detaljnaAnaliza_btn_Click(object sender, EventArgs e)
		{
			int ppid = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["PPID"].Value
			);

			using (fm_Partner_Analiza_General_Index p = new fm_Partner_Analiza_General_Index(ppid))
			{
				p.ShowDialog();
			}
		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e) { }

		private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
		{
			if (dataGridView1.Rows.Count == 0 || dataGridView1.SelectedCells.Count == 0)
				return;

			int ppid = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["PPID"].Value
			);

			Dictionary<int, double> promet = new Dictionary<int, double>();
			Dictionary<int, double> prometPerioda = new Dictionary<int, double>();

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
						cmd.Parameters.AddWithValue("@PPID", ppid);

						using (FbDataReader dr = cmd.ExecuteReader())
							promet[godina] = dr.Read() ? Convert.ToDouble(dr[0]) : 0;
					}

					using (
						FbCommand cmd = new FbCommand(
							"SELECT COALESCE(SUM(POTRAZUJE), 0) FROM DOKUMENT WHERE (VRDOK = 15 OR VRDOK = 13) AND PPID = @PPID AND DATUM <= @DAT",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@PPID", ppid);
						cmd.Parameters.AddWithValue(
							"@DAT",
							new DateTime(godina, DateTime.Now.Month, DateTime.Now.Day)
						);

						using (FbDataReader dr = cmd.ExecuteReader())
							prometPerioda[godina] = dr.Read() ? Convert.ToDouble(dr[0]) : 0;
					}
				}
			}

			int brojRacunaOveGodine = 0;

			using (
				FbConnection con = new FbConnection(
					Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
				)
			)
			{
				con.Open();
				using (
					FbCommand cmd = new FbCommand(
						"SELECT COALESCE(COUNT(VRDOK), 0) FROM DOKUMENT WHERE (VRDOK = 15 OR VRDOK = 13) AND PPID = @PPID",
						con
					)
				)
				{
					cmd.Parameters.AddWithValue("@PPID", ppid);

					using (FbDataReader dr = cmd.ExecuteReader())
						if (dr.Read())
							brojRacunaOveGodine = Convert.ToInt32(dr[0]);
				}
			}

			brojRacunaOVeGodine_txt.Text = brojRacunaOveGodine.ToString();

			foreach (int god in promet.Keys.OrderBy(x => x))
			{
				if (promet[god] > 0)
					break;

				promet.Remove(god);
			}

			foreach (int god in prometPerioda.Keys.OrderBy(x => x))
			{
				if (prometPerioda[god] > 0)
					break;

				prometPerioda.Remove(god);
			}

			double prometUOdnosuNaPrethodnuGodinu =
				promet.Keys.Count <= 1
					? 0
					: (promet[DateTime.Now.Year] / promet[DateTime.Now.Year - 1]) - 1;
			double prometPeriodaOdnos =
				prometPerioda.Keys.Count <= 1
					? 0
					: (prometPerioda[DateTime.Now.Year] / prometPerioda[DateTime.Now.Year - 1]) - 1;
			double prosecniMesecniPrometK =
				promet.Keys.Count <= 1
					? 0
					: (
						(promet[DateTime.Now.Year] / DateTime.Now.Month)
						/ (promet[DateTime.Now.Year - 1] / 12)
					) - 1;
			double cagr =
				promet.Keys.Count == 0
					? 0
					: Math.Pow(
						(promet[promet.Keys.Max()] / promet[promet.Keys.Min()]),
						(double)((double)1 / promet.Count)
					) - 1;

			prvaKupovina_lbl.Text =
				promet.Keys.Count == 0
					? $"Kupac nije imao kupovina u proslosti!"
					: $"Prva kupovina {promet.Keys.Min()} godine. ({(DateTime.Now.Year - promet.Keys.Min())})";
			prometUOdnosuNaPrethodnuGodinu_txt.Text = prometUOdnosuNaPrethodnuGodinu.ToString(
				"#,##0.00 %"
			);
			prometZaTrenutniPeriod_txt.Text = prometPeriodaOdnos.ToString("#,##0.00 %");
			prosecniMesecniPromet_txt.Text = prosecniMesecniPrometK.ToString("#,##0.00 %");
			cagr_txt.Text = cagr.ToString("#,###.## %");

			cagr_txt.BackColor = cagr > 0 ? Color.LightGreen : Color.Coral;
			prometUOdnosuNaPrethodnuGodinu_txt.BackColor =
				prometUOdnosuNaPrethodnuGodinu > 0 ? Color.LightGreen : Color.Coral;
			prometZaTrenutniPeriod_txt.BackColor =
				prometPeriodaOdnos > 0 ? Color.LightGreen : Color.Coral;
			prosecniMesecniPromet_txt.BackColor =
				prosecniMesecniPrometK > 0 ? Color.LightGreen : Color.Coral;

			beleska_txt.Text = _beleskeKupaca.Result.Tag.ContainsKey(ppid)
				? _beleskeKupaca.Result.Tag[ppid]
				: "";

			_trenutniPPID = ppid;
		}

		private void sacuvajBelesku_txt_Click(object sender, EventArgs e)
		{
			_beleskeKupaca.Result.Tag[_trenutniPPID] = beleska_txt.Text;
			_beleskeKupaca.Result.UpdateOrInsert();

			MessageBox.Show("Beleska uspesno sacuvana!");
		}

		private void korisnik_cmb_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!korisnik_cmb.Enabled)
				return;

			_trenutniKorisnik = Convert.ToInt32(korisnik_cmb.SelectedValue);
			UcitajMojeKupce();
		}

		private void ukloniKupcaIzMojeListeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int ppid = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["PPID"].Value
			);

			_beleskeKupaca.Result.Tag.Remove(ppid);
			_beleskeKupaca.Result.UpdateOrInsert();

			_okupiraniKupci.Result.Tag.Remove(ppid);
			_okupiraniKupci.Result.UpdateOrInsert();

			UcitajMojeKupce();
			MessageBox.Show("Partner uklonjen iz tvoje liste!");
		}
	}
}
