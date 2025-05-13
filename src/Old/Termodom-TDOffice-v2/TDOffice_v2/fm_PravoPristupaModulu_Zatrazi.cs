using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
	public partial class fm_PravoPristupaModulu_Zatrazi : Form
	{
		public int ModulID { get; set; }

		public fm_PravoPristupaModulu_Zatrazi(string title, string text, int modulID)
		{
			InitializeComponent();
			this.Text = title;
			this.label1.Text = text;
			ModulID = modulID;
		}

		private void btn_ZatraziPravo_Click(object sender, EventArgs e)
		{
			TDOffice.Pravo p = TDOffice.Pravo.Get(ModulID, Program.TrenutniKorisnik.ID);
			TDOffice.PravoDefinicija pd = TDOffice
				.Pravo.definicijePrava.Where(x => x.ModulID == ModulID)
				.FirstOrDefault();
			if (p != null)
			{
				if (p.Value == -1)
				{
					MessageBox.Show(
						"Pravo za modul <"
							+ pd.Naziv
							+ ">, ID "
							+ ModulID.ToString()
							+ "\n Vam je ZAUVEK oduzeto"
					);
					this.Close();
					return;
				}
			}

			string naslov = "Pravo pristupa modulu ";
			string tekst =
				"Korisnik "
				+ Program.TrenutniKorisnik.Username
				+ " je zatrazio pravo <"
				+ pd.Naziv
				+ ">, ID "
				+ ModulID.ToString();
			TDOffice.Poruka.Insert(
				TDOffice
					.User.List()
					.Where(x =>
						x.Tag != null
						&& x.Tag.PrimaObavestenja[
							TDOffice.User.TipAutomatskogObavestenja.PravoPristupaModulu
						] == true
					)
					.Select(x => x.ID)
					.ToArray(),
				naslov,
				tekst,
				new TDOffice.PorukaAdditionalInfo()
				{
					Action = TDOffice.PorukaAction.PravoPristupaModulu,
					AdditionalInfo = ModulID
				}
			);
			MessageBox.Show(
				"Zahtev za dodelu prava za modul " + ModulID.ToString() + " uspesno poslat"
			);
			this.Close();
		}

		private void ipakMiNeTreba_btn_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
