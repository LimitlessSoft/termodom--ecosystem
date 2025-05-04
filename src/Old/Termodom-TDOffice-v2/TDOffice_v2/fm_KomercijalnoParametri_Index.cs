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
	public partial class fm_KomercijalnoParametri_Index : Form
	{
		private static string[] sablonItems = new string[]
		{
			"Magacin 12 moze da ide u minus",
			"Magacin 13 moze da ide u minus",
			"Magacin 14 moze da ide u minus",
			"Magacin 15 moze da ide u minus",
			"Magacin 16 moze da ide u minus",
			"Magacin 17 moze da ide u minus",
			"Magacin 18 moze da ide u minus",
			"Magacin 19 moze da ide u minus",
			"Magacin 20 moze da ide u minus",
			"Magacin 21 moze da ide u minus",
			"Magacin 22 moze da ide u minus",
			"Magacin 23 moze da ide u minus",
			"Magacin 24 moze da ide u minus",
			"Magacin 25 moze da ide u minus",
			"Magacin 26 moze da ide u minus",
			"Magacin 27 moze da ide u minus",
			"Magacin 28 moze da ide u minus",
			"Magacin 50 moze da ide u minus",
			"Kljuc Menja Datum VrDok 15",
			"Kljuc Menja Datum VrDok 13",
			"Kljuc Menja Datum VrDok 0",
			"Kljuc Menja Datum VrDok 7",
			"Kljuc Menja Datum VrDok 32",
			"Izmena Menja Datum VrDok 15",
			"Izmena Menja Datum VrDok 13",
			"Izmena Menja Datum VrDok 0",
			"Izmena Menja Datum VrDok 7",
			"Izmena Menja Datum VrDok 32",
			"Pin cine samo brojevi",
			"Podrazumevani rok za placanje na profakturi",
			"Podrazumevana vrsta dokumenta",
			"Podrazumevani magacin",
			"Zabranjeno dupliranje kombinacije katbr+katbrpro",
			"Maksim. datum ili broj dana za izmenu obj. (fin)",
		};

		public fm_KomercijalnoParametri_Index()
		{
			MessageBox.Show("Ovaj modul trenutno ne radi! Kontaktirati administratora!");
			this.Close();
			return;
			if (!Program.TrenutniKorisnik.ImaPravo(168100))
			{
				TDOffice.Pravo.NematePravoObavestenje(168100);
				this.Close();
				return;
			}
			InitializeComponent();
		}

		private void fm_KomercijalnoParametri_Index_Load(object sender, EventArgs e)
		{
			//UcitajParametre();
		}

		//private void UcitajParametre()
		//{
		//    DataTable dt = new DataTable();
		//    dt.Columns.Add("Parametar", typeof(string));
		//    dt.Columns.Add("Sablonski", typeof(string));
		//    dt.Columns.Add("Trenutno", typeof(string));

		//    TDOffice.Config<Dictionary<string, string>> sablon = TDOffice.Config<Dictionary<string, string>>.Get(TDOffice.ConfigParameter.KomercijalnoParametriSablon);

		//    if(sablon.Tag == null)
		//    {
		//        sablon.Tag = new Dictionary<string, string>();
		//        sablon.UpdateOrInsert();
		//    }

		//    foreach(string si in sablonItems)
		//    {
		//        string trenutnaVrednost = GetTrenutnuVrednost(si);
		//        DataRow dr = dt.NewRow();
		//        dr["Parametar"] = si;
		//        dr["Sablonski"] = sablon.Tag.ContainsKey(si) && !string.IsNullOrWhiteSpace(sablon.Tag[si]) ? sablon.Tag[si] : trenutnaVrednost;
		//        dr["Trenutno"] = trenutnaVrednost;
		//        dt.Rows.Add(dr);
		//    }

		//    dataGridView1.RowHeadersVisible = false;
		//    dataGridView1.DataSource = dt;
		//    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

		//    dataGridView1.Columns["Parametar"].ReadOnly = true;
		//    dataGridView1.Columns["Sablonski"].DefaultCellStyle.BackColor = Color.LightYellow;
		//    dataGridView1.Columns["Trenutno"].ReadOnly = true;
		//}

		//private string GetTrenutnuVrednost(string parametar)
		//{
		//    switch(parametar)
		//    {
		//        case "Magacin 12 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 12).MozeMinus.ToString();
		//        case "Magacin 13 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 13).MozeMinus.ToString();
		//        case "Magacin 14 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 14).MozeMinus.ToString();
		//        case "Magacin 15 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 15).MozeMinus.ToString();
		//        case "Magacin 16 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 16).MozeMinus.ToString();
		//        case "Magacin 17 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 17).MozeMinus.ToString();
		//        case "Magacin 18 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 18).MozeMinus.ToString();
		//        case "Magacin 19 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 19).MozeMinus.ToString();
		//        case "Magacin 20 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 20).MozeMinus.ToString();
		//        case "Magacin 21 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 21).MozeMinus.ToString();
		//        case "Magacin 22 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 22).MozeMinus.ToString();
		//        case "Magacin 23 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 23).MozeMinus.ToString();
		//        case "Magacin 24 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 24).MozeMinus.ToString();
		//        case "Magacin 25 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 25).MozeMinus.ToString();
		//        case "Magacin 26 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 26).MozeMinus.ToString();
		//        case "Magacin 27 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 27).MozeMinus.ToString();
		//        case "Magacin 28 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 28).MozeMinus.ToString();
		//        case "Magacin 50 moze da ide u minus":
		//            return Komercijalno.Magacin.Get(DateTime.Now.Year, 50).MozeMinus.ToString();
		//        case "Kljuc Menja Datum VrDok 15":
		//            return Komercijalno.VrstaDok.Get(DateTime.Now.Year, 15).KljucMenjaDatum.ToString();
		//        case "Kljuc Menja Datum VrDok 13":
		//            return Komercijalno.VrstaDok.Get(DateTime.Now.Year, 13).KljucMenjaDatum.ToString();
		//        case "Kljuc Menja Datum VrDok 0":
		//            return Komercijalno.VrstaDok.Get(DateTime.Now.Year, 0).KljucMenjaDatum.ToString();
		//        case "Kljuc Menja Datum VrDok 7":
		//            return Komercijalno.VrstaDok.Get(DateTime.Now.Year, 7).KljucMenjaDatum.ToString();
		//        case "Kljuc Menja Datum VrDok 32":
		//            return Komercijalno.VrstaDok.Get(DateTime.Now.Year, 32).KljucMenjaDatum.ToString();
		//        case "Izmena Menja Datum VrDok 15":
		//            return Komercijalno.VrstaDok.Get(DateTime.Now.Year, 15).IzmenaMenjaDatum.ToString();
		//        case "Izmena Menja Datum VrDok 13":
		//            return Komercijalno.VrstaDok.Get(DateTime.Now.Year, 13).IzmenaMenjaDatum.ToString();
		//        case "Izmena Menja Datum VrDok 0":
		//            return Komercijalno.VrstaDok.Get(DateTime.Now.Year, 0).IzmenaMenjaDatum.ToString();
		//        case "Izmena Menja Datum VrDok 7":
		//            return Komercijalno.VrstaDok.Get(DateTime.Now.Year, 7).IzmenaMenjaDatum.ToString();
		//        case "Izmena Menja Datum VrDok 32":
		//            return Komercijalno.VrstaDok.Get(DateTime.Now.Year, 32).IzmenaMenjaDatum.ToString();
		//        case "Pin cine samo brojevi":
		//            return Komercijalno.Parametri.Get(DateTime.Now.Year, "pin_samo_broj").Vrednost;
		//        case "Podrazumevani rok za placanje na profakturi":
		//            return Komercijalno.Parametri.Get(DateTime.Now.Year, "vazidana").Vrednost;
		//        case "Podrazumevana vrsta dokumenta":
		//            return Komercijalno.Parametri.Get(DateTime.Now.Year, "vrdok").Vrednost;
		//        case "Podrazumevani magacin":
		//            return Komercijalno.Parametri.Get(DateTime.Now.Year, "magacinid").Vrednost;
		//        case "Zabranjeno dupliranje kombinacije katbr+katbrpro":
		//            return Komercijalno.Parametri.Get(DateTime.Now.Year, "duplirankb_kbp").Vrednost;
		//        case "Maksim. datum ili broj dana za izmenu obj. (fin)":
		//            return Komercijalno.Parametri.Get(DateTime.Now.Year, "maxdatilibrojdana_0").Vrednost;
		//        default:
		//            throw new Exception("Nepoznat parametar!");
		//    }
		//}
		//private static void SetVrednostKaoNaSablonu(string parametar, string vrednost)
		//{
		//    Komercijalno.Magacin m = null;
		//    Komercijalno.VrstaDok v = null;
		//    Komercijalno.Parametri p = null;
		//    switch (parametar)
		//    {
		//        case "Magacin 12 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 12);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 13 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 13);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 14 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 14);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 15 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 15);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 16 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 16);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 17 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 17);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 18 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 18);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 19 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 19);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 20 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 20);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 21 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 21);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 22 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 22);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 23 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 23);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 24 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 24);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 25 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 25);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 26 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 26);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 27 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 27);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 28 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 28);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Magacin 50 moze da ide u minus":
		//            m = Komercijalno.Magacin.Get(DateTime.Now.Year, 50);
		//            m.MozeMinus = Convert.ToInt32(vrednost);
		//            m.Update(DateTime.Now.Year);
		//            return;
		//        case "Kljuc Menja Datum VrDok 15":
		//            v = Komercijalno.VrstaDok.Get(DateTime.Now.Year, 15);
		//            v.KljucMenjaDatum = Convert.ToInt32(vrednost);
		//            v.Update(DateTime.Now.Year);
		//            return;
		//        case "Kljuc Menja Datum VrDok 13":
		//            v = Komercijalno.VrstaDok.Get(DateTime.Now.Year, 13);
		//            v.KljucMenjaDatum = Convert.ToInt32(vrednost);
		//            v.Update(DateTime.Now.Year);
		//            return;
		//        case "Kljuc Menja Datum VrDok 0":
		//            v = Komercijalno.VrstaDok.Get(DateTime.Now.Year, 0);
		//            v.KljucMenjaDatum = Convert.ToInt32(vrednost);
		//            v.Update(DateTime.Now.Year);
		//            return;
		//        case "Kljuc Menja Datum VrDok 7":
		//            v = Komercijalno.VrstaDok.Get(DateTime.Now.Year, 7);
		//            v.KljucMenjaDatum = Convert.ToInt32(vrednost);
		//            v.Update(DateTime.Now.Year);
		//            return;
		//        case "Kljuc Menja Datum VrDok 32":
		//            v = Komercijalno.VrstaDok.Get(DateTime.Now.Year, 32);
		//            v.KljucMenjaDatum = Convert.ToInt32(vrednost);
		//            v.Update(DateTime.Now.Year);
		//            return;
		//        case "Izmena Menja Datum VrDok 15":
		//            v = Komercijalno.VrstaDok.Get(DateTime.Now.Year, 15);
		//            v.IzmenaMenjaDatum = Convert.ToInt32(vrednost);
		//            v.Update(DateTime.Now.Year);
		//            return;
		//        case "Izmena Menja Datum VrDok 13":
		//            v = Komercijalno.VrstaDok.Get(DateTime.Now.Year, 13);
		//            v.IzmenaMenjaDatum = Convert.ToInt32(vrednost);
		//            v.Update(DateTime.Now.Year);
		//            return;
		//        case "Izmena Menja Datum VrDok 0":
		//            v = Komercijalno.VrstaDok.Get(DateTime.Now.Year, 0);
		//            v.IzmenaMenjaDatum = Convert.ToInt32(vrednost);
		//            v.Update(DateTime.Now.Year);
		//            return;
		//        case "Izmena Menja Datum VrDok 7":
		//            v = Komercijalno.VrstaDok.Get(DateTime.Now.Year, 7);
		//            v.IzmenaMenjaDatum = Convert.ToInt32(vrednost);
		//            v.Update(DateTime.Now.Year);
		//            return;
		//        case "Izmena Menja Datum VrDok 32":
		//            v = Komercijalno.VrstaDok.Get(DateTime.Now.Year, 32);
		//            v.IzmenaMenjaDatum = Convert.ToInt32(vrednost);
		//            v.Update(DateTime.Now.Year);
		//            return;
		//        case "Pin cine samo brojevi":
		//            p = Komercijalno.Parametri.Get(DateTime.Now.Year, "pin_samo_broj");
		//            p.Vrednost = vrednost.ToString();
		//            p.Update(DateTime.Now.Year);
		//            return;
		//        case "Podrazumevani rok za placanje na profakturi":
		//            p = Komercijalno.Parametri.Get(DateTime.Now.Year, "vazidana");
		//            p.Vrednost = vrednost.ToString();
		//            p.Update(DateTime.Now.Year);
		//            return;
		//        case "Podrazumevana vrsta dokumenta":
		//            p = Komercijalno.Parametri.Get(DateTime.Now.Year, "vrdok");
		//            p.Vrednost = vrednost.ToString();
		//            p.Update(DateTime.Now.Year);
		//            return;
		//        case "Podrazumevani magacin":
		//            p = Komercijalno.Parametri.Get(DateTime.Now.Year, "magacinid");
		//            p.Vrednost = vrednost.ToString();
		//            p.Update(DateTime.Now.Year);
		//            return;
		//        case "Zabranjeno dupliranje kombinacije katbr+katbrpro":
		//            p = Komercijalno.Parametri.Get(DateTime.Now.Year, "duplirankb_kbp");
		//            p.Vrednost = vrednost.ToString();
		//            p.Update(DateTime.Now.Year);
		//            return;
		//        case "Maksim. datum ili broj dana za izmenu obj. (fin)":
		//            p = Komercijalno.Parametri.Get(DateTime.Now.Year, "maxdatilibrojdana_0");
		//            p.Vrednost = vrednost.ToString();
		//            p.Update(DateTime.Now.Year);
		//            return;
		//        default:
		//            throw new Exception("Nepoznat parametar!");
		//    }
		//}
		//public static void PodesiVrednostiParametaraPoSablonu(List<DTO.KomecijalnoPodesiVrednostiParametaraPoSablonuDTO> parametri)
		//{
		//    foreach (DTO.KomecijalnoPodesiVrednostiParametaraPoSablonuDTO r in parametri)
		//        if (!string.IsNullOrWhiteSpace(r.VrednostParametra))
		//            SetVrednostKaoNaSablonu(r.NazivParametra, r.VrednostParametra);
		//}
		private void button2_Click(object sender, EventArgs e)
		{
			//    TDOffice.Config<Dictionary<string, string>> sablon = TDOffice.Config<Dictionary<string, string>>.Get(TDOffice.ConfigParameter.KomercijalnoParametriSablon);
			//    foreach(DataGridViewRow r in dataGridView1.Rows)
			//    {
			//        string parametar = r.Cells["Parametar"].Value.ToString();
			//        string vrednost = r.Cells["Sablonski"].Value.ToString();

			//        if (sablon.Tag == null)
			//            sablon.Tag = new Dictionary<string, string>();

			//        if (!sablon.Tag.ContainsKey(parametar))
			//            sablon.Tag[parametar] = vrednost;

			//        sablon.Tag[parametar] = vrednost;
			//    }

			//    sablon.UpdateOrInsert();
			//    MessageBox.Show("Izmene na sablonu uspesno sacuvane!");
		}

		private void button1_Click(object sender, EventArgs e)
		{
			//    List<DTO.KomecijalnoPodesiVrednostiParametaraPoSablonuDTO> list = new List<DTO.KomecijalnoPodesiVrednostiParametaraPoSablonuDTO>();
			//    foreach (DataGridViewRow r in dataGridView1.Rows)
			//        list.Add(new DTO.KomecijalnoPodesiVrednostiParametaraPoSablonuDTO(r.Cells["Parametar"].Value.ToString(), r.Cells["Sablonski"].Value.ToString()));

			//    PodesiVrednostiParametaraPoSablonu(list);
			//    MessageBox.Show("Parametri sablona uspesno primenjeni!");
			//    UcitajParametre();
		}
	}
}
