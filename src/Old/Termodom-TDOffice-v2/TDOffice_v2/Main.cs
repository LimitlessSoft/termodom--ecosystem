using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using FirebirdSql.Data.FirebirdClient;
using MigraDoc.DocumentObjectModel;
using Newtonsoft.Json;
using TDOffice_v2.EventHandlers;
using TDOffice_v2.Forms;
using TDOffice_v2.Forms.MC;
using TDOffice_v2.Komercijalno;
using TDOffice_v2.TDOffice;
using static TDOffice_v2.DTO.KursGetDTO;

namespace TDOffice_v2
{
	public partial class Main : Form
	{
		/// <summary>
		/// Interval za NewMessageListenerLoop
		/// </summary>
		private readonly TimeSpan NEW_MESSAGE_LISTENER_INTERVAL = TimeSpan.FromSeconds(1);
		private Task _opominjiZaNeizvrseniZadatakLoop { get; set; }

		private DataTable _planerDataTable { get; set; }

		static Main() { }

		public Main(TDOffice.User loggedUser)
		{
			InitializeComponent();

			Program.TrenutniKorisnik = loggedUser;
			currentUser_ts_lbl.Text = Program.TrenutniKorisnik.Username;

			// TDOffice.Kurs.UcitajIZapisiKursUBazuAsync();

			StartNewMessagesListenerAsync();
			StartProveraOsvezavanjaPravaAsync();

			UcitajBeleske();

			StartTaskCheckerLoopAsync();

			StartProgramVersionCheckLoopAsync();

			danas_lbl.Text = string.Format(
				"Danas je {0} {1}",
				DateTime.Now.DayOfWeek.ToString(true),
				DateTime.Now.ToString("dd.MM.yyyy")
			);
			UcitajPinovanePoruke();
		}

		private void Main_Load(object sender, EventArgs e)
		{
			version_lbl.Text =
				"v" + (new AssemblyName(Assembly.GetExecutingAssembly().FullName)).Version;
			UcitajPlanerAsync();

			NamestiLostFocusBeleski();

			if (Program.TrenutniKorisnik.OpomeniZaNeizvrseneZadatke)
				PokreniOpomeniZaNeizvrseniZadatakLoop();

			Task.Run(() =>
			{
				while (Program.IsRunning)
				{
					try
					{
						nThreads_lbl.Text =
							"nThreads: " + Process.GetCurrentProcess().Threads.Count;
						physicalMemoryUsage_lbl.Text =
							"Physical Memory Usage: "
							+ Process.GetCurrentProcess().WorkingSet64 / 1048576;
						Thread.Sleep(TimeSpan.FromSeconds(10));
					}
					catch (Exception) { }
				}
			});

			Task.Run(() =>
			{
				TDOffice.Config<Dictionary<DateTime, string>> _patchLog = TDOffice.Config<
					Dictionary<DateTime, string>
				>.Get(TDOffice.ConfigParameter.PatchLog);

				if (
					_patchLog != null
					&& _patchLog.Tag != null
					&& _patchLog.Tag.Keys.Count > 0
					&& _patchLog.Tag.Keys.Max(x => x) > LocalSettings.Settings.LastPatchLogSeen
				)
					using (fm_PatchLog_Index pl = new fm_PatchLog_Index())
						pl.ShowDialog();
			});
		}

		private void Main_FormClosing(object sender, FormClosingEventArgs e)
		{
			RichTextBox beleskaRtb = tabControl1
				.SelectedTab.Controls.OfType<RichTextBox>()
				.FirstOrDefault();
			if (
				TDOffice
					.User.Get(Program.TrenutniKorisnik.ID)
					.Tag.Beleske.Where(x => x.ID == Convert.ToInt32(beleskaRtb.Tag))
					.FirstOrDefault()
					.Body
				!= Program
					.TrenutniKorisnik.Tag.Beleske.Where(x =>
						x.ID == Convert.ToInt32(beleskaRtb.Tag)
					)
					.FirstOrDefault()
					.Body
			)
				if (
					MessageBox.Show(
						"Imate izmene na belesci.\nZelite da ih sacuvate?",
						"Beleske",
						MessageBoxButtons.YesNo
					) == DialogResult.Yes
				)
					Program.TrenutniKorisnik.Update();

			Program.IsRunning = false;
			Environment.Exit(0);
		}

		private void PokreniOpomeniZaNeizvrseniZadatakLoop()
		{
			Task.Run(() =>
			{
				Thread.Sleep(TimeSpan.FromMinutes(1));
				_opominjiZaNeizvrseniZadatakLoop = Task.Run(() =>
				{
					List<TDOffice.CheckListItem> zadaci = TDOffice.CheckListItem.ListByKorisnikID(
						Program.TrenutniKorisnik.ID
					);

					int probilih = 0;

					foreach (TDOffice.CheckListItem zad in zadaci)
						if (
							zad.DatumIzvrsenja == null
							|| (DateTime.Now - (DateTime)zad.DatumIzvrsenja).TotalDays
								> zad.IntervalDana
						)
							probilih++;

					if (probilih > 0)
						MessageBox.Show("Imas zadataka koji te cekaju. Pogedaj checklistu!!!");

					Thread.Sleep(TimeSpan.FromMinutes(30));
				});
			});
		}

		private Task StartProgramVersionCheckLoopAsync()
		{
			return Task.Run(async () =>
			{
				while (true)
				{
					if (await Program.IsProgramOldVersionedAsync())
					{
						await Program.DownloadAndInstallNewVersionAsync();
						return;
					}
					Thread.Sleep(TimeSpan.FromMinutes(1));
				}
			});
		}

		private Task StartProveraOsvezavanjaPravaAsync()
		{
			return Task.Run(() =>
			{
				while (Program.IsRunning)
				{
					TDOffice.Config<List<int>> conf = TDOffice.Config<List<int>>.Get(
						TDOffice.ConfigParameter.OsvezavanjePrava
					);
					if (conf.Tag.Contains(Program.TrenutniKorisnik.ID))
					{
						Program.TrenutniKorisnik = TDOffice.User.Get(Program.TrenutniKorisnik.ID);
						conf.Tag.Remove(Program.TrenutniKorisnik.ID);
						conf.UpdateOrInsert();
					}

					Thread.Sleep(1 * 1000);
				}
			});
		}

		private Task StartNewMessagesListenerAsync()
		{
			return Task.Run(() =>
			{
				bool prviPut = true;
				while (Program.IsRunning)
				{
					List<TDOffice.Poruka> porukeZaPrikazati = TDOffice
						.Poruka.List(
							$"PRIMALAC = {Program.TrenutniKorisnik.ID} AND ARHIVIRANA <> 1"
						)
						.Where(x => !x.Arhivirana)
						.ToList();

					int[] ideviPrikazanihPoruka = System
						.Windows.Forms.Application.OpenForms.OfType<_1301_fm_Poruka_Index>()
						.Select(x => x.PorukaID)
						.ToArray();

					if (prviPut)
						if (
							MessageBox.Show(
								"Da li zelite prikazati neprocitane poruke?",
								"Prikazati poruke?",
								MessageBoxButtons.YesNo
							) == DialogResult.No
						)
						{
							ideviPrikazanihPoruka = new List<int>(
								porukeZaPrikazati.Select(x => x.ID)
							).ToArray();
							foreach (int sid in ideviPrikazanihPoruka)
								_1301_fm_Poruka_Index.IDeviSakrivenihPoruka.Add(sid);
						}

					prviPut = false;

					porukeZaPrikazati.RemoveAll(x =>
						ideviPrikazanihPoruka.Contains(x.ID)
						|| _1301_fm_Poruka_Index.IDeviSakrivenihPoruka.Contains(x.ID)
					);

					if (porukeZaPrikazati.Count > 0)
					{
						this.Invoke(
							(MethodInvoker)
								delegate
								{
									for (int i = 0; i < porukeZaPrikazati.Count; i++)
									{
										_1301_fm_Poruka_Index pi = new _1301_fm_Poruka_Index(
											porukeZaPrikazati[i]
										);
										pi.Location = new Point(
											(pi.Location.X + Random.Next(-5, 5)),
											(pi.Location.Y + Random.Next(-5, 5))
										);
										pi.Show();
										_1301_fm_Poruka_Index.IDeviSakrivenihPoruka.Add(
											pi.PorukaID
										);
									}
								}
						);
					}
					Thread.Sleep(NEW_MESSAGE_LISTENER_INTERVAL);
				}
			});
		}

		#region PinovanePoruke
		private void UcitajPinovanePoruke()
		{
			for (int i = 0; i < Program.TrenutniKorisnik.Tag.Pinovi.Count; i++)
			{
				//if (Program.TrenutniKorisnik.Tag.Pinovi[i].prikazana == 0)
				//{
				int id = Program.TrenutniKorisnik.Tag.Pinovi[i].PinID;
				TDOffice.Poruka pp = TDOffice.Poruka.Get(id);
				Task.Run(() =>
				{
					int id = pp.ID;
					_1301_fm_Poruka_Index p = new _1301_fm_Poruka_Index(TDOffice.Poruka.Get(id));
					p.SkupiPoruku();
					p.ShowDialog();
				});
				Program.TrenutniKorisnik.Tag.Pinovi[i].prikazana = 1;
				//}
			}
			Program.TrenutniKorisnik.Update();
		}
		#endregion
		#region Beleske
		private void NamestiLostFocusBeleski()
		{
			foreach (Control c in this.GetAllControlsAndSubControlls())
			{
				if (c.Name == "sacuvajBelesku_btn" || c.Name == "odustaniOdCuvanjaBeleske_btn")
					continue;

				c.MouseDown += Beleska_LostFocus;

				if (c.GetType() == typeof(MenuStrip))
				{
					foreach (ToolStripItem item in (c as MenuStrip).GetAllItems())
					{
						item.MouseDown += Beleska_LostFocus;
					}
				}
			}
		}

