using FirebirdSql.Data.FirebirdClient;
using MigraDoc.DocumentObjectModel.Internals;
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
    public partial class fm_ObracunPoreza_Index : Form
    {
        private fm_Help _helpForm { get; set; }

        public fm_ObracunPoreza_Index()
        {
            InitializeComponent();
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.WrapContents = false;
        }

        private async void fm_ObracunPoreza_Index_LoadAsync(object sender, EventArgs e)
        {
            _helpForm = await this.InitializeHelpModulAsync((int)Modul.fm_ObracunPoreza_Index);
        }

        private async Task CreateDGV(int magacinID, int godina)
        {
            // 132 
            var magacin = Komercijalno.Magacin.Get(DateTime.Now.Year, magacinID);

            string osnovaKonta = "";

            switch(magacin.Vrsta)
            {
                case 1:
                    osnovaKonta = "1320";
                    break;
                case 2:
                    osnovaKonta = "1340";
                    break;
                default:
                    MessageBox.Show("Nije definisana osnova konta za ovu vrstu magacina u kodu!");
                    return;
            }
            Komercijalno.Promena.PromenaCollection pocetnoStanjePromene = await Komercijalno.Promena.CollectionAsync(godina, osnovaKonta + magacinID.ToString("00"), new int[] { 0 });
            Komercijalno.Promena.PromenaCollection ulazRobePromene = await Komercijalno.Promena.CollectionAsync(godina, osnovaKonta + magacinID.ToString("00"), new int[] { 1, 6, 8, 11, 12, 15, 20 });
            Komercijalno.Promena.PromenaCollection izlazRobePromene = await Komercijalno.Promena.CollectionAsync(godina, osnovaKonta + magacinID.ToString("00"), new int[] { 13, 14 });

            DataGridView dgv = new DataGridView();
            dgv.Tag = godina;
            this.flowLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(dgv)).BeginInit();
            this.SuspendLayout();

            int nDgvs = this.flowLayoutPanel1.Controls.OfType<DataGridView>().Count();
            this.flowLayoutPanel1.Controls.Add(dgv);
            dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgv.Name = "dataGridView" + Random.Next(Int32.MaxValue);
            dgv.Size = new System.Drawing.Size(this.flowLayoutPanel1.Width - 9, 0);
            dgv.TabIndex = 0;

            DataTable dt = new DataTable();
            dt.Columns.Add("Column1", typeof(string));
            dt.Columns.Add("Column2", typeof(string));
            dt.Columns.Add("Column3", typeof(string));
            dt.Columns.Add("Column4", typeof(string));
            dt.Columns.Add("Column5", typeof(string));
            dt.Columns.Add("Column6", typeof(string));
            dt.Columns.Add("Column7", typeof(string));
            dt.Columns.Add("Column8", typeof(string));
            dt.Columns.Add("Column9", typeof(string));
            dt.Columns.Add("Column10", typeof(string));
            dt.Columns.Add("Column11", typeof(string));
            dt.Columns.Add("Column12", typeof(string));

            DataRow dr = dt.NewRow();
            dr["Column1"] = $"M{magacinID} - [ {godina} ]";
            dt.Rows.Add(dr);

            double pocetnoStanje = pocetnoStanjePromene.AsQueryable().Sum(x => (double)x.Duguje);
            double ukupanUlaz = (ulazRobePromene.AsQueryable().Sum(x => (double)x.Duguje) + izlazRobePromene.AsQueryable().Sum(y => (double)y.Duguje));
            double ukupanIzlaz = izlazRobePromene.AsQueryable().Sum(x => (double)x.Potrazuje) + ulazRobePromene.AsQueryable().Sum(x => (double)x.Potrazuje);

            DataRow dr1 = dt.NewRow();
            dr1["Column1"] = "Popis";
            dr1["Column2"] = pocetnoStanje.ToString("#,##0.00");
            dr1["Column4"] = "Ulaz Robe";
            dr1["Column5"] = ukupanUlaz.ToString("#,##0.00");
            dr1["Column7"] = "Izlaz Robe";
            dr1["Column8"] = ukupanIzlaz.ToString("#,##0.00");
            dr1["Column10"] = "Popis";
            dr1["Column11"] = (pocetnoStanje + ukupanUlaz - ukupanIzlaz).ToString("#,##0.00");
            dt.Rows.Add(dr1);

            DataRow dr2 = dt.NewRow();
            dr2["Column1"] = "Rabat";
            dr2["Column2"] = "1.000.000,00";
            dr2["Column4"] = "Rabat";
            dr2["Column5"] = "1.000.000,00";
            dr2["Column7"] = "";
            dr2["Column8"] = "";
            dr2["Column10"] = "";
            dr2["Column11"] = "";
            dt.Rows.Add(dr2);

            DataRow dr3 = dt.NewRow();
            dr3["Column1"] = "Porez 20%";
            dr3["Column2"] = "1.000.000,00";
            dr3["Column4"] = "Porez 20%";
            dr3["Column5"] = "1.000.000,00";
            dr3["Column7"] = "Porez 20%";
            dr3["Column8"] = "1.000.000,00";
            dr3["Column10"] = "Porez 20%";
            dr3["Column11"] = "1.000.000,00";
            dt.Rows.Add(dr3);

            DataRow dr4 = dt.NewRow();
            dr4["Column1"] = "Porez 10%";
            dr4["Column2"] = "1.000.000,00";
            dr4["Column4"] = "Porez 10%";
            dr4["Column5"] = "1.000.000,00";
            dr4["Column7"] = "Porez 10%";
            dr4["Column8"] = "1.000.000,00";
            dr4["Column10"] = "Porez 10%";
            dr4["Column11"] = "1.000.000,00";
            dt.Rows.Add(dr4);

            DataRow dr5 = dt.NewRow();
            dt.Rows.Add(dr5);

            DataRow dr6 = dt.NewRow();
            dr6["Column1"] = "Vr. bez poreza";
            dr6["Column2"] = "1.000.000,00";
            dr6["Column4"] = "Vr. bez poreza";
            dr6["Column5"] = "1.000.000,00";
            dr6["Column7"] = "Vr. bez poreza";
            dr6["Column8"] = "1.000.000,00";
            dr6["Column10"] = "Vr. bez poreza";
            dr6["Column11"] = "1.000.000,00";
            dt.Rows.Add(dr6);

            DataRow dr7 = dt.NewRow();
            dt.Rows.Add(dr7);

            DataRow dr8 = dt.NewRow();
            dt.Rows.Add(dr8);

            DataRow dr9 = dt.NewRow();
            dr9["Column1"] = "RUC";
            dr9["Column2"] = "1.000.000,00";
            dr9["Column3"] = "10%";
            dr9["Column4"] = "RUC";
            dr9["Column5"] = "1.000.000,00";
            dr9["Column6"] = "10%";
            dr9["Column7"] = "RUC";
            dr9["Column8"] = "1.000.000,00";
            dr9["Column9"] = "10%";
            dr9["Column10"] = "RUC";
            dr9["Column11"] = "1.000.000,00";
            dr9["Column12"] = "10%";
            dt.Rows.Add(dr9);

            DataRow dr10 = dt.NewRow();
            dt.Rows.Add(dr10);

            DataRow dr11 = dt.NewRow();
            dr11["Column1"] = "RUC";
            dr11["Column2"] = "1.000.000,00";
            dr11["Column3"] = "10%";
            dr11["Column4"] = "RUC";
            dr11["Column5"] = "1.000.000,00";
            dr11["Column6"] = "10%";
            dr11["Column7"] = "RUC";
            dr11["Column8"] = "1.000.000,00";
            dr11["Column9"] = "10%";
            dr11["Column10"] = "RUC";
            dr11["Column11"] = "1.000.000,00";
            dr11["Column12"] = "10%";
            dt.Rows.Add(dr11);

            dgv.RowHeadersVisible = false;
            dgv.DataSource = dt;
            dgv.AllowUserToResizeRows = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.ReadOnly = true;

            foreach (DataGridViewColumn col in dgv.Columns)
                col.HeaderText = "";

            int hght = dgv.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + 20;
            dgv.Height = hght;

            this.flowLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(dgv)).EndInit();
            this.ResumeLayout(false);
        }

        private void ObojiNeslaganja()
        {
            Dictionary<int, Tuple<double, double>> stanja = new Dictionary<int, Tuple<double, double>>();
            foreach(DataGridView dgv in flowLayoutPanel1.Controls.OfType<DataGridView>())
                stanja.Add(Convert.ToInt32(dgv.Tag), new Tuple<double, double>(Convert.ToDouble(dgv.Rows[1].Cells["Column2"].Value), Convert.ToDouble(dgv.Rows[1].Cells["Column11"].Value)));

            foreach(DataGridView dgv in flowLayoutPanel1.Controls.OfType<DataGridView>())
            {
                int dgvGodina = Convert.ToInt32(dgv.Tag);

                double razlikaKrajnje = !stanja.ContainsKey(dgvGodina + 1) ? 0 : stanja[dgvGodina].Item2 - stanja[dgvGodina + 1].Item1;
                double razlikaPocetno = !stanja.ContainsKey(dgvGodina - 1) ? 0 : stanja[dgvGodina].Item1 - stanja[dgvGodina - 1].Item2;

                if (Math.Abs(razlikaPocetno) > 1000)
                    dgv.Rows[1].Cells["Column2"].Style.BackColor = Color.Coral;

                if (Math.Abs(razlikaKrajnje) > 1000)
                    dgv.Rows[1].Cells["Column11"].Style.BackColor = Color.Coral;
            }
        }
        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            foreach (DataGridView dgv in this.flowLayoutPanel1.Controls)
            {
                dgv.Size = new System.Drawing.Size(this.flowLayoutPanel1.Width - 9, dgv.Rows.GetRowsHeight(DataGridViewElementStates.Visible) + 20);
            }
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private async void button1_ClickAsync(object sender, EventArgs e)
        {
            int magacinID;
            this.flowLayoutPanel1.Controls.Clear();
            if(int.TryParse(textBox1.Text, out magacinID))
            {
                for (int i = DateTime.Now.Year; i >= (DateTime.Now.Year - 5); i--)
                    await CreateDGV(magacinID, i);

                ObojiNeslaganja();
            }
            else
            {
                MessageBox.Show("Neispravan ID magacina!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _helpForm.ShowDialog();
        }
    }
}
