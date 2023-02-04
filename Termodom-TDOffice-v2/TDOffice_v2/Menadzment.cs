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

                using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[2022]))
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

        private void prenosRobeDopunaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (fm_PrenosRobeDopuna_Index p = new fm_PrenosRobeDopuna_Index())
                p.ShowDialog();
        }
    }
}