using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using FirebirdSql.Data.FirebirdClient;
using TDOffice_v2.Komercijalno;
using Termodom.Data.Entities.DBSettings;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
	public partial class fm_KopirajPodatkeTabeleSTAVKAIzDokumentaUDokument : Form
	{
		private List<string> _poljaTabeleStavka = new List<string>()
		{
			"PRODCENABP",
			"KOREKCIJA",
			"PRODAJNACENA",
			"DEVIZNACENA",
			"DEVPRODCENA",
			"POREZ",
			"RABAT",
			"MARZA",
			"FAKTURNACENA",
			"NABCENABT",
			"NABCENASAPOR",
			"TROSKOVI",
			"NABAVNACENA",
			"KOLICINA",
			"POREZ_ULAZ",
			"POREZ_IZ"
		};

		public fm_KopirajPodatkeTabeleSTAVKAIzDokumentaUDokument()
		{
			InitializeComponent();
		}

		private void fm_KopirajPodatkeTabeleSTAVKAIzDokumentaUDokument_Load(
			object sender,
			EventArgs e
		)
		{
			SetStatus("Ucitavanje...");
			ToggleUi(false);
			LoadAsync();
		}

		private void SetStatus(string status)
		{
			this.Invoke(
				(MethodInvoker)
					delegate
					{
						toolStripStatusLabel1.Text = status;
					}
			);
		}

		private void ToggleUi(bool state)
		{
			groupBox1.Enabled = state;
			groupBox2.Enabled = state;
			izvrsiKopiranje_btn.Enabled = state;
		}

		private async void LoadAsync()
		{
			var distinctPutanjeDoBazaTask = BazaManager.DistinctConnectionInfoListAsync();
			var vrsteDokumenataTask = VrstaDokManager.DictionaryAsync();

			var vrsteDokumenata = (await vrsteDokumenataTask).Values.ToList();

			izvorniVrDok_cmb.DataSource = new List<VrstaDok>(vrsteDokumenata);
			izvorniVrDok_cmb.DisplayMember = "NazivDok";
			izvorniVrDok_cmb.ValueMember = "VrDok";

			destinacioniVrDok_cmb.DataSource = new List<VrstaDok>(vrsteDokumenata);
			destinacioniVrDok_cmb.DisplayMember = "NazivDok";
			destinacioniVrDok_cmb.ValueMember = "VrDok";

			var putanjeDoBazaTuple = new List<Tuple<string, string>>();

			foreach (DistinctConnectionInfo csi in await distinctPutanjeDoBazaTask)
			{
				string[] putanjaParts = csi.PutanjaDoBaze.Split("/");
				putanjeDoBazaTuple.Add(
					new Tuple<string, string>(
						csi.PutanjaDoBaze,
						$"{csi.Godina} - {putanjaParts[putanjaParts.Length - 1]}"
					)
				);
			}

			izvornaBaza_cmb.DataSource = new List<Tuple<string, string>>(putanjeDoBazaTuple);
			izvornaBaza_cmb.DisplayMember = "Item2";
			izvornaBaza_cmb.ValueMember = "Item1";

			destinacionaBaza_cmb.DataSource = new List<Tuple<string, string>>(putanjeDoBazaTuple);
			destinacionaBaza_cmb.DisplayMember = "Item2";
			destinacionaBaza_cmb.ValueMember = "Item1";

			izvornoPolje_cmb.DataSource = new List<string>(_poljaTabeleStavka.OrderBy(x => x));
			destinacionoPolje_cmb.DataSource = new List<string>(_poljaTabeleStavka.OrderBy(x => x));

			SetStatus("Spremno za rad!");
			ToggleUi(true);
		}

		private void izvrsiKopiranje_btn_Click(object sender, EventArgs e)
		{
			if (
				MessageBox.Show(
					"Da li sigurno zelite pokrenuti ovu akciju? Proverite jos jednom baze!!!",
					"Potvrdi",
					MessageBoxButtons.YesNo
				) != DialogResult.Yes
			)
			{
				MessageBox.Show("Ponistavam!");
				return;
			}

			SetStatus("Zapocinjem kopiranje stavki...");
			ToggleUi(false);
			try
			{
				int izvorniBrDok;
				int destinacioniBrDok;

				if (!int.TryParse(izvorniBrDok_txt.Text, out izvorniBrDok))
				{
					MessageBox.Show("Neispravan broj izvornog dokumenta");
					ToggleUi(true);
					return;
				}

				if (!int.TryParse(destinacioniBrDok_txt.Text, out destinacioniBrDok))
				{
					MessageBox.Show("Neispravan broj destinacionog dokumenta");
					ToggleUi(true);
					return;
				}

				var izvornaBaza = izvornaBaza_cmb.SelectedValue as string;
				var destinacionaBaza = destinacionaBaza_cmb.SelectedValue as string;

				var izvorniVrDok = izvorniVrDok_cmb.SelectedItem as VrstaDok;
				var destinacioniVrDok = destinacioniVrDok_cmb.SelectedItem as VrstaDok;

				var izvornoPolje = izvornoPolje_cmb.SelectedItem as string;
				var destinacionoPolje = destinacionoPolje_cmb.SelectedItem as string;

				var izvorniString =
					$"data source=4monitor; initial catalog = {izvornaBaza}; user=SYSDBA; password=m";
				var destinacioniString =
					$"data source=4monitor; initial catalog = {destinacionaBaza}; user=SYSDBA; password=m";

				var izvorneVrednosti = new Dictionary<int, string>();

				using (FbConnection con = new FbConnection(izvorniString))
				{
					con.Open();

					SetStatus("Proveravam da li izvorni dokument postoji...");
					using (
						FbCommand cmd = new FbCommand(
							"SELECT COUNT(*) FROM DOKUMENT WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@VRDOK", izvorniVrDok.VrDok);
						cmd.Parameters.AddWithValue("@BRDOK", izvorniBrDok);

						using (FbDataReader dr = cmd.ExecuteReader())
							if (dr.Read())
								if (Convert.ToInt32(dr[0]) == 0)
								{
									MessageBox.Show("Izvorni dokument ne postoji!");
									return;
								}
					}

					SetStatus("Proveravam da li destinacioni dokument postoji...");
					using (
						FbCommand cmd = new FbCommand(
							"SELECT COUNT(*) FROM DOKUMENT WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@VRDOK", destinacioniVrDok.VrDok);
						cmd.Parameters.AddWithValue("@BRDOK", destinacioniBrDok);

						using (FbDataReader dr = cmd.ExecuteReader())
							if (dr.Read())
								if (Convert.ToInt32(dr[0]) == 0)
								{
									MessageBox.Show("Destinacioni dokument ne postoji!");
									return;
								}
					}

					SetStatus("Selektujem stavke izvornog dokumenta...");
					using (
						FbCommand cmd = new FbCommand(
							@$"SELECT ROBAID, {izvornoPolje}
                        FROM STAVKA
                        WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@VRDOK", izvorniVrDok.VrDok);
						cmd.Parameters.AddWithValue("@BRDOK", izvorniBrDok);

						using (FbDataReader dr = cmd.ExecuteReader())
							while (dr.Read())
								izvorneVrednosti.Add(
									Convert.ToInt32(dr["ROBAID"]),
									dr[izvornoPolje] is DBNull
										? null
										: dr[izvornoPolje].ToStringOrDefault()
								);
					}
				}

				if (izvorneVrednosti.Count == 0)
				{
					MessageBox.Show("Izvorni dokument je prazan!");
					ToggleUi(true);
					return;
				}

				using (FbConnection con = new FbConnection(destinacioniString))
				{
					con.Open();

					SetStatus("Proveravam da li destinacioni dokument postoji...");
					using (
						FbCommand cmd = new FbCommand(
							"SELECT COUNT(*) FROM STAVKA WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@VRDOK", destinacioniVrDok.VrDok);
						cmd.Parameters.AddWithValue("@BRDOK", destinacioniBrDok);

						using (FbDataReader dr = cmd.ExecuteReader())
							if (dr.Read())
								if (Convert.ToInt32(dr[0]) == 0)
								{
									MessageBox.Show("Destinacioni dokument je prazan!");
									return;
								}
					}

					SetStatus("Selektujem stavke destinacionog dokumenta...");
					using (
						FbCommand cmd = new FbCommand(
							@$"UPDATE STAVKA SET
                        {destinacionoPolje} = @VREDNOST WHERE VRDOK = @VRDOK AND BRDOK = @BRDOK AND ROBAID = @ROBAID",
							con
						)
					)
					{
						cmd.Parameters.AddWithValue("@VRDOK", destinacioniVrDok.VrDok);
						cmd.Parameters.AddWithValue("@BRDOK", destinacioniBrDok);

						cmd.Parameters.Add(new FbParameter("@VREDNOST", FbDbType.VarChar));
						cmd.Parameters.Add(new FbParameter("@ROBAID", FbDbType.Integer));

						foreach (int key in izvorneVrednosti.Keys)
						{
							cmd.Parameters["@VREDNOST"].Value = izvorneVrednosti[key];
							cmd.Parameters["@ROBAID"].Value = key;
							cmd.ExecuteNonQuery();
						}
					}
				}
				SetStatus("Gotovo.");
				MessageBox.Show("Gotovo!");
			}
			catch (Exception ex)
			{
				SetStatus("Doslo je do greske prilikom izvrsenja akcija. Akcija obustavljena.");
				MessageBox.Show(ex.ToString());
			}
			ToggleUi(true);
		}
	}
}
