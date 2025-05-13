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
using TDOffice_v2.TDOffice;

namespace TDOffice_v2
{
	/// <summary>
	/// Radio sasa, ne moze se unaprediti brzina
	/// </summary>
	public partial class fm_SmsMasovni_Index : Form
	{
		private static char[] _allowedSMSCharacters = new char[74]
		{
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			'0',
			'!',
			'?',
			'.',
			',',
			'@',
			'-',
			'+',
			'*',
			'/',
			'%',
			' ',
			'='
		};

		private bool _np { get; set; } = true;
		private bool _naPauzi
		{
			get { return _np; }
			set
			{
				_np = value;

				if (!_np)
				{
					_slanjeSMSova = Task.Run(() =>
					{
						List<TDOffice.SMS> smss = TDOffice.SMS.List();

						foreach (
							TDOffice.SMS s in smss.Where(x =>
								x.Status == TDOffice.SMSStatus.UReduZaSlanje
							)
						)
						{
							Debug.Log("L1: Send " + DateTime.Now.ToString("[ dd.MM.yyyy HH:mm]"));
							if (SMSGateway.PosaljiSms(s.Broj, s.Text))
								s.Status = TDOffice.SMSStatus.Poslata;
							else
								s.Status = TDOffice.SMSStatus.GreskaPriSlanju;

							Debug.Log("L2: Update " + DateTime.Now.ToString("[ dd.MM.yyyy HH:mm]"));
							s.Update();

							this.Invoke(
								(MethodInvoker)
									delegate
									{
										OsveziDGV();
									}
							);
							if (_np)
								break;
						}
						smss = TDOffice.SMS.List();
						if (
							smss.Count(x =>
								x.Status == TDOffice.SMSStatus.UReduZaSlanje
								|| x.Status == TDOffice.SMSStatus.Pauzirana
							) == 0
						)
						{
							this.Invoke(
								(MethodInvoker)
									delegate
									{
										akcijaSMSPoruke_btn.Text = "Sve poruke su poslate!";
									}
							);
						}
					});
				}
			}
		}

		private Task _slanjeSMSova { get; set; }

		public fm_SmsMasovni_Index()
		{
			if (!Program.TrenutniKorisnik.ImaPravo(136560))
			{
				TDOffice.Pravo.NematePravoObavestenje(136560);
				this.Close();
				return;
			}
			InitializeComponent();
		}

		private void fm_SmsMasovni_Index_Load(object sender, EventArgs e)
		{
			OsveziDGV();
			if (!TrenutnoSalje())
			{
				akcijaSMSPoruke_btn.Text = "Posalji SMS poruke";
			}
			else
			{
				akcijaSMSPoruke_btn.Text = "Pauziraj slanje SMS poruka";
			}
		}

		private void OsveziDGV()
		{
			this.Invoke(
				(MethodInvoker)
					delegate
					{
						DataTable dt = new DataTable();
						dt.Columns.Add("ID", typeof(int));
						dt.Columns.Add("Mobilni", typeof(string));
						dt.Columns.Add("Text", typeof(string));
						dt.Columns.Add("Status", typeof(string));

						List<TDOffice.SMS> smss = TDOffice.SMS.List();
						foreach (TDOffice.SMS s in smss)
						{
							DataRow dr = dt.NewRow();
							dr["ID"] = s.ID;
							dr["Mobilni"] = s.Broj;
							dr["Text"] = s.Text;
							dr["Status"] = s.Status.ToString();
							dt.Rows.Add(dr);
						}

						dataGridView1.DataSource = dt;

						slogova_lbl.Text = "Slogova: " + smss.Count.ToString("#,##0");
					}
			);
		}

		private bool TrenutnoSalje()
		{
			List<TDOffice.SMS> smss = TDOffice.SMS.List();
			if (smss.Count(x => x.Status == TDOffice.SMSStatus.UReduZaSlanje) > 0)
			{
				return true;
			}
			return false;
		}

