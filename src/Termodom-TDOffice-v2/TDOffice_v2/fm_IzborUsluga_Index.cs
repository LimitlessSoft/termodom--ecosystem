using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_IzborUsluga_Index : Form
    {
        public bool DisposeOnClose { get; set; } = false;
        private Task<Termodom.Data.Entities.Komercijalno.RobaDictionary> _robaTask { get; set; }


        public delegate void OnPotvrdaUsluge(int robaID, double kolicina, double cena, string naziv, string jm);

        public OnPotvrdaUsluge PotvrdaUsluge;

        public fm_IzborUsluga_Index()
        {
            InitializeComponent();
        }

        private void fm_IzborUsluga_Index_Load(object sender, System.EventArgs e)
        {
            _ = UcitajPodatkeAsync();
        }

        private void fm_IzborUsluga_Index_Shown(object sender, System.EventArgs e)
        {
            if (PotvrdaUsluge == null)
                throw new Exception($"Ne mozete prikazati Izbor Robe bez i jednog event handlera na {nameof(PotvrdaUsluge)} eventu!");
        }

        private void ToggleUI(bool state)
        {
            this.pretraga_cmb.Enabled = state;
            this.pretraga_txt.Enabled = state;
            this.dataGridView1.Enabled = state;
        }

        private void ToggleInputUI(bool state)
        {
            this.kolicina_txt.Enabled = state;
            this.cena_txt.Enabled = state;
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
        private async Task UcitajPodatkeAsync()
        {
            ToggleUI(false);
            ToggleInputUI(false);
            _robaTask = Komercijalno.Roba.Dictionary(DateTime.Now.Year);
            await PopuniDGVAsync();
            ToggleUI(true);
        }
        private async Task PopuniDGVAsync()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RobaID", typeof(int));
            dt.Columns.Add("KatBr", typeof(string));
            dt.Columns.Add("KatBrPro", typeof(string));
            dt.Columns.Add("Naziv", typeof(string));
            dt.Columns.Add("JM", typeof(string));
            dt.Columns.Add("GrupaID", typeof(string));
            dt.Columns.Add("Podgrupa", typeof(int));
            dt.Columns.Add("ProID", typeof(string));
            dt.Columns.Add("DOB_PPID", typeof(int));

            foreach (Termodom.Data.Entities.Komercijalno.Roba r in (await _robaTask).Values.Where(x => new int[] { 2, 4 }.Contains(x.Vrsta)))
            {
                DataRow dr = dt.NewRow();
                dr["RobaID"] = r.ID;
                dr["KatBr"] = r.KatBr ?? "Undefined";
                dr["KatBrPro"] = r.KatBrPro ?? "Undefined";
                dr["Naziv"] = r.Naziv ?? "Undefined";
                dr["JM"] = r.JM ?? "Undefined";
                dr["GrupaID"] = r.GrupaID;

                if (r.Podgrupa != null)
                    dr["Podgrupa"] = r.Podgrupa;

                dr["ProID"] = r.ProID;

                if (r.DOB_PPID != null)
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

            dataGridView1.Sort(dataGridView1.Columns["Naziv"], ListSortDirection.Ascending);
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter && e.KeyCode != Keys.Return)
                return;

            ToggleInputUI(true);

            e.SuppressKeyPress = true;
        }

        private void kolicina_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ToggleInputUI(false);
                return;
            }

            if (e.KeyData != Keys.Decimal && e.KeyData != Keys.OemPeriod && !e.KeyData.IsNumber())
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyData == Keys.Decimal || e.KeyData == Keys.OemPeriod)
                if (kolicina_txt.Text.Any(x => x == '.' || x == ',' || x == '.'))
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }
        }

        private void cena_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                ToggleInputUI(false);
                return;
            }

            if (e.KeyData != Keys.Decimal && e.KeyData != Keys.OemPeriod && !e.KeyData.IsNumber())
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                return;
            }

            if (e.KeyData == Keys.Decimal || e.KeyData == Keys.OemPeriod)
                if (kolicina_txt.Text.Any(x => x == '.' || x == ',' || x == '.'))
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    return;
                }
        }

        private void unesi_btn_Click(object sender, EventArgs e)
        {
            PotvrdiInput();
        }

        private void PotvrdiInput()
        {
            int robaID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["RobaID"].Value);
            var roba = _robaTask.GetAwaiter().GetResult()[robaID];
            double inputKolicina;
            double inputCena;

            if(!double.TryParse(kolicina_txt.Text, out inputKolicina))
            {
                MessageBox.Show("Neispravna kolicina!");
                ToggleInputUI(false);
                return;
            }

            if (!double.TryParse(cena_txt.Text, out inputCena) || inputCena < 0)
            {
                MessageBox.Show("Neispravna cena!");
                ToggleInputUI(false);
                return;
            }

            PotvrdaUsluge(robaID, inputKolicina, inputCena, roba.Naziv, roba.JM);
            ToggleInputUI(false);
        }

        private void fm_IzborUsluga_Index_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!DisposeOnClose && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ToggleInputUI(true);
        }
    }
}
