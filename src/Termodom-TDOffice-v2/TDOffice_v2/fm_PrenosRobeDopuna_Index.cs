using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;
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
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
    public partial class fm_PrenosRobeDopuna_Index : Form
    {
        Komercijalno.Magacin.MagacinCollection magaciniCollection { get; set; }
        public fm_PrenosRobeDopuna_Index()
        {
            InitializeComponent();
            ToggleUI(false);
        }

        private void fm_PrenosRobeDopuna_Index_Load(object sender, EventArgs e)
        {
            UcitajMagacineAsync().ContinueWith((copmletedTask) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    ToggleUI(true);
                });
            });
        }

        private async Task UcitajMagacineAsync()
        {
            magacin_cmb.Enabled = false;
            magaciniCollection = await Komercijalno.Magacin.CollectionAsync();
            var list = magaciniCollection.ToList();

            magacin_cmb.DataSource = list;
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.ValueMember = "ID";

            magacin_cmb.Enabled = true;
        }

        private void ToggleUI(bool state)
        {
            button1.Enabled = state;
            destinacioniBrDok_txt.Enabled = state;
            destinacioniVrDok_txt.Enabled = state;
            magacin_cmb.Enabled = state;
        }

        private void button1_ClickAsync(object sender, EventArgs e)
        {
            if (checkBox1.Checked && checkBox2.Checked)
            {
                MessageBox.Show("Ne mozete selektovati obe akcije u isto vreme!");
            }
            if (checkBox1.Checked)
            {
                DopunaRobePoProdaji();
            }
            else if (checkBox2.Checked)
            {
                DopunaRobePoStanjuAsync();
            }
            else
            {
                MessageBox.Show("Morate selektovati barem jednu akciju!");
            }
        }

        private void DopunaRobePoProdaji()
        {
            try
            {
                DateTime odDatuma = odDatuma_dtp.Value;
                DateTime doDatuma = doDatuma_dtp.Value;
                int magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
                int destinacioniVrDok;
                int destinacioniBrDok;

                if (!int.TryParse(destinacioniVrDok_txt.Text, out destinacioniVrDok))
                {
                    MessageBox.Show("Destinaciona vrsta dokumenta nije ispravna!");
                    return;
                }

                if (!int.TryParse(destinacioniBrDok_txt.Text, out destinacioniBrDok))
                {
                    MessageBox.Show("Destinacioni broj dokumenta nije ispravan!");
                    return;
                }

                Dictionary<int, double> izlazRobePoRobaIDZaPeriod = new Dictionary<int, double>();
                //using (FbConnection con = new FbConnection("data source=4monitor; initial catalog = e:\\4monitor\\poslovanje\\baze\\2023\\FRANSIZA2023TCMD.FDB; user=SYSDBA; password=masterkey; pooling=True"))
                using (FbConnection con = new FbConnection("data source=4monitor; initial catalog = c:\\poslovanje\\baze\\2023\\FRANSIZA2023TCMD.FDB; user=SYSDBA; password=m; pooling=True"))
                {
                    con.Open();
                    using (FbCommand cmd = new FbCommand($"select S.ROBAID, SUM(S.KOLICINA) FROM STAVKA S LEFT OUTER JOIN DOKUMENT D ON D.VRDOK = S.VRDOK AND D.BRDOK = S.BRDOK WHERE S.VRDOK IN (15, 19) AND S.MAGACINID = {magacinID} AND D.DATUM >= '{odDatuma.Date.ToString("dd.MM.yyyy")}' AND " +
                        $"D.DATUM <= '{doDatuma.Date.ToString("dd.MM.yyyy")}' GROUP BY S.ROBAID", con))
                    using (FbDataReader dr = cmd.ExecuteReader())
                        while (dr.Read())
                            izlazRobePoRobaIDZaPeriod.Add(Convert.ToInt32(dr[0]), Convert.ToDouble(dr[1]));
                }

                //using (FbConnection con = new FbConnection("data source=4monitor; initial catalog = e:\\4monitor\\poslovanje\\baze\\2023\\TERMODOM2023.FDB; user=SYSDBA; password=masterkey; pooling=True"))
                using (FbConnection con = new FbConnection("data source=4monitor; initial catalog = c:\\poslovanje\\baze\\2023\\TERMODOM2023.FDB; user=SYSDBA; password=m; pooling=True"))
                {
                    con.Open();

                    var destinacioniDokument = Komercijalno.Dokument.Get(con, destinacioniVrDok, destinacioniBrDok);
                    var roba = Komercijalno.Roba.List(con);
                    var robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinID(con, magacinID);

                    if (destinacioniDokument == null)
                    {
                        MessageBox.Show($"Dokument {destinacioniVrDok}, {destinacioniBrDok} nije pronadjen!");
                        return;
                    }

                    if (destinacioniDokument.Flag != 0)
                    {
                        MessageBox.Show("Destinacioni dokument mora biti otkljucan!");
                        return;
                    }

                    using (FbCommand cmd = new FbCommand($"SELECT COUNT(VRDOK) FROM STAVKA WHERE VRDOK = {destinacioniVrDok} AND BRDOK = {destinacioniBrDok}", con))
                    {
                        using (FbDataReader dr = cmd.ExecuteReader())
                        {
                            if (!dr.Read())
                            {
                                MessageBox.Show("Greska 289412y49");
                                return;
                            }

                            if (Convert.ToInt32(dr[0]) > 0)
                            {
                                MessageBox.Show("Destinacioni dokument mora biti prazan!");
                                return;
                            }
                        }
                    }

                    foreach (int key in izlazRobePoRobaIDZaPeriod.Keys)
                    {
                        var r = roba.FirstOrDefault(x => x.ID == key);

                        if (r == null)
                        {
                            MessageBox.Show($"Greska ucitavanja Robe {key} prilikom inserta u dokument, preskacem i nastavljam dalje!");
                            continue;
                        }

                        var rum = robaUMagacinu.FirstOrDefault(x => x.RobaID == key);
                        if (rum == null)
                        {
                            MessageBox.Show($"Greska ucitavanja RobeUMagacinu {key} prilikom inserta u dokument, preskacem i nastavljam dalje!");
                            continue;
                        }

                        Komercijalno.Stavka.Insert(con, destinacioniDokument, r, rum, izlazRobePoRobaIDZaPeriod[key], 0);
                    }
                }

                MessageBox.Show("Gotovo!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DopunaRobePoStanjuAsync()
        {
            try
            {
                DateTime doDatuma = doDatuma_dtp.Value;
                int magacinID = Convert.ToInt32(magacin_cmb.SelectedValue);
                int destinacioniVrDok;
                int destinacioniBrDok;

                if (!int.TryParse(destinacioniVrDok_txt.Text, out destinacioniVrDok))
                {
                    MessageBox.Show("Destinaciona vrsta dokumenta nije ispravna!");
                    return;
                }

                if (!int.TryParse(destinacioniBrDok_txt.Text, out destinacioniBrDok))
                {
                    MessageBox.Show("Destinacioni broj dokumenta nije ispravan!");
                    return;
                }

                using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
                {
                    con.Open();

                    var robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinID(con, magacinID);
                    var destinacioniDokument = Komercijalno.Dokument.Get(con, destinacioniVrDok, destinacioniBrDok);
                    var roba = Komercijalno.Roba.List(con);

                    if (destinacioniDokument == null)
                    {
                        MessageBox.Show($"Dokument {destinacioniVrDok}, {destinacioniBrDok} nije pronadjen!");
                        return;
                    }

                    if (destinacioniDokument.Flag != 0)
                    {
                        MessageBox.Show("Destinacioni dokument mora biti otkljucan!");
                        return;
                    }

                    foreach (var rum in robaUMagacinu)
                    {
                        Procedure.SrediKarticu(con, magacinID, rum.RobaID, DateTime.Now.AddYears(-1));
                    }

                    var stavkeDokumenta = Komercijalno.Stavka.ListByDokument(con, destinacioniVrDok, destinacioniBrDok);

                    var stavke = Komercijalno.Stavka.ListByMagacinID(con, magacinID);
                    foreach (var rum in robaUMagacinu)
                    {
                        var stavkeRobe = stavke.Where(x => x.RobaID == rum.RobaID && x.TrenStanje != null).ToList();
                        if (stavkeRobe.Count == 0)
                            continue;

                        double kolicina = stavkeRobe.Min(x => (double)x.TrenStanje);

                        if (kolicina < 0)
                        {
                            var stavkaDokumenta = stavkeDokumenta.FirstOrDefault(x => x.RobaID == rum.RobaID);

                            if(stavkaDokumenta == null)
                                Komercijalno.Stavka.Insert(con, destinacioniDokument, roba.First(x => x.ID == rum.RobaID), rum, Math.Abs(kolicina), 0);
                            else
                            {
                                stavkaDokumenta.Kolicina += Math.Abs(kolicina);
                                stavkaDokumenta.Update(con);
                            }
                        }
                    }

                    MessageBox.Show("Gotovo!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
