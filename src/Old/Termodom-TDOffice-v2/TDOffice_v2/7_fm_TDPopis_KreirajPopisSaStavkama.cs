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
using FirebirdSql.Data.FirebirdClient;

namespace TDOffice_v2
{
	public partial class _7_fm_TDPopis_KreirajPopisSaStavkama : Form
	{
		private Task<List<Komercijalno.RobaUMagacinu>> _robaUMagacinu =
			Komercijalno.RobaUMagacinu.ListAsync();
		private Task<List<Komercijalno.Stavka>> _stavke = Komercijalno.Stavka.ListAsync(
			DateTime.Now.Year
		);
		private Task<List<Komercijalno.Dokument>> _dokumenti = Komercijalno.Dokument.ListAsync();
		private Task<fm_Help> _helpForm { get; set; }

		public _7_fm_TDPopis_KreirajPopisSaStavkama()
		{
			InitializeComponent();
			SetUI();
			_helpForm = this.InitializeHelpModulAsync(Modul.Popis_KreirajPopisSaStavkama);
		}

		private void _7_fm_TDPopis_KreirajPopisSaStavkama_Load(object sender, EventArgs e) { }

		private void SetUI()
		{
			clb_Magacini.DataSource = Komercijalno.Magacin.ListAsync().Result;
			clb_Magacini.DisplayMember = "Naziv";
			clb_Magacini.ValueMember = "ID";

			danKreiranjaPopisa_cmb.SelectedIndex = 0;

			numericUpDown1.Minimum = 1;
			numericUpDown1.Maximum = (decimal)
				(DateTime.Now - new DateTime(DateTime.Now.Year, 1, 1)).TotalDays;

			numericUpDown2.Minimum = 1;
			numericUpDown2.Maximum = (decimal)
				(DateTime.Now - new DateTime(DateTime.Now.Year, 1, 1)).TotalDays;
		}

		private void btn_KreirajPopise_Click(object sender, EventArgs e)
		{
			btn_KreirajPopise.Text = "Kreiram popise...";
			btn_KreirajPopise.Enabled = false;

			Form mf = this;

			int momenatKreiranjaPopisa = danKreiranjaPopisa_cmb.SelectedIndex;
			mf.Enabled = false;
			Task kreiranjePopisa = Task.Run(() =>
			{
				if (clb_Magacini.CheckedItems.Count == 0)
				{
					MessageBox.Show("Morate izabrati barem jedan magacin!");
					OmoguciKontroleAsync(mf);
					return;
				}
				if (momenatKreiranjaPopisa == 0)
				{
					MessageBox.Show("Morate izabrati momenat popisivanja");
					OmoguciKontroleAsync(mf);
					return;
				}

				DateTime datumPopisaKomercijalno = DateTime.Now.AddDays(-1);

				if (momenatKreiranjaPopisa == 1)
				{
					DateTime tempDate = DateTime.Now;

					while (tempDate.DayOfWeek != DayOfWeek.Sunday)
						tempDate = tempDate.AddDays(-1);

					datumPopisaKomercijalno = tempDate;
				}
				int trenutniMagacin = 0;

				foreach (Komercijalno.Magacin magacin in clb_Magacini.CheckedItems)
				{
					trenutniMagacin++;
					mf.Invoke(
						(MethodInvoker)
							delegate
							{
								label3.Text =
									$"Magacin: {trenutniMagacin} / {clb_Magacini.CheckedItems.Count}";
							}
					);
					List<Komercijalno.RobaUMagacinu> robaUOvomMagacinu = _robaUMagacinu
						.Result.Where(x => x.MagacinID == magacin.ID)
						.ToList();
					List<int> robaZaDodatiUDokument = new List<int>();

					robaZaDodatiUDokument = rb_KojeNisuPopisivanePoslednjih.Checked
						? GetNepopisivanuRobu(magacin.ID)
						: rb_KokeImajuNeSlaganjaUPoslednjih.Checked
							? GetRobuSaNeslaganjima(magacin.ID)
							: btn_IzborListe.Tag as List<int>;

					if (robaZaDodatiUDokument == null || robaZaDodatiUDokument.Count == 0)
					{
						MessageBox.Show(
							"Pod ovim uslovima za ovaj magacin ne bi bila dodata ni jedna stavka. Zaobilazim ovaj magacin!"
						);
						continue;
					}

					int noviTDOfficePopisBrDok = TDOffice.DokumentPopis.Insert(
						TDOffice.TDOffice.PPID,
						magacin.ID,
						0,
						null,
						null,
						TDOffice.PopisType.Vanredni,
						null,
						null
					);
					int noviKomercijalnoPopisBrDok = Komercijalno.Dokument.Insert(
						DateTime.Now.Year,
						7,
						"TD2  " + noviTDOfficePopisBrDok.ToString(),
						null,
						"TDOffice_v2",
						1,
						magacin.ID,
						Program.TrenutniKorisnik.KomercijalnoUserID,
						null
					);

					Komercijalno.Dokument komDokPop = Komercijalno.Dokument.Get(
						DateTime.Now.Year,
						7,
						noviKomercijalnoPopisBrDok
					);
					komDokPop.Datum = datumPopisaKomercijalno;
					komDokPop.Update();

					TDOffice.DokumentPopis TDOfficePopis = TDOffice.DokumentPopis.Get(
						noviTDOfficePopisBrDok
					);
					TDOfficePopis.KomercijalnoPopisBrDok = noviKomercijalnoPopisBrDok;
					TDOfficePopis.Update();

					using (
						FbConnection fbCon = new FbConnection(TDOffice.TDOffice.connectionString)
					)
					{
						fbCon.Open();

						using (
							FbConnection komCon = new FbConnection(
								Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]
							)
						)
						{
							komCon.Open();
							int trenutnaStavka = 0;
							foreach (int robaID in robaZaDodatiUDokument)
							{
								trenutnaStavka++;
								mf.Invoke(
									(MethodInvoker)
										delegate
										{
											label4.Text =
												$"Stavka: {trenutnaStavka} / {robaZaDodatiUDokument.Count}";
										}
								);

								Komercijalno.RobaUMagacinu rum = robaUOvomMagacinu
									.Where(x => x.RobaID == robaID)
									.FirstOrDefault();

								if (rum == null)
								{
									Task.Run(() =>
									{
										MessageBox.Show(
											"Doslo je do greske prilikom insertovanja stavke RobaID: "
												+ robaID
												+ " jer roba sa ovim ID-em nije pronadjena u sifarniku. Zaobilazim ovu stavku i nastavljam dalje!"
										);
									});
									continue;
								}

								TDOffice.StavkaPopis.Insert(
									fbCon,
									noviTDOfficePopisBrDok,
									robaID,
									rum.ProdajnaCena,
									null,
									0,
									0
								);
								Komercijalno.Stavka.Insert(
									komCon,
									komDokPop,
									Komercijalno.Roba.Get(DateTime.Now.Year, rum.RobaID),
									rum,
									0,
									0
								);
							}
						}
					}
				}

				MessageBox.Show("Popisi kreirani!");
			});

