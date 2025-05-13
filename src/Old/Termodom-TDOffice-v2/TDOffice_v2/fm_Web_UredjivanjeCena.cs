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

namespace TDOffice_v2
{
	public partial class fm_Web_UredjivanjeCena : Form
	{
		Task<List<Komercijalno.Roba>> komercijalnoRoba;
		Task<List<Komercijalno.RobaUMagacinu>> komercijalnoRobaUMagacinu50 { get; set; }
		Task<List<TDWeb.Proizvod>> webProizvodi;
		Task<List<Komercijalno.Magacin>> komercijalnoMagacini;
		private Task<fm_Help> _helpFrom { get; set; }
		Task<List<Komercijalno.Stavka>> komercijalnoStavkeNabavke50 = Task.Run(() =>
		{
			List<Komercijalno.Stavka> stavkeNabavke = new List<Komercijalno.Stavka>();

			var stavkeMagacina = Komercijalno.Stavka.ListByMagacinIDAsync(150);
			var dokumentiNabavke50 = Komercijalno
				.Dokument.ListByMagacinID(DateTime.Now.Year, 150)
				.Where(x => new int[] { 0, 1, 2, 3, 36 }.Contains(x.VrDok))
				.OrderByDescending(x => x.DatRoka)
				.ToList();

			var stavkeNabavkeMagacina = stavkeMagacina
				.Result.Where(x => new int[] { 0, 1, 2, 3, 36 }.Contains(x.VrDok))
				.ToList();

			foreach (var dn in dokumentiNabavke50)
			foreach (
				var s in stavkeNabavkeMagacina.Where(x =>
					x.VrDok == dn.VrDok && x.BrDok == dn.BrDok
				)
			)
				stavkeNabavke.Add(s);

			return stavkeNabavke;
		});

		// kartica robe > period vazenja

		DataTable _baseData = null;
		DataTable _dgvData = null;

		fm_Web_UredjivanjeCena_Item witem = new fm_Web_UredjivanjeCena_Item()
		{
			TopMost = true,
			TopLevel = true
		};

		private bool _suspendValidation = true;

		public fm_Web_UredjivanjeCena()
		{
			InitializeComponent();
			_helpFrom = this.InitializeHelpModulAsync(Modul.Web_UredjivanjeCena);
			periodAnalize_gb.DesniKlik_DatumRange(null);
			komercijalnoRoba = Komercijalno.Roba.ListAsync(DateTime.Now.Year);
			komercijalnoRobaUMagacinu50 = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync(150);
			webProizvodi = TDWeb.Proizvod.ListAsync();
			komercijalnoMagacini = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);

			witem.ItemUpdated += OnItemWindowItemUpdate;
		}

		private void fm_Web_UredjivanjeCena_Load(object sender, EventArgs e)
		{
			doDatuma_dtp.Value = new DateTime(DateTime.Now.Year, 12, 31);
			UcitajStavke();
		}

		private void fm_Web_UredjivanjeCena_FormClosing(object sender, FormClosingEventArgs e)
		{
			witem.Hide();
			witem.Dispose();
		}

