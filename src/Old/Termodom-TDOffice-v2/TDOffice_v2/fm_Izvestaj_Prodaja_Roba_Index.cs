using System;
using System.Collections.Concurrent;
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
    public partial class fm_Izvestaj_Prodaja_Roba_Index : Form
    {
        private DataTable _dataTable { get; set; }
        private Task<fm_Help> _help { get; set; }
        private Models.Periodi _periodi { get; set; }
        private string _tipIzvestaja { get; set; }

        private List<int> _listaRobe = new List<int>();

        private List<int> _magacini = new List<int>();

        public bool DozvoliSlanjeIzvestaja
        {
            get
            {
                return posaljiKaoIzvestaj_btn.Enabled;
            }
            set
            {
                posaljiKaoIzvestaj_btn.Enabled = value;
            }
        }
        public bool DozvoliPrikazZbiraPoMagacinu
        {
            get
            {
                return prikaziZbirnoPoMagacinima_btn.Enabled;
            }
            set
            {
                prikaziZbirnoPoMagacinima_btn.Enabled = value;
            }
        }
        public bool DozvoliPrikazZbiraPoRobi
        {
            get
            {
                return prikaziZbirnoPoRobi_btn.Enabled;
            }
            set
            {
                prikaziZbirnoPoRobi_btn.Enabled = value;
            }
        }
        public fm_Izvestaj_Prodaja_Roba_Index(DataTable dataTable, Models.Periodi periodi,string tipIZv, List<int> listaRobe, List<int> magacini)
        {
            InitializeComponent();
            _help = this.InitializeHelpModulAsync(Modul.fm_Izvestaj_Prodaja_Roba_Index);
            _dataTable = dataTable;
            _periodi = periodi;
            _tipIzvestaja = tipIZv;
            _listaRobe = listaRobe;
            _magacini = magacini;

            filterMagacini_clb.Enabled = false;

            _dataTable.Columns["MAGACINID"].SetOrdinal(0);
            _dataTable.Columns["ROBAID"].SetOrdinal(1);
            _dataTable.Columns["NAZIV"].SetOrdinal(2);
            _dataTable.Columns["JM"].SetOrdinal(3);

            int j = 0;

            foreach (int godina in _periodi.Godine.OrderBy(x => x))
            {
                if(_dataTable.Columns.Contains("kGOD" + godina))
                    _dataTable.Columns["kGOD" + godina].SetOrdinal(3 + j);
                if (_dataTable.Columns.Contains("vGOD" + godina))
                    _dataTable.Columns["vGOD" + godina].SetOrdinal(3 + j + _periodi.Count);

            }
            dataGridView1.AutoGenerateColumns = true;
            dataGridView1.DataSource = _dataTable;
            dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns["MAGACINID"].Width = 50;
            dataGridView1.Columns["MAGACINID"].DisplayIndex = 0;

            dataGridView1.Columns["ROBAID"].Visible = false;
            dataGridView1.Columns["ROBAID"].DisplayIndex = 1;

            dataGridView1.Columns["NAZIV"].Width = 300;
            dataGridView1.Columns["NAZIV"].DisplayIndex = 2;

            dataGridView1.Columns["JM"].Width = 50;
            dataGridView1.Columns["JM"].DisplayIndex = 3;
            int y = 0;
            
            foreach (int godina in _periodi.Godine.OrderBy(x => x))
            {
                y++;

                Color c = Color.FromArgb(Random.Next(150, 255), Random.Next(150, 255), Random.Next(150, 255));

                if (_dataTable.Columns.Contains("kGOD" + godina))
                {
                    dataGridView1.Columns["kGOD" + godina].DefaultCellStyle.Format = "#,##0";
                    dataGridView1.Columns["kGOD" + godina].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["kGOD" + godina].Width = 100;
                    if (_dataTable.Columns.Contains("vGOD" + godina))
                        dataGridView1.Columns["kGOD" + godina].DisplayIndex = 3 + y;
                    dataGridView1.Columns["kGOD" + godina].HeaderText = $"KOLICINA GOD {godina} [{_periodi[godina].OdDatuma.ToString("dd.MM.yyyy")} - {_periodi[godina].DoDatuma.ToString("dd.MM.yyyy")}]";
                    dataGridView1.Columns["kGOD" + godina].DefaultCellStyle.BackColor = c;
                }
                if (_dataTable.Columns.Contains("vGOD" + godina))
                {
                    dataGridView1.Columns["vGOD" + godina].DefaultCellStyle.Format = "#,##0.00 " + _tipIzvestaja;
                    dataGridView1.Columns["vGOD" + godina].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                    dataGridView1.Columns["vGOD" + godina].Width = 150;
                    if (_dataTable.Columns.Contains("kGOD" + godina))
                        dataGridView1.Columns["vGOD" + godina].DisplayIndex = 3 + y + _periodi.Count;
                    dataGridView1.Columns["vGOD" + godina].HeaderText = $"VREDNOST GOD {godina} [{_periodi[godina].OdDatuma.ToString("dd.MM.yyyy")} - {_periodi[godina].DoDatuma.ToString("dd.MM.yyyy")}]";
                    dataGridView1.Columns["vGOD" + godina].DefaultCellStyle.BackColor = c;
                }
            }
            dataGridView1.Visible = true;
            SakrijKoloneDGV();
        }

        private void fm_Izvestaj_Prodaja_Roba_Index_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                List<int> magaciniUTabeli = _dataTable.AsEnumerable().Select(x => Convert.ToInt32(x["MAGACINID"])).Distinct().ToList();
                List<Komercijalno.Magacin> komercijalnoMagacini = Komercijalno.Magacin.ListAsync().Result;

                this.Invoke((MethodInvoker)delegate
                {
                    filterMagacini_clb.DataSource = komercijalnoMagacini.Where(x => magaciniUTabeli.Contains(x.ID)).ToList();
                    filterMagacini_clb.ValueMember = "ID";
                    filterMagacini_clb.DisplayMember = "Naziv";

                    for (int i = 0; i < filterMagacini_clb.Items.Count; i++)
                        filterMagacini_clb.SetItemChecked(i, true);

                    filterMagacini_clb.Enabled = true;
                });
            });
        }

        private void posaljiKaoIzvestaj_btn_Click(object sender, EventArgs e)
        {
            using (fm_Izvestaj_Prodaja_Roba_PosaljiKaoIzvestaj_Setup s = new fm_Izvestaj_Prodaja_Roba_PosaljiKaoIzvestaj_Setup(_dataTable, _periodi, _tipIzvestaja,_listaRobe))
                s.ShowDialog();
        }

        private void SakrijKoloneDGV()
        {
            foreach (int godina in _periodi.Godine.OrderBy(x => x))
            {

                if (_dataTable.Columns.Contains("vGOD" + godina))
                {
                    dataGridView1.Columns["vGOD" + godina].Visible = prikaziVrednosti_cb.Checked;
                }

                if (_dataTable.Columns.Contains("kGOD" + godina))
                {
                    dataGridView1.Columns["kGOD" + godina].Visible = prikaziKolicine_cb.Checked;
                }
            }
        }

        private void prikaziKolicine_cb_CheckedChanged(object sender, EventArgs e)
        {
            SakrijKoloneDGV();
        }

        private void prikaziVrednosti_cb_CheckedChanged(object sender, EventArgs e)
        {
            SakrijKoloneDGV();
        }

        private void prikaziZbirnoPoMagacinima_btn_Click(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                using (fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima gm = new fm_Izvestaj_Prodaja_Roba_ZbirnoPoMagacinima(_dataTable, _periodi, _tipIzvestaja, _listaRobe))
                    gm.ShowDialog();
            });
        }

        private void filterMagacini_clb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!filterMagacini_clb.Enabled)
                return;

            if(filterMagacini_clb.CheckedItems.Count == 0)
            {
                dataGridView1.DataSource = null;
                return;
            }

            DataRow[] rows = _dataTable.Select($"MAGACINID IN ({string.Join(", ", filterMagacini_clb.CheckedItems.OfType<Komercijalno.Magacin>().Select(x => x.ID))})");

            dataGridView1.DataSource = rows == null || rows.Length == 0 ? null : rows.CopyToDataTable(); ;
        }

        private void cekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < filterMagacini_clb.Items.Count; i++)
                filterMagacini_clb.SetItemChecked(i, true);
        }

        private void decekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < filterMagacini_clb.Items.Count; i++)
                filterMagacini_clb.SetItemChecked(i, false);
        }

        private void prikaziZbirnoPoRobi_btn_Click(object sender, EventArgs e)
        {
            using(fm_Izvestaj_Prodaja_Roba_ZbirnoPoRobi fm = new fm_Izvestaj_Prodaja_Roba_ZbirnoPoRobi(_dataTable, _periodi))
            {
                fm.DozvoliSlanjeIzvestaja = false;
                fm.ShowDialog();
            }
        }

        private void help_btn_Click(object sender, EventArgs e)
        {
            _help.Result.ShowDialog();
        }
    }
}
