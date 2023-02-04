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
    public partial class _7_fm_TDPopis_List : Form
    {
        private List<Komercijalno.Magacin> magacini = new List<Komercijalno.Magacin>();
        private DataTable _sviDokumenti;
        private bool _loaded = false;
        private Task<fm_Help> _helpFrom { get; set; }
        public _7_fm_TDPopis_List()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.Popis_List);

            panel2.DesniKlik_DatumRange(odDatuma_dtp_CloseUp);

            magacini = Komercijalno.Magacin.ListAsync().Result;
            magacini.Add(new Komercijalno.Magacin() {
                ID = -1,
                Naziv = "Svi magacini"
            });

            magacin_cmb.DataSource = magacini.OrderBy(x => x.ID).ToList();
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.ValueMember = "ID";

            if (Program.TrenutniKorisnik.ImaPravo(700004))
            {
                magacin_cmb.SelectedValue = -1;
                magacin_cmb.Enabled = true;
            }
            else
            {
                magacin_cmb.SelectedValue = Program.TrenutniKorisnik.MagacinID;
                magacin_cmb.Enabled = false;
            }

            odDatuma_dtp.Value = DateTime.Now;
            doDatuma_dtp.Value = DateTime.Now;

            PopulateDT();

            tipPopisa_cmb.SelectedIndex = 0;

            _loaded = true;
        }
        private void PopulateDT()
        {
            List<TDOffice.DokumentPopis> popisi = Program.TrenutniKorisnik.ImaPravo(700004) ? TDOffice.DokumentPopis.List() : TDOffice.DokumentPopis.List().Where(x => x.MagacinID == Program.TrenutniKorisnik.MagacinID).ToList();

            DataTable dt = new DataTable();

            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Date", typeof(DateTime)));
            dt.Columns.Add(new DataColumn("Type", typeof(int)));
            dt.Columns.Add(new DataColumn("MagacinID", typeof(int)));
            dt.Columns.Add(new DataColumn("Magacin", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(int)));
            dt.Columns.Add(new DataColumn("tipPopisa", typeof(int)));

            foreach (TDOffice.DokumentPopis p in popisi)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = p.ID;
                dr["Date"] = p.Datum;
                dr["MagacinID"] = p.MagacinID;
                Komercijalno.Magacin mag = magacini.Where(x => x.ID == p.MagacinID).FirstOrDefault();
                dr["Magacin"] = p.MagacinID.ToString() + " - " + (mag == null ? "unknown" : mag.Naziv);
                dr["Status"] = (int)p.Status;
                dr["tipPopisa"] = (int)p.Tip;
                dt.Rows.Add(dr);
            }

            _sviDokumenti = dt;
        }
        private void RefreshDGV()
        {
            DateTime pocetak = odDatuma_dtp.Value;
            DateTime kraj = doDatuma_dtp.Value;

            string _sqlWhere = "DATE > '" + new DateTime(pocetak.Year, pocetak.Month, pocetak.Day, 0, 0, 0) + "' AND DATE < '" + new DateTime(kraj.Year, kraj.Month, kraj.Day, 23, 59, 59) + "'";
            if (magacin_cmb.SelectedValue != null && Convert.ToInt32(magacin_cmb.SelectedValue) != -1)
                _sqlWhere += " AND MAGACINID = '" + Convert.ToInt32(magacin_cmb.SelectedValue) + "'";

            if ((int)tipPopisa_cmb.SelectedIndex > 0)
                _sqlWhere += " AND tipPopisa = '" + ((int)tipPopisa_cmb.SelectedIndex - 1) + "'";

            string _sqlOrder = "ID ASC";

            DataRow[] rows = _sviDokumenti.Select(_sqlWhere, _sqlOrder);
            if (rows != null && rows.Length > 0)
            {
                dataGridView1.DataSource = rows.CopyToDataTable();
                dataGridView1.Columns["ID"].HeaderText = "Broj Dok.";
                dataGridView1.Columns["Date"].HeaderText = "Datum";
                dataGridView1.Columns["Type"].Visible = false;
                dataGridView1.Columns["MagacinID"].Visible = false;
                dataGridView1.Columns["Date"].DefaultCellStyle.Format = "dd.MM.yyyy ( HH:mm )";
                dataGridView1.Columns["Magacin"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dataGridView1.Columns["Date"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns["ID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                dataGridView1.Columns["Status"].Visible = false;
                dataGridView1.Columns["tipPopisa"].Visible = false;
            }
            else
                dataGridView1.DataSource = null;
            foreach (DataGridViewRow r in dataGridView1.Rows)
            {
                r.DefaultCellStyle.Font = new Font("Arial", 8, FontStyle.Bold);
                if (Convert.ToInt32(r.Cells["Status"].Value) == 0)
                {
                    r.DefaultCellStyle.ForeColor = Color.Green;
                }
                else
                {
                    r.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
            slogova_lbl.Text = "Slogova: " + dataGridView1.Rows.Count;
        }
        private void nova_btn_Click(object sender, EventArgs e)
        {
            if(!Program.TrenutniKorisnik.ImaPravo(700001))
            {
                TDOffice.Pravo.NematePravoObavestenje(700001);
                return;
            }

            int popisID = -1;
            using (_7_fm_TDPopis_Choice choice = new _7_fm_TDPopis_Choice((PopisChoiceResponseArgs args) =>
            {
                popisID = args.popis.ID;
            }))
            choice.ShowDialog();

            if (popisID <= 0)
            {
                MessageBox.Show("Doslo je do greske prilikom kreiranja popisa!");
                return;
            }

            Task.Run(() =>
            {
                using (_7_fm_TDPopis_Index pl = new _7_fm_TDPopis_Index(TDOffice.DokumentPopis.Get(popisID)))
                    pl.ShowDialog();

                GC.Collect();
            });

            PopulateDT();
            RefreshDGV();
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int brDok = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);

            Task.Run(() =>
            {
                using (_7_fm_TDPopis_Index p = new _7_fm_TDPopis_Index(TDOffice.DokumentPopis.Get(brDok)))
                    p.ShowDialog();

                GC.Collect();
            });
        }
        private void odDatuma_dtp_ValueChanged(object sender, EventArgs e)
        {
            
        }
        private void magacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            RefreshDGV();
        }
        private void _7_fm_TDPopis_List_Load(object sender, EventArgs e)
        {
            RefreshDGV();
        }
        private void tipPopisa_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            RefreshDGV();
        }
        
        private void btn_KreirajPopisSaStavkama_Click(object sender, EventArgs e)
        {
            using (_7_fm_TDPopis_KreirajPopisSaStavkama kps = new _7_fm_TDPopis_KreirajPopisSaStavkama())
                kps.ShowDialog();

            PopulateDT();
            RefreshDGV();
        }

        private void odDatuma_dtp_CloseUp(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            RefreshDGV();
        }

        private void _7_fm_TDPopis_List_FormClosed(object sender, FormClosedEventArgs e)
        {
            Program.GCCollectWithDelayAsync();
        }

        private void btn_Help_Click(object sender, EventArgs e)
        {
            using (fm_Help h = new fm_Help(TDOffice.Modul.GetWithInsert((int)Modul.Popis_List)))
                h.ShowDialog();
        }

        private void kreirajIUvuciStavkePoPopisuIzKomercijalnog_btn_Click(object sender, EventArgs e)
        {
            using (InputBox ib = new InputBox("Unesite Broj Popisa Iz Komercijalnog", "Unesite Broj Dokumenta Popisa Iz Komercijalnog Poslovanja"))
            {
                ib.ShowDialog();

                int brPopisa;

                if(!Int32.TryParse(ib.returnData, out brPopisa))
                {
                    MessageBox.Show("Broj Popisa nije validan!");
                    return;
                }

                Komercijalno.Dokument popisKomercijalno = Komercijalno.Dokument.Get(DateTime.Now.Year, 7, brPopisa);

                if(popisKomercijalno == null)
                {
                    MessageBox.Show("Popis broj " + brPopisa + " nije pronadjen u komercijalnom poslovanju!");
                    return;
                }

                this.Enabled = false;
                Task.Run(() =>
                {
                    List<Komercijalno.Stavka> stavkePopisaKomercijalno = Komercijalno.Stavka.ListByDokument(DateTime.Now.Year, 7, brPopisa);

                    int noviPopisTDOffice = TDOffice.DokumentPopis.Insert(Program.TrenutniKorisnik.ID, popisKomercijalno.MagacinID, 0, null, null, TDOffice.PopisType.Vanredni, brPopisa, null);

                    int a = 0;
                    int c = stavkePopisaKomercijalno.Count;
                    foreach (Komercijalno.Stavka s in stavkePopisaKomercijalno)
                    {
                        this.Invoke((MethodInvoker)delegate
                        {
                            slogova_lbl.Text = $"{a} / {c}";
                        });
                        TDOffice.StavkaPopis.Insert(noviPopisTDOffice, s.RobaID, s.ProdCenaBP == 0 ? s.ProdajnaCena : (s.ProdCenaBP + (s.Korekcija == null ? 0 : (double)s.Korekcija)), 0, 0, 0);
                        a++;
                    }

                    this.Invoke((MethodInvoker) delegate
                    {
                        this.Enabled = true;
                        RefreshDGV();
                    });

                    MessageBox.Show("Popis uspesno kreiran i popunjen!");
                });
            }
        }
    }
}
