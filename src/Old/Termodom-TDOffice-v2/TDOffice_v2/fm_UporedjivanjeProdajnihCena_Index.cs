using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace TDOffice_v2
{
	public partial class fm_UporedjivanjeProdajnihCena_Index : Form
	{
		private Task _ucitavanjeBaza { get; set; }
		private List<Termodom.Data.Entities.DBSettings.ConnectionInfo> _connectionInfos { get; set; }
		private int[] _VPDokumenti { get; } = new int[] { 25, 26 };

		public fm_UporedjivanjeProdajnihCena_Index()
		{
			InitializeComponent();
			LogStatus("Initializing...");
			ToggleUI(false);
			LogStatus("Initialized");
		}

		private void fm_UporedjivanjeProdajnihCena_Index_Load(object sender, EventArgs e)
		{
			StartUcitavanjeBazaAsync();
		}

		private void LogStatus(string message)
		{
			this.status_lbl.Text = message;
		}

		private void StartUcitavanjeBazaAsync()
		{
			LogStatus("Ucitavanje baza...");
			_ucitavanjeBaza = UcitajBazeAsync();
			_ucitavanjeBaza.ContinueWith(
				(prev) =>
				{
					this.Invoke(
						(MethodInvoker)
							delegate
							{
								ToggleUI(true);
								LogStatus("Baze ucitane");
							}
					);
				}
			);
		}

		private async Task UcitajBazeAsync()
		{
			var result = await TDBrain_v3.GetAsync("/dbsettings/baza/komercijalno/list");
			if (result.StatusCode == System.Net.HttpStatusCode.OK)
			{
				var response = await result.Content.ReadAsStringAsync();
				_connectionInfos = JsonConvert.DeserializeObject<
					List<Termodom.Data.Entities.DBSettings.ConnectionInfo>
				>(response);
				List<string> paths = _connectionInfos
					.Select(x => x.PutanjaDoBaze)
					.Distinct()
					.ToList();

				List<Tuple<int, string>> list = new List<Tuple<int, string>>();

				foreach (string path in paths)
					this.izvornaBaza_cmb.Items.Add(path);
			}
			else
			{
				MessageBox.Show($"Greska ucitavanja baza sa API-ja. Status: " + result.StatusCode);
			}
		}

		private void ToggleUI(bool enabled)
		{
			this.izvornaBaza_cmb.Enabled = enabled;
			this.izvorniVrDok_txt.Enabled = enabled;
			this.period_rb.Enabled = enabled;
			this.odDatuma_dtp.Enabled = enabled;
			this.doDatuma_dtp.Enabled = enabled;
			this.broj_rb.Enabled = enabled;
			this.izvorniBrDok_txt.Enabled = enabled;
			this.uporediKolicine_cb.Enabled = enabled;
			this.uporediNabavneCene_cb.Enabled = enabled;
			this.uporediProdajneCene_cb.Enabled = enabled;
			this.uporedi_btn.Enabled = enabled;
		}

		private async void uporedi_btn_ClickAsync(object sender, EventArgs e)
		{
			if (
				!this.uporediKolicine_cb.Checked
				&& !this.uporediNabavneCene_cb.Checked
				&& !this.uporediProdajneCene_cb.Checked
			)
			{
				MessageBox.Show("Morate cekirati barem jedan uslov poredjivajna!");
				return;
			}
			ToggleUI(false);
			string bazaPath = this.izvornaBaza_cmb.SelectedItem.ToString();
			Termodom.Data.Entities.DBSettings.ConnectionInfo connInfo = _connectionInfos.First(x =>
				x.PutanjaDoBaze == bazaPath
			);

			int vrDok;
			if (!int.TryParse(this.izvorniVrDok_txt.Text, out vrDok))
			{
				MessageBox.Show("Neispravna vrsta dokumenta!");
				return;
			}

			if (this.broj_rb.Checked)
			{
				int brDok;
				if (!int.TryParse(this.izvorniBrDok_txt.Text, out brDok))
				{
					MessageBox.Show("Neispravan broj dokumenta!");
					return;
				}

				await UporediIzvorniIDestinacioniDokumentAsync(vrDok, brDok, connInfo);
			}
			else
			{
				await UporediIzvorneIDestinacioneDokumenteZaPeriod(
					vrDok,
					this.odDatuma_dtp.Value,
					this.doDatuma_dtp.Value,
					connInfo
				);
			}

			ToggleUI(true);
		}

		private async Task UporediIzvorniIDestinacioniDokumentAsync(
			int vrDok,
			int brDok,
			Termodom.Data.Entities.DBSettings.ConnectionInfo connInfo
		)
		{
			Task<Termodom.Data.Entities.Komercijalno.RobaDictionary> robaTask =
				Komercijalno.Roba.Dictionary(connInfo.Godina);
			Task<Termodom.Data.Entities.Komercijalno.TarifaDictionary> tarifeTask =
				Komercijalno.Tarife.Dictionary(connInfo.Godina);
			Termodom.Data.Entities.Komercijalno.Dokument izvorniDokument =
				await Komercijalno.Dokument.GetAsync(
					connInfo.MagacinID,
					vrDok,
					brDok,
					connInfo.Godina
				);

			if (izvorniDokument == null)
			{
				MessageBox.Show("Dokument nije pronadjen!");
				return;
			}

			if (izvorniDokument.VrDokOut == null || izvorniDokument.BrDokOut == null)
			{
				MessageBox.Show("Dokument nije pretvoren dalje u novi dokument!");
				return;
			}

			Termodom.Data.Entities.Komercijalno.Dokument destinacioniDokument =
				await Komercijalno.Dokument.GetAsync(
					connInfo.MagacinID,
					(int)izvorniDokument.VrDokOut,
					(int)izvorniDokument.BrDokOut,
					connInfo.Godina
				);

			if (destinacioniDokument == null)
			{
				MessageBox.Show(
					"Destinacioni dokument je zabelezen u izvornom ali nije pronadjen!"
				);
				return;
			}

			Termodom.Data.Entities.Komercijalno.StavkaDictionary izvorneStavke =
				await Komercijalno.Stavka.DictionaryAsync(
					connInfo.MagacinID,
					izvorniDokument.VrDok,
					izvorniDokument.BrDok,
					connInfo.Godina
				);
			Termodom.Data.Entities.Komercijalno.StavkaDictionary destinacioneStavke =
				await Komercijalno.Stavka.DictionaryAsync(
					connInfo.MagacinID,
					destinacioniDokument.VrDok,
					destinacioniDokument.BrDok,
					connInfo.Godina
				);

			DataTable dt = new DataTable();
			dt.Columns.Add("RobaID", typeof(int));
			dt.Columns.Add("Naziv", typeof(string));
			dt.Columns.Add("Opis", typeof(string));

			foreach (
				Termodom.Data.Entities.Komercijalno.Stavka izvornaStavka in izvorneStavke.Values
			)
			{
				Termodom.Data.Entities.Komercijalno.Stavka destinacionaStavka =
					destinacioneStavke.Values.FirstOrDefault(x => x.RobaID == izvornaStavka.RobaID);
				if (destinacionaStavka == null)
				{
					DataRow dr = dt.NewRow();
					dr["RobaID"] = izvornaStavka.RobaID;
					dr["Naziv"] = izvornaStavka.Naziv;
					dr["Opis"] = "Roba postoji u izvornom dokumentu ali ne i u destinacionom!";
					dt.Rows.Add(dr);
				}
			}

			foreach (
				Termodom.Data.Entities.Komercijalno.Stavka destinacionaStavka in destinacioneStavke.Values
			)
			{
				Termodom.Data.Entities.Komercijalno.Stavka izvornaStavka =
					izvorneStavke.Values.FirstOrDefault(x => x.RobaID == destinacionaStavka.RobaID);
				if (izvornaStavka == null)
				{
					DataRow dr = dt.NewRow();
					dr["RobaID"] = destinacionaStavka.RobaID;
					dr["Naziv"] = destinacionaStavka.Naziv;
					dr["Opis"] = "Roba postoji u destinacionom dokumentu ali ne i u izvornom!";
					dt.Rows.Add(dr);
				}
			}

			if (dt.Rows.Count > 0)
			{
				MessageBox.Show("Postoji neslaganje u stavkama izvornog i nastalog dokumenta!");
			}
			else
			{
				Termodom.Data.Entities.Komercijalno.RobaDictionary roba = await robaTask;
				Termodom.Data.Entities.Komercijalno.TarifaDictionary tarife = await tarifeTask;

				foreach (int robaid in izvorneStavke.Values.Select(x => x.RobaID))
				{
					Termodom.Data.Entities.Komercijalno.Stavka izvornaStavka =
						izvorneStavke.Values.First(x => x.RobaID == robaid);
					Termodom.Data.Entities.Komercijalno.Stavka destinacionaStavka =
						destinacioneStavke.Values.First(x => x.RobaID == robaid);
					Termodom.Data.Entities.Komercijalno.Roba rob = roba[robaid];
					Termodom.Data.Entities.Komercijalno.Tarifa tarifa = tarife[rob.TarifaID];
					double razlika = 0;
					if (this.uporediKolicine_cb.Checked)
						if (izvornaStavka.Kolicina != destinacionaStavka.Kolicina)
						{
							DataRow dr = dt.NewRow();
							dr["RobaID"] = robaid;
							dr["Naziv"] = izvornaStavka.Naziv;
							dr["Opis"] =
								$"Izvorna kolicina: {izvornaStavka.Kolicina.ToString("#,##0.00")} - Destinaciona kolicina: {destinacionaStavka.Kolicina.ToString("#,##0.00")}";
							dt.Rows.Add(dr);
						}

					if (this.uporediNabavneCene_cb.Checked)
						razlika = Math.Abs(
							izvornaStavka.NabavnaCena - destinacionaStavka.NabavnaCena
						);
					if (razlika > 0.01)
					{
						DataRow dr = dt.NewRow();
						dr["RobaID"] = robaid;
						dr["Naziv"] = izvornaStavka.Naziv;
						dr["Opis"] =
							$"Izvorna nabavna cena: {izvornaStavka.NabavnaCena.ToString("#,##0.00")} - Destinaciona nabavna cena: {destinacionaStavka.NabavnaCena.ToString("#,##0.00")}";
						dt.Rows.Add(dr);
					}

					if (this.uporediProdajneCene_cb.Checked)
					{
						double izvornaProdajna =
							izvornaStavka.ProdajnaCena
							* (
								_VPDokumenti.Contains(izvornaStavka.VrDok)
									? (1d + ((double)tarifa.Stopa / 100d))
									: 1
							);
						double destinacionaProdajna =
							destinacionaStavka.ProdajnaCena
							* (
								_VPDokumenti.Contains(destinacionaStavka.VrDok)
									? (1d + ((double)tarifa.Stopa / 100d))
									: 1
							);
						razlika = Math.Abs(izvornaProdajna - destinacionaProdajna);
						if (razlika > 0.01)
						{
							DataRow dr = dt.NewRow();
							dr["RobaID"] = robaid;
							dr["Naziv"] = izvornaStavka.Naziv;
							dr["Opis"] =
								$"Izvorna prodajna cena: {izvornaProdajna.ToString("#,##0.0000")} - Destinaciona prodajna cena: {destinacionaProdajna.ToString("#,##0.0000")}";
							dt.Rows.Add(dr);
						}
					}
				}
			}

			if (dt.Rows.Count > 0)
				using (DataGridViewSelectBox d = new DataGridViewSelectBox(dt))
					d.ShowDialog();
			else
				MessageBox.Show("Sve se slaze!");
		}

		private async Task UporediIzvorneIDestinacioneDokumenteZaPeriod(
			int vrDok,
			DateTime odDatuma,
			DateTime doDatuma,
			Termodom.Data.Entities.DBSettings.ConnectionInfo connInfo
		)
		{
			if (odDatuma.Date > doDatuma.Date)
			{
				MessageBox.Show("Raspon datuma nije dobar!");
				return;
			}

			Task<Termodom.Data.Entities.Komercijalno.RobaDictionary> robaTask =
				Komercijalno.Roba.Dictionary(connInfo.Godina);
			Task<Termodom.Data.Entities.Komercijalno.TarifaDictionary> tarifeTask =
				Komercijalno.Tarife.Dictionary(connInfo.Godina);

			Task<Termodom.Data.Entities.Komercijalno.DokumentDictionary> dokumentiTask =
				Komercijalno.DokumentManager.DictionaryAsync(
					connInfo.MagacinID,
					connInfo.Godina,
					new int[] { 19 }
				);

			Termodom.Data.Entities.Komercijalno.DokumentDictionary dokumenti = await dokumentiTask;

			DataTable dt = new DataTable();
			dt.Columns.Add("VrDok", typeof(int));
			dt.Columns.Add("BrDok", typeof(int));
			dt.Columns.Add("RobaID", typeof(int));
			dt.Columns.Add("Naziv", typeof(string));
			dt.Columns.Add("Opis", typeof(string));

			foreach (int v in dokumenti.Keys)
			{
				foreach (int b in dokumenti[v].Keys)
				{
					Termodom.Data.Entities.Komercijalno.StavkaDictionary izvorneStavke =
						await Komercijalno.Stavka.DictionaryAsync(
							connInfo.MagacinID,
							v,
							b,
							connInfo.Godina
						);
					Termodom.Data.Entities.Komercijalno.StavkaDictionary destinacioneStavke =
						await Komercijalno.Stavka.DictionaryAsync(
							connInfo.MagacinID,
							v,
							b,
							connInfo.Godina
						);

					foreach (
						Termodom.Data.Entities.Komercijalno.Stavka izvornaStavka in izvorneStavke.Values
					)
					{
						Termodom.Data.Entities.Komercijalno.Stavka destinacionaStavka =
							destinacioneStavke.Values.FirstOrDefault(x =>
								x.RobaID == izvornaStavka.RobaID
							);
						if (destinacionaStavka == null)
						{
							DataRow dr = dt.NewRow();
							dr["VrDok"] = v;
							dr["BrDoj"] = b;
							dr["RobaID"] = izvornaStavka.RobaID;
							dr["Naziv"] = izvornaStavka.Naziv;
							dr["Opis"] =
								"Roba postoji u izvornom dokumentu ali ne i u destinacionom!";
							dt.Rows.Add(dr);
						}
					}

					foreach (
						Termodom.Data.Entities.Komercijalno.Stavka destinacionaStavka in destinacioneStavke.Values
					)
					{
						Termodom.Data.Entities.Komercijalno.Stavka izvornaStavka =
							izvorneStavke.Values.FirstOrDefault(x =>
								x.RobaID == destinacionaStavka.RobaID
							);
						if (izvornaStavka == null)
						{
							DataRow dr = dt.NewRow();
							dr["VrDok"] = v;
							dr["BrDoj"] = b;
							dr["RobaID"] = destinacionaStavka.RobaID;
							dr["Naziv"] = destinacionaStavka.Naziv;
							dr["Opis"] =
								"Roba postoji u destinacionom dokumentu ali ne i u izvornom!";
							dt.Rows.Add(dr);
						}
					}

					if (dt.Rows.Count > 0)
					{
						MessageBox.Show(
							"Postoji neslaganje u stavkama izvornog i nastalog dokumenta!"
						);
					}
					else
					{
						Termodom.Data.Entities.Komercijalno.RobaDictionary roba = await robaTask;
						Termodom.Data.Entities.Komercijalno.TarifaDictionary tarife =
							await tarifeTask;

						foreach (int robaid in izvorneStavke.Values.Select(x => x.RobaID))
						{
							Termodom.Data.Entities.Komercijalno.Stavka izvornaStavka =
								izvorneStavke.Values.First(x => x.RobaID == robaid);
							Termodom.Data.Entities.Komercijalno.Stavka destinacionaStavka =
								destinacioneStavke.Values.First(x => x.RobaID == robaid);
							Termodom.Data.Entities.Komercijalno.Roba rob = roba[robaid];
							Termodom.Data.Entities.Komercijalno.Tarifa tarifa = tarife[
								rob.TarifaID
							];
							double razlika = 0;
							if (this.uporediKolicine_cb.Checked)
								if (izvornaStavka.Kolicina != destinacionaStavka.Kolicina)
								{
									DataRow dr = dt.NewRow();
									dr["VrDok"] = v;
									dr["BrDoj"] = b;
									dr["RobaID"] = robaid;
									dr["Naziv"] = izvornaStavka.Naziv;
									dr["Opis"] =
										$"Izvorna kolicina: {izvornaStavka.Kolicina.ToString("#,##0.00")} - Destinaciona kolicina: {destinacionaStavka.Kolicina.ToString("#,##0.00")}";
									dt.Rows.Add(dr);
								}

							if (this.uporediNabavneCene_cb.Checked)
								razlika = Math.Abs(
									izvornaStavka.NabavnaCena - destinacionaStavka.NabavnaCena
								);
							if (razlika > 0.01)
							{
								DataRow dr = dt.NewRow();
								dr["VrDok"] = v;
								dr["BrDoj"] = b;
								dr["RobaID"] = robaid;
								dr["Naziv"] = izvornaStavka.Naziv;
								dr["Opis"] =
									$"Izvorna nabavna cena: {izvornaStavka.NabavnaCena.ToString("#,##0.00")} - Destinaciona nabavna cena: {destinacionaStavka.NabavnaCena.ToString("#,##0.00")}";
								dt.Rows.Add(dr);
							}

							if (this.uporediProdajneCene_cb.Checked)
							{
								double izvornaProdajna =
									izvornaStavka.ProdajnaCena
									* (
										_VPDokumenti.Contains(izvornaStavka.VrDok)
											? (1d + ((double)tarifa.Stopa / 100d))
											: 1
									);
								double destinacionaProdajna =
									destinacionaStavka.ProdajnaCena
									* (
										_VPDokumenti.Contains(destinacionaStavka.VrDok)
											? (1d + ((double)tarifa.Stopa / 100d))
											: 1
									);
								razlika = Math.Abs(izvornaProdajna - destinacionaProdajna);
								if (razlika > 0.01)
								{
									DataRow dr = dt.NewRow();
									dr["VrDok"] = v;
									dr["BrDoj"] = b;
									dr["RobaID"] = robaid;
									dr["Naziv"] = izvornaStavka.Naziv;
									dr["Opis"] =
										$"Izvorna prodajna cena: {izvornaProdajna.ToString("#,##0.0000")} - Destinaciona prodajna cena: {destinacionaProdajna.ToString("#,##0.0000")}";
									dt.Rows.Add(dr);
								}
							}
						}
					}
				}
			}

			if (dt.Rows.Count > 0)
				using (DataGridViewSelectBox d = new DataGridViewSelectBox(dt))
					d.ShowDialog();
			else
				MessageBox.Show("Sve se slaze!");
		}

		private void proveriDaliImaDestinacioniDokument_cb_CheckedChanged(
			object sender,
			EventArgs e
		)
		{
			if (this.proveriDaliImaDestinacioniDokument_cb.Checked)
			{
				this.uporediKolicine_cb.Checked = false;
				this.uporediNabavneCene_cb.Checked = false;
				this.uporediProdajneCene_cb.Checked = false;
			}
		}
	}
}