		private void uvuciKontakteIzKomercijalnog_btn_Click(object sender, EventArgs e)
		{
			foreach (Button btn in this.Controls.OfType<Button>())
			{
				btn.Enabled = false;
			}
			Task.Run(async () =>
			{
				int i = 0;
				List<Komercijalno.Partner> list = await Komercijalno.Partner.ListAsync();

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							statusLabel_lbl.Text = "0/" + list.Count;
						}
				);
				using (FbConnection con = new FbConnection(TDOffice.TDOffice.connectionString))
				{
					con.Open();
					foreach (Komercijalno.Partner p in list)
					{
						if (!string.IsNullOrWhiteSpace(p.Mobilni))
						{
							try
							{
								foreach (
									MobileNumber mn in MobileNumber.SplitAndGenerateMultiplyNumber(
										p.Mobilni
									)
								)
								{
									try
									{
										TDOffice.SMS.Insert(
											con,
											string.IsNullOrWhiteSpace(mn.Value)
											|| mn.Value.Length > 16
												? "neispravan"
												: mn.Value,
											"Bez Teksta"
										);
									}
									catch { }
								}
							}
							catch { }
						}
						i++;

						this.Invoke(
							(MethodInvoker)
								delegate
								{
									statusLabel_lbl.Text = i + "/" + list.Count;
								}
						);
					}
				}
				OsveziDGV();
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							foreach (Button btn in this.Controls.OfType<Button>())
							{
								btn.Enabled = true;
							}
							MessageBox.Show("Kontakti iz komercijalnog uvuceni!");
						}
				);
			});
		}

		private void uvuciKontakteSaSajta_btn_Click(object sender, EventArgs e)
		{
			foreach (Button btn in this.Controls.OfType<Button>())
			{
				btn.Enabled = false;
			}
			Task.Run(() =>
			{
				int i = 0;
				List<API.Korisnik> list = API.Korisnik.List();

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							statusLabel_lbl.Text = "0/" + list.Count;
						}
				);
				using (FbConnection con = new FbConnection(TDOffice.TDOffice.connectionString))
				{
					con.Open();
					foreach (API.Korisnik p in list)
					{
						if (!string.IsNullOrWhiteSpace(p.Mobilni))
						{
							try
							{
								foreach (
									MobileNumber mn in MobileNumber.SplitAndGenerateMultiplyNumber(
										p.Mobilni
									)
								)
								{
									try
									{
										TDOffice.SMS.Insert(
											con,
											string.IsNullOrWhiteSpace(mn.Value)
											|| mn.Value.Length > 16
												? "neispravan"
												: mn.Value,
											"Bez Teksta"
										);
									}
									catch { }
								}
							}
							catch { }
						}

						i++;

						this.Invoke(
							(MethodInvoker)
								delegate
								{
									statusLabel_lbl.Text = i + "/" + list.Count;
								}
						);
					}
				}
				OsveziDGV();
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							foreach (Button btn in this.Controls.OfType<Button>())
							{
								btn.Enabled = true;
							}
							MessageBox.Show("Uvuceni kontakti sa sajta!");
						}
				);
			});
		}

		private void uvuciKontakteIzTDOfficea_btn_Click(object sender, EventArgs e)
		{
			foreach (Button btn in this.Controls.OfType<Button>())
			{
				btn.Enabled = false;
			}
			Task.Run(() =>
			{
				int i = 0;
				List<TDOffice.Partner> list = TDOffice.Partner.List();

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							statusLabel_lbl.Text = "0/" + list.Count;
						}
				);
				using (FbConnection con = new FbConnection(TDOffice.TDOffice.connectionString))
				{
					con.Open();
					foreach (TDOffice.Partner p in list)
					{
						if (!string.IsNullOrWhiteSpace(p.Mobilni))
						{
							try
							{
								foreach (
									MobileNumber mn in MobileNumber.SplitAndGenerateMultiplyNumber(
										p.Mobilni
									)
								)
								{
									try
									{
										TDOffice.SMS.Insert(
											con,
											string.IsNullOrWhiteSpace(mn.Value)
											|| mn.Value.Length > 16
												? "neispravan"
												: mn.Value,
											"Bez Teksta"
										);
									}
									catch { }
								}
							}
							catch { }
						}

						i++;

						this.Invoke(
							(MethodInvoker)
								delegate
								{
									statusLabel_lbl.Text = i + "/" + list.Count;
								}
						);
					}
				}
				OsveziDGV();
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							foreach (Button btn in this.Controls.OfType<Button>())
							{
								btn.Enabled = true;
							}
							MessageBox.Show("Uvuceni kontakti iz TDOffice-a");
						}
				);
			});
		}

		private void ukloniSveIzSeta_btn_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show(
					"Da li sigurno zelite ukloniti ovaj slot?",
					"Potvrdite",
					MessageBoxButtons.YesNo
				) != DialogResult.Yes
			)
				return;

			TDOffice.SMS.Clear();

			OsveziDGV();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			foreach (Button btn in this.Controls.OfType<Button>())
			{
				btn.Enabled = false;
			}
			Task.Run(() =>
			{
				int i = 0;
				List<TDOffice.SMS> smss = TDOffice.SMS.List();

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							statusLabel_lbl.Text = "0/" + smss.Count;
						}
				);
				using (FbConnection con = new FbConnection(TDOffice.TDOffice.connectionString))
				{
					con.Open();
					foreach (TDOffice.SMS s in smss)
					{
						s.Text = tekstPoruke_txt.Text;
						s.Update();
						i++;

						this.Invoke(
							(MethodInvoker)
								delegate
								{
									statusLabel_lbl.Text = i + "/" + smss.Count;
								}
						);
					}
				}
				OsveziDGV();
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							foreach (Button btn in this.Controls.OfType<Button>())
							{
								btn.Enabled = true;
							}
							MessageBox.Show("Tekst poruke uspesno podesen!");
						}
				);
			});
		}

		private void akcijaSMSPoruke_btn_Click(object sender, EventArgs e)
		{
			if (TrenutnoSalje())
			{
				akcijaSMSPoruke_btn.Text = "Posalji SMS poruke";
				List<TDOffice.SMS> smss = TDOffice.SMS.List();

				foreach (
					TDOffice.SMS s in smss.Where(x => x.Status == TDOffice.SMSStatus.UReduZaSlanje)
				)
				{
					s.Status = TDOffice.SMSStatus.Pauzirana;
					s.Update();
				}
				_naPauzi = true;
			}
			else
			{
				List<TDOffice.SMS> smss = TDOffice.SMS.List();

				if (
					smss.Count(x =>
						x.Status == TDOffice.SMSStatus.Pauzirana
						|| x.Status == TDOffice.SMSStatus.Priprema
					) > 0
				)
				{
					akcijaSMSPoruke_btn.Text = "Pauziraj slanje SMS poruke";
					foreach (
						TDOffice.SMS s in smss.Where(x =>
							x.Status == TDOffice.SMSStatus.Pauzirana
							|| x.Status == TDOffice.SMSStatus.Priprema
						)
					)
					{
						s.Status = TDOffice.SMSStatus.UReduZaSlanje;
						s.Update();
					}
					_naPauzi = false;
				}
				else
				{
					MessageBox.Show("Sve poruke su poslate!");
				}
			}
			OsveziDGV();
		}

		private void ukloniKontakteKojiPostojeNaSajtu_btn_Click(object sender, EventArgs e)
		{
			foreach (Button btn in this.Controls.OfType<Button>())
			{
				btn.Enabled = false;
			}

			Task.Run(() =>
			{
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							int i = 0;
							List<API.Korisnik> korisnici = API.Korisnik.List();
							List<TDOffice.SMS> smss = TDOffice.SMS.List();
							this.Invoke(
								(MethodInvoker)
									delegate
									{
										statusLabel_lbl.Text = "0/" + smss.Count;
									}
							);
							foreach (TDOffice.SMS s in smss)
							{
								if (korisnici.Any(x => MobileNumber.Collate(x.Mobilni) == s.Broj))
								{
									TDOffice.SMS.Delete(s.ID);
								}
								i++;
								this.Invoke(
									(MethodInvoker)
										delegate
										{
											statusLabel_lbl.Text = "0/" + smss.Count;
										}
								);
							}
							OsveziDGV();
							foreach (Button btn in this.Controls.OfType<Button>())
							{
								btn.Enabled = true;
							}
							MessageBox.Show("Kontakti koji postoje na sajtu su uklonjeni!");
						}
				);
			});
		}

		private void ukloniDuplikate_btn_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							statusLabel_lbl.Text = "Zapocinjem uklanjanje duplikata...";
						}
				);
				int i = 0;
				List<TDOffice.SMS> newList = new List<TDOffice.SMS>();
				List<TDOffice.SMS> smss = TDOffice.SMS.List();
				foreach (TDOffice.SMS s in smss)
				{
					if (!newList.Any(x => x.Broj == s.Broj))
						newList.Add(s);

					i++;

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								statusLabel_lbl.Text = $"Cistim {i} / {smss.Count}";
							}
					);
				}

				TDOffice.SMS.Clear();

				i = 0;
				foreach (TDOffice.SMS s in newList)
				{
					TDOffice.SMS.Insert(s.Broj, s.Text);
					i++;
					this.Invoke(
						(MethodInvoker)
							delegate
							{
								statusLabel_lbl.Text = $"Insertujem {i} / {smss.Count}";
							}
					);
				}

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							statusLabel_lbl.Text = "Gotovo uklanjanje duplikata!";
						}
				);
				OsveziDGV();
				MessageBox.Show("Gotovo uklanjanje duplikata!");
			});
		}

		private void ukloniNeispravne_btn_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							statusLabel_lbl.Text = "Zapocinjem uklanjanje neispravnih sms-ova";
						}
				);

				int i = 0;

				List<TDOffice.SMS> newList = new List<TDOffice.SMS>();
				List<TDOffice.SMS> smss = TDOffice.SMS.List();
				foreach (TDOffice.SMS s in smss)
				{
					if (MobileNumber.IsValid(s.Broj))
						newList.Add(s);

					i++;

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								statusLabel_lbl.Text = $"Cistim {i} / {smss.Count}";
							}
					);
				}

				TDOffice.SMS.Clear();

				i = 0;
				foreach (TDOffice.SMS s in newList)
				{
					TDOffice.SMS.Insert(s.Broj, s.Text);
					i++;
					this.Invoke(
						(MethodInvoker)
							delegate
							{
								statusLabel_lbl.Text = $"Insertujem {i} / {smss.Count}";
							}
					);
				}

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							statusLabel_lbl.Text = "Gotovo uklanjanje neispravnih sms-ova!";
						}
				);
				OsveziDGV();
				MessageBox.Show("Gotovo uklanjanje neispravnih sms-ova!");
			});
		}

		private void ukloniBlokirane_btn_Click(object sender, EventArgs e)
		{
			Task.Run(() =>
			{
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							statusLabel_lbl.Text = "Zapocinjem uklanjanje blokiranih...";
						}
				);

				int i = 0;
				List<TDOffice.Partner> blokirani = TDOffice
					.Partner.List()
					.Where(x => x.SMSBlok)
					.ToList();
				List<TDOffice.SMS> smss = TDOffice.SMS.List();
				foreach (TDOffice.Partner p in blokirani)
				{
					try
					{
						foreach (
							MobileNumber mn in MobileNumber.SplitAndGenerateMultiplyNumber(
								p.Mobilni
							)
						)
						{
							try
							{
								TDOffice.SMS s = smss.FirstOrDefault(x => x.Broj == p.Mobilni);
								if (s != null)
									TDOffice.SMS.Delete(s.ID);
							}
							catch { }
						}
					}
					catch { }
					i++;
					this.Invoke(
						(MethodInvoker)
							delegate
							{
								statusLabel_lbl.Text = $"{i} / {blokirani.Count}";
							}
					);
				}
				;
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							statusLabel_lbl.Text = $"Gotovo!";
						}
				);
				MessageBox.Show("Zavrsen proces uklanjanja blokiranih!");
			});
		}

		private void tekstPoruke_txt_KeyUp(object sender, KeyEventArgs e)
		{
			karaktera_lbl.Text = string.IsNullOrWhiteSpace(tekstPoruke_txt.Text)
				? "0"
				: tekstPoruke_txt.Text.Length.ToString();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			try
			{
				SMS.UpdateAllStatus(SMSStatus.Priprema);
				OsveziDGV();
				MessageBox.Show("Gotovo!");
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}
	}
}
