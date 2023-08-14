using FirebirdSql.Data.FirebirdClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Forms.Menadzment;
using TDOffice_v2.Komercijalno;

namespace TDOffice_v2
{
    public partial class Menadzment : Form
    {
        public Menadzment()
        {
            InitializeComponent();
        }

        private void fakturisi5034ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Menadzment_Fakturisi0634_List f = new Menadzment_Fakturisi0634_List())
                f.ShowDialog();
        }

        private void razduziLager0634ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Menadzment_RazduziLager0634_Index r = new Menadzment_RazduziLager0634_Index())
                r.ShowDialog();
        }

        private void lager0634ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Menadzment_Lager0634_Index r = new Menadzment_Lager0634_Index())
                r.ShowDialog();
        }

        private void svediStanje1934ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Menadzment_SvediStanje1934_Index i = new Menadzment_SvediStanje1934_Index())
                i.ShowDialog();
        }

        private void svediPocetnoStanjeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            using (Menadzment_SvediPocetnoStanje_Index i = new Menadzment_SvediPocetnoStanje_Index())
                i.ShowDialog();
        }

        private void svediCenePocetnogStanjaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Mendazment_SvediPocetnoStanjeCene_Index i = new Mendazment_SvediPocetnoStanjeCene_Index())
                i.ShowDialog();
        }

        private void tempAkcijaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AnimationBox ab = AnimationBox.Show("Brisanje roba sa **** koja je neaktivna je u toku!");

            MessageBox.Show("Postoji i aniamtion box! (ne radi nista ova akcija)");
            ab.Close();
            return;
            Task.Run(() =>
            {
                List<int> neuspeli = new List<int>();
                List<Komercijalno.Roba> roba = Komercijalno.Roba.List(2021);

                roba.RemoveAll(x => x.Naziv.Length > 4);
                roba.RemoveAll(x => !x.Naziv.Contains("****"));

                var lista1 = Komercijalno.Stavka.ListAsync(2021, "KOLICINA <> 0");
                var lista2 = Komercijalno.Stavka.ListAsync(2022, "KOLICINA <> 0");

                for (int y = roba.Count - 1; y >= 0; y--)
                    if (lista1.Result.Any(x => x.RobaID == roba[y].ID))
                        roba.RemoveAt(y);

                for (int y = roba.Count - 1; y >= 0; y--)
                    if (lista2.Result.Any(x => x.RobaID == roba[y].ID))
                        roba.RemoveAt(y);

                using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[2022]))
                {
                    con.Open();
                    using (FbCommand cmd = new FbCommand("DELETE FROM STAVKA WHERE ROBAID = @RID", con))
                    {
                        cmd.Parameters.Add("@RID", FbDbType.Integer);

                        foreach (Komercijalno.Roba r in roba)
                        {
                            try
                            {
                                cmd.Parameters["@RID"].Value = r.ID;
                                cmd.ExecuteNonQuery();

                                Komercijalno.Roba.Delete(con, r.ID);
                            }
                            catch
                            {
                                neuspeli.Add(r.ID);
                            }
                        }
                    }
                }

                ab.Dispose();

                MessageBox.Show("Gotovo!");
                MessageBox.Show("Neuspeli: " + String.Join(Environment.NewLine, neuspeli));
            });
        }

        private void svediRazlikuMPRacunaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (Menadzment_SvediRazlikuMPRacuna_Index sr = new Menadzment_SvediRazlikuMPRacuna_Index())
                sr.ShowDialog();
        }

        private void uporedjivanjeProdajnihCenaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (fm_UporedjivanjeProdajnihCena_Index u = new fm_UporedjivanjeProdajnihCena_Index())
                u.ShowDialog();
        }

        bool block = true;

        private void svediKolicinePocetnogStanjaNaMinimalnoMoguceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (block)
            {
                MessageBox.Show("Ovu akciju ne treba pokretati jer se treba prebaciti u poseban prozor! Da bi je sada pokrenuli, " +
                    "ukoliko znate sta radite, pokrenite akciju ponovo i ova poruka se nece prikazati vec ce se akcija startovati!");
                block = false;
                return;
            }

            block = true;
            string zakucaniConnString = "data source=4monitor; initial catalog = c:\\poslovanje\\baze\\2023\\TERMODOM2023.FDB; user=SYSDBA; password=m; pooling=True";

            using (FbConnection con = new FbConnection(zakucaniConnString))
            {
                con.Open();

                int magacinID = 50;
                int brDokPocetnogStanja = 25;

                var robaUMagacinu = RobaUMagacinu.ListByMagacinID(con, magacinID);
                var stavkePocetnogStanja = Stavka.ListByDokument(con, 0, brDokPocetnogStanja);
                var roba = Roba.List(con);

                // Sredjivanje kartica u magacinu
                foreach (var rum in robaUMagacinu)
                    Procedure.SrediKarticu(con, magacinID, rum.RobaID, DateTime.Now);

                Dictionary<int, double> minTrenStanje = new Dictionary<int, double>();
                using (FbCommand cmd = new FbCommand(@"SELECT ROBAID, MIN(TREN_STANJE) AS MIN_STANJE FROM STAVKA
                    WHERE MAGACINID = @MID
                    AND VRDOK IN (
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
            992,
            13,
            14,
            15,
            17,
            19,
            23,
            28,
            29,
            35,
            993)
                    GROUP BY(ROBAID) ORDER BY MIN_STANJE ASC
                    ", con))
                {
                    cmd.Parameters.AddWithValue("@MID", magacinID);

                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            minTrenStanje.Add(Convert.ToInt32(dr[0]), Convert.ToDouble(dr[1]));
                }

                foreach (int key in minTrenStanje.Keys)
                {
                    var r = roba.FirstOrDefault(x => x.ID == key);
                    var k = r.Naziv;
                    var stavka = stavkePocetnogStanja.FirstOrDefault(x => x.RobaID == key);
                    var minTS = minTrenStanje[key];
                    if (stavka == null || stavka.Kolicina <= 0 || minTS <= 0)
                        continue;

                    var buducaKol = stavka.Kolicina - minTS;
                    stavka.Kolicina = buducaKol < 0 ? 0 : buducaKol;
                    stavka.Update(con);
                }

                MessageBox.Show("Gotovo!");
            }
        }

        private void kopirajPodatkeTabeleSTAVKAIzDokumentaUDokumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                using (fm_KopirajPodatkeTabeleSTAVKAIzDokumentaUDokument k = new fm_KopirajPodatkeTabeleSTAVKAIzDokumentaUDokument())
                    k.ShowDialog();
            });
        }

        private void fakturisi5034ToolStripMenuItem_Click_1(object sender, EventArgs e)
        {

        }

        private void razduzenjeMagacinaPoOtpremnicamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                using (fm_Menadzment_RazduzenjeMagacinaPoOtpremnicama i = new fm_Menadzment_RazduzenjeMagacinaPoOtpremnicama())
                    i.ShowDialog();
            });
        }

        private void prenosRobeDopunaToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (fm_PrenosRobeDopuna_Index p = new fm_PrenosRobeDopuna_Index())
                p.ShowDialog();
        }

        private void svediPocetnoStanjeToolStripMenuItem1_Click_1(object sender, EventArgs e)
        {

        }

        private void svediKolicinePocetnogStanjaNaMinimalnoMoguceToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            using (Menadzment_SvediPocetnoStanje_Index i = new Menadzment_SvediPocetnoStanje_Index())
                i.ShowDialog();
        }
    }
}