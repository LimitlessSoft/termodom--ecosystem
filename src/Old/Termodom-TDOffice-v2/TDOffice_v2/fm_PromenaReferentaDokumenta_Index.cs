using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
	public partial class fm_PromenaReferentaDokumenta_Index : Form
	{
		private Komercijalno.Dokument dokument { get; set; } = null;
		private Task<List<Config.Zaposleni>> _zaposleni { get; set; } =
			Config.Zaposleni.ListAsync();
		private Task<VrstaDokDictionary> _vrstaDok { get; set; } =
			Komercijalno.VrstaDokManager.DictionaryAsync();

		public fm_PromenaReferentaDokumenta_Index()
		{
			InitializeComponent();
			List<int> baze = Komercijalno.Komercijalno.CONNECTION_STRING.Keys.ToList();
			baze.Sort((y, x) => x.CompareTo(y));

			cmb_Godine.DataSource = new List<int>(baze);

			List<VrstaDok> vrdokList = _vrstaDok.GetAwaiter().GetResult().Values.ToList();
			vrdokList.Add(new VrstaDok() { VrDok = -1, NazivDok = " < vrsta dokumenta > " });
			vrdokList.Sort((x, y) => x.VrDok.CompareTo(y.VrDok));

			cmb_VrstaDokumenta.DataSource = vrdokList;
			cmb_VrstaDokumenta.DisplayMember = "NazivDok";
			cmb_VrstaDokumenta.ValueMember = "VrDok";

			List<Config.Zaposleni> zap = _zaposleni.Result;
			zap.Add(new Config.Zaposleni() { ZapID = -1, Naziv = "< Referent >" });
			zap.Sort((x, y) => x.ZapID.CompareTo(y.ZapID));

			cmb_NoviReferent.DataSource = zap;
			cmb_NoviReferent.DisplayMember = "Naziv";
			cmb_NoviReferent.ValueMember = "ZapID";
		}

		private void btn_Ucitaj_Click(object sender, EventArgs e)
		{
			if (cmb_VrstaDokumenta.SelectedIndex == 0)
			{
				MessageBox.Show("Niste izabrali vrstu dokumenta");
				return;
			}

			int vrDok = Convert.ToInt32(cmb_VrstaDokumenta.SelectedValue);
			int brDok = Convert.ToInt32(txt_BrDok.Text);
			dokument = Komercijalno.Dokument.Get(Convert.ToInt32(cmb_Godine.Text), vrDok, brDok);

			if (dokument == null)
			{
				MessageBox.Show("Dokument" + " nije pronadjen");
				return;
			}

			label4.Text =
				"Trenutni referent <"
				+ cmb_VrstaDokumenta.Text
				+ "> - "
				+ txt_BrDok.Text
				+ " je <"
				+ Config.Zaposleni.Get(dokument.RefID).Naziv
				+ ">";
			panel2.Enabled = true;
		}

		private void btn_IzmeniReferenta_Click(object sender, EventArgs e)
		{
			dokument.RefID = Convert.ToInt32(cmb_NoviReferent.SelectedValue);
			dokument.ZapID = Convert.ToInt32(cmb_NoviReferent.SelectedValue);
			dokument.Update();
			MessageBox.Show("Uspesno izvrsene izmene");
			Close();
		}
	}
}
