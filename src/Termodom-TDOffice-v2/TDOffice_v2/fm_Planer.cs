using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class fm_Planer : Form
    {
        public static bool Prikazan { get; set; } = false;

        private DataTable _planerDataTable { get; set; }

        public fm_Planer(DataTable source)
        {
            InitializeComponent();

            this.Text = "Planer <" + Program.TrenutniKorisnik.Username + ">";

            this._planerDataTable = source;
        }
        private void fm_Planer_Load(object sender, EventArgs e)
        {
            OsveziPlaner();
            Prikazan = true;
        }

        private void OsveziPlaner()
        {
            dgv_Planer.DataSource = _planerDataTable;

            dgv_Planer.Columns["planerStavkaID"].Visible = false;
            dgv_Planer.Columns["Vrednost"].Width = 400;

            int redZaSelektovati = (int)(DateTime.Now - DateTime.Now.AddYears(-1)).TotalDays;

            dgv_Planer.FirstDisplayedScrollingRowIndex = redZaSelektovati > 0 ? redZaSelektovati - 2 : 0;
            dgv_Planer.Rows[redZaSelektovati].Selected = true;
            dgv_Planer.CurrentCell = dgv_Planer.Rows[redZaSelektovati].Cells["Datum"];
            dgv_Planer.Columns["Datum"].DefaultCellStyle.Format = "dd.MM.yyyy (ddd)";
            dgv_Planer.Columns["Datum"].Width = 100;
            dgv_Planer.Columns["Datum"].ReadOnly = true;
        }

        private void dgv_Planer_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgv_Planer.Columns["Vrednost"].Index == e.ColumnIndex)
                {
                    DataGridViewRow dgvRow = dgv_Planer.Rows[e.RowIndex];
                    int idStavke = Convert.ToInt32(dgvRow.Cells["planerStavkaID"].Value);
                    string vrednost = dgvRow.Cells["Vrednost"].Value.ToString();

                    if (idStavke == -1)
                    {
                        if (string.IsNullOrWhiteSpace(vrednost))
                            return;

                        DateTime dateTime = Convert.ToDateTime(dgvRow.Cells["Datum"].Value);
;
                        int noviID = TDOffice.Planer.Stavka.Insert(Program.TrenutniKorisnik.ID, dateTime, vrednost);

                        _planerDataTable.Rows[e.RowIndex]["planerStavkaID"] = noviID;

                        //OsveziPlaner();
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(vrednost))
                        {

                            TDOffice.Planer.Stavka.Remove(idStavke);
                        }
                        else
                        {
                            TDOffice.Planer.Stavka s = TDOffice.Planer.Stavka.Get((int)idStavke);
                            s.Body = vrednost;
                            s.Update();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

       

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                DateTime izabraniDatum = dateTimePicker1.Value;

                for (int i = 0; i < _planerDataTable.Rows.Count; i++)
                {
                    DateTime d = Convert.ToDateTime(_planerDataTable.Rows[i]["Datum"]);

                    if (d.Date == izabraniDatum.Date)
                    {
                        dgv_Planer.FirstDisplayedScrollingRowIndex = i > 2 ? i - 2 : 0;
                        dgv_Planer.Rows[i].Selected = true;
                        dgv_Planer.CurrentCell = dgv_Planer.Rows[i].Cells["Datum"];
                        break;
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void fm_Planer_FormClosed(object sender, FormClosedEventArgs e)
        {
            Prikazan = false;
        }

        private void korisnik_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Prikazan)
                return;

            List<TDOffice.Planer.Stavka> planerStavkeKorisnika = TDOffice.Planer.Stavka.ListByUserID(Program.TrenutniKorisnik.ID);

            _planerDataTable = new DataTable();
            _planerDataTable.Columns.Add("planerStavkaID", typeof(int));
            _planerDataTable.Columns.Add("Datum", typeof(DateTime));
            _planerDataTable.Columns.Add("Vrednost", typeof(string));

            DateTime workingDatum = DateTime.Now.AddYears(-1);
            DateTime krajnjiDatumPlanera = new DateTime(DateTime.Now.Year, 12, 31);
            while (workingDatum <= krajnjiDatumPlanera)
            {
                TDOffice.Planer.Stavka s = planerStavkeKorisnika.Where(x => x.Datum.Date == workingDatum.Date).FirstOrDefault();
                DataRow r = _planerDataTable.NewRow();
                r["planerStavkaID"] = s == null ? -1 : s.ID;
                r["Datum"] = workingDatum;
                r["Vrednost"] = s == null ? null : s.Body;

                workingDatum = workingDatum.AddDays(1);
                _planerDataTable.Rows.Add(r);
            }

            OsveziPlaner();
        }
    }
}
