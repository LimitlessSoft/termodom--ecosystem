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
	public partial class Mendazment_SvediPocetnoStanjeCene_Index : Form
	{
		public Mendazment_SvediPocetnoStanjeCene_Index()
		{
			InitializeComponent();
		}

		private void Mendazment_SvediPocetnoStanjeCene_Index_Load(object sender, EventArgs e)
		{
			checkedListBox1.DataSource = Komercijalno.Magacin.ListAsync().Result;
			checkedListBox1.ValueMember = "ID";
			checkedListBox1.DisplayMember = "Naziv";
		}

		private void button1_Click(object sender, EventArgs e)
		{
			button1.Enabled = false;
			Task.Run(() =>
			{
				UpdateStatusLabel("Zapoceta je akcija svodnjavanja pocetnih stanja magacina");

				using (
					FbConnection con = new FbConnection(
						Komercijalno.Komercijalno.CONNECTION_STRING[2020]
					)
				)
				{
					con.Open();

					using (
						FbConnection con2 = new FbConnection(
							Komercijalno.Komercijalno.CONNECTION_STRING[2021]
						)
					)
					{
						con2.Open();

						UpdateStatusLabel(" / Ucitavam robu u magacinu prethodne godine");
						List<Komercijalno.RobaUMagacinu> rums = Komercijalno.RobaUMagacinu.List(
							con
						);

						foreach (
							Komercijalno.Magacin magacin in checkedListBox1.CheckedItems.OfType<Komercijalno.Magacin>()
						)
						{
							UpdateStatusLabel(magacin.Naziv + " / Ucitavam pocetno stanje");
							Komercijalno.Dokument dokumentPocetnogStanja = Komercijalno
								.Dokument.List(con2, "VRDOK = 0 AND MAGACINID = " + magacin.ID)
								.FirstOrDefault();

							UpdateStatusLabel(magacin.Naziv + " / Ucitavam stavke pocetnog stanja");
							List<Komercijalno.Stavka> stavkePocetnogStanja =
								Komercijalno.Stavka.List(
									con2,
									"VRDOK = 0 AND BRDOK = "
										+ dokumentPocetnogStanja.BrDok.ToString()
								);

							UpdateStatusLabel(magacin.Naziv + " / Ucitavam robu za ovaj magacin");
							List<Komercijalno.RobaUMagacinu> robaZaOvajMagacin = rums.Where(x =>
									x.MagacinID == magacin.ID
								)
								.ToList();

							int i = 0;
							int total = robaZaOvajMagacin.Count;
							foreach (Komercijalno.RobaUMagacinu rum in robaZaOvajMagacin)
							{
								i++;
								UpdateStatusLabel(
									magacin.Naziv + " / Azuriranje cene " + i + " od " + total
								);

								var sps = stavkePocetnogStanja.FirstOrDefault(x =>
									x.RobaID == rum.RobaID
								);
								if (sps == null)
								{
									sps = Komercijalno.Stavka.Get(
										con2,
										Komercijalno.Stavka.Insert(
											con2,
											dokumentPocetnogStanja,
											Komercijalno.Roba.Get(con2, rum.RobaID),
											rum,
											0,
											0
										)
									);
								}
								sps.NabavnaCena = rum.NabavnaCena;
								sps.ProdajnaCena = rum.ProdajnaCena;
								sps.Update(con2);
							}
						}
					}
				}

				this.Invoke(
					(MethodInvoker)
						delegate
						{
							button1.Enabled = true;
							MessageBox.Show("Gotovo!");
						}
				);
			});
		}

		private void UpdateStatusLabel(string message)
		{
			this.Invoke(
				(MethodInvoker)
					delegate
					{
						label1.Text = message;
					}
			);
		}
	}
}
