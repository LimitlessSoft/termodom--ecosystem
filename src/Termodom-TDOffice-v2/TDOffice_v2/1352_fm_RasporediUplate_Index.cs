using FirebirdSql.Data.FirebirdClient;
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

namespace TDOffice_v2
{
    public partial class _1352_fm_RasporediUplate_Index : Form
    {
        public _1352_fm_RasporediUplate_Index()
        {
            MessageBox.Show("Ovaj modul ne radi! Kontaktiraj administratora!");
            return;

            InitializeComponent();
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            //if(comboBox1.SelectedItem == null || !new string[] { "2021", "2020", "2019", "2018", "2017" }.Contains(comboBox1.SelectedItem.ToString()))
            //{
            //    MessageBox.Show("Neispravna godina!");
            //    return;
            //}

            //string godina = comboBox1.SelectedItem.ToString();

            //using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[Convert.ToInt32(godina)]))
            //{
            //    con.Open();

            //    List<IzvodManager> izvodi = IzvodManager.List(con).Where(x => x.VrDok == 90).ToList();
            //    List<IstUpl> istorijeUplata = IstUpl.List(con);

            //    // Prvo proveravam da li ima nepravailno rasporedjenih upalata
            //    // To podrazumeva da je rasporedjeno vise nego sto je raspolozivo
            //    // Ukoliko je to slucaj, ne dozovolajvam dalje akciju dok se ovo ne sredi!
            //    List<string> nepravilniLog = new List<string>();
            //    foreach (IzvodManager i in izvodi)
            //        if (i.RDuguje > i.Duguje || i.RPotrazuje > i.Potrazuje)
            //            nepravilniLog.Add(string.Format("VrDok: {0} - BrDok: {1}", i.VrDok, i.BrDok));

            //    if (nepravilniLog.Count > 0)
            //    {
            //        MessageBox.Show("Imate nepravilo rasporedjenih uplata. Prvo to resite rucno pa mozemo nastaivit dalje!");
            //        MessageBox.Show(string.Join(Environment.NewLine, nepravilniLog));
            //        return;
            //    }

            //    List<Dokument> dokumentiIzvoda = Dokument.ListByVrDok(con, 90);
            //    List<Partner> partneri = await Partner.ListAsync();

            //    foreach (IzvodManager i in izvodi.Where(x => !new string[] { "165" }.Contains(x.SifraPlac)))
            //    {
            //        if (i.RDuguje < i.Duguje || i.RPotrazuje < i.Potrazuje)
            //        {
            //            Partner part = partneri.Where(x => x.PPID == i.PPID).FirstOrDefault();
            //            DialogResult dr = MessageBox.Show("Da li zelite da povezem uplate za partnera (" + i.PPID + ") " + part == null ? "nepoznat" : part.Naziv + " po izvodu " + i.BrDok + ".\nZa prekidanje cele poeracije pritisnite CANCEL!", "Potvrdi", MessageBoxButtons.YesNoCancel);
            //            if (dr == DialogResult.Cancel)
            //            {
            //                MessageBox.Show("Operacija prekinuta!");
            //                return;
            //            }

            //            if (dr == DialogResult.No)
            //                continue;

            //            List<IstUpl> istorijeZaOvajIzvodID = istorijeUplata.Where(x => x.PromenaID == i.IzvodID).ToList();
            //            List<IstUpl> nerasporedjeneUplatezaOvajIzvodID = istorijeZaOvajIzvodID.Where(x => x.VrDok == 91).ToList();
            //            Dokument izvod = dokumentiIzvoda.Where(x => x.BrDok == i.BrDok).FirstOrDefault();

            //            int maxNerasporedjenaUplataID = nerasporedjeneUplatezaOvajIzvodID.Count > 0 ? nerasporedjeneUplatezaOvajIzvodID.Max(x => x.BrDok) : 0;

            //            double razlikaDuguje = i.Duguje - i.RDuguje;
            //            double razlikaPotrazuje = i.Potrazuje - i.RPotrazuje;

            //            if (razlikaDuguje > 0)
            //            {
            //                maxNerasporedjenaUplataID++;
            //                IstUpl.Insert(con, 89, maxNerasporedjenaUplataID,
            //                    razlikaDuguje, "Izvod br. [" + izvod.BrDok + "] " +
            //                    izvod.IntBroj + " (isplata: " + razlikaDuguje.ToString("#,##0.00") + ")",
            //                    i.PPID, TDOffice.TDOffice.PPID, 1, 1, -5, 1, "111", "DIN", 1, i.IzvodID, 0, null, null, null);

            //                using (FbCommand cmd = new FbCommand("UPDATE IZVOD SET RPOTRAZUJE = POTRAZUJE WHERE IZVODID = @ID", con))
            //                {
            //                    cmd.Parameters.AddWithValue("@ID", i.IzvodID);
            //                    cmd.ExecuteNonQuery();
            //                }
            //            }

            //            if (razlikaPotrazuje > 0)
            //            {
            //                maxNerasporedjenaUplataID++;
            //                IstUpl.Insert(con, 91, maxNerasporedjenaUplataID,
            //                    razlikaPotrazuje, "Izvod br. [" + izvod.BrDok + "] " +
            //                    izvod.IntBroj + " (uplata: " + razlikaPotrazuje.ToString("#,##0.00") + ")",
            //                    i.PPID, TDOffice.TDOffice.PPID, 0, 1, -5, 1, "111", "DIN", 1, i.IzvodID, 0, null, null, null);

            //                using (FbCommand cmd = new FbCommand("UPDATE IZVOD SET RDUGUJE = DUGUJE WHERE IZVODID = @ID", con))
            //                {
            //                    cmd.Parameters.AddWithValue("@ID", i.IzvodID);
            //                    cmd.ExecuteNonQuery();
            //                }
            //            }
            //        }
            //    }
            //    MessageBox.Show("Gotovo!");
            //}

        }
    }
}
