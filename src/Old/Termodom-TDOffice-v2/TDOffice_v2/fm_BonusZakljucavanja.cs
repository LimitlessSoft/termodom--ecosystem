using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;
using TDOffice_v2.Models;

namespace TDOffice_v2
{
	public partial class fm_BonusZakljucavanja : Form
	{
		public fm_BonusZakljucavanja()
		{
			InitializeComponent();
			OsveziPodatke();
		}

		private void OsveziPodatke()
		{
			TDOffice.User us = TDOffice.User.Get(Program.TrenutniKorisnik.ID);
			label2.Text = "Broj zakljucavanja:" + us.BonusZakljucavanjaCount.ToString();
			label3.Text = "Limit bonusa:" + us.BonusZakljucavanjaLimit.ToString();
		}

		private void btnPodesavanjaBonusa_Click(object sender, EventArgs e)
		{
			using (fm_BonusPodesavanja bp = new fm_BonusPodesavanja())
			{
				bp.ShowDialog();
			}
		}

		private void btnZatraziBonuseZaklkjucavanja_Click(object sender, EventArgs e)
		{
			var korisniciKojiPrimaju = TDOffice
				.User.List()
				.Where(x =>
					x.Tag != null
					&& x.Tag.PrimaObavestenja[
						TDOffice
							.User
							.TipAutomatskogObavestenja
							.PrimaObavestenjaOObnoviBonusaZakljucavanja
					] == true
				);
			foreach (TDOffice.User kp in korisniciKojiPrimaju)
			{
				TDOffice.Poruka.Insert(
					new TDOffice.Poruka()
					{
						Datum = DateTime.Now,
						Naslov = "Bonus zakljucavanje",
						Posiljalac = Program.TrenutniKorisnik.ID,
						Primalac = kp.ID,
						Status = TDOffice.PorukaTip.Standard,
						Tag = new TDOffice.PorukaAdditionalInfo()
						{
							Action = TDOffice.PorukaAction.BonusZakljucavanje,
							AdditionalInfo = Program.TrenutniKorisnik.ID
						},
						Tekst = string.Join(
							Environment.NewLine,
							"",
							"Zahtev za reset bonusa zakljucavanja"
						)
					}
				);
			}
		}

		private void btnZakljucajuzpomocbonusa_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtMpRacun.Text))
			{
				MessageBox.Show("Morate uneti broj dokumenta!");
				return;
			}
			int brDok = Convert.ToInt32(this.txtMpRacun.Text);
			TDOffice.User us = TDOffice.User.Get(Program.TrenutniKorisnik.ID);
			Komercijalno.Dokument dok = Komercijalno.Dokument.Get(DateTime.Now.Year, 15, brDok);
			if (dok == null)
			{
				MessageBox.Show("Dokument" + " nije pronadjen");
				return;
			}
			if (dok.PPID == null)
			{
				MessageBox.Show("Dokument nema PPID");
				return;
			}
			if (us.BonusZakljucavanjaCount == 0)
			{
				MessageBox.Show("Iskoristili ste bonuse");
				return;
			}
			else
			{
				if (us.BonusZakljucavanjaLimit < dok.Potrazuje)
				{
					dok.Flag = 1;
					dok.Update();
					MessageBox.Show("Dokument br." + dok.BrDok.ToString() + " zakljucan");
				}
				else
				{
					MessageBox.Show(
						"Vrednost Dokumenta br." + dok.BrDok.ToString() + " premasuje limit"
					);
					return;
				}
			}
		}
	}
}
