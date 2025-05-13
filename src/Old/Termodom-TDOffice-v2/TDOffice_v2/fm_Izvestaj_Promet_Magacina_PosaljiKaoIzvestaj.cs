using System;
using System.Collections.Concurrent;
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
	public partial class fm_Izvestaj_Promet_Magacina_PosaljiKaoIzvestaj : Form
	{
		private DataTable _data = new DataTable();

		private Dictionary<int, Tuple<DateTime, DateTime>> _periodi;
		private Task<List<TDOffice.User>> _tdOfficeUsers { get; set; } = TDOffice.User.ListAsync();
		private List<int> _magaciniUIzvestaju { get; set; } = new List<int>();
		private Task<fm_Help> _help { get; set; }

		/// <summary>
		///
		/// </summary>
		/// <param name="data"></param>
		/// <param name="periodi">Predstavlja dictionary koji za prvi element ima godinu, a drugi element od datuma - do datuma unutar te godine</param>
		public fm_Izvestaj_Promet_Magacina_PosaljiKaoIzvestaj(
			DataTable data,
			Dictionary<int, Tuple<DateTime, DateTime>> periodi
		)
		{
			InitializeComponent();

			_data = data;
			_periodi = periodi;

			_help = this.InitializeHelpModulAsync(
				Modul.fm_Izvestaj_Prodaja_Roba_PosaljiKaoIzvestaj_Setup
			);

			_magaciniUIzvestaju = _data
				.AsEnumerable()
				.Where(x => int.TryParse(x["MAGACINID"].ToString(), out _))
				.Select(x => Convert.ToInt32(x["MAGACINID"]))
				.Distinct()
				.ToList();

			PodesiCLB();
		}

		private void PodesiCLB()
		{
			List<TDOffice.MagacinClan> _clanoviMagacina = TDOffice.MagacinClan.List();

			korisnicima_clb.DataSource =
				korisnicimaMagacinaCeoIzvestaj_rb.Checked || izvestajKakavVIdim_rb.Checked
					? _tdOfficeUsers.Result
					: _tdOfficeUsers
						.Result.Where(x =>
							_clanoviMagacina.Any(y =>
								y.KorisnikID == x.ID && _magaciniUIzvestaju.Contains(y.MagacinID)
							)
						)
						.ToList();

			korisnicima_clb.DisplayMember = "Username";
			korisnicima_clb.ValueMember = "ID";

			foreach (int mag in _magaciniUIzvestaju)
			{
				if (_clanoviMagacina.Count(x => x.MagacinID == mag) == 0)
				{
					Task.Run(() =>
					{
						MessageBox.Show("Magacin " + mag + " nema ni jednog clana!");
					});
				}
			}
		}

		private void posalji_btn_Click(object sender, EventArgs e)
		{
			if (korisnicima_clb.CheckedItems.Count == 0)
			{
				MessageBox.Show("Morate izabrati barem nekog primaoca!");
				return;
			}
			List<TDOffice.MagacinClan> _clanoviMagacina = TDOffice.MagacinClan.List();

			foreach (TDOffice.User u in korisnicima_clb.CheckedItems.OfType<TDOffice.User>())
			{
				DataTable izvestaj = _data.Copy();

				if (korisnicimaMagacinaSamoSvojePodatke_rb.Checked)
					izvestaj = _data
						.AsEnumerable()
						.Where(x =>
							Convert.ToInt32(x["MAGACINID"])
							== _clanoviMagacina.FirstOrDefault(y => y.KorisnikID == u.ID).MagacinID
						)
						.CopyToDataTable();
				else if (korisnicimaMagacinaCeoIzvestaj_rb.Checked)
					izvestaj = _data.Copy();
				string naslov = "Izvestaj Promet magacina";
				if (tb_NaslovIzvestaja.TextLength > 0)
					naslov = tb_NaslovIzvestaja.Text;
				TDOffice.Poruka.Insert(
					new TDOffice.Poruka()
					{
						Datum = DateTime.Now,
						Naslov = naslov,
						Posiljalac = Program.TrenutniKorisnik.ID,
						Primalac = u.ID,
						Status = TDOffice.PorukaTip.Standard,
						Tag = new TDOffice.PorukaAdditionalInfo()
						{
							Action = TDOffice.PorukaAction.IzvestajPrometaMagacina,
							AdditionalInfo = izvestaj
						},
						Tekst = string.Join(
							Environment.NewLine,
							komentar_rtb.Text,
							"Izvestaj Prometa Magacina. Pogledaj prilog gore desno!"
						)
					}
				);
			}

			MessageBox.Show("Izvestaj uspesno poslat!");
		}

		private void korisnicimaMagacinaSamoSvojePodatke_rb_CheckedChanged(
			object sender,
			EventArgs e
		)
		{
			PodesiCLB();
		}

		private void izvestajKakavVIdim_rb_CheckedChanged(object sender, EventArgs e)
		{
			PodesiCLB();
		}

		private void korisnicimaMagacinaCeoIzvestaj_rb_CheckedChanged(object sender, EventArgs e)
		{
			PodesiCLB();
		}

		private void help_btn_Click(object sender, EventArgs e)
		{
			_help.Result.ShowDialog();
		}

		private void btn_ReperniMagacini_Click(object sender, EventArgs e)
		{
			List<Komercijalno.Magacin> magacini = Komercijalno.Magacin.ListAsync().Result;
			List<Tuple<int, string>> dict = new List<Tuple<int, string>>();
			posalji_btn.Enabled = false;

			foreach (Komercijalno.Magacin m in magacini)
				dict.Add(new Tuple<int, string>(m.ID, m.Naziv));

			using (Input_CheckedListBox cb = new Input_CheckedListBox())
			{
				cb.DataSource = dict;
				cb.CheckOnClick = true;

				cb.ShowDialog();

				if (cb.CheckedValues.Count <= 0)
				{
					posalji_btn.Enabled = true;
					MessageBox.Show("Niste izabrali ni jedan magacin za REPER!");
					return;
				}

				List<int> CheckedValues = cb.CheckedValues;

				Task.Run(() =>
				{
					DataTable dt = new DataTable();
					foreach (int godina in _periodi.Keys.OrderBy(x => x))
					{
						this.Invoke(
							(MethodInvoker)
								delegate
								{
									status_lbl.Text = $"Ucitavanje {godina} ...";
								}
						);
						using (
							FbConnection con = new FbConnection(
								Komercijalno.Komercijalno.CONNECTION_STRING[godina]
							)
						)
						{
							con.Open();
							using (
								FbCommand cmd = new FbCommand(
									@"
SELECT t1.MAGACINID, SUM(t1.vGOD"
										+ godina
										+ @" - t2.vGOD"
										+ godina
										+ @") AS vGOD"
										+ godina
										+ @"
FROM
    (
    SELECT s.MAGACINID, SUM(s.KOLICINA * s.PRODAJNACENA * (100 - s.rabat)/100) AS vGOD"
										+ godina
										+ @"
    FROM STAVKA s
    LEFT OUTER JOIN DOKUMENT d ON d.VRDOK = s.VRDOK AND d.BRDOK = s.BRDOK
    WHERE d.FLAG = 1 AND d.MAGACINID IN ("
										+ string.Join(", ", CheckedValues)
										+ @") AND (d.VRDOK = 13 OR d.VRDOK = 15) AND DATUM >= @ODDATUMA AND DATUM <= @DODATUMA
    GROUP BY s.MAGACINID ORDER BY S.MAGACINID desc
    ) as t1
    LEFT OUTER JOIN
    (
    SELECT s.MAGACINID, SUM(s.KOLICINA * s.PRODAJNACENA * (100 - s.rabat)/100) AS vGOD"
										+ godina
										+ @"
    FROM STAVKA s
    LEFT OUTER JOIN DOKUMENT d ON d.VRDOK = s.VRDOK AND d.BRDOK = s.BRDOK
    WHERE d.MAGACINID IN ("
										+ string.Join(", ", CheckedValues)
										+ @") AND (d.VRDOK = 22) AND DATUM >= @ODDATUMA AND DATUM <= @DODATUMA
    GROUP BY s.MAGACINID ORDER BY S.MAGACINID desc
    ) as t2 on t1.MAGACINID = t2.MAGACINID
    GROUP BY t1.MAGACINID",
									con
								)
							)
							{
								cmd.Parameters.AddWithValue("@ODDATUMA", _periodi[godina].Item1);
								cmd.Parameters.AddWithValue("@DODATUMA", _periodi[godina].Item2);
								using (FbDataAdapter da = new FbDataAdapter(cmd))
								{
									DataTable tempDT = new DataTable();
									if (dt.Rows.Count == 0)
									{
										da.Fill(dt);
										dt.PrimaryKey = new DataColumn[]
										{
											dt.Columns["MAGACINID"]
										};
										continue;
									}

									da.Fill(tempDT);
									tempDT.PrimaryKey = new DataColumn[]
									{
										tempDT.Columns["MAGACINID"]
									};

									dt.Merge(tempDT, false, MissingSchemaAction.Add);
								}
							}
						}
					}

					DataTable finalDT = dt.Clone();
					finalDT.Columns["MagacinID"].DataType = typeof(string);

					foreach (DataRow r in dt.Rows)
						finalDT.ImportRow(r);

					DataRow dr = finalDT.NewRow();
					dr["MagacinID"] = "REPER";

					DataTable izvestajDT = _data.Clone();
					izvestajDT.Columns["MagacinID"].DataType = typeof(string);

					foreach (DataRow r in _data.Rows)
						izvestajDT.ImportRow(r);

					DataRow drizv = izvestajDT.NewRow();
					drizv["MagacinID"] = "REPER(" + string.Join(", ", CheckedValues) + ")";

					foreach (int godina in _periodi.Keys.OrderBy(x => x))
					{
						dr["vGOD" + godina] = finalDT.Compute($"Sum(vGOD{godina})", string.Empty);
						drizv["vGOD" + godina] = finalDT.Compute(
							$"Sum(vGOD{godina})",
							string.Empty
						);
					}
					finalDT.Rows.Add(dr);
					izvestajDT.Rows.Add(drizv);

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								_data = izvestajDT;
								posalji_btn.Enabled = true;
								status_lbl.Text = $"";
							}
					);
				});
			}
		}
	}
}
