using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_IzborRobe_v2_Index : Form
    {
        private int MagacinID { get; set; }
        public bool DisposeOnClose { get; set; } = false;
        private Task<Termodom.Data.Entities.Komercijalno.RobaUMagacinuDictionary> _robaUMagacinu { get; set; }
        private Task<Termodom.Data.Entities.Komercijalno.RobaDictionary> _roba { get; set; }

        public delegate void OnPotvrdaKolicine(int robaID, double kolicina);

        public OnPotvrdaKolicine PotvrdaKolicine;

        public fm_IzborRobe_v2_Index()
        {
            InitializeComponent();
        }

        private void fm_IzborRobe_v2_Index_Load(object sender, EventArgs e)
        {
        }

        private void fm_IzborRobe_v2_Index_Shown(object sender, EventArgs e)
        {
            if (PotvrdaKolicine == null)
                throw new Exception("Ne mozete prikazati Izbor Robe bez i jednog event handlera na PotvrdaKolicine eventu!");
        }

        private void ToggleUI(bool state)
        {
            this.pretraga_cmb.Enabled = state;
            this.pretraga_txt.Enabled = state;
            this.dataGridView1.Enabled = state;
        }

        private void ToggleKolicinaUI(bool state)
        {
            this.kolicina_txt.Enabled = state;
            this.unesi_btn.Enabled = state;

            if (state)
            {
                kolicina_txt.SelectAll();
                kolicina_txt.Focus();
            }
            else
            {
                dataGridView1.Focus();
            }
        }

        /// <summary>
        /// Postavlja magacin za koji ce biti prikazana roba u izboru robe
        /// </summary>
        /// <param name="magacinID"></param>
        /// <returns></returns>
        public void SetMagacin(int magacinID)
        {
            MagacinID = magacinID;
            _ = UcitajPodatkeAsync();
        }

        /// <summary>
        /// Ucitava robu u magacinu
        /// </summary>
        /// <returns></returns>
        private async Task UcitajPodatkeAsync()
        {
            ToggleUI(false);
            ToggleKolicinaUI(false);
            _robaUMagacinu = Komercijalno.RobaUMagacinu.DictionaryAsync(MagacinID);
            _roba = Komercijalno.Roba.Dictionary();
            await PopuniDGVAsync();
            ToggleUI(true);
        }

        /// <summary>
        /// Popunjava DataGridView sa podacima robe
        /// </summary>
        private async Task PopuniDGVAsync()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RobaID", typeof(int));
            dt.Columns.Add("KatBr", typeof(string));
            dt.Columns.Add("KatBrPro", typeof(string));
            dt.Columns.Add("Naziv", typeof(string));
            dt.Columns.Add("JM", typeof(string));
            dt.Columns.Add("ProdajnaCena", typeof(double));
            dt.Columns.Add("GrupaID", typeof(string));
            dt.Columns.Add("Podgrupa", typeof(int));
            dt.Columns.Add("ProID", typeof(string));
            dt.Columns.Add("DOB_PPID", typeof(int));

            foreach(Termodom.Data.Entities.Komercijalno.RobaUMagacinu rum in (await _robaUMagacinu)[MagacinID].Values)
            {
                Termodom.Data.Entities.Komercijalno.Roba r = (await _roba)[rum.RobaID];

                DataRow dr = dt.NewRow();
                dr["RobaID"] = rum.RobaID;
                dr["KatBr"] = r.KatBr ?? "Undefined";
                dr["KatBrPro"] = r.KatBrPro ?? "Undefined";
                dr["Naziv"] = r.Naziv ?? "Undefined";
                dr["JM"] = r.JM ?? "Undefined";
                dr["ProdajnaCena"] = rum.ProdajnaCena;
                dr["GrupaID"] = r.GrupaID;

                if(r.Podgrupa != null)
                    dr["Podgrupa"] = r.Podgrupa;

                dr["ProID"] = r.ProID;

                if(r.DOB_PPID != null)
                    dr["DOB_PPID"] = r.DOB_PPID;

                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["GrupaID"].Visible = false;
            dataGridView1.Columns["Podgrupa"].Visible = false;
            dataGridView1.Columns["DOB_PPID"].Visible = false;

            dataGridView1.Columns["KatBr"].Width = 150;
            dataGridView1.Columns["KatBrPro"].Width = 150;
            dataGridView1.Columns["Naziv"].Width = 300;
            dataGridView1.Columns["JM"].Width = 50;
            dataGridView1.Columns["JM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["ProdajnaCena"].Width = 80;
            dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Format = "#,##0.00";
            dataGridView1.Columns["ProdajnaCena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dataGridView1.Sort(dataGridView1.Columns["Naziv"], ListSortDirection.Ascending);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter && e.KeyCode != Keys.Return)
                return;

            ToggleKolicinaUI(true);

            e.SuppressKeyPress = true;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ToggleKolicinaUI(true);
        }

        private void kolicina_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Back || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right || e.KeyCode == Keys.Delete)
            {
                return;
            }

            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                PotvrdiInputKolicine();
                return;
            }

            if(e.KeyCode == Keys.Escape)
            {
                ToggleKolicinaUI(false);
                return;
            }

            if (e.KeyData != Keys.Decimal && e.KeyData != Keys.OemPeriod && !e.KeyData.IsNumber())
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if(e.KeyData == Keys.Decimal || e.KeyData == Keys.OemPeriod)
                if (kolicina_txt.Text.Any(x => x == '.' || x == ',' || x == '.'))
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }
        }

        private void unesi_btn_Click(object sender, EventArgs e)
        {
            PotvrdiInputKolicine();
        }

        private void PotvrdiInputKolicine()
        {
            int robaID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["RobaID"].Value);
            double inputKolicina = Convert.ToDouble(kolicina_txt.Text);
            PotvrdaKolicine(robaID, inputKolicina);
            ToggleKolicinaUI(false);
        }

        private void fm_IzborRobe_v2_Index_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!DisposeOnClose && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}
