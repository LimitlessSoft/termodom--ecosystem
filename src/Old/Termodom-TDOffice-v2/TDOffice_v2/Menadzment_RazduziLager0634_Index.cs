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
	public partial class Menadzment_RazduziLager0634_Index : Form
	{
		private static int _godina { get; set; } = DateTime.Now.Year;

		// magacin, Dict<robaid, kolicina>
		private Task<TDOffice.Config<
			Dictionary<int, Dictionary<int, double>>
		>> _lager { get; set; } =
			TDOffice.Config<Dictionary<int, Dictionary<int, double>>>.GetAsync(
				TDOffice.ConfigParameter.Lager0634
			);

		// magacin, List<brDok34>
		private Task<TDOffice.Config<Dictionary<int, List<int>>>> _razduzene34 { get; set; } =
			TDOffice.Config<Dictionary<int, List<int>>>.GetAsync(
				TDOffice.ConfigParameter.Lager0634RazduzenaDokumenta
			);
		private Task<List<Komercijalno.Magacin>> _komercijalnoMagacini { get; set; } =
			Komercijalno.Magacin.ListAsync(DateTime.Now.Year);

		public Menadzment_RazduziLager0634_Index()
		{
			InitializeComponent();
			magacin_cmb.Enabled = false;
		}

		private void Menadzment_RazduziLager0634_Index_Load(object sender, EventArgs e)
		{
			magacin_cmb.ValueMember = "ID";
			magacin_cmb.DisplayMember = "Naziv";
			magacin_cmb.DataSource = _komercijalnoMagacini.Result;
			magacin_cmb.Enabled = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			status_lbl.Text = "Zapoceto razduzivanje magacina...";
			Task.Run(() =>
			{
				int magacinID = 0;
				this.Invoke(
					(MethodInvoker)
						delegate
						{
							magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
							status_lbl.Text = "Otvaram konekciju sa bazom...";
						}
				);
				using (
					FbConnection con = new FbConnection(
						Komercijalno.Komercijalno.CONNECTION_STRING[_godina]
					)
				)
				{
					con.Open();
					this.Invoke(
						(MethodInvoker)
							delegate
							{
								status_lbl.Text = "Hvatam sve 34 za dati magacin...";
							}
					);
					List<Komercijalno.Dokument> nerazduzene34 = Komercijalno.Dokument.List(
						con,
						"VRDOK = 34 AND MAGACINID = " + magacinID
					);

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								status_lbl.Text = "Pospremam mesto za lager...";
							}
					);
					if (_razduzene34.Result.Tag == null)
						_razduzene34.Result.Tag = new Dictionary<int, List<int>>();

					if (
						!_razduzene34.Result.Tag.ContainsKey(magacinID)
						|| _razduzene34.Result.Tag[magacinID] == null
					)
						_razduzene34.Result.Tag[magacinID] = new List<int>();

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								status_lbl.Text = "Uklanjam 34 koji su vec razduzeni iz buffera...";
							}
					);
					if (_razduzene34.Result.Tag[magacinID].Count > 0) // Ovo samo zbog perfomansi
						nerazduzene34.RemoveAll(x =>
							_razduzene34.Result.Tag[magacinID].Contains(x.BrDok)
						);

					if (_lager.Result.Tag == null)
						_lager.Result.Tag = new Dictionary<int, Dictionary<int, double>>();

					if (
						!_lager.Result.Tag.ContainsKey(magacinID)
						|| _lager.Result.Tag[magacinID] == null
					)
						_lager.Result.Tag[magacinID] = new Dictionary<int, double>();

					Dictionary<int, double> stavkeZaDodatiULager = new Dictionary<int, double>();

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								status_lbl.Text = "Ucitavam Robu U Magaicnu...";
							}
					);
					List<Komercijalno.RobaUMagacinu> robaUMagacinu =
						Komercijalno.RobaUMagacinu.ListByMagacinID(con, magacinID);

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								status_lbl.Text = "Podesavam buffer stavki za dodati u lager...";
							}
					);
					foreach (Komercijalno.RobaUMagacinu rum in robaUMagacinu)
						stavkeZaDodatiULager[rum.RobaID] = 0;

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								status_lbl.Text =
									"Prolazim kroz trenutni lager i punim buffer stavki za dodati u lager...";
							}
					);
					foreach (int key in _lager.Result.Tag[magacinID].Keys)
						stavkeZaDodatiULager[key] += _lager.Result.Tag[magacinID][key];

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								status_lbl.Text =
									"Prolazim kroz nerazduzene 34 i dodajem u buffer stavki za dodati u lager...";
							}
					);
					foreach (Komercijalno.Dokument dok in nerazduzene34)
					{
						_razduzene34.Result.Tag[magacinID].Add(dok.BrDok);

						foreach (
							Komercijalno.Stavka s in Komercijalno.Stavka.ListByDokument(
								con,
								34,
								dok.BrDok
							)
						)
							if (s.Vrsta == 1)
								stavkeZaDodatiULager[s.RobaID] += s.Kolicina;
					}

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								status_lbl.Text = "Prolazim kroz buffer i punim lager";
							}
					);
					foreach (int key in stavkeZaDodatiULager.Keys)
					{
						if (stavkeZaDodatiULager[key] == 0)
							continue;

						_lager.Result.Tag[magacinID][key] = stavkeZaDodatiULager[key];
					}

					_lager.Result.UpdateOrInsert();
					_razduzene34.Result.UpdateOrInsert();

					this.Invoke(
						(MethodInvoker)
							delegate
							{
								status_lbl.Text = "Lager uspesno razduzen!";
								MessageBox.Show("Lager uspesno razduzen");
							}
					);
				}
			});
		}
	}
}
