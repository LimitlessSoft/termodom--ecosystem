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

namespace TDOffice_v2
{
    public partial class Menadzment_SvediPocetnoStanje_Index : Form
    {
        private List<int> vrDokKojiPovecavajuStanje = new List<int>() {
            0,
            1,
            2,
            3,
            11,
            12,
            16,
            18,
            22,
            26,
            30,
            992
        };
        private List<int> vrDokKojiSmanjujuStanje = new List<int>() {
            13,
            14,
            15,
            17,
            19,
            23,
            28,
            29,
            35,
            993
        };
        public Menadzment_SvediPocetnoStanje_Index()
        {
            InitializeComponent();
        }

        private string zakucaniConnString = "data source=4monitor; initial catalog = c:\\poslovanje\\baze\\2023\\TERMODOM2023.FDB; user=SYSDBA; password=m; pooling=True";

        private void Menadzment_SvediPocetnoStanje_Index_Load(object sender, EventArgs e)
        {
            using(FbConnection con = new FbConnection(zakucaniConnString))
            {
                con.Open();
                checkedListBox1.DataSource = Komercijalno.Magacin.List(con);
                checkedListBox1.ValueMember = "ID";
                checkedListBox1.DisplayMember = "Naziv";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            Task.Run(() =>
            {
                UpdateStatusLabel("Zapoceta je akcija svodnjavanja pocetnih stanja magacina");

                using (FbConnection con = new FbConnection(zakucaniConnString))
                {
                    con.Open();
                    foreach (Komercijalno.Magacin magacin in checkedListBox1.CheckedItems.OfType<Komercijalno.Magacin>())
                    {
                        UpdateStatusLabel(magacin.Naziv + " / Ucitavam pocetno stanje");
                        Komercijalno.Dokument dokumentPocetnogStanja = Komercijalno.Dokument.List(con, "VRDOK = 0 AND MAGACINID = " + magacin.ID).FirstOrDefault();

                        UpdateStatusLabel(magacin.Naziv + " / Ucitavam stavke pocetnog stanja");
                        List<Komercijalno.Stavka> stavkePocetnogStanja = Komercijalno.Stavka.List(con, "VRDOK = 0 AND BRDOK = " + dokumentPocetnogStanja.BrDok.ToString());

                        UpdateStatusLabel(magacin.Naziv + " / Ucitavam robu u magacinu");
                        List<Komercijalno.RobaUMagacinu> robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinID(con, magacin.ID);

                        UpdateStatusLabel(magacin.Naziv + " / Zapocinjem azuriranje kolicina ...");
                        int i = 0;
                        int total = robaUMagacinu.Count;
                        foreach(Komercijalno.RobaUMagacinu rum in robaUMagacinu)
                        {
                            i++;
                            UpdateStatusLabel(magacin.Naziv + " / Azuriranje kolicina " + i + " od " + total);
                            List<Tuple<int, double>> vrdokIKolicinaIzTabeleStavkaPoredjaneRedosledomZaDatiRobaID = new List<Tuple<int, double>>();
                            using(FbCommand cmd = new FbCommand(@"SELECT D.VRDOK, S.KOLICINA FROM STAVKA S
LEFT OUTER JOIN DOKUMENT D ON S.VRDOK = D.VRDOK AND S.BRDOK = D.BRDOK
WHERE S.ROBAID = @RID AND D.VRDOK IN (" + string.Join(", ", vrDokKojiPovecavajuStanje) + string.Join(", ", vrDokKojiSmanjujuStanje) + @") AND D.MAGACINID = @MID
ORDER BY D.DATUM ASC, D.LINKED", con))
                            {
                                cmd.Parameters.AddWithValue("@MID", magacin.ID);
                                cmd.Parameters.AddWithValue("@RID", rum.RobaID);

                                using (FbDataReader dr = cmd.ExecuteReader())
                                    while (dr.Read())
                                        vrdokIKolicinaIzTabeleStavkaPoredjaneRedosledomZaDatiRobaID.Add(new Tuple<int, double>(Convert.ToInt32(dr[0]), Convert.ToDouble(dr[1])));
                            }

                            double trenutnoStanje = 0;
                            double minimalnoStanje = Int32.MaxValue;

                            foreach(Tuple<int, double> t in vrdokIKolicinaIzTabeleStavkaPoredjaneRedosledomZaDatiRobaID)
                            {
                                trenutnoStanje += vrDokKojiPovecavajuStanje.Contains(t.Item1) ? t.Item2 : t.Item2 * -1;
                                if (trenutnoStanje < minimalnoStanje)
                                    minimalnoStanje = trenutnoStanje;
                            }

                            var sps = stavkePocetnogStanja.FirstOrDefault(x => x.RobaID == rum.RobaID);
                            if (sps != null)
                            {
                                double novaKolicina = sps.Kolicina + (minimalnoStanje * -1);
                                sps.Kolicina = novaKolicina < 0 ? 0 : novaKolicina;
                                sps.Update(con);
                            }
                            else
                            {
                                Komercijalno.Stavka.Insert(con, dokumentPocetnogStanja, Komercijalno.Roba.Get(con, rum.RobaID), rum,
                                    minimalnoStanje * -1, 0);
                            }
                        }
                    }
                }

                this.Invoke((MethodInvoker) delegate
                {
                    button1.Enabled = true;
                    MessageBox.Show("Gotovo!");
                });
            });
        }

        private void UpdateStatusLabel(string message)
        {
            this.Invoke((MethodInvoker)delegate {
                label1.Text = message;
            });
        }
    }
}
