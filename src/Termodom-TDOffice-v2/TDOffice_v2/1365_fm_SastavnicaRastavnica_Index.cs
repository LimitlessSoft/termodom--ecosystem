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
    public partial class _1365_fm_SastavnicaRastavnica_Index : Form
    {
        private TDOffice.DokumentSastavnica _sastavnica { get; set; }
        private Task<fm_Help> _helpFrom { get; set; }

        private bool _loaded = false;

        public _1365_fm_SastavnicaRastavnica_Index(TDOffice.DokumentSastavnica sastavnica)
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.SastavnicaRastavnica_Index);
            _sastavnica = sastavnica;
            SetUI();
        }

        private void _1365_fm_SastavnicaRastavnica_Index_Load(object sender, EventArgs e)
        {
            ReloadData();
            _loaded = true;
        }

        private void SetUI()
        {
            List<TDOffice.DokumentSastavnica> pravilaList = TDOffice.DokumentSastavnica.ListByTip(TDOffice.Enums.DokumentSastavnicaTip.Pravilo).ToList();
            pravilaList.Add(new TDOffice.DokumentSastavnica() { ID = -1, Tag = "< Izaberi pravilo > " });
            pravilaList.Sort((x,y) =>  x.ID.CompareTo(y.ID));

            pravilaSastavnica_cmb.DisplayMember = "Tag";
            pravilaSastavnica_cmb.ValueMember = "ID";
            pravilaSastavnica_cmb.DataSource = pravilaList;

            this.BackColor = _sastavnica.Status == TDOffice.DokumentStatus.Zakljucan ? Color.Red : Color.Green;
        }
        private void ReloadData()
        {
            int id;
            if (_sastavnica.Status == TDOffice.DokumentStatus.Zakljucan)
            {
                id = _sastavnica.ID;
                tb_BrDokKomerc.Text = _sastavnica.BrDokKom.ToStringOrDefault();
            }
            else
            {
                 id = Convert.ToInt32(pravilaSastavnica_cmb.SelectedValue);
            }
            
            if (id < 0)
                return;

            List<TDOffice.StavkaSastavnica> stavkeSastavnice = TDOffice.StavkaSastavnica.ListBySastavnicaID(id);

            List<Komercijalno.Roba> svaRoba = Komercijalno.Roba.List(DateTime.Now.Year);

            // Smanjuje se
            DataTable dt = new DataTable();

            // Povecava se
            DataTable dt1 = new DataTable();

            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("ROBAID", typeof(int));
            dt.Columns.Add("Naziv", typeof(string));
            dt.Columns.Add("KatBr", typeof(string));
            dt.Columns.Add("KOLICINA", typeof(double));
            dt.Columns.Add("JM", typeof(string));

            dt1.Columns.Add("ID", typeof(int));
            dt1.Columns.Add("ROBAID", typeof(int));
            dt1.Columns.Add("Naziv", typeof(string));
            dt1.Columns.Add("KatBr", typeof(string));
            dt1.Columns.Add("KOLICINA", typeof(double));
            dt1.Columns.Add("JM", typeof(string));

            foreach (TDOffice.StavkaSastavnica ss in stavkeSastavnice)
            {
                Komercijalno.Roba roba = svaRoba.Where(x => x.ID == ss.RobaID).FirstOrDefault();
                DataRow dr = ss.Kolicina < 0 ? dt.NewRow() : dt1.NewRow();
                dr["ID"] = ss.ID;
                dr["ROBAID"] = ss.RobaID;
                dr["KOLICINA"] = _sastavnica.Status == TDOffice.DokumentStatus.Zakljucan ? ss.Kolicina : ss.Kolicina * (double)numericUpDown1.Value;
                dr["Naziv"] = roba == null ? "UNKNOWN" : roba.Naziv;
                dr["KatBr"] = roba == null ? "UNKNOWN" : roba.KatBr;
                dr["JM"] = roba == null ? "UNKNOWN" : roba.JM;

                if (ss.Kolicina < 0)
                    dt.Rows.Add(dr);
                else
                    dt1.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["ID"].Visible = false;

            dataGridView1.Columns["RobaID"].Visible = false;

            dataGridView1.Columns["Naziv"].Width = 220;
            dataGridView1.Columns["Naziv"].ReadOnly = true;

            dataGridView1.Columns["KatBr"].Width = 150;
            dataGridView1.Columns["KatBr"].ReadOnly = true;

            dataGridView1.Columns["KOLICINA"].Width = 65;
            dataGridView1.Columns["KOLICINA"].ReadOnly = true;
            dataGridView1.Columns["KOLICINA"].DefaultCellStyle.Format = "#,##0.00";


            dataGridView2.DataSource = dt1;

            dataGridView2.Columns["ID"].Visible = false;

            dataGridView2.Columns["RobaID"].Visible = false;

            dataGridView2.Columns["Naziv"].Width = 220;
            dataGridView2.Columns["Naziv"].ReadOnly = true;

            dataGridView2.Columns["KatBr"].Width = 150;
            dataGridView2.Columns["KatBr"].ReadOnly = true;

            dataGridView2.Columns["KOLICINA"].Width = 65;
            dataGridView2.Columns["KOLICINA"].ReadOnly = true;
            dataGridView2.Columns["KOLICINA"].DefaultCellStyle.Format = "#,##0.00";

            this.tb_IDSastavnice.Text = _sastavnica.ID.ToString();
            this.tb_BrDokKomerc.Text = _sastavnica.BrDokKom.ToString();

            if (_sastavnica.Status == TDOffice.DokumentStatus.Zakljucan)
            {
                this.button1.Visible = false;
                this.label1.Visible = false;
                this.label2.Visible = false;
                this.pravilaSastavnica_cmb.Visible = false;
                this.numericUpDown1.Visible = false;
            }
            this.Text = "Sastavnica [" + _sastavnica.ID.ToString() + "] za magacin ID[" + _sastavnica.MagacinID.ToString() + "]";
        }

        private void pravilaSastavnica_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            ReloadData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int psid = Convert.ToInt32(pravilaSastavnica_cmb.SelectedValue);

            if (psid < 0)
            {
                MessageBox.Show("Niste izabrali pravilo seta sastavnice!");
                return;
            }

            if (MessageBox.Show("Sigurno zelite da kreirate " + numericUpDown1.Value.ToString() + " setova po sastavnici " + this.pravilaSastavnica_cmb.Text + "?", "Kreiranje satavnice", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                using(FbConnection con = new FbConnection(TDOffice.TDOffice.connectionString))
                {
                    con.Open();

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                        TDOffice.StavkaSastavnica.Insert(con, _sastavnica.ID, Convert.ToInt32(row.Cells["Robaid"].Value), Convert.ToDouble(row.Cells["KOLICINA"].Value));

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                        TDOffice.StavkaSastavnica.Insert(con, _sastavnica.ID, Convert.ToInt32(row.Cells["Robaid"].Value), Convert.ToDouble(row.Cells["KOLICINA"].Value));
                }


                using(FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[DateTime.Now.Year]))
                {
                    con.Open();
                    int komercijalnoBrDok = Komercijalno.Dokument.Insert(con, 16, _sastavnica.ID.ToString(), null, "TDOffice_v2", 1, _sastavnica.MagacinID, Program.TrenutniKorisnik.KomercijalnoUserID, null);
                    Komercijalno.Dokument dok = Komercijalno.Dokument.Get(con, 16, komercijalnoBrDok);

                    List<Komercijalno.Roba> robaKom = Komercijalno.Roba.List(con);
                    List<Komercijalno.RobaUMagacinu> rumKom = Komercijalno.RobaUMagacinu.ListByMagacinID(con, _sastavnica.MagacinID);

                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        int robaID = Convert.ToInt32(row.Cells["Robaid"].Value);
                        Komercijalno.Stavka.Insert(DateTime.Now.Year, dok, robaKom.Where(x => x.ID == robaID).FirstOrDefault(), rumKom.Where(x => x.RobaID == robaID).FirstOrDefault(), Convert.ToDouble(row.Cells["KOLICINA"].Value), 0);
                    }

                    foreach (DataGridViewRow row in dataGridView2.Rows)
                    {
                        int robaID = Convert.ToInt32(row.Cells["Robaid"].Value);
                        Komercijalno.Stavka.Insert(DateTime.Now.Year, dok, robaKom.Where(x => x.ID == robaID).FirstOrDefault(), rumKom.Where(x => x.RobaID == robaID).FirstOrDefault(), Convert.ToDouble(row.Cells["KOLICINA"].Value), 0);
                    }

                    _sastavnica.BrDokKom = komercijalnoBrDok;

                    dok = Komercijalno.Dokument.Get(con, 16, komercijalnoBrDok);
                    dok.Flag = 1;
                    dok.Update();
                }

                _sastavnica.Status = TDOffice.DokumentStatus.Zakljucan;
                _sastavnica.Update();
                _sastavnica = TDOffice.DokumentSastavnica.Get(_sastavnica.ID);

                this.BackColor = _sastavnica.Status == TDOffice.DokumentStatus.Zakljucan ? Color.Red : Color.Green;
                ReloadData();
            }  
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            ReloadData();
        }
    }
}