		private void UcitajBeleske()
		{
			tabControl1.Controls.Clear();

			if (Program.TrenutniKorisnik.Tag == null)
				Program.TrenutniKorisnik.Tag = new TDOffice.User.Info();

			if (Program.TrenutniKorisnik.Tag.Beleske.Count == 0)
			{
				Program.TrenutniKorisnik.Tag.Beleske.Add(
					new TDOffice.User.Beleska()
					{
						ID = 1,
						Body = "",
						Naziv = "N"
					}
				);
				Program.TrenutniKorisnik.Update();
			}

			foreach (TDOffice.User.Beleska b in Program.TrenutniKorisnik.Tag.Beleske)
				tabControl1.Controls.Add(BeleskaTabPageTemplate(b));

			tabControl1.SelectedIndex = LocalSettings.Settings.indexPoslednjePrikazaneBeleske;
		}

		private TabPage BeleskaTabPageTemplate(TDOffice.User.Beleska b)
		{
			RichTextBox rtb = new RichTextBox();
			rtb.Dock = System.Windows.Forms.DockStyle.Fill;
			rtb.Location = new System.Drawing.Point(3, 3);
			rtb.Name = "bt" + b.ID;
			rtb.Size = new System.Drawing.Size(379, 80);
			rtb.TabIndex = 0;
			rtb.Text = b.Body;
			rtb.Tag = b.ID;
			rtb.TextChanged += Rtb_TextChanged;

			TabPage tp = new TabPage();
			tp.Controls.Add(rtb);
			tp.Location = new System.Drawing.Point(4, 22);
			tp.Name = "tp" + b.ID;
			tp.Padding = new System.Windows.Forms.Padding(3);
			tp.Size = new System.Drawing.Size(385, 86);
			tp.TabIndex = 0;
			tp.Leave += Beleska_LostFocus;
			tp.Text = b.Naziv;
			tp.Tag = b.ID;
			tp.UseVisualStyleBackColor = true;

			return tp;
		}

		private void NovaBeleska_btn_Click(object sender, EventArgs e)
		{
			TDOffice.User.Beleska bel = new TDOffice.User.Beleska()
			{
				ID = Program.TrenutniKorisnik.Tag.Beleske.Max(x => x.ID) + 1,
				Body = "",
				Naziv = "B_" + Program.TrenutniKorisnik.Tag.Beleske.Max(x => x.ID) + 1
			};

			Program.TrenutniKorisnik.Tag.Beleske.Add(bel);
			Program.TrenutniKorisnik.Update();

			tabControl1.Controls.Add(BeleskaTabPageTemplate(bel));
			LocalSettings.Settings.indexPoslednjePrikazaneBeleske = tabControl1.Controls.Count - 1;
			LocalSettings.Update();
			tabControl1.SelectedIndex = LocalSettings.Settings.indexPoslednjePrikazaneBeleske;
		}

		private void Rtb_TextChanged(object sender, EventArgs e)
		{
			RichTextBox c = sender as RichTextBox;
			int bid = Convert.ToInt32(c.Tag);

			for (int i = 0; i < Program.TrenutniKorisnik.Tag.Beleske.Count; i++)
			{
				if (Program.TrenutniKorisnik.Tag.Beleske[i].ID == bid)
				{
					Program.TrenutniKorisnik.Tag.Beleske[i].Body = c.Text;
					break;
				}
			}

			sacuvajBelesku_btn.Visible = true;
			odustaniOdCuvanjaBeleske_btn.Visible = true;
		}

