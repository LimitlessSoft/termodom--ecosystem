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
    public partial class fm_CheckLista_Index : Form
    {
        private Task<List<TDOffice.User>> _tdOfficeKorisnici = TDOffice.User.ListAsync();
        public fm_CheckLista_Index()
        {
            if(!Program.TrenutniKorisnik.ImaPravo(136700))
            {
                Task.Run(() =>
                {
                    TDOffice.Pravo.NematePravoObavestenje(136700);
                });
                this.Close();
                return;
            }

            InitializeComponent();
            korisnik_cmb.Enabled = false;
            zadaci_cmb.Enabled = false;
            dodajZadatak_btn.Enabled = false;
        }

        private void fm_CheckLista_Index_Load(object sender, EventArgs e)
        {
            korisnik_cmb.DisplayMember = "Username";
            korisnik_cmb.ValueMember = "ID";
            korisnik_cmb.DataSource = _tdOfficeKorisnici.Result;
            korisnik_cmb.SelectedValue = Program.TrenutniKorisnik.ID;
            OsveziListu();

            korisnik_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(136701);
            zadaci_cmb.Enabled = Program.TrenutniKorisnik.ImaPravo(136702);
            dodajZadatak_btn.Enabled = Program.TrenutniKorisnik.ImaPravo(136702);
        }

        private void OsveziListu()
        {
            dataGridView1.Columns.Clear();
            int korisnikID = Convert.ToInt32(korisnik_cmb.SelectedValue);

            List<TDOffice.CheckListItem> items = TDOffice.CheckListItem.ListByKorisnikID(korisnikID);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("ProbioRok", typeof(bool));
            dt.Columns.Add("ZadatakID", typeof(int));
            dt.Columns.Add("Zadatak", typeof(string));
            dt.Columns.Add("Izvrsen", typeof(string));
            dt.Columns.Add("IntervalDana", typeof(int));

            foreach (TDOffice.CheckListItem it in items)
            {
                DataRow dr = dt.NewRow();
                dr["ID"] = it.ID;
                dr["ZadatakID"] = (int)it.Job;
                dr["ProbioRok"] = it.DatumIzvrsenja == null || (DateTime.Now - (DateTime)it.DatumIzvrsenja).TotalDays > it.IntervalDana ? true : false;
                dr["Zadatak"] = it.Job.ToString().DivideOnCapital();
                dr["Izvrsen"] = it.DatumIzvrsenja == null ? "Nikada" : ((DateTime)it.DatumIzvrsenja).ToString("dd.MM.yyyy [HH:mm:ss]");
                dr["IntervalDana"] = it.IntervalDana;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            DataGridViewButtonColumn bCol = new DataGridViewButtonColumn();
            bCol.Name = "PokreniZadatak";
            bCol.Text = "Pokreni Zadatak";
            bCol.UseColumnTextForButtonValue = true;
            int colIndex = dataGridView1.Columns.Count;
            dataGridView1.Columns.Insert(colIndex, bCol);

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["ZadatakID"].Visible = false;
            dataGridView1.Columns["ProbioRok"].Visible = false;
            dataGridView1.Columns["Zadatak"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["Izvrsen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["IntervalDana"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            List<Tuple<int, string>> jobs = new List<Tuple<int, string>>();

            foreach (TDOffice.CheckList.Jobs job in Enum.GetValues(typeof(TDOffice.CheckList.Jobs)))
                if(!items.Any(x => x.Job == job))
                    jobs.Add(new Tuple<int, string>((int)job, job.ToString()));

            zadaci_cmb.ValueMember = "Item1";
            zadaci_cmb.DisplayMember = "Item2";
            zadaci_cmb.DataSource = jobs;

            for(int i = 0; i < dataGridView1.Rows.Count; i++)
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["ProbioRok"].Value))
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Coral;
        }

        private void korisnik_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!korisnik_cmb.Enabled)
                return;

            OsveziListu();
        }

        private void dodajZadatak_btn_Click(object sender, EventArgs e)
        {
            int korisnikID = Convert.ToInt32(korisnik_cmb.SelectedValue);
            int jobID = Convert.ToInt32(zadaci_cmb.SelectedValue);

            int noviID = TDOffice.CheckListItem.Insert((TDOffice.CheckList.Jobs)jobID, korisnikID, 7);

            if (noviID > 0)
            {
                MessageBox.Show("Uspesno dodat zadatak!");
                OsveziListu();
            }
            else
            {
                MessageBox.Show("Doslo je do greske prilikom insertovanja zadatka!");
            }
        }

        private void izmeniIntervalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.TrenutniKorisnik.ImaPravo(136702))
            {
                TDOffice.Pravo.NematePravoObavestenje(136702);
                return;
            }
            if (dataGridView1.SelectedCells.Count == 0)
            {
                MessageBox.Show("Nijedan zadatak nije izabran!");
                return;
            }
            int id = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);

            try
            {
                using(InputBox ib = new InputBox("Novi interval", "Unesite novi interval dana za ovaj zadatak."))
                {
                    ib.ShowDialog();

                    int noviInterval = Convert.ToInt32(ib.returnData);

                    TDOffice.CheckListItem item = TDOffice.CheckListItem.Get(id);
                    item.IntervalDana = noviInterval;
                    item.Update();

                    OsveziListu();
                    MessageBox.Show("Uspesno azuriran interval!");
                }
            }
            catch
            {
                MessageBox.Show("Neispravna vrednost!");
            }
        }

        private void ukloniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!Program.TrenutniKorisnik.ImaPravo(136702))
            {
                TDOffice.Pravo.NematePravoObavestenje(136702);
                return;
            }
            if (dataGridView1.SelectedCells.Count == 0)
            {
                MessageBox.Show("Nijedan zadatak nije izabran!");
                return;
            }
            int id = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);

            if (MessageBox.Show("Da li sigurno zelite ukloniti ovaj zadatak korisniku?", "Potvrdi!", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            TDOffice.CheckListItem.Delete(id);

            OsveziListu();
            MessageBox.Show("Uspesno uklonjen zadatak!");
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if(e.ColumnIndex == dataGridView1.Columns["PokreniZadatak"].Index)
            {
                TDOffice.CheckList.Jobs job = (TDOffice.CheckList.Jobs)Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ZadatakID"].Value);

                switch(job)
                {
                    case TDOffice.CheckList.Jobs.IzvestajProdajeRobe:
                        using (fm_Izvestaj_Prodaja_Roba_Setup set = new fm_Izvestaj_Prodaja_Roba_Setup())
                            if (!set.IsDisposed)
                                set.ShowDialog();
                        break;
                    case TDOffice.CheckList.Jobs.DetaljnaAnalizaPartnera:
                        using (fm_Partner_Analiza_General_Index gen = new fm_Partner_Analiza_General_Index())
                            if (!gen.IsDisposed)
                                gen.ShowDialog();
                        break;
                    case TDOffice.CheckList.Jobs.NeaktivniPartneri:
                        using (fm_Partner_Analiza_Neaktivni_Index np = new fm_Partner_Analiza_Neaktivni_Index())
                            if (!np.IsDisposed)
                                np.ShowDialog();
                        break;
                    case TDOffice.CheckList.Jobs.PrometMagacina:
                        if (Application.OpenForms.OfType<fm_Izvestaj_Promet_Magacina>().FirstOrDefault() != null)
                            return;
                        Task.Run(() =>
                        {
                            using (fm_Izvestaj_Promet_Magacina pm = new fm_Izvestaj_Promet_Magacina())
                                if (!pm.IsDisposed)
                                    pm.ShowDialog();
                        });
                        break;
                    case TDOffice.CheckList.Jobs.OdobreniRabatiUProdaji:
                        if (Application.OpenForms.OfType<fm_OdobreniRabati>().FirstOrDefault() != null)
                            return;
                        Task.Run(() =>
                        {
                            using (fm_OdobreniRabati orp = new fm_OdobreniRabati())
                                if (!orp.IsDisposed)
                                    orp.ShowDialog();
                        });
                        break;
                    case TDOffice.CheckList.Jobs.NelogicneMarze:
                        Main.PosaljiIzvestajDokumenataUKojimaJeMarzaManjaOdProsecneKaoPoruku();
                        break;
                    default:
                        MessageBox.Show("Nepoznat zadatak!");
                        break;
                }

                OsveziListu();
            }
        }
    }
}