		private void UcitajStavke()
		{
			_suspendValidation = true;
			dataGridView1.Columns.Clear();
			List<TDOffice.WebUredjivanjeCenaStavka> stavkeUTDOfficeu =
				TDOffice.WebUredjivanjeCenaStavka.List();

			DataTable dt = new DataTable();
			dt.Columns.Add("RobaID", typeof(int));
			dt.Columns.Add("KatBr", typeof(string));
			dt.Columns.Add("Naziv", typeof(string));

			dt.Columns.Add("CenaIzMagacina", typeof(int));

			dt.Columns.Add("KomercijalnoNabavnaCena", typeof(double));
			dt.Columns.Add("KomercijalnoProdajnaCena", typeof(double));

			dt.Columns.Add("WebNabavnaCenaUslov", typeof(int));
			dt.Columns.Add("Modifikator", typeof(double));

			dt.Columns.Add("TrenutnaWebNabavnaCena", typeof(double));
			dt.Columns.Add("PlaniranaWebNabavnaCena", typeof(double));

			dt.Columns.Add("TrenutnaWebProdajnaCena", typeof(double));

			dt.Columns.Add("RabatPlatinum", typeof(double));

			dt.Columns.Add("PlaniraniRabat", typeof(double));

			dt.Columns.Add("ReferentniProizvod", typeof(int));

			dt.Columns.Add("Aktivan", typeof(int));

			Dictionary<int, string> errorOutput = new Dictionary<int, string>();
			foreach (var s in stavkeUTDOfficeu.Where(x => x.RobaID > 0))
			{
				var kRoba = komercijalnoRoba.Result.Where(x => x.ID == s.RobaID).FirstOrDefault();
				var poslednjaNabavka = komercijalnoStavkeNabavke50
					.Result.Where(x => x.RobaID == s.RobaID)
					.FirstOrDefault();
				var kRUM = komercijalnoRobaUMagacinu50
					.Result.Where(x => x.RobaID == s.RobaID)
					.FirstOrDefault();
				var wProizvod = webProizvodi
					.Result.Where(x => x.RobaID == s.RobaID)
					.FirstOrDefault();

				if (kRoba == null)
				{
					errorOutput.Add(
						s.RobaID,
						"TDOffice_Web_Stavka koja ima dodeljeni RobaID: "
							+ s.RobaID
							+ " nije pronadjena u komercijalnom sifarniku robe."
					);
					continue;
				}
				if (kRUM == null)
				{
					errorOutput.Add(
						s.RobaID,
						"TDOffice_Web_Stavka koja ima dodeljeni RobaID: "
							+ s.RobaID
							+ " nije pronadjena u komercijalnom Robi U Magacinu 50."
					);
					continue;
				}
				if (poslednjaNabavka == null)
				{
					errorOutput.Add(
						s.RobaID,
						"TDOffice_Web_Stavka koja ima dodeljeni RobaID: "
							+ s.RobaID
							+ " nije pronadjena poslednja nabavka."
					);
					continue;
				}
				if (wProizvod == null)
				{
					errorOutput.Add(
						s.RobaID,
						"TDOffice_Web_Stavka koja ima dodeljeni RobaID: "
							+ s.RobaID
							+ " nije pronadjena kao proizvod na sajtu."
					);
					continue;
				}
				DataRow dr = dt.NewRow();

				dr["RobaID"] = s.RobaID;
				dr["KatBr"] = kRoba == null ? "Unknown" : kRoba.KatBr;
				dr["Naziv"] = kRoba == null ? "Unknown" : kRoba.Naziv;

				dr["CenaIzMagacina"] = s.MagacinID;

				dr["KomercijalnoNabavnaCena"] =
					poslednjaNabavka == null ? 0 : poslednjaNabavka.NabavnaCena;
				dr["KomercijalnoProdajnaCena"] = kRUM != null ? kRUM.ProdajnaCena : -1;

				dr["WebNabavnaCenaUslov"] = (int)s.Uslov;
				dr["Modifikator"] = s.Modifikator;

				dr["TrenutnaWebNabavnaCena"] = wProizvod == null ? -1 : wProizvod.NabavnaCena;

				double planiranaWebNabavnaCena = 0;
				switch (s.Uslov)
				{
					case TDOffice.Enums.WebUredjivanjeCenaUslov.FiksnaCena:
						planiranaWebNabavnaCena = Convert.ToDouble(s.Modifikator);
						break;

					case TDOffice.Enums.WebUredjivanjeCenaUslov.PoslednjaNabavnaCena:
						planiranaWebNabavnaCena =
							poslednjaNabavka == null
								? 0
								: poslednjaNabavka.NabavnaCena
									+ (poslednjaNabavka.NabavnaCena * (s.Modifikator / 100));
						break;

					case TDOffice.Enums.WebUredjivanjeCenaUslov.ProdajnaCena:
						planiranaWebNabavnaCena =
							kRUM.ProdajnaCena + (kRUM.ProdajnaCena * (s.Modifikator / 100));
						break;

					case TDOffice.Enums.WebUredjivanjeCenaUslov.ReferentniProizvod:
						if (s.ReferentniProizvod == null)
						{
							planiranaWebNabavnaCena = 0;
							break;
						}
						var poslednjaNabavkaReferentnogProizvoda = komercijalnoStavkeNabavke50
							.Result.Where(x => x.RobaID == (int)s.ReferentniProizvod)
							.FirstOrDefault();
						planiranaWebNabavnaCena =
							poslednjaNabavkaReferentnogProizvoda.NabavnaCena
							+ (
								poslednjaNabavkaReferentnogProizvoda.NabavnaCena
								* (s.Modifikator / 100)
							);
						break;

					default:
						break;
				}

				dr["PlaniranaWebNabavnaCena"] = planiranaWebNabavnaCena;

				double trenutnaWebProdajnaCena = wProizvod == null ? -1 : wProizvod.ProdajnaCena;
				dr["TrenutnaWebProdajnaCena"] = trenutnaWebProdajnaCena;

				double webProdajnaCena = Convert.ToDouble(dr["TrenutnaWebProdajnaCena"]);
				double webNabavnaCena = Convert.ToDouble(dr["TrenutnaWebNabavnaCena"]);

				double rabatPlatinum =
					kRUM == null
						? -1
						: (
							(
								(webProdajnaCena - ((webProdajnaCena - webNabavnaCena) * 0.75))
								/ kRUM.ProdajnaCena
							) - 1
						) * -100;
				dr["RabatPlatinum"] = rabatPlatinum;

				double planiraniPlatinumRabat =
					kRUM == null
						? -1
						: (
							(
								(webProdajnaCena - ((webProdajnaCena - webNabavnaCena) * 0.75))
								/ kRUM.ProdajnaCena
							) - 1
						) * -100;
				if (double.IsNaN(planiraniPlatinumRabat))
					planiraniPlatinumRabat = 0;
				dr["PlaniraniRabat"] = planiraniPlatinumRabat;

				dr["ReferentniProizvod"] =
					s.ReferentniProizvod == null ? -1 : (int)s.ReferentniProizvod;

				dr["Aktivan"] = wProizvod.Aktivan;

				dt.Rows.Add(dr);
			}

			if (errorOutput.Count > 0)
				MessageBox.Show(string.Join(Environment.NewLine, errorOutput.Select(x => x.Value)));

			dataGridView1.AutoGenerateColumns = false;

			DataGridViewComboBoxColumn magaciniCmbCol = new DataGridViewComboBoxColumn()
			{
				Name = "CenaIzMagacina",
				HeaderText = "Cena Iz Magacina",
				DataPropertyName = "CenaIzMagacina",
				Width = 200,
				HeaderCell = new DataGridViewColumnHeaderCell() { Value = "Cena Iz Magacina" },
				DataSource = komercijalnoMagacini.Result,
				DisplayMember = "Naziv",
				ValueMember = "ID"
			};
			DataGridViewComboBoxColumn usloviCmbCol = new DataGridViewComboBoxColumn()
			{
				Name = "WebNabavnaCenaUslov",
				HeaderText = "Uslov Kreiranja Web Nabavne Cene",
				DataPropertyName = "WebNabavnaCenaUslov",
				Width = 200,
				HeaderCell = new DataGridViewColumnHeaderCell()
				{
					Style = new DataGridViewCellStyle()
					{
						Alignment = DataGridViewContentAlignment.MiddleCenter
					},
					Value = "Uslov Kreiranja Web Nabavne Cene"
				},
				DefaultCellStyle = new DataGridViewCellStyle()
				{
					Alignment = DataGridViewContentAlignment.MiddleRight,
				},
				DataSource = new List<Tuple<int, string>>()
				{
					new Tuple<int, string>(0, "Nedefinisano"),
					new Tuple<int, string>(1, "Poslednja Nabavna Cena +/- %"),
					new Tuple<int, string>(2, "Fiksna Cena"),
					new Tuple<int, string>(3, "Komercijalno Prodajna Cena +/- %"),
					new Tuple<int, string>(4, "Referentni Proizvod")
				},
				DisplayMember = "Item2",
				ValueMember = "Item1"
			};

			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "RobaID",
					HeaderText = "RobaID",
					DataPropertyName = "RobaID",
					Width = 50,
					ReadOnly = true,
					HeaderCell = new DataGridViewColumnHeaderCell() { Value = "RobaID" }
				}
			);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "KatBr",
					HeaderText = "KatBr",
					DataPropertyName = "KatBr",
					Width = 100,
					ReadOnly = true,
					HeaderCell = new DataGridViewColumnHeaderCell() { Value = "KatBr" }
				}
			);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "Naziv",
					HeaderText = "Naziv",
					DataPropertyName = "Naziv",
					Width = 300,
					ReadOnly = true,
					HeaderCell = new DataGridViewColumnHeaderCell() { Value = "Naziv" }
				}
			);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "KomercijalnoNabavnaCena",
					HeaderText = "Komercijalno Nabavna Cena",
					DataPropertyName = "KomercijalnoNabavnaCena",
					Width = 70,
					ReadOnly = true,
					HeaderCell = new DataGridViewColumnHeaderCell()
					{
						Style = new DataGridViewCellStyle()
						{
							Alignment = DataGridViewContentAlignment.MiddleCenter
						},
						Value = "Komercijalno Nabavna Cena"
					},
					DefaultCellStyle = new DataGridViewCellStyle()
					{
						Alignment = DataGridViewContentAlignment.MiddleRight,
						BackColor = Color.FromArgb(0, 153, 255),
						ForeColor = Color.Black
					}
				}
			);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "KomercijalnoProdajnaCena",
					HeaderText = "Komercijalno Prodajna Cena",
					DataPropertyName = "KomercijalnoProdajnaCena",
					Width = 70,
					ReadOnly = true,
					HeaderCell = new DataGridViewColumnHeaderCell()
					{
						Style = new DataGridViewCellStyle()
						{
							Alignment = DataGridViewContentAlignment.MiddleCenter
						},
						Value = "Komercijalno Prodajna Cena"
					},
					DefaultCellStyle = new DataGridViewCellStyle()
					{
						Alignment = DataGridViewContentAlignment.MiddleRight,
						BackColor = Color.FromArgb(0, 153, 255),
						ForeColor = Color.Black
					}
				}
			);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "TrenutnaWebNabavnaCena",
					HeaderText = "Trenutna Web Nabavna Cena",
					DataPropertyName = "TrenutnaWebNabavnaCena",
					Width = 70,
					ReadOnly = true,
					HeaderCell = new DataGridViewColumnHeaderCell()
					{
						Style = new DataGridViewCellStyle()
						{
							Alignment = DataGridViewContentAlignment.MiddleCenter
						},
						Value = "Trenutna Web Nabavna Cena"
					},
					DefaultCellStyle = new DataGridViewCellStyle()
					{
						Alignment = DataGridViewContentAlignment.MiddleRight,
						BackColor = Color.FromArgb(255, 91, 91),
						ForeColor = Color.White
					}
				}
			);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "RabatPlatinum",
					HeaderText = "Trenutni Rabat Platinum",
					DataPropertyName = "RabatPlatinum",
					Width = 70,
					ReadOnly = true,
					HeaderCell = new DataGridViewColumnHeaderCell()
					{
						Style = new DataGridViewCellStyle()
						{
							Alignment = DataGridViewContentAlignment.MiddleCenter
						},
						Value = "Trenutni Rabat Platinum"
					},
					DefaultCellStyle = new DataGridViewCellStyle()
					{
						Alignment = DataGridViewContentAlignment.MiddleRight,
						BackColor = Color.FromArgb(255, 91, 91),
						ForeColor = Color.White,
						Format = "0.00"
					}
				}
			);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "TrenutnaWebProdajnaCena",
					HeaderText = "Trenutna Web Prodajna Cena",
					DataPropertyName = "TrenutnaWebProdajnaCena",
					Width = 70,
					ReadOnly = true,
					HeaderCell = new DataGridViewColumnHeaderCell()
					{
						Style = new DataGridViewCellStyle()
						{
							Alignment = DataGridViewContentAlignment.MiddleCenter
						},
						Value = "Trenutna Web Prodajna Cena"
					},
					DefaultCellStyle = new DataGridViewCellStyle()
					{
						Alignment = DataGridViewContentAlignment.MiddleRight,
						BackColor = Color.FromArgb(255, 91, 91),
						ForeColor = Color.White
					}
				}
			);
			dataGridView1.Columns.Add(usloviCmbCol);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "Modifikator",
					HeaderText = "Modifikator",
					DataPropertyName = "Modifikator",
					Width = 70,
					ReadOnly = false,
					HeaderCell = new DataGridViewColumnHeaderCell()
					{
						Style = new DataGridViewCellStyle()
						{
							Alignment = DataGridViewContentAlignment.MiddleCenter
						},
						Value = "Modifikator (+/- %)"
					},
					DefaultCellStyle = new DataGridViewCellStyle()
					{
						Alignment = DataGridViewContentAlignment.MiddleRight,
						BackColor = Color.FromArgb(255, 153, 51),
						ForeColor = Color.Black
					}
				}
			);
			dataGridView1.Columns.Add(magaciniCmbCol);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "PlaniranaWebNabavnaCena",
					HeaderText = "Planirana Web Nabavna Cena",
					DataPropertyName = "PlaniranaWebNabavnaCena",
					Width = 70,
					ReadOnly = true,
					HeaderCell = new DataGridViewColumnHeaderCell()
					{
						Style = new DataGridViewCellStyle()
						{
							Alignment = DataGridViewContentAlignment.MiddleCenter
						},
						Value = "Planirana Web Nabavna Cena"
					},
					DefaultCellStyle = new DataGridViewCellStyle()
					{
						Alignment = DataGridViewContentAlignment.MiddleRight,
					}
				}
			);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "PlaniraniRabat",
					HeaderText = "Planirani Rabat",
					DataPropertyName = "PlaniraniRabat",
					Width = 70,
					ReadOnly = true,
					HeaderCell = new DataGridViewColumnHeaderCell()
					{
						Style = new DataGridViewCellStyle()
						{
							Alignment = DataGridViewContentAlignment.MiddleCenter
						},
						Value = "Planirani Platinum Rabat"
					},
					DefaultCellStyle = new DataGridViewCellStyle()
					{
						Alignment = DataGridViewContentAlignment.MiddleRight,
						Format = "0.00"
					}
				}
			);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "ReferentniProizvod",
					HeaderText = "Referentni Proizvod",
					DataPropertyName = "ReferentniProizvod",
					Visible = false,
					ReadOnly = true
				}
			);
			dataGridView1.Columns.Add(
				new DataGridViewTextBoxColumn()
				{
					Name = "Aktivan",
					DataPropertyName = "Aktivan",
					Visible = false,
					ReadOnly = true
				}
			);

			_baseData = dt;
			_dgvData = _baseData;

			dataGridView1.DataSource = _dgvData;

			ObojiPodateDgv();

			slogova_lbl.Text = "Slogova: " + dataGridView1.Rows.Count;

			_suspendValidation = false;
		}

		private void ObojiPodateDgv()
		{
			for (int i = 0; i < dataGridView1.Rows.Count; i++)
			{
				if (
					Convert.ToDouble(dataGridView1.Rows[i].Cells["TrenutnaWebNabavnaCena"].Value)
					!= Convert.ToDouble(
						dataGridView1.Rows[i].Cells["PlaniranaWebNabavnaCena"].Value
					)
				)
					dataGridView1.Rows[i].Cells["PlaniranaWebNabavnaCena"].Style.ForeColor =
						Color.Red;
				else
					dataGridView1.Rows[i].Cells["PlaniranaWebNabavnaCena"].Style.ForeColor =
						Color.Green;

				if (Convert.ToInt32(dataGridView1.Rows[i].Cells["Aktivan"].Value) != 1)
					dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Gray;
			}
		}

		private void OnItemWindowItemUpdate(
			object sender,
			TDOffice.WebUredjivanjeCenaStavka webStavka
		)
		{
			DataRow dr = (dataGridView1.DataSource as DataTable).Select(
				"RobaID = " + webStavka.RobaID
			)[0];

			var kRoba = komercijalnoRoba
				.Result.Where(x => x.ID == webStavka.RobaID)
				.FirstOrDefault();
			var poslednjaNabavka = komercijalnoStavkeNabavke50
				.Result.Where(x => x.RobaID == webStavka.RobaID)
				.FirstOrDefault();
			var kRUM = komercijalnoRobaUMagacinu50
				.Result.Where(x => x.RobaID == webStavka.RobaID)
				.FirstOrDefault();
			var wProizvod = webProizvodi
				.Result.Where(x => x.RobaID == webStavka.RobaID)
				.FirstOrDefault();

			dr["CenaIzMagacina"] = webStavka.MagacinID;

			dr["WebNabavnaCenaUslov"] = (int)webStavka.Uslov;
			dr["Modifikator"] = webStavka.Modifikator;

			dr["ReferentniProizvod"] =
				webStavka.ReferentniProizvod == null ? -1 : webStavka.ReferentniProizvod;

			double planiranaWebNabavnaCena = 0;
			switch (webStavka.Uslov)
			{
				case TDOffice.Enums.WebUredjivanjeCenaUslov.FiksnaCena:
					planiranaWebNabavnaCena = Convert.ToDouble(webStavka.Modifikator);
					break;

				case TDOffice.Enums.WebUredjivanjeCenaUslov.PoslednjaNabavnaCena:
					planiranaWebNabavnaCena =
						poslednjaNabavka == null
							? 0
							: poslednjaNabavka.NabavnaCena
								+ (poslednjaNabavka.NabavnaCena * (webStavka.Modifikator / 100));
					break;

				case TDOffice.Enums.WebUredjivanjeCenaUslov.ProdajnaCena:
					planiranaWebNabavnaCena =
						kRUM.ProdajnaCena + (kRUM.ProdajnaCena * (webStavka.Modifikator / 100));
					break;

				case TDOffice.Enums.WebUredjivanjeCenaUslov.ReferentniProizvod:
					if (webStavka.ReferentniProizvod == null)
					{
						planiranaWebNabavnaCena = 0;
						break;
					}
					var poslednjaNabavkaReferentnogProizvoda = komercijalnoStavkeNabavke50
						.Result.Where(x => x.RobaID == (int)webStavka.ReferentniProizvod)
						.FirstOrDefault();
					planiranaWebNabavnaCena =
						poslednjaNabavkaReferentnogProizvoda.NabavnaCena
						+ (
							poslednjaNabavkaReferentnogProizvoda.NabavnaCena
							* (webStavka.Modifikator / 100)
						);
					break;

				default:
					break;
			}

			dr["PlaniranaWebNabavnaCena"] = planiranaWebNabavnaCena;

			double trenutnaWebProdajnaCena = wProizvod == null ? -1 : wProizvod.ProdajnaCena;
			dr["TrenutnaWebProdajnaCena"] = trenutnaWebProdajnaCena;

			double webProdajnaCena = Convert.ToDouble(dr["TrenutnaWebProdajnaCena"]);
			double webNabavnaCena = Convert.ToDouble(dr["TrenutnaWebNabavnaCena"]);

			double rabatPlatinum =
				kRUM == null
					? -1
					: (
						(
							(webProdajnaCena - ((webProdajnaCena - webNabavnaCena) * 0.75))
							/ kRUM.ProdajnaCena
						) - 1
					) * -100;
			dr["RabatPlatinum"] = rabatPlatinum;

			double planiraniPlatinumRabat =
				kRUM == null
					? -1
					: (
						(
							(webProdajnaCena - ((webProdajnaCena - webNabavnaCena) * 0.75))
							/ kRUM.ProdajnaCena
						) - 1
					) * -100;
			dr["PlaniraniRabat"] = planiraniPlatinumRabat;

			ObojiPodateDgv();
		}

		private void uvuciProizvodeSaSajtaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			List<TDWeb.Proizvod> proizvodiSaSajta = TDWeb.Proizvod.List();
			List<TDOffice.WebUredjivanjeCenaStavka> stavkeUTDOfficeu =
				TDOffice.WebUredjivanjeCenaStavka.List();

			foreach (TDOffice.WebUredjivanjeCenaStavka s in stavkeUTDOfficeu)
				proizvodiSaSajta.RemoveAll(x => x.RobaID == s.RobaID);

			Parallel.ForEach(
				proizvodiSaSajta,
				p =>
				{
					TDOffice.WebUredjivanjeCenaStavka.Insert(
						p.RobaID,
						TDOffice.Enums.WebUredjivanjeCenaUslov.None,
						0,
						150
					);
				}
			);

			UcitajStavke();

			MessageBox.Show("Zavrseno sinhronizovanje proizvoda sa sajta!");
		}

		private void dataGridView1_CellValidating(
			object sender,
			DataGridViewCellValidatingEventArgs e
		) { }

		private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
		{
			if (_suspendValidation)
				return;

			try
			{
				int robaID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["RobaID"].Value);
				TDOffice.WebUredjivanjeCenaStavka stavka;

				switch (dataGridView1.Columns[e.ColumnIndex].Name)
				{
					case "Modifikator":
						stavka = TDOffice.WebUredjivanjeCenaStavka.Get(robaID);
						stavka.Modifikator = Convert.ToDouble(
							dataGridView1.Rows[e.RowIndex].Cells["Modifikator"].Value
						);
						stavka.Update();
						break;

					case "CenaIzMagacina":
						stavka = TDOffice.WebUredjivanjeCenaStavka.Get(robaID);
						stavka.MagacinID = Convert.ToInt32(
							dataGridView1.Rows[e.RowIndex].Cells["CenaIzMagacina"].Value
						);
						stavka.Update();
						break;

					case "WebNabavnaCenaUslov":
						stavka = TDOffice.WebUredjivanjeCenaStavka.Get(robaID);
						stavka.Uslov = (TDOffice.Enums.WebUredjivanjeCenaUslov)
							Convert.ToInt32(
								dataGridView1.Rows[e.RowIndex].Cells["WebNabavnaCenaUslov"].Value
							);
						stavka.Update();
						break;

					default:
						break;
				}

				DataGridViewRow dr = dataGridView1.Rows[e.RowIndex];

				var kRUM = komercijalnoRobaUMagacinu50
					.Result.Where(x => x.RobaID == robaID)
					.FirstOrDefault();
				var poslednjaNabavka = komercijalnoStavkeNabavke50
					.Result.Where(x => x.RobaID == robaID)
					.FirstOrDefault();

				double planiranaWebNabavnaCena = 0;
				TDOffice.Enums.WebUredjivanjeCenaUslov uslov =
					(TDOffice.Enums.WebUredjivanjeCenaUslov)dr.Cells["WebNabavnaCenaUslov"].Value;
				switch (uslov)
				{
					case TDOffice.Enums.WebUredjivanjeCenaUslov.FiksnaCena:
						planiranaWebNabavnaCena = Convert.ToDouble(
							dataGridView1.Rows[e.RowIndex].Cells["Modifikator"].Value
						);
						break;

					case TDOffice.Enums.WebUredjivanjeCenaUslov.PoslednjaNabavnaCena:
						planiranaWebNabavnaCena =
							poslednjaNabavka == null
								? 0
								: poslednjaNabavka.NabavnaCena
									+ (
										poslednjaNabavka.NabavnaCena
										* (
											Convert.ToDouble(
												dataGridView1
													.Rows[e.RowIndex]
													.Cells["Modifikator"]
													.Value
											) / 100
										)
									);
						break;

					case TDOffice.Enums.WebUredjivanjeCenaUslov.ProdajnaCena:
						planiranaWebNabavnaCena =
							kRUM.ProdajnaCena
							+ (
								kRUM.ProdajnaCena
								* (
									Convert.ToDouble(
										dataGridView1.Rows[e.RowIndex].Cells["Modifikator"].Value
									) / 100
								)
							);
						break;

					case TDOffice.Enums.WebUredjivanjeCenaUslov.ReferentniProizvod:
						if (
							Convert.ToInt32(
								dataGridView1.Rows[e.RowIndex].Cells["ReferentniProizvod"].Value
							) <= 0
						)
						{
							planiranaWebNabavnaCena = 0;
							break;
						}
						var referentiProizvodiRobaID = Convert.ToInt32(
							dataGridView1.Rows[e.RowIndex].Cells["ReferentniProizvod"].Value
						);
						var poslednjaNabavkaReferentnogProizvoda = komercijalnoStavkeNabavke50
							.Result.Where(x => x.RobaID == referentiProizvodiRobaID)
							.FirstOrDefault();
						planiranaWebNabavnaCena =
							poslednjaNabavkaReferentnogProizvoda.NabavnaCena
							+ (
								poslednjaNabavkaReferentnogProizvoda.NabavnaCena
								* (
									Convert.ToDouble(
										dataGridView1.Rows[e.RowIndex].Cells["Modifikator"].Value
									) / 100
								)
							);
						break;

					default:
						break;
				}

				dataGridView1.Rows[e.RowIndex].Cells["PlaniranaWebNabavnaCena"].Value =
					planiranaWebNabavnaCena;

				double webProdajnaCena = Convert.ToDouble(
					dr.Cells["TrenutnaWebProdajnaCena"].Value
				);
				double webNabavnaCena = Convert.ToDouble(dr.Cells["TrenutnaWebNabavnaCena"].Value);

				double rabatPlatinum =
					kRUM == null
						? -1
						: (
							(
								(webProdajnaCena - ((webProdajnaCena - webNabavnaCena) * 0.75))
								/ kRUM.ProdajnaCena
							) - 1
						) * -100;
				dataGridView1.Rows[e.RowIndex].Cells["RabatPlatinum"].Value = rabatPlatinum;

				double planiraniPlatinumRabat =
					kRUM == null
						? -1
						: (
							(
								(webProdajnaCena - ((webProdajnaCena - webNabavnaCena) * 0.75))
								/ kRUM.ProdajnaCena
							) - 1
						) * -100;
				dataGridView1.Rows[e.RowIndex].Cells["PlaniraniRabat"].Value =
					planiraniPlatinumRabat;

				ObojiPodateDgv();
			}
			catch
			{
				dataGridView1.CancelEdit();
			}
		}

		private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
		{
			bool validClick = (e.RowIndex != -1 && e.ColumnIndex != -1); //Make sure the clicked row/column is valid.
			var datagridview = sender as DataGridView;

			// Check to make sure the cell clicked is the cell containing the combobox
			if (datagridview.Columns[e.ColumnIndex] is DataGridViewComboBoxColumn && validClick)
			{
				datagridview.BeginEdit(true);
				((ComboBox)datagridview.EditingControl).DroppedDown = true;
			}
		}

		private void dataGridView1_CurrentCellDirtyStateChanged(object sender, EventArgs e)
		{
			if (sender is DataGridViewComboBoxCell)
				dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
		}

		private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
		{
			if (witem != null && !witem.IsDisposed && witem.Visible)
				witem.UcitajStavku(
					Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["RobaID"].Value),
					odDatuma_dtp.Value,
					doDatuma_dtp.Value
				);
		}

		private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
		{
			if (witem != null && !witem.IsDisposed && !witem.Visible)
			{
				witem.Show();
				witem.UcitajStavku(
					Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["RobaID"].Value),
					odDatuma_dtp.Value,
					doDatuma_dtp.Value
				);
			}
		}

		private void karticaRobeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int robaID = Convert.ToInt32(
				dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["RobaID"].Value
			);

			using (
				_7_fm_Komercijalno_Roba_Kartica kr = new _7_fm_Komercijalno_Roba_Kartica(
					robaID,
					150
				)
			)
				kr.ShowDialog();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show(
					"Da li sigurno zelite postaviti planirane cene kao stvarne cene sajta?",
					"Potvrdi",
					MessageBoxButtons.YesNo
				) != DialogResult.Yes
			)
				return;

			Task.Run(() =>
			{
				var webProizvodi = TDWeb.Proizvod.List();

				DataRow[] rows = null;
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							slogova_lbl.Text = "Azuriram nabavne cene na sajtu...";
							rows = (dataGridView1.DataSource as DataTable).Select();
						}
				);

				int i = 0;
				Parallel.ForEach(
					rows,
					dr =>
					{
						try
						{
							TDWeb.Proizvod p = webProizvodi
								.Where(x => x.RobaID == Convert.ToInt32(dr["RobaID"]))
								.FirstOrDefault();

							if (p == null)
								return;

							p.NabavnaCena = Convert.ToDouble(dr["PlaniranaWebNabavnaCena"]);
							p.Update();
							i++;
						}
						catch (Exception ex)
						{
							Task.Run(() =>
							{
								MessageBox.Show(ex.ToString());
							});
							i++;
						}

						this.Invoke(
							(MethodInvoker)
								delegate
								{
									slogova_lbl.Text =
										$"Azuriram nabavne cene na sajtu... ({i} / {rows.Length})";
								}
						);
					}
				);

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							slogova_lbl.Text = "Gotovo azuriranje nabavnih cena na sajtu";
							MessageBox.Show("Nabavne cene na sajtu uspesno azurirane!");

							this.webProizvodi = null;
							this.webProizvodi = TDWeb.Proizvod.ListAsync();
							this.webProizvodi.Wait();
							UcitajStavke();
						}
				);
			});
		}

		private void textBox1_TextChanged(object sender, EventArgs e)
		{
			_suspendValidation = true;
			_dgvData = _baseData.Select("Naziv Like '%" + textBox1.Text + "%'").CopyToDataTable();
			dataGridView1.DataSource = _dgvData;
			_suspendValidation = false;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			_helpFrom.Result.ShowDialog();
		}

		private void dataGridView1_Sorted(object sender, EventArgs e)
		{
			ObojiPodateDgv();
		}
	}
}