			Task.Run(() =>
			{
				kreiranjePopisa.Wait();
				OmoguciKontroleAsync(mf);
			});
		}

		private void OmoguciKontroleAsync(Form uiForm)
		{
			uiForm.Invoke(
				(MethodInvoker)
					delegate
					{
						uiForm.Enabled = true;
						btn_KreirajPopise.Text = "Kreiraj popis";
						btn_KreirajPopise.Enabled = true;
					}
			);
		}

		private List<int> GetNepopisivanuRobu(int magacinID)
		{
			List<int> relativniBrDok = _dokumenti
				.Result.Where(x =>
					Math.Abs((x.Datum - DateTime.Now).TotalDays)
						< Convert.ToInt32(numericUpDown1.Value)
					&& x.VrDok == 7
					&& x.MagacinID == magacinID
				)
				.Select(x => x.BrDok)
				.ToList();
			List<Komercijalno.Stavka> optimizedStavke = _stavke
				.Result.Where(x => relativniBrDok.Contains(x.BrDok))
				.ToList();
			List<int> popisivanaRoba = optimizedStavke
				.Where(x => x.VrDok == 7)
				.Select(x => x.RobaID)
				.Distinct()
				.ToList();
			return _robaUMagacinu
				.Result.Where(x => !popisivanaRoba.Contains(x.RobaID))
				.Select(x => x.RobaID)
				.Distinct()
				.ToList();
		}

		private List<int> GetRobuSaNeslaganjima(int magacinID)
		{
			List<Komercijalno.Dokument> relativniDokumentiPopisa = _dokumenti
				.Result.Where(x =>
					Math.Abs((x.Datum - DateTime.Now).TotalDays)
						< Convert.ToInt32(numericUpDown1.Value)
					&& x.VrDok == 7
					&& x.MagacinID == magacinID
				)
				.ToList();
			List<Komercijalno.Stavka> optimizedStavke = _stavke
				.Result.Where(x => relativniDokumentiPopisa.Count(y => y.BrDok == x.BrDok) != 0)
				.ToList();

			List<int> list = new List<int>();

			foreach (Komercijalno.Dokument dok in relativniDokumentiPopisa)
			{
				List<Komercijalno.Stavka> stavkeDokumenta = optimizedStavke
					.Where(x => x.VrDok == 7 && x.BrDok == dok.BrDok)
					.ToList();
				foreach (Komercijalno.Stavka stavka in stavkeDokumenta)
					if (
						stavka.Kolicina
						!= Komercijalno.Procedure.StanjeDoDatuma(
							dok.Datum,
							dok.MagacinID,
							stavka.RobaID
						)
					)
						list.Add(stavka.RobaID);
			}
			return list;
		}

		private void rb_PoListi_Click(object sender, EventArgs e)
		{
			this.btn_IzborListe.Enabled = this.rb_PoListi.Checked;
		}

		private void btn_IzborListe_Click(object sender, EventArgs e)
		{
			using (fm_ListaRobe lr = new fm_ListaRobe())
			{
				lr.Text = "TD Popis <<Lista robe>>";
				lr.robaUListi =
					btn_IzborListe.Tag == null ? new List<int>() : btn_IzborListe.Tag as List<int>;
				lr.ShowDialog();
				btn_IzborListe.Tag = lr.robaUListi;
				tb_ListaRobe.Text = lr.nazivListe;
			}
		}

		private void btn_Help_Click(object sender, EventArgs e)
		{
			_helpForm.Result.ShowDialog();
		}
	}
}