		private void Beleska_LostFocus(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				Thread.Sleep(100);
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							if (
								!sacuvajBelesku_btn.Visible || !odustaniOdCuvanjaBeleske_btn.Visible
							)
								return;

							RichTextBox rtb = null;

							rtb = tabControl1
								.SelectedTab.Controls.OfType<RichTextBox>()
								.FirstOrDefault();

							int idBelekse = Convert.ToInt32(rtb.Tag);

							TDOffice.User.Beleska b = TDOffice
								.User.Get(Program.TrenutniKorisnik.ID)
								.Tag.Beleske.Where(x => x.ID == idBelekse)
								.FirstOrDefault();

							if (b.Body == rtb.Text)
								return;

							DialogResult result = MessageBox.Show(
								"Imate izmene na belesci.\nZelite da ih sacuvate?",
								"Beleske",
								MessageBoxButtons.YesNoCancel,
								MessageBoxIcon.Question
							);
							if (result == DialogResult.Yes)
							{
								Program.TrenutniKorisnik.Update();
								sacuvajBelesku_btn.Visible = false;
								odustaniOdCuvanjaBeleske_btn.Visible = false;
							}
							else if (result == DialogResult.No)
							{
								sacuvajBelesku_btn.Visible = true;
								odustaniOdCuvanjaBeleske_btn.Visible = true;
							}
							else if (result == DialogResult.Cancel)
							{
								rtb.Text = b.Body;
								sacuvajBelesku_btn.Visible = false;
								odustaniOdCuvanjaBeleske_btn.Visible = false;
							}
						}
				);
			});
		}

		private void tabControl1_Selected(object sender, TabControlEventArgs e)
		{
			LocalSettings.Settings.indexPoslednjePrikazaneBeleske = tabControl1.SelectedIndex;
			LocalSettings.Update();
		}

		private void brisiBeleskuToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int idBeleske = Convert.ToInt32(tabControl1.Controls[tabControl1.SelectedIndex].Tag);

			if (idBeleske < 0)
				return;

			if (
				MessageBox.Show(
					"Sigurno brisete selektovanu Belesku.",
					"Brisanje beleske",
					MessageBoxButtons.YesNo
				) != DialogResult.Yes
			)
				return;

			Program.TrenutniKorisnik.Tag.Beleske.Remove(
				Program.TrenutniKorisnik.Tag.Beleske.Where(x => x.ID == idBeleske).FirstOrDefault()
			);
			Program.TrenutniKorisnik.Update();
			for (int i = 0; i < tabControl1.Controls.Count; i++)
			{
				if (tabControl1.Controls[i].Tag == null)
					continue;

				if (Convert.ToInt32(tabControl1.Controls[i].Tag) == idBeleske)
				{
					tabControl1.Controls.RemoveAt(i);
					LocalSettings.Settings.indexPoslednjePrikazaneBeleske = 0;
					LocalSettings.Update();
					tabControl1.SelectedIndex = LocalSettings
						.Settings
						.indexPoslednjePrikazaneBeleske;
					return;
				}
			}
		}

		private void tabControl1_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				for (int i = 0; i < tabControl1.TabCount; ++i)
				{
					Rectangle r = tabControl1.GetTabRect(i);
					if (r.Contains(e.Location))
					{
						contextMenuStrip1.Show(tabControl1, e.Location);
						break;
					}
				}
			}
		}

		private void promeniNazivBeleskeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (
				InputBox form = new InputBox(
					"Unesite naziv beleske",
					"Stari naziv je: " + tabControl1.TabPages[tabControl1.SelectedIndex].Text
				)
			)
			{
				form.ShowDialog();
				if (form.returnData != null)
				{
					Program.TrenutniKorisnik.Tag.Beleske[tabControl1.SelectedIndex].Naziv =
						form.returnData;
					tabControl1.TabPages[tabControl1.SelectedIndex].Text = form.returnData;
				}
			}
			Program.TrenutniKorisnik.Update();
		}

		private void sacuvajBelesku_btn_Click(object sender, EventArgs e)
		{
			sacuvajBelesku_btn.Visible = false;
			odustaniOdCuvanjaBeleske_btn.Visible = false;
			Program.TrenutniKorisnik.Update();
		}

		private void odustaniOdCuvanjaBeleske_btn_Click(object sender, EventArgs e)
		{
			RichTextBox rtb = tabControl1
				.SelectedTab.Controls.OfType<RichTextBox>()
				.FirstOrDefault();
			Program.TrenutniKorisnik = TDOffice.User.Get(Program.TrenutniKorisnik.ID);
			tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault().Text = Program
				.TrenutniKorisnik.Tag.Beleske.Where(x => x.ID == Convert.ToInt32(rtb.Tag))
				.FirstOrDefault()
				.Body;

			sacuvajBelesku_btn.Visible = false;
			odustaniOdCuvanjaBeleske_btn.Visible = false;
		}
		#endregion

		#region Planer
		private Task UcitajPlanerAsync()
		{
			Form trenutnaForma = this;

			return Task.Run(() =>
			{
				List<TDOffice.Planer.Stavka> planerStavkeKorisnika =
					TDOffice.Planer.Stavka.ListByUserID(Program.TrenutniKorisnik.ID);

				_planerDataTable = new DataTable();
				_planerDataTable.Columns.Add("planerStavkaID", typeof(int));
				_planerDataTable.Columns.Add("Datum", typeof(DateTime));
				_planerDataTable.Columns.Add("Vrednost", typeof(string));

				DateTime workingDatum = DateTime.Now.AddYears(-1);
				DateTime krajnjiDatumPlanera = new DateTime(DateTime.Now.AddYears(5).Year, 12, 31);
				while (workingDatum <= krajnjiDatumPlanera)
				{
					TDOffice.Planer.Stavka s = planerStavkeKorisnika
						.Where(x => x.Datum.Date == workingDatum.Date)
						.FirstOrDefault();
					DataRow r = _planerDataTable.NewRow();
					r["planerStavkaID"] = s == null ? -1 : s.ID;
					r["Datum"] = workingDatum;
					r["Vrednost"] = s == null ? null : s.Body;

					workingDatum = workingDatum.AddDays(1);
					_planerDataTable.Rows.Add(r);
				}

				trenutnaForma.Invoke(
					(MethodInvoker)
						delegate
						{
							OsveziPlaner();
							dgv_Planer.Columns["planerStavkaID"].Visible = false;

							dgv_Planer.Columns["Datum"].DefaultCellStyle.Format =
								"dd.MM.yyyy (ddd)";
							dgv_Planer.Columns["Datum"].Width = 100;
							dgv_Planer.Columns["Datum"].ReadOnly = true;

							dgv_Planer.Columns["Vrednost"].Width = 100;
							dgv_Planer.Columns["Vrednost"].ReadOnly = false;

							int redZaSelektovati = (int)
								(DateTime.Now - DateTime.Now.AddYears(-1)).TotalDays;

							dgv_Planer.FirstDisplayedScrollingRowIndex =
								redZaSelektovati > 1 ? redZaSelektovati - 2 : 0;
							dgv_Planer.Rows[redZaSelektovati + 1].Selected = true;
							dgv_Planer.CurrentCell = dgv_Planer.Rows[redZaSelektovati + 1].Cells[
								"Datum"
							];
						}
				);
			});
		}

		private void OsveziPlaner()
		{
			dgv_Planer.DataSource = _planerDataTable;
			dgv_Planer.Columns["Vrednost"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
		}

		private void dgv_Planer_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			if (dgv_Planer.Columns["Vrednost"].Index == e.ColumnIndex)
			{
				DataGridViewRow dgvRow = dgv_Planer.Rows[e.RowIndex];
				int idStavke = Convert.ToInt32(dgvRow.Cells["planerStavkaID"].Value);
				string vrednost = dgvRow.Cells["Vrednost"].Value.ToString();

				if (idStavke == -1)
				{
					if (string.IsNullOrWhiteSpace(vrednost))
						return;
					DateTime dateTime = Convert.ToDateTime(dgvRow.Cells["Datum"].Value);
					int noviID = TDOffice.Planer.Stavka.Insert(
						Program.TrenutniKorisnik.ID,
						dateTime,
						vrednost
					);

					_planerDataTable.Rows[e.RowIndex]["planerStavkaID"] = noviID;

					OsveziPlaner();
				}
				else
				{
					if (string.IsNullOrWhiteSpace(vrednost))
					{
						TDOffice.Planer.Stavka.Remove(idStavke);
					}
					else
					{
						TDOffice.Planer.Stavka s = TDOffice.Planer.Stavka.Get((int)idStavke);
						s.Body = vrednost;
						s.Update();
					}
				}
			}
		}

		private void btn_Uvecaj_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_Planer>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				fm_Planer fm = new fm_Planer(_planerDataTable);
				fm.ShowDialog();
			});
		}
		#endregion

		private void popisToolStripMenuItem_Click(object sender, EventArgs e) { }

		private void korisniciToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(000000))
			{
				TDOffice.Pravo.NematePravoObavestenje(000000);
				return;
			}

			using (_1337_fm_Korisnici_List formObject = new _1337_fm_Korisnici_List())
				formObject.ShowDialog();
		}

		private void osveziMiPravaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Program.TrenutniKorisnik = TDOffice.User.Get(Program.TrenutniKorisnik.ID);
			MessageBox.Show("Prava su ti osvezena!");
		}

		private void porukeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<_1301_fm_Poruka_List>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (_1301_fm_Poruka_List pl = new _1301_fm_Poruka_List())
					pl.ShowDialog();
			});
		}

		private void zamenaRobeTD1325ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(132500))
			{
				TDOffice.Pravo.NematePravoObavestenje(132500);
				return;
			}

			if (Program.TrenutniKorisnik.MagacinID == 0)
			{
				if (!Program.TrenutniKorisnik.ImaPravo(132505))
				{
					MessageBox.Show(
						"Morate imati dodeljem magacin ili pravo za rad sa vise magacina [132505]"
					);
					return;
				}
			}

			if (Application.OpenForms.OfType<_1325_fm_ZamenaRobe_List>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (_1325_fm_ZamenaRobe_List f = new _1325_fm_ZamenaRobe_List())
					f.ShowDialog();
			});
		}

		private void noviToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(134800))
			{
				TDOffice.Pravo.NematePravoObavestenje(134800);
				return;
			}
			if (Application.OpenForms.OfType<_1348_fm_Partner_Index>().FirstOrDefault() != null)
				return;
			Task.Run(() =>
			{
				using (_1348_fm_Partner_Index pi = new _1348_fm_Partner_Index())
					pi.ShowDialog();
			});
			//_1348_fm_Partner_Index pl = Application.OpenForms.OfType<_1348_fm_Partner_Index>().FirstOrDefault();

			//if (pl == null)
			//    pl = new _1348_fm_Partner_Index();

			//pl.Show();
		}

		private void pregledDanaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(138900))
			{
				TDOffice.Pravo.NematePravoObavestenje(138900);
				return;
			}
			if (Application.OpenForms.OfType<_1389_fm_PregledDana_Index>().FirstOrDefault() != null)
				return;
			Task.Run(() =>
			{
				using (_1389_fm_PregledDana_Index formObject = new _1389_fm_PregledDana_Index())
					formObject.ShowDialog();
			});
		}

		private void specifikacijaNovcaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(134400))
			{
				TDOffice.Pravo.NematePravoObavestenje(134400);
				return;
			}
			if (
				Application.OpenForms.OfType<_1344_fm_SpecifikacijaNovca_Index>().FirstOrDefault()
				!= null
			)
				return;
			Task.Run(() =>
			{
				using (
					_1344_fm_SpecifikacijaNovca_Index formObject =
						new _1344_fm_SpecifikacijaNovca_Index()
				)
					formObject.ShowDialog();
			});
		}

		private void nalogZaPrevozToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(136600))
			{
				TDOffice.Pravo.NematePravoObavestenje(136600);
				return;
			}

			using (_1366_fm_NalogZaPrevoz_List formObject = new _1366_fm_NalogZaPrevoz_List())
			{
				formObject.Text = "Nalozi za prevoz";
				formObject.ShowDialog();
			}
		}

		private void zakljucaj500ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(135000))
			{
				TDOffice.Pravo.NematePravoObavestenje(135000);
				return;
			}

			using (_1350_fm_Zakljucaj500_Index formObject = new _1350_fm_Zakljucaj500_Index())
				formObject.ShowDialog();
		}

		private void ocekivaneUplateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(136000))
			{
				TDOffice.Pravo.NematePravoObavestenje(136000);
				return;
			}
			if (
				Application.OpenForms.OfType<_1360_fm_OcekivaneUplate_Index>().FirstOrDefault()
				!= null
			)
				return;
			Task.Run(() =>
			{
				using (
					_1360_fm_OcekivaneUplate_Index formObject = new _1360_fm_OcekivaneUplate_Index()
				)
					formObject.ShowDialog();
			});
		}

		private void definisanjeProdajneCeneToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(137000))
			{
				TDOffice.Pravo.NematePravoObavestenje(137000);
				return;
			}

			if (
				Application
					.OpenForms.OfType<_1370_fm_DefinisanjeProdajneCene_Index>()
					.FirstOrDefault() != null
			)
				return;
			Task.Run(() =>
			{
				using (
					_1370_fm_DefinisanjeProdajneCene_Index formObject =
						new _1370_fm_DefinisanjeProdajneCene_Index()
				)
					formObject.ShowDialog();
			});
		}

		private void predlogProracunaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(133200))
			{
				TDOffice.Pravo.NematePravoObavestenje(133200);
				return;
			}

			if (Application.OpenForms.OfType<_1332_fm_Proracun_List>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (_1332_fm_Proracun_List fm = new _1332_fm_Proracun_List())
					fm.ShowDialog();
			});
		}

		private void poklonToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (_1326_fm_Poklon_Index pi = new _1326_fm_Poklon_Index())
				pi.ShowDialog();
		}

		private void sifarnikToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<IzborRobe>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (IzborRobe ir = new IzborRobe())
				{
					ir.DozvoliMenjanjeMagacina = true;
					ir.Text = "Sifarnik robe";
					ir.ShowDialog();
				}
			});
		}

		private void pravilaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(136500))
			{
				TDOffice.Pravo.NematePravoObavestenje(136500);
				return;
			}
			if (
				Application
					.OpenForms.OfType<_1365_fm_SastavnicaRastavnica_Pravila_List>()
					.FirstOrDefault() != null
			)
				return;
			Task.Run(() =>
			{
				using (
					_1365_fm_SastavnicaRastavnica_Pravila_List pl =
						new _1365_fm_SastavnicaRastavnica_Pravila_List()
				)
					pl.ShowDialog();
			});
		}

		private void dokumentiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(136550))
			{
				TDOffice.Pravo.NematePravoObavestenje(136550);
				return;
			}

			if (
				Application.OpenForms.OfType<_1365_fm_SastavnicaRastavnica_List>().FirstOrDefault()
				!= null
			)
				return;

			Task.Run(() =>
			{
				using (
					_1365_fm_SastavnicaRastavnica_List srl =
						new _1365_fm_SastavnicaRastavnica_List()
				)
					srl.ShowDialog();
			});
		}

		private void ironToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(1838801))
			{
				TDOffice.Pravo.NematePravoObavestenje(1838801);
				return;
			}
			MessageBox.Show(
				"Azuriranje web IRON cena pocinje nakon sto pritisnete OK. Bicete obavesteni kada bude gotovo"
			);
			TDWeb.TDWeb.AzurirajIronCene();
			MessageBox.Show("Azuriranje IRON cena uspesno zavrseno!");
		}

		private void uredjivanjeCenaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Ako se pokrene kroz drugi thread, padajuci menu u DGV pravi problem

			using (fm_Web_UredjivanjeCena c = new fm_Web_UredjivanjeCena())
				c.ShowDialog();
		}

		private void azurirajKataloskeBrojeveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(1838802))
			{
				TDOffice.Pravo.NematePravoObavestenje(1838802);
				return;
			}

			if (
				MessageBox.Show(
					"Da li sigurno zelite azurirati kataloske brojeve na sajtu?",
					"Potvrdi",
					MessageBoxButtons.YesNo
				) == DialogResult.No
			)
				return;

			var webProizvodi = TDWeb.Proizvod.ListAsync();
			var komProizvodi = Komercijalno.Roba.ListAsync(DateTime.Now.Year);

			foreach (var p in webProizvodi.Result)
			{
				var pKom = komProizvodi.Result.Where(x => x.ID == p.RobaID).FirstOrDefault();

				if (pKom == null)
					continue;

				p.KatBr = pKom.KatBr;
				p.Update();
			}
		}

		private void radzuzenjeMagacinaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(133600))
			{
				TDOffice.Pravo.NematePravoObavestenje(133600);
				return;
			}

			using (_1336_fm_RazduzenjeMagacina_List rml = new _1336_fm_RazduzenjeMagacina_List())
				rml.ShowDialog();
		}

		private void cekoviToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_Cekovi_List>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (fm_Cekovi_List cl = new fm_Cekovi_List())
					if (!cl.IsDisposed)
						cl.ShowDialog();
			});
		}

		private void porukaZaPlanerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_PlanerPoruka_Nova>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (fm_PlanerPoruka_Nova fm = new fm_PlanerPoruka_Nova())
					if (!fm.IsDisposed)
						fm.ShowDialog();
			});
		}

		private void osveziPlaner_btn_Click(object sender, EventArgs e)
		{
			UcitajPlanerAsync();
		}

		private void wEBLOGINToolStripMenuItem_Click(object sender, EventArgs e)
		{
			try
			{
				string apiKey = Program.APIToken;
				if (string.IsNullOrWhiteSpace(apiKey))
				{
					MessageBox.Show("Logovanje neupsesno!");
					return;
				}
				MessageBox.Show("Uspesno ste se ulogovali!");
				wEBLOGINToolStripMenuItem.Visible = false;
				azurirajCeneToolStripMenuItem.Visible = true;
				azurirajKataloskeBrojeveToolStripMenuItem.Visible = true;
				uredjivanjeCenaToolStripMenuItem.Visible = true;
			}
			catch
			{
				MessageBox.Show("Pogresno korisnicko ime ili lozinka!");
			}
		}

		private void sampaEtiketeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_StampaEtikete>().FirstOrDefault() != null)
				return;
			Task.Run(() =>
			{
				using (fm_StampaEtikete stet = new fm_StampaEtikete())
				{
					stet.Text = "Stampa etikete";
					stet.ShowDialog();
				}
			});
		}

		private void nezakljucaneKalkulacijeFalePotpisaniDokumentiToolStripMenuItem1_Click(
			object sender,
			EventArgs e
		)
		{
			PosaljiIzvestajNezakljucanihMPKalkulacijaKaoPoruku();
			MessageBox.Show("Izvestaj Vam je poslat u vidu poruke!");
		}

		private void kontrolaLageraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_KontrolaLagera_Index>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (fm_KontrolaLagera_Index i = new fm_KontrolaLagera_Index())
					i.ShowDialog();
			});
		}

		private void masovniSMSToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_SmsMasovni_Index>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (fm_SmsMasovni_Index s = new fm_SmsMasovni_Index())
					if (!s.IsDisposed)
						s.ShowDialog();
			});
		}

		private void kontaktiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_Kontakt_List>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (fm_Kontakt_List k = new fm_Kontakt_List())
					if (!k.IsDisposed)
						k.ShowDialog();
			});
		}

		private void istorijaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_SmsIstorija_Index>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (fm_SmsIstorija_Index k = new fm_SmsIstorija_Index())
					if (!k.IsDisposed)
						k.ShowDialog();
			});
		}

		private void promenjiveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (
				Application.OpenForms.OfType<fm_PodesavanjePromenjivih_List>().FirstOrDefault()
				!= null
			)
				return;

			Task.Run(() =>
			{
				using (fm_PodesavanjePromenjivih_List l = new fm_PodesavanjePromenjivih_List())
					if (!l.IsDisposed)
						l.ShowDialog();
			});
		}

		private void postaviMinimalnuVerzijuToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(2))
			{
				TDOffice.Pravo.NematePravoObavestenje(2);
				//return;
			}
			TDOffice.Config<string> minimalProductVersion = TDOffice.Config<string>.Get(1337);
			if (
				MessageBox.Show(
					"Trenutna minimalna verzija programa je "
						+ minimalProductVersion.Tag
						+ ". Da li je zelite promeniti?",
					"Minimalna verzija",
					MessageBoxButtons.YesNo
				) != DialogResult.Yes
			)
				return;

			using (
				InputBox ib = new InputBox(
					"Nova Minimalna Verzija",
					"Unesite novu minimalnu verziju programa"
				)
			)
			{
				ib.ShowDialog();

				if (string.IsNullOrWhiteSpace(ib.returnData))
				{
					MessageBox.Show("Neispravna Verzija!");
					return;
				}

				minimalProductVersion.Tag = ib.returnData.ToString();
				minimalProductVersion.UpdateOrInsert();
			}
		}

		private void magaciniToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(140000))
			{
				TDOffice.Pravo.NematePravoObavestenje(140000);
				return;
			}

			if (Application.OpenForms.OfType<fm_Magacin_List>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (fm_Magacin_List l = new fm_Magacin_List())
					if (!l.IsDisposed)
						l.ShowDialog();
			});
		}

		private void listaMagacinaUPopisuToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show("Radi se...");
			return;
			if (Application.OpenForms.OfType<fm_MagacinUPopisu_Index>().FirstOrDefault() != null)
				return;
			Task.Run(() =>
			{
				using (fm_MagacinUPopisu_Index mup = new fm_MagacinUPopisu_Index())
					mup.ShowDialog();
			});
		}

		private void robaToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (
				Application.OpenForms.OfType<fm_Izvestaj_Prodaja_Roba_Setup>().FirstOrDefault()
				!= null
			)
				return;
			Task.Run(() =>
			{
				using (fm_Izvestaj_Prodaja_Roba_Setup mup = new fm_Izvestaj_Prodaja_Roba_Setup())
					if (!mup.IsDisposed) // moze biti disposed ako nema pravo
						mup.ShowDialog();
			});
		}

		private void prodajaRobeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			robaToolStripMenuItem1.PerformClick();
		}

		private void prodajaRobeToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			robaToolStripMenuItem1.PerformClick();
		}

		private void detaljnaAnalizaPartneraToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				using (fm_Partner_Analiza_General_Index ag = new fm_Partner_Analiza_General_Index())
					if (!ag.IsDisposed)
						ag.ShowDialog();
			});
		}

		private void detaljnaAnalizaPartneraToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			detaljnaAnalizaPartneraToolStripMenuItem1.PerformClick();
		}

		private void neaktivniPartneriToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (
				Application.OpenForms.OfType<fm_Partner_Analiza_Neaktivni_Index>().FirstOrDefault()
				!= null
			)
				return;

			Task.Run(() =>
			{
				using (
					fm_Partner_Analiza_Neaktivni_Index np = new fm_Partner_Analiza_Neaktivni_Index()
				)
					if (!np.IsDisposed)
						np.ShowDialog();
			});
		}

		private void checkListaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_CheckLista_Index>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (fm_CheckLista_Index ci = new fm_CheckLista_Index())
					if (!ci.IsDisposed)
						ci.ShowDialog();
			});
		}

		private void neaktivniPartneriToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (
				Application.OpenForms.OfType<fm_Partner_Analiza_Neaktivni_Index>().FirstOrDefault()
				!= null
			)
				return;

			Task.Run(() =>
			{
				using (
					fm_Partner_Analiza_Neaktivni_Index np = new fm_Partner_Analiza_Neaktivni_Index()
				)
					if (!np.IsDisposed)
						np.ShowDialog();
			});
		}

		private void prometMagacinaToolStripMenuItem_Click_1(object sender, EventArgs e)
		{
			if (
				Application.OpenForms.OfType<fm_Izvestaj_Promet_Magacina>().FirstOrDefault() != null
			)
				return;

			Task.Run(() =>
			{
				using (fm_Izvestaj_Promet_Magacina pm = new fm_Izvestaj_Promet_Magacina())
					if (!pm.IsDisposed)
						pm.ShowDialog();
			});
		}

		private void odobreniRabatiUProdajiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_OdobreniRabati>().FirstOrDefault() != null)
				return;
			Task.Run(() =>
			{
				using (fm_OdobreniRabati orp = new fm_OdobreniRabati())
					if (!orp.IsDisposed)
						orp.ShowDialog();
			});
		}

		private void prevozniciToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_Prevoznik_List>().FirstOrDefault() != null)
				return;
			Task.Run(() =>
			{
				using (fm_Prevoznik_List pl = new fm_Prevoznik_List())
					if (!pl.IsDisposed) // moze biti disposed ako nema pravo
						pl.ShowDialog();
			});
		}

		private void kursnaListaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				DataTable dt = new DataTable();
				dt.Columns.Add("Datum", typeof(DateTime));
				dt.Columns.Add("EUR", typeof(double));
				dt.Columns.Add("RSD", typeof(double));
				dt.Columns.Add("USD", typeof(double));

				List<TDOffice.Kurs> kursList = TDOffice.Kurs.List();
				foreach (TDOffice.Kurs k in kursList)
				{
					DataRow dr = dt.NewRow();
					dr["Datum"] = k.Datum.Date;
					dr["EUR"] = k.Eur;
					dr["RSD"] = k.Rsd;
					dr["USD"] = k.Usd;
					dt.Rows.Add(dr);
				}
				using (DataGridViewSelectBox pl = new DataGridViewSelectBox(dt))
					if (!pl.IsDisposed) // moze biti disposed ako nema pravo
						pl.ShowDialog();
			});
		}

		private void prometMagacinaToolStripMenuItem1_Click_1(object sender, EventArgs e)
		{
			prometMagacinaToolStripMenuItem.PerformClick();
		}

		private void odobreniRabatiUProdajiToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			odobreniRabatiUProdajiToolStripMenuItem.PerformClick();
		}

		private void btn_OsveziBelesku_Click(object sender, EventArgs e)
		{
			RichTextBox rtb = tabControl1
				.SelectedTab.Controls.OfType<RichTextBox>()
				.FirstOrDefault();
			Program.TrenutniKorisnik = TDOffice.User.Get(Program.TrenutniKorisnik.ID);
			tabControl1.SelectedTab.Controls.OfType<RichTextBox>().FirstOrDefault().Text = Program
				.TrenutniKorisnik.Tag.Beleske.Where(x => x.ID == Convert.ToInt32(rtb.Tag))
				.FirstOrDefault()
				.Body;
			odustaniOdCuvanjaBeleske_btn.Hide();
			sacuvajBelesku_btn.Hide();
		}

		private void nelogicneMarzePoslednjih10DanaToolStripMenuItem_Click(
			object sender,
			EventArgs e
		)
		{
			if (Program.PokrenutIzvestajNelogicnihMarzi)
			{
				MessageBox.Show(
					$"Vec ste pokrenuli ovu akciju, sacekajte svoj izvestaj!\nTrenutni napredak: {Program.IzvestajNelogicnihMarziTrenutniStage} / {Program.IzvestajNelogicnihMarziMaxStage}"
				);
				return;
			}
			Task.Run(() =>
			{
				PosaljiIzvestajDokumenataUKojimaJeMarzaManjaOdProsecneKaoPoruku();
			});
			MessageBox.Show(
				"Izvestaj se radi u pozadini i bice Vam poslat kao poruka kada bude gotov!"
			);
		}

		private void nelogicneMarzeUProdajiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			nelogicneMarzePoslednjih10DanaToolStripMenuItem.PerformClick();
		}

		private async void skiniNajnovijuVerzijuProgramaToolStripMenuItem_Click(
			object sender,
			EventArgs e
		)
		{
			await Program.DownloadAndInstallNewVersionAsync();
		}

		private int _menadzment { get; set; } = 0;

		private void Main_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.M)
			{
				_menadzment++;

				if (Program.TrenutniKorisnik.ID <= 2 && _menadzment >= 5) // 1 Aleksa, 2 Sasa
				{
					using (Menadzment m = new Menadzment())
						m.ShowDialog();
				}
			}
		}

		private void patchLogToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_PatchLog_Index pl = new fm_PatchLog_Index())
			{
				pl.ShowDialog();
			}
		}

		private void biznisPlanToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_BiznisPlan_Index>().FirstOrDefault() != null)
				return;
			Task.Run(() =>
			{
				using (fm_BiznisPlan_Index dk = new fm_BiznisPlan_Index())
					if (!dk.IsDisposed)
						dk.ShowDialog();
			});
		}

		private void mojKupacToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_MojKupac_Index>().FirstOrDefault() != null)
				return;

			using (fm_MojKupac_Index mk = new fm_MojKupac_Index())
				if (!mk.IsDisposed)
					mk.ShowDialog();
		}

		private void popisRobeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(700000))
			{
				TDOffice.Pravo.NematePravoObavestenje(700000);
				return;
			}

			if (Application.OpenForms.OfType<_7_fm_TDPopis_List>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (_7_fm_TDPopis_List pl = new _7_fm_TDPopis_List())
					if (!pl.IsDisposed)
						pl.ShowDialog();
			});
		}

		private void automatskiMiGenerisiVanredniPopisToolStripMenuItem_Click(
			object sender,
			EventArgs e
		)
		{
			if (
				MessageBox.Show(
					"Ovom akcijom ce Vam automatski biti generisan vanredni popis za Vas magacin. Da li ga zelite generisati?",
					"Generisanje popisa",
					MessageBoxButtons.YesNo
				) != DialogResult.Yes
			)
				return;

			Models.TDTask.GenerisiPopis(
				Program.TrenutniKorisnik.MagacinID,
				Program.TrenutniKorisnik.ID
			);

			MessageBox.Show("Popis uspesno generisan!");
		}

		private void sakrijSvePorukeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(1866000))
			{
				TDOffice.Pravo.NematePravoObavestenje(1866000);
				return;
			}

			IEnumerable<Form> forms = Application
				.OpenForms.OfType<_1301_fm_Poruka_Index>()
				.ToList();
			foreach (var f in forms)
			{
				(f as _1301_fm_Poruka_Index).ForceClose = true;
				f.Close();
			}
		}

		private void prikaziNearhiviranePorukeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			_1301_fm_Poruka_Index.IDeviSakrivenihPoruka.Clear();
		}

		private void ulaznaPonudaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(136580))
			{
				TDOffice.Pravo.NematePravoObavestenje(136580);
				return;
			}

			if (Application.OpenForms.OfType<fm_UlaznaPonuda_List>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (fm_UlaznaPonuda_List up = new fm_UlaznaPonuda_List())
					if (!up.IsDisposed)
						up.ShowDialog();
			});
		}

		private void ugovorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_Ugovor_List us = new fm_Ugovor_List())
				if (!us.IsDisposed)
					us.ShowDialog();
		}

		private void zaposleniToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_Zaposleni_List>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (fm_Zaposleni_List zap = new fm_Zaposleni_List())
				{
					zap.Text = "Zaposleni";
					zap.ShowDialog();
				}
			});
		}

		private void godisjniPregledLageraKomercijalnoKEPUToolStripMenuItem_Click(
			object sender,
			EventArgs e
		)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(136580))
			{
				TDOffice.Pravo.NematePravoObavestenje(136580);
				return;
			}

			if (Application.OpenForms.OfType<fm_UlaznaPonuda_List>().FirstOrDefault() != null)
				return;

			Task.Run(() =>
			{
				using (
					fm_GodisnjiPregledLageraKomercijalnoKepu_Index km =
						new fm_GodisnjiPregledLageraKomercijalnoKepu_Index()
				)
					if (!km.IsDisposed)
						km.ShowDialog();
			});
		}

		private void firmeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_Firme_List f = new fm_Firme_List())
				f.ShowDialog();
		}

		private void pregledStorniranihPovratnicaDuploFIskalizovanihRacunaToolStripMenuItem_Click(
			object sender,
			EventArgs e
		)
		{
			using (
				fm_PregledStorniranihDuploPovratnica_Index p =
					new fm_PregledStorniranihDuploPovratnica_Index()
			)
				if (!p.IsDisposed)
					p.ShowDialog();
		}

		private void sviPartneriToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_Partner_List pl = new fm_Partner_List())
				if (!pl.IsDisposed)
					pl.ShowDialog();
		}

		private void komercijalnoParametriToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_KomercijalnoParametri_Index pi = new fm_KomercijalnoParametri_Index())
				if (!pi.IsDisposed)
					pi.ShowDialog();
		}

		private void fiskalniRacuniToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_FiskalniRacuni_list frl = new fm_FiskalniRacuni_list())
				frl.ShowDialog();
		}

		private void nelogicniNaciniUplataToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_NelogicniNaciniUplata_Index nu = new fm_NelogicniNaciniUplata_Index())
				if (!nu.IsDisposed)
					nu.ShowDialog();
		}

		private void promenaReferentaDokumentaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (
				fm_PromenaReferentaDokumenta_Index prd = new fm_PromenaReferentaDokumenta_Index()
			)
				if (!prd.IsDisposed)
					prd.ShowDialog();
		}

		private void klonirajToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			// fm_Komercijalno_Dokument_Kloniraj_Index
			if (
				Application
					.OpenForms.OfType<fm_Komercijalno_Dokument_Kloniraj_Index>()
					.FirstOrDefault() != null
			)
				return;
			Task.Run(() =>
			{
				using (
					fm_Komercijalno_Dokument_Kloniraj_Index dKloniraj =
						new fm_Komercijalno_Dokument_Kloniraj_Index()
				)
					if (!dKloniraj.IsDisposed)
						dKloniraj.ShowDialog();
			});
		}

		private void kopirajStavkeToolStripMenuItem1_Click(object sender, EventArgs e)
		{ // fm_Komercijalno_Dokument_Kloniraj_Index
			if (
				Application
					.OpenForms.OfType<fm_Komercijalno_Dokument_KopirajStavke_Index>()
					.FirstOrDefault() != null
			)
				return;
			Task.Run(() =>
			{
				using (
					fm_Komercijalno_Dokument_KopirajStavke_Index dk =
						new fm_Komercijalno_Dokument_KopirajStavke_Index()
				)
					if (!dk.IsDisposed)
						dk.ShowDialog();
			});
		}

		private void tToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Program.TrenutniKorisnik.ID > 2)
			{
				return;
			}

			List<Komercijalno.IstUpl> istUpls = Komercijalno.IstUpl.List();
			List<Komercijalno.Dokument> doks = Komercijalno.Dokument.List("VRDOK = 15");
			foreach (Komercijalno.Dokument dok in doks.Where(x => x.NUID == NacinUplate.Kartica))
			{
				List<Komercijalno.IstUpl> iu = istUpls
					.Where(x => x.VrDok == dok.VrDok && x.BrDok == dok.BrDok)
					.ToList();

				if (!iu.Any(x => x.NUID == 5))
					continue;

				if (iu.Count(x => x.NUID == 11 || x.NUID == 5) == iu.Count)
				{
					foreach (IstUpl i in iu.Where(x => x.NUID == 5))
					{
						i.NUID = 11;
						i.Update(DateTime.Now.Year);
					}
				}
			}

			MessageBox.Show("Gotovo!");
		}

		private void pocetnoKrajnjeToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(134700))
			{
				TDOffice.Pravo.NematePravoObavestenje(134700);
				return;
			}
			if (
				Application
					.OpenForms.OfType<_1347_fm_PartnerPocetnoKrajnje_Index>()
					.FirstOrDefault() != null
			)
				return;
			Task.Run(() =>
			{
				using (
					_1347_fm_PartnerPocetnoKrajnje_Index formObject =
						new _1347_fm_PartnerPocetnoKrajnje_Index()
				)
					formObject.ShowDialog();
			});
		}

		private void poveziUplateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(135200))
			{
				TDOffice.Pravo.NematePravoObavestenje(135200);
				return;
			}
			if (
				Application.OpenForms.OfType<_1352_fm_RasporediUplate_Index>().FirstOrDefault()
				!= null
			)
				return;
			Task.Run(() =>
			{
				using (
					_1352_fm_RasporediUplate_Index formObject = new _1352_fm_RasporediUplate_Index()
				)
					formObject.ShowDialog();
			});
		}

		private void izdvojMPRacuneKojiUKomercijalnomNisufiskalizovaniToolStripMenuItem_Click(
			object sender,
			EventArgs e
		)
		{
			Task.Run(() =>
			{
				List<Komercijalno.Dokument> doks = Komercijalno
					.Dokument.List("VRDOK = 15 AND FLAG <> 0 AND DUGUJE > 0")
					.Where(x => x.Datum.Date >= new DateTime(2022, 4, 14).Date)
					.ToList();
				List<Komercijalno.DokumentFisk> dokFisk = Komercijalno.DokumentFisk.List(
					DateTime.Now.Year
				);

				List<string> output = new List<string>();
				foreach (Dokument dok in doks)
					if (!dokFisk.Any(x => x.VrDok == dok.VrDok && x.BrDok == dok.BrDok))
						output.Add($"{dok.VrDok}, {dok.BrDok}");

				string path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
				string fileName = "MPRacuniNefiskKomercijalno.txt";
				System.IO.File.WriteAllLines(System.IO.Path.Combine(path, fileName), output);
				MessageBox.Show(
					$"Generisan fajl '{fileName}' na radnoj povrsini! Fajl ce biti automatski otvoren nako zatvaranja ovog prozora."
				);
				Process.Start(System.IO.Path.Combine(path, fileName));
			});
			MessageBox.Show("Pokrenuli ste akciju. Bicete obavesteni kada bude gotova!");
		}

		private void robaToolStripMenuItem2_Click(object sender, EventArgs e)
		{
			var asd = TDWeb.Proizvod.List();

			return;

			//HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "http://35.158.133.211/api/Korisnik/GetToken?username=" + username + "&password=" + password);
			var request = new HttpRequestMessage();
			HttpClient client = new HttpClient();
			HttpResponseMessage response = client.SendAsync(request).Result;

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				Program.APIToken = response.Content.ReadAsStringAsync().Result;
				return;
			}

			return;
		}

		private void retroRabat3ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (
				fm_Partner_Analiza_Retro_Popust_Index rp =
					new fm_Partner_Analiza_Retro_Popust_Index()
			)
				if (!rp.IsDisposed)
					rp.ShowDialog();
		}

		private void specijalniCenovnikToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_SpecijalniCenovnik_List cl = new fm_SpecijalniCenovnik_List())
				if (!cl.IsDisposed)
					cl.ShowDialog();
		}

		private void parametriPartneraKomercijalnoToolStripMenuItem_Click(
			object sender,
			EventArgs e
		)
		{
			using (_1348_fm_KomercijalnoPartner_List pi = new _1348_fm_KomercijalnoPartner_List())
				if (!pi.IsDisposed)
					pi.ShowDialog();
		}

		private void definisanjeProdajneCenePoSpecijalnimCenovnicimaToolStripMenuItem_Click(
			object sender,
			EventArgs e
		)
		{
			using (
				fm_DefinisanjeProdajneCenePoSpecijalnomCenovniku_Index d =
					new fm_DefinisanjeProdajneCenePoSpecijalnomCenovniku_Index()
			)
				if (!d.IsDisposed)
					d.ShowDialog();
		}

		private void backupToolStripMenuItem_Click(object sender, EventArgs e) { }

		private void napraviToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				using (fm_Backup_Napravi_Index bn = new fm_Backup_Napravi_Index())
					if (!bn.IsDisposed)
						bn.ShowDialog();
			});

			//using (fm_Backup_Napravi_Index bn = new fm_Backup_Napravi_Index())
			//{
			//    bn.ShowDialog();
			//}
		}

		private void skiniToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				using (fm_Backup_Skini_Index bs = new fm_Backup_Skini_Index())
					if (!bs.IsDisposed)
						bs.ShowDialog();
			});
			//using (fm_Backup_Skini_Index bs = new fm_Backup_Skini_Index())
			//{
			//    bs.ShowDialog();
			//}
		}

		private void tDBrainv3ToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (
				Program.TrenutniKorisnik.ID != 1
				&& Program.TrenutniKorisnik.ID != 24
				&& Program.TrenutniKorisnik.ID != 2
			)
			{
				MessageBox.Show("Greska!");
				return;
			}

			using (fm_Brain_Index bi = new fm_Brain_Index())
				bi.ShowDialog();
		}

		private void bonusZakljucavanjaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_BonusZakljucavanja bz = new fm_BonusZakljucavanja())
			{
				bz.ShowDialog();
			}
		}

		private void oslobodiWebProracun8IFlagToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(1839500))
			{
				TDOffice.Pravo.NematePravoObavestenje(1839500);
				return;
			}
			using (
				InputBox ib = new InputBox(
					"Oslobodi Web Proracun (8 i flag)",
					"Unesite broj dokumenta proracuna kome zelite staviti flag na 0 i osloboditi '8'!"
				)
			)
			{
				ib.ShowDialog();
				if (ib.DialogResult != DialogResult.OK)
					return;

				string ret = ib.returnData;
				int brDok = 0;
				try
				{
					brDok = Convert.ToInt32(ret);
				}
				catch
				{
					MessageBox.Show("Neispravan broj dokumenta!");
				}

				Komercijalno.Dokument dok = Komercijalno.Dokument.Get(DateTime.Now.Year, 32, brDok);

				if (dok == null)
				{
					MessageBox.Show("Dokument proracuna sa datim brojem nije pronadjen!");
					return;
				}

				dok.VrDokIn = null;
				dok.Flag = 0;
				dok.Update();

				MessageBox.Show("Dokument uspesno oslobodjen!");
			}
		}

		private void testToolStripMenuItem_Click(object sender, EventArgs e) { }

		private void SmanjiMarzuUPocetnimStanjimaPoNekimKriterijumima()
		{
			try
			{
#if DEBUG
				string connStr1 =
					"data source=192.168.0.3; initial catalog = e:\\4monitor\\Poslovanje\\Baze\\2022\\FIRMA2022.FDB; user=SYSDBA; password=masterkey";
#else
				string connStr1 =
					"data source=4monitor; initial catalog = C:\\Poslovanje\\Baze\\2022\\FIRMA2022.FDB; user=SYSDBA; password=m; pooling=True";
#endif

				FbConnection con = new FbConnection(connStr1);
				con.Open();

				List<Komercijalno.Stavka> stavke1 = Komercijalno.Stavka.List(
					con,
					"VRDOK = 0 AND MAGACINID IN (13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28)"
				);

				List<Komercijalno.Roba> roba = Komercijalno.Roba.List(con);
				List<Komercijalno.Tarife> tarife = Komercijalno.Tarife.List(con);
				foreach (Komercijalno.Stavka stavka in stavke1)
				{
					var rob = roba.FirstOrDefault(x => x.ID == stavka.RobaID);
					var tarifa = tarife.FirstOrDefault(x => x.TarifaID == stavka.TarifaID);

					if (rob == null)
						continue;

					if (tarifa == null)
						continue;

					if (stavka.Kolicina > 0 && stavka.ProdajnaCena == 0)
					{
						Task.Run(() =>
						{
							MessageBox.Show(
								$"Stavka ID [{stavka.StavkaID}] proizvod [{rob.Naziv}] u"
									+ $"dokumentu {stavka.VrDok}, {stavka.BrDok} ima kolicinu, a prodajna cena joj je 0!"
							);
						});
						continue;
					}

					if (stavka.ProdajnaCena == 0)
						continue;

					double prodajnaCenaBezPoreza =
						stavka.ProdajnaCena / (((double)100 + (double)tarifa.Stopa) / (double)100);
					double nabavnaCena = stavka.NabavnaCena;

					if (stavka.NabavnaCena == 0)
					{
						stavka.NabavnaCena = prodajnaCenaBezPoreza / 1.1;
						stavka.Update(con);
						continue;
					}

					double trenutnaMarza = prodajnaCenaBezPoreza / stavka.NabavnaCena * 100 - 100;
					double buducaMarza = 30;
					if (trenutnaMarza < 3)
						buducaMarza = 19;
					else if (trenutnaMarza < 10)
						buducaMarza = 22;
					else if (trenutnaMarza < 30)
						buducaMarza = 24;

					double buducaNabavna = prodajnaCenaBezPoreza / ((100 + buducaMarza) / 100);

					stavka.NabavnaCena = buducaNabavna;
					stavka.Update(con);
				}
				con.Close();
				con.Dispose();
				MessageBox.Show("Gotovo!");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		StringBuilder log = new StringBuilder();
		int cDepth = 0;
		int maxDepth = 10;

		private void SrediNabavneCeneSvihDokumenataSvihMagacina()
		{
#if DEBUG
			string connStr1 =
				"data source=192.168.0.3; initial catalog = e:\\4monitor\\Poslovanje\\Baze\\2022\\FIRMA2022.FDB; user=SYSDBA; password=masterkey";
#else
			string connStr1 =
				"data source=4monitor; initial catalog = C:\\Poslovanje\\Baze\\2022\\FIRMA2022.FDB; user=SYSDBA; password=m; pooling=True";
#endif
			log = new StringBuilder();
			log.AppendLine("Zapocinjem akciju sredjivanja nabavnih cena u svim dokumentima");

			log.AppendLine($"Kreiram konekciju sa bazom. Conn string: {connStr1}");
			FbConnection con = new FbConnection(connStr1);
			log.AppendLine("Otvaram konekciju");
			con.Open();

			log.AppendLine("Ucitavam dokumente iz baze...");
			List<Komercijalno.Dokument> dokumenti = Komercijalno.Dokument.List(
				con,
				"VRDOK <> 90 AND VRDOK <> 36 AND VRDOK <> 6"
			);
			log.AppendLine("Dokumenti ucitani!");
			log.AppendLine("Ucitavam stavke iz baze...");
			List<Komercijalno.Stavka> stavkeList = Komercijalno.Stavka.List(
				con,
				"VRDOK <> 90 AND VRDOK <> 36 AND VRDOK <> 6"
			);

			// Stavke spakovanje u dict i to prvi key je vrdok, drugi brdok, vrednosti su stavke u tom dokumentu
			Dictionary<int, Dictionary<int, List<Komercijalno.Stavka>>> stavke =
				new Dictionary<int, Dictionary<int, List<Stavka>>>();

			foreach (Komercijalno.Stavka st in stavkeList)
			{
				if (!stavke.ContainsKey(st.VrDok))
					stavke.Add(st.VrDok, new Dictionary<int, List<Stavka>>());

				if (!stavke[st.VrDok].ContainsKey(st.BrDok))
					stavke[st.VrDok].Add(st.BrDok, new List<Stavka>());

				stavke[st.VrDok][st.BrDok].Add(st);
			}
			log.AppendLine("Stavke ucitane!");

			// Dict cuva za svaki magacin za svaku robu njenu trenutnu cenu u memoriji
			// Ide redom dokumentima, ako dokument diktira cenu magacinu, cena ce biti azurirana u dict za tu robu taj magacin
			// Ako ne diktira cenu onda ce uzeti iz dict-a cenu i staviti je u sebe
			Dictionary<int, Dictionary<int, double>> nabavnaCena =
				new Dictionary<int, Dictionary<int, double>>();

			// Instanciram sve magacine koje nadjem gore u dokumentima u dict
			foreach (int magacinid in dokumenti.Select(x => x.MagacinID).Distinct())
				nabavnaCena.Add(magacinid, new Dictionary<int, double>());

			log.AppendLine(
				"Zapocinjem proveru da li je moguce izvrsiti sredjivanje nabavnih cena..."
			);
			DateTime datum = new DateTime(2022, 1, 1);

			try
			{
				//while (datum.Year == 2022)
				//{
				//    log.AppendLine($"[ Provera ] Dan: {datum.ToString("dd.MM.yyyy")}");
				//    log.AppendLine($"[ Provera ] Ucitavam dokumente na dan i redjam ih...");
				//    List<Komercijalno.Dokument> dokumentiNaDan = dokumenti.Where(x => x.Datum.Date == datum.Date)
				//            .OrderBy(x => Convert.ToInt64(x.Linked)).ToList();

				//    Dictionary<int, List<Tuple<int, int>>> sredjeniBrojeviDokumenataZaDan = new Dictionary<int, List<Tuple<int, int>>>();

				//    foreach (int magacinid in dokumentiNaDan.Select(x => x.MagacinID).Distinct())
				//        sredjeniBrojeviDokumenataZaDan.Add(magacinid, new List<Tuple<int, int>>());

				//    log.AppendLine($"[ Provera ] Zapocinjem prolazak kroz magacine i proveru mogucnosti izvrsenja sredjivanja nabavnih cena za svaki");
				//    // Popunjavam dict magacinima koji imaju dokumente na danasnji dan
				//    foreach (int magacinid in dokumentiNaDan.Select(x => x.MagacinID).Distinct())
				//    {
				//        log.AppendLine($"[ Provera ] Zapocinjem proveru mogucnosti izvrsenja sredjivanja nabavnih cena za magacin {magacinid} za dan {datum.ToString("dd.MM.yyyy")}");
				//        SrediNabavneCeneUDokumentimaZaMagacinNaDan1(dokumentiNaDan, magacinid, dokumenti, ref nabavnaCena,
				//            ref stavke, ref sredjeniBrojeviDokumenataZaDan, con);
				//    }

				//    log.AppendLine($"[ Provera ] Zavrsena provera mogucnosti izvrsenja sredjivanja nabavnih cena za svaki za dan ${datum.ToString("dd.MM.yyyy")}");

				//    datum = datum.AddDays(1);
				//}
				//log.AppendLine($"Provera uspesna!");
				//MessageBox.Show("Provera je zavrsena. Nakon klika na OK ce se pokrenuti akcija nad bazom!");

				while (datum.Year == 2022)
				{
					log.AppendLine($"[ Akcija ] Dan: {datum.ToString("dd.MM.yyyy")}");
					log.AppendLine($"[ Akcija ] Ucitavam dokumente na dan i redjam ih...");
					List<Komercijalno.Dokument> dokumentiNaDan = dokumenti
						.Where(x => x.Datum.Date == datum.Date)
						.OrderBy(x => Convert.ToInt64(x.Linked))
						.ToList();

					Dictionary<int, List<Tuple<int, int>>> sredjeniBrojeviDokumenataZaDan =
						new Dictionary<int, List<Tuple<int, int>>>();

					foreach (int magacinid in dokumentiNaDan.Select(x => x.MagacinID).Distinct())
						sredjeniBrojeviDokumenataZaDan.Add(magacinid, new List<Tuple<int, int>>());

					log.AppendLine(
						$"[ Akcija ] Zapocinjem prolazak kroz magacine i izvrsavam sredjivanja nabavnih cena za svaki"
					);
					// Popunjavam dict magacinima koji imaju dokumente na danasnji dan
					foreach (int magacinid in dokumentiNaDan.Select(x => x.MagacinID).Distinct())
					{
						log.AppendLine(
							$"[ Akcija ] Zapocinjem izvrsenje sredjivanja nabavnih cena za magacin {magacinid} za dan {datum.ToString("dd.MM.yyyy")}"
						);
						SrediNabavneCeneUDokumentimaZaMagacinNaDan1(
							dokumentiNaDan,
							magacinid,
							dokumenti,
							ref nabavnaCena,
							ref stavke,
							ref sredjeniBrojeviDokumenataZaDan,
							con,
							false
						);
					}

					log.AppendLine(
						$"[ Akcija ] Izvrseno sredjivanje nabavnih cena za dan ${datum.ToString("dd.MM.yyyy")}"
					);

					datum = datum.AddDays(1);
				}
				log.AppendLine($"Izvrsena akcija sredjivanja nabavnih cena!");
				MessageBox.Show("Izvrsena je akcija sredjivanja nabavnih cena!");
			}
			catch (aaaException ex)
			{
				System.IO.File.WriteAllText(
					System.IO.Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
						"TDOffice_Log_Mini.txt"
					),
					""
				);
				System.IO.File.WriteAllText(
					System.IO.Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
						"TDOffice_Log.txt"
					),
					log.ToString()
				);

				string[] lines = System.IO.File.ReadAllLines(
					System.IO.Path.Combine(
						Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
						"TDOffice_Log.txt"
					)
				);

				int nL = 0;
				for (int i = lines.Length - 1; i >= 0; i--)
				{
					if (lines[i].ToString().ToLower().IndexOf("dokumentu izvorni") >= 0)
					{
						System.IO.File.AppendAllLines(
							System.IO.Path.Combine(
								Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
								"TDOffice_Log_Mini.txt"
							),
							new string[] { lines[i], lines[i + 1] }
						);
						nL++;
						if (nL > 20)
							break;
					}
				}

				MessageBox.Show(ex.ToString());
			}
			con.Close();
			con.Dispose();
		}

		private void SrediNabavneCeneUDokumentimaZaMagacinNaDan1(
			List<Komercijalno.Dokument> dokumentiNaDan,
			int magacinid,
			List<Komercijalno.Dokument> dokumenti,
			ref Dictionary<int, Dictionary<int, double>> nabavnaCena,
			ref Dictionary<int, Dictionary<int, List<Komercijalno.Stavka>>> stavke,
			ref Dictionary<int, List<Tuple<int, int>>> sredjeniDokumentiZaDan,
			FbConnection con,
			bool provera = true
		)
		{
			List<int> robaIDsUDokumentimaNaDan = new List<int>();

			log.AppendLine(
				$"[ {(provera ? "Provera" : "Izvrsenje")} ] Selektujem robaID-eve stavki koje su se pojavile u dokumentima na dan..."
			);
			foreach (
				Komercijalno.Dokument dokumentNaDan in dokumentiNaDan.Where(x =>
					x.MagacinID == magacinid
				)
			)
				if (
					stavke.ContainsKey(dokumentNaDan.VrDok)
					&& stavke[dokumentNaDan.VrDok].ContainsKey(dokumentNaDan.BrDok)
				)
					robaIDsUDokumentimaNaDan.AddRange(
						stavke[dokumentNaDan.VrDok][dokumentNaDan.BrDok].Select(x => x.RobaID)
					);

			robaIDsUDokumentimaNaDan = robaIDsUDokumentimaNaDan.Distinct().ToList();

			log.AppendLine(
				$"[ {(provera ? "Provera" : "Izvrsenje")} ] Lista robe koja se pojavila na dan {string.Join(", ", robaIDsUDokumentimaNaDan)}"
			);

			foreach (int robaid in robaIDsUDokumentimaNaDan)
			{
				log.AppendLine(
					$"[ {(provera ? "Provera" : "Izvrsenje")} ] Zapocinjem akciju nad robom id {robaid} magacina {magacinid} dan {dokumentiNaDan[0].Datum.ToString("dd.MM.yyyy")}"
				);
				SrediNabavneCeneUDokumentimaZaMagacinNaDanZaRobaID(
					dokumentiNaDan,
					dokumenti,
					ref stavke,
					ref nabavnaCena,
					robaid,
					magacinid,
					ref sredjeniDokumentiZaDan,
					con
				);
			}
		}

		class aaaException : Exception
		{
			public aaaException() { }

			public aaaException(string message)
				: base(message) { }
		}

		private void SrediNabavneCeneUDokumentimaZaMagacinNaDanZaRobaID(
			List<Komercijalno.Dokument> dokumentiNaDan,
			List<Komercijalno.Dokument> dokumenti,
			ref Dictionary<int, Dictionary<int, List<Komercijalno.Stavka>>> stavke,
			ref Dictionary<int, Dictionary<int, double>> nabavnaCena,
			int robaid,
			int magacinid,
			ref Dictionary<int, List<Tuple<int, int>>> sredjeniDokumentiZaDan,
			FbConnection con,
			bool provera = true
		)
		{
			cDepth++;
			if (maxDepth < cDepth)
			{
				throw new aaaException(
					"Vrtim u beskonacno, pogledaj log na desktopu da vidis izmedju kojih dokumenata!"
				);
			}
			foreach (
				Komercijalno.Dokument dok in dokumentiNaDan
					.Where(x => x.MagacinID == magacinid)
					.OrderBy(x => Convert.ToInt64(x.Linked))
			)
			{
				log.AppendLine(
					$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}]"
				);
				if (!stavke.ContainsKey(dok.VrDok) || !stavke[dok.VrDok].ContainsKey(dok.BrDok))
				{
					log.AppendLine(
						$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Nema u sebi robu {robaid}"
					);
					continue;
				}

				Komercijalno.Stavka stavka = stavke[dok.VrDok]
					[dok.BrDok]
					.FirstOrDefault(x => x.RobaID == robaid);

				if (stavka == null)
				{
					log.AppendLine(
						$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Nema u sebi robu {robaid}"
					);
					continue;
				}

				if (new int[] { 0, 1, 2, 3 }.Contains(dok.VrDok))
				{
					log.AppendLine(
						$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Dokument sam sebi diktira cenu, postavljam cenu {stavka.NabavnaCena.ToString("#,##0.00")} kao novu"
					);
					if (!nabavnaCena[dok.MagacinID].ContainsKey(robaid))
						nabavnaCena[dok.MagacinID].Add(robaid, stavka.NabavnaCena);
					else
						nabavnaCena[dok.MagacinID][stavka.RobaID] = stavka.NabavnaCena;
				}
				else if (new int[] { 18, 26 }.Contains(dok.VrDok))
				{
					log.AppendLine(
						$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Dokumentu izvorni dokument diktira cenu. Izvorni dokument vr: {dok.VrDokIn}, br: {dok.BrDokIn}"
					);
					Komercijalno.Dokument izvorniDokument = dokumenti.First(x =>
						x.VrDok == dok.VrDokIn && x.BrDok == dok.BrDokIn
					);

					if (
						sredjeniDokumentiZaDan[izvorniDokument.MagacinID]
							.FirstOrDefault(x =>
								x.Item1 == izvorniDokument.VrDok && x.Item2 == izvorniDokument.BrDok
							) == null
					)
					{
						log.AppendLine(
							$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Izvorni dokument nije prosao sredjivanje cena te moram srediti prvo magacin taj..."
						);
						SrediNabavneCeneUDokumentimaZaMagacinNaDanZaRobaID(
							dokumentiNaDan,
							dokumenti,
							ref stavke,
							ref nabavnaCena,
							robaid,
							izvorniDokument.MagacinID,
							ref sredjeniDokumentiZaDan,
							con
						);
					}

					stavka.NabavnaCena = stavke[izvorniDokument.VrDok]
						[izvorniDokument.BrDok]
						.First(x => x.RobaID == stavka.RobaID)
						.NabavnaCena;
					nabavnaCena[dok.MagacinID][stavka.RobaID] = stavka.NabavnaCena;

					log.AppendLine(
						$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Azuriram cenu u bazi..."
					);
					stavka.Update(con);
					log.AppendLine(
						$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Cena u bazi azurirana!"
					);
				}
				else
				{
					if (!nabavnaCena[dok.MagacinID].ContainsKey(stavka.RobaID))
					{
						stavka.NabavnaCena = 0;
						log.AppendLine(
							$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Roba nema prethodnu cenu te azuriram cenu u dokumentu na 0"
						);
					}
					else
					{
						stavka.NabavnaCena = nabavnaCena[dok.MagacinID][stavka.RobaID];
						log.AppendLine(
							$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Dokument nasledjuje poslednju cenu ({nabavnaCena[dok.MagacinID][stavka.RobaID]})"
						);
					}

					log.AppendLine(
						$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Azuriram cenu u bazi..."
					);
					stavka.Update(con);
					log.AppendLine(
						$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Cena u bazi azurirana!"
					);
				}

				log.AppendLine(
					$"[ {(provera ? "Provera" : "Izvrsenje")} ] [VrDok: {dok.VrDok}, BrDok: {dok.BrDok}] Belezim da je dokument uspesno prosao"
				);
				sredjeniDokumentiZaDan[dok.MagacinID]
					.Add(new Tuple<int, int>(dok.VrDok, dok.BrDok));
			}
			cDepth--;
		}

		private void unesiPartneraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_UnesiPartnera_Index up = new fm_UnesiPartnera_Index())
				if (!up.IsDisposed)
					up.ShowDialog();
		}

		private void analizaPorudzbinaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_Web_AnalizaPorudzbina_Index ap = new fm_Web_AnalizaPorudzbina_Index())
				if (!ap.IsDisposed)
					ap.ShowDialog();
		}

		private void novaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_Roba_Nova n = new fm_Roba_Nova())
				if (!n.IsDisposed)
					n.ShowDialog();
		}

		private void obracunPorezaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_ObracunPoreza_Index op = new fm_ObracunPoreza_Index())
				op.ShowDialog();
		}

		private void obracunPorezaToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			using (fm_ObracunPoreza_Index op = new fm_ObracunPoreza_Index())
				op.ShowDialog();
		}

		private void exportBeleskiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Program.TrenutniKorisnik.ID != 1)
			{
				MessageBox.Show("Samo ID 1!");
				return;
			}

			List<User> users = User.List();

			System.IO.File.WriteAllText(
				System.IO.Path.Combine(
					Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
					"TDOffice_user_data.json"
				),
				JsonConvert.SerializeObject(users)
			);

			MessageBox.Show("User data saved to desktop!");
		}

		private void stanjeRacunaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			using (fm_StanjeRacuna_Index sr = new fm_StanjeRacuna_Index())
				if (!sr.IsDisposed)
					sr.ShowDialog();
		}

		private void obracunIUplataPazaraToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(187000))
			{
				TDOffice.Pravo.NematePravoObavestenje(187000);
				return;
			}
			if (Application.OpenForms.OfType<fm_ObracunIUplataPazara>().FirstOrDefault() != null)
				return;
			Task.Run(() =>
			{
				using (fm_ObracunIUplataPazara foup = new fm_ObracunIUplataPazara())
					foup.ShowDialog();
			});
		}

		private void razduzenjeMagacinaSopstvenePotrosnjeToolStripMenuItem_Click(
			object sender,
			EventArgs e
		)
		{
			Task.Run(() =>
			{
				using (
					fm_RazduzenjeMagacinaSopstvenePotrosnje_Index rm =
						new fm_RazduzenjeMagacinaSopstvenePotrosnje_Index()
				)
					if (!rm.IsDisposed)
						rm.ShowDialog();
			});
		}

		private void tabelarniPrgledIzvodaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (Application.OpenForms.OfType<fm_TabelarniPregledIzvoda>().FirstOrDefault() != null)
				return;
			Task.Run(() =>
			{
				using (fm_TabelarniPregledIzvoda tpi = new fm_TabelarniPregledIzvoda())
					tpi.ShowDialog();
			});
		}

		public class PingGetDto
		{
			public string Value { get; set; }
		}

		public class PingGetRequest
		{
			public string SomeFilter1 { get; set; }
		}

		public class fsanfoasfnasas
		{
			public string? Vrednost1 { get; set; }
			public int? Korisnik { get; set; }
			public DateTime? Datum { get; set; }
		}

		public class fsafsasDto
		{
			public string? Vrednost1 { get; set; }
			public int? Korisnik { get; set; }
			public DateTime? Datum { get; set; }
		}

		private void pingGetToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				var resp = TDAPI
					.GetAsync<fsanfoasfnasas, fsafsasDto>(
						"/ping1",
						new fsanfoasfnasas() { Vrednost1 = "asd", Datum = DateTime.Now }
					)
					.GetAwaiter()
					.GetResult();
				var rawResponse = TDAPI.GetRawAsync("/ping/raw").GetAwaiter().GetResult();
				var rawResponseWithPayload = TDAPI
					.GetRawAsync<string>("/ping/raw")
					.GetAwaiter()
					.GetResult();
				var response = TDAPI.GetAsync<PingGetDto>("/ping").GetAwaiter().GetResult();
				var responseWithParameters = TDAPI
					.GetAsync<PingGetRequest, PingGetDto>(
						"/ping",
						new PingGetRequest() { SomeFilter1 = "Hello" }
					)
					.GetAwaiter()
					.GetResult();
			});
		}

		private void pingPutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				var response = TDAPI.PutRawAsync("/ping").GetAwaiter().GetResult();
			});
		}

		private void uredjivanjeProizvodaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				using (fm_WebUredjivanjeProizvoda up = new fm_WebUredjivanjeProizvoda())
					if (!up.IsDisposed)
						up.ShowDialog();
			});
		}

		private void nabavkaRobeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				using (fm_mc_NabavkaRobe_Index nr = new fm_mc_NabavkaRobe_Index())
					if (!nr.IsDisposed)
						nr.ShowDialog();
			});
		}

		private void nabavkaRobeUporediCenovnikeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				using (
					fm_mc_NabavkaRobe_UporediCenovnike_Index uc =
						new fm_mc_NabavkaRobe_UporediCenovnike_Index()
				)
					if (!uc.IsDisposed)
						uc.ShowDialog();
			});
		}
	}
}
