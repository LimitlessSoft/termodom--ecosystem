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
    public partial class fm_SmsIstorija_Index : Form
    {
        private Task<List<TDOffice.SMSIstorija>> _istorija = TDOffice.SMSIstorija.ListAsync();
        private Task<List<TDOffice.Partner>> _partneri = TDOffice.Partner.ListAsync();

        public fm_SmsIstorija_Index()
        {
            if(!Program.TrenutniKorisnik.ImaPravo(136570))
            {
                TDOffice.Pravo.NematePravoObavestenje(136570);
                this.Close();
                return;
            }
            InitializeComponent();
            panel2.DesniKlik_DatumRange();
            odDatuma_dtp.Value = new DateTime(DateTime.Now.Year, 1, 1);
            doDatuma_dtp.Value = DateTime.Now;
        }
        private void fm_SmsIstorija_Index_Load(object sender, EventArgs e)
        {
            NamestiDGV();
        }

        private void NamestiDGV()
        {
            dataGridView1.Visible = false;
            DataTable dt = new DataTable();
            dt.Columns.Add("Mobilni", typeof(string));
            dt.Columns.Add("Text", typeof(string));
            dt.Columns.Add("Datum", typeof(DateTime));
            dt.Columns.Add("Hash", typeof(string));

            foreach(TDOffice.SMSIstorija ist in _istorija.Result.Where(x => x.Datum > odDatuma_dtp.Value && x.Datum < doDatuma_dtp.Value))
            {
                TDOffice.Partner part = _partneri.Result.FirstOrDefault(x => x.ID == ist.PartnerID);

                DataRow dr = dt.NewRow();
                dr["Mobilni"] = part.Mobilni;
                dr["Text"] = ist.Tekst;
                dr["Datum"] = ist.Datum;
                dr["Hash"] = ist.Hash;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["Mobilni"].Width = 120;

            dataGridView1.Columns["Text"].Width = 200;

            dataGridView1.Columns["Datum"].DefaultCellStyle.Format = "dd.MM.yyyy [ HH:mm:ss ]";
            dataGridView1.Columns["Datum"].Width = 120;

            dataGridView1.Columns["Hash"].Width = 80;

            ObojiDGV();
            dataGridView1.Visible = true;
        }
        private void ObojiDGV()
        {
            string lastHash = "";
            Color currentColor = Color.Black;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (string.IsNullOrWhiteSpace(lastHash) || row.Cells["Hash"].Value.ToString() != lastHash)
                {
                    lastHash = row.Cells["Hash"].Value.ToString();
                    currentColor = Color.FromArgb(Random.Next(100, 255), Random.Next(100, 255), Random.Next(100, 255));
                }

                row.DefaultCellStyle.BackColor = currentColor;
            }
        }
        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            ObojiDGV();
        }

        private void primeniFilter_btn_Click(object sender, EventArgs e)
        {
            NamestiDGV();
        }
    }
}
