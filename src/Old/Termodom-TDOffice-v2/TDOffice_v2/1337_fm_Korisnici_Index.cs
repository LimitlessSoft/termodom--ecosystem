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
    public partial class _1337_fm_Korisnici_Index : Form
    {
        TDOffice.User korisnik;
        List<Komercijalno.Magacin> magacini;

        private bool _loaded = false;
        private Task<DataTable> _tdTasksTable { get; set; }
        private Task<List<TDOffice.TDTaskItem>> _tdTaskItems { get; set; }
        private Task<List<TDOffice.Grad>> _gradovi { get; set; } = TDOffice.Grad.ListAsync();
        private DataTable _baseDataTable { get; set; }
        private Task<fm_Help> _helpForm { get; set; } = fm_Help.GenerateAsync(TDOffice.Modul.GetWithInsert((int)Modul.Korisnici_Index));

        public _1337_fm_Korisnici_Index(int userID)
        {
            InitializeComponent();

            korisnik = TDOffice.User.Get(userID);
            UcitajTaskoveAsync(true);

            if (korisnik.Tag == null)
                korisnik.Tag = new TDOffice.User.Info();

            if(korisnik == null)
            {
                MessageBox.Show("Doslo je do greske prilikom ucitavanja korisnika!");
                this.Close();
            }

            grad_cmb.ValueMember = "ID";
            grad_cmb.DisplayMember = "Naziv";
            grad_cmb.DataSource = _gradovi.Result;

            grad_cmb.SelectedValue = korisnik.Grad;
        }
        private void _1337_fm_Korisnici_Index_Load(object sender, EventArgs e)
        {
            List<Tuple<int, string>> listaTipovaAutomatskogObavestenja = new List<Tuple<int, string>>();
            foreach(TDOffice.User.TipAutomatskogObavestenja to in (TDOffice.User.TipAutomatskogObavestenja[])Enum.GetValues(typeof(TDOffice.User.TipAutomatskogObavestenja)))
            {
                listaTipovaAutomatskogObavestenja.Add(new Tuple<int, string>((int)to, to.ToString().DivideOnCapital()));
            }

            clb_PrimalacObavestenja.DataSource = listaTipovaAutomatskogObavestenja;
            clb_PrimalacObavestenja.DisplayMember = "Item2";
            clb_PrimalacObavestenja.ValueMember = "Item1";

            foreach(TDOffice.User.TipAutomatskogObavestenja t in korisnik.Tag.PrimaObavestenja.Keys)
            {
                SetujCheckStateTipaPrimanjaObavestenja(t, korisnik.Tag.PrimaObavestenja[t]);
            }

            magacini = Komercijalno.Magacin.ListAsync().Result;

            magacin_cmb.DataSource = magacini;
            magacin_cmb.DisplayMember = "Naziv";
            magacin_cmb.ValueMember = "ID";

            id_txt.Text = korisnik.ID.ToString();
            usernam_txt.Text = korisnik.Username;

            magacin_cmb.SelectedValue = korisnik.MagacinID;

            robaPopisPPIDNarudzbenice_txt.Text = korisnik.Tag.narudbenicaPPID.ToString();

            komercijalnoNalogID_txt.Text = korisnik.KomercijalnoUserID.ToStringOrDefault();
            UcitajListuPrava();

            _loaded = true;
        }
        private Task UcitajTaskoveAsync(bool popuniDGV = false)
        {
            return Task.Run(() =>
            {
                _tdTaskItems = TDOffice.TDTaskItem.ListByUserIDAsync(korisnik.ID);
                _tdTasksTable = Task.Run(() =>
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Naziv", typeof(string));

                    foreach (TDOffice.TDTask t in Enum.GetValues(typeof(TDOffice.TDTask)))
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = (int)t;
                        dr["Naziv"] = t.ToString().DivideOnCapital();
                        dt.Rows.Add(dr);
                    }

                    return dt;
                });

                Task.WaitAll(_tdTaskItems, _tdTasksTable);

                if (!popuniDGV)
                    return;

                Task.Run(() =>
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ID", typeof(int));
                    dt.Columns.Add("Task", typeof(string));
                    dt.Columns.Add("Interval", typeof(double));
                    dt.Columns.Add("Uradjen", typeof(string));

                    foreach (TDOffice.TDTaskItem item in _tdTaskItems.Result)
                    {
                        DataRow dr = dt.NewRow();
                        dr["ID"] = item.ID;
                        dr["Task"] = (TDOffice.TDTask)item.TDTaskID;
                        dr["Interval"] = item.Interval;
                        dr["Uradjen"] = item.Done == null ? "Nikada" : ((DateTime)item.Done).ToString("dd.MM.yyyy [HH:mm]");
                        dt.Rows.Add(dr);
                    }

                    this.Invoke((MethodInvoker)delegate
                    {
                        taskovi_dgv.DataSource = dt;

                        taskovi_dgv.Columns["ID"].Visible = false;

                        taskovi_dgv.Columns["Task"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                        taskovi_dgv.Columns["Task"].ReadOnly = true;

                        taskovi_dgv.Columns["Interval"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        taskovi_dgv.Columns["Interval"].ReadOnly = false;
                        taskovi_dgv.Columns["Interval"].DefaultCellStyle.BackColor = Color.LightYellow;
                        taskovi_dgv.Columns["Interval"].HeaderText = "Interval ( minuti )";

                        taskovi_dgv.Columns["Uradjen"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        taskovi_dgv.Columns["Uradjen"].ReadOnly = true;
                    });
                }).Wait();
            });
        }
        private void PopulateDGV()
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
                if (Convert.ToInt32(r.Cells["Status"].Value) == -1)
                    r.DefaultCellStyle.BackColor = Color.Coral;
        }
        private void SetujCheckStateTipaPrimanjaObavestenja(TDOffice.User.TipAutomatskogObavestenja tip, bool value)
        {
            for (int i = 0; i < clb_PrimalacObavestenja.Items.Count; i++)
            {
                Tuple<int, string> item = clb_PrimalacObavestenja.Items[i] as Tuple<int, string>;
                if (item.Item1 == (int)tip)
                {
                    clb_PrimalacObavestenja.SetItemChecked(i, value);
                    return;
                }
            }
        }
        private void UcitajListuPrava()
        {
            List<TDOffice.Pravo> prava = TDOffice.Pravo.List().Where(x => x.UserID == korisnik.ID).ToList();

            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Modul");
            dt.Columns.Add("Pravo");
            dt.Columns.Add("Status");

            foreach(TDOffice.PravoDefinicija pd in TDOffice.Pravo.definicijePrava)
            {
                TDOffice.Pravo pravo = prava.Where(x => x.ModulID == pd.ModulID).FirstOrDefault();
                DataRow dr = dt.NewRow();
                dr["ID"] = pravo == null ? -1 : pravo.ID;
                dr["Modul"] = pd.ModulID;
                dr["Pravo"] = pd.Naziv;
                dr["Status"] = pravo == null ? 0 : pravo.Value;
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;
            _baseDataTable = dt.Copy();

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["Modul"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            dataGridView1.Columns["Modul"].ReadOnly = true;
            dataGridView1.Columns["Pravo"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["Pravo"].ReadOnly = true;
            dataGridView1.Columns["Status"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            PopulateDGV();
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != dataGridView1.Rows[0].Cells["Status"].ColumnIndex)
                return;

            int pravoID = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            int modul = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Modul"].Value);
            int status = Convert.ToInt32(e.FormattedValue);

            if (pravoID == -1)
                TDOffice.Pravo.Insert(modul, status, korisnik.ID);
            else
            {
                TDOffice.Pravo p = TDOffice.Pravo.Get(pravoID);
                p.Value = status;
                p.Update();
            }

            TDOffice.Config<List<int>> config = TDOffice.Config<List<int>>.Get(TDOffice.ConfigParameter.OsvezavanjePrava);
            if (!config.Tag.Contains(korisnik.ID))
            {
                config.Tag.Add(korisnik.ID);
                config.UpdateOrInsert();
            }

            for(int i = 0; i < _baseDataTable.Rows.Count; i++)
            {
                if(Convert.ToInt32(_baseDataTable.Rows[i]["ID"]) == pravoID)
                {
                    _baseDataTable.Rows[i]["Status"] = status;
                    return;
                }
            }
        }

        private void magacin_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            korisnik.MagacinID = (int)magacin_cmb.SelectedValue;
            komercijalnoNalogIDSacuvaj_btn.Visible = true;
        }

        private void robaPopisPPIDNarudbeniceSacuvaj_btn_Click(object sender, EventArgs e)
        {
            try
            {
                korisnik.Tag.narudbenicaPPID = Convert.ToInt32(robaPopisPPIDNarudzbenice_txt.Text);
                korisnik.Update();
                robaPopisPPIDNarudbeniceSacuvaj_btn.Visible = false;
                MessageBox.Show("Izmene uspesno sacuvane!");
            }
            catch(Exception)
            {
                MessageBox.Show("Doslo je do greske prilikom izmene PPID-a narudzbenice!");
            }
        }

        private void robaPopisPPIDNarudzbenice_txt_TextChanged(object sender, EventArgs e)
        {
            robaPopisPPIDNarudbeniceSacuvaj_btn.Visible = true;
        }

        private void komercijalnoNalogID_txt_TextChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            korisnik.KomercijalnoUserID = Convert.ToInt32(komercijalnoNalogID_txt.Text);
            komercijalnoNalogIDSacuvaj_btn.Visible = true;
        }

        private void komercijalnoNalogIDSacuvaj_btn_Click(object sender, EventArgs e)
        {
            try
            {
                korisnik.Update();
                komercijalnoNalogIDSacuvaj_btn.Visible = false;
                MessageBox.Show("Izmene uspesno sacuvane!");
            }
            catch (Exception)
            {
                MessageBox.Show("Doslo je do greske prilikom izmene korisnika!");
            }
        }

        private void clb_PrimalacObavestenja_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            var a = clb_PrimalacObavestenja.SelectedIndex;

            Tuple<int, string> item = clb_PrimalacObavestenja.Items[clb_PrimalacObavestenja.SelectedIndex] as Tuple<int, string>;

            korisnik.Tag.PrimaObavestenja[(TDOffice.User.TipAutomatskogObavestenja)item.Item1] = clb_PrimalacObavestenja.GetItemChecked(clb_PrimalacObavestenja.SelectedIndex);
            korisnik.Update();
        }

        private void dodeliTask_btn_Click(object sender, EventArgs e)
        {
            using (DataGridViewSelectBox sb = new DataGridViewSelectBox(_tdTasksTable.Result))
            {
                sb.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                sb.RowHeaderVisible = false;
                sb.CloseOnSelect = true;
                sb.OnRowSelected += async (DataGridViewSelectBox.RowSelectEventArgs args) =>
                {
                    int id = Convert.ToInt32(args.SelectedRow["ID"]);

                    if(_tdTaskItems.Result.Count(x => x.TDTaskID == id) > 0)
                    {
                        MessageBox.Show("Korisnik vec ima dodeljen ovaj task!");
                        return;
                    }

                    TDOffice.TDTaskItem.Insert(id, korisnik.ID);
                    await UcitajTaskoveAsync(true);
                    MessageBox.Show("Task uspesno dodat!");
                };
                sb.ShowDialog();
            }
        }

        private void taskovi_dgv_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex != taskovi_dgv.Columns["Interval"].Index)
                return;

            double interval = Convert.ToDouble(e.FormattedValue);

            if (interval == Convert.ToDouble(taskovi_dgv.Rows[e.RowIndex].Cells["Interval"].Value))
                return;

            if(interval <= 0)
            {
                MessageBox.Show("Interval nije validan!");
                e.Cancel = true;
                return;
            }
            int itemID = Convert.ToInt32(taskovi_dgv.Rows[e.RowIndex].Cells["ID"].Value);
            var item = _tdTaskItems.Result.Where(x => x.ID == itemID).FirstOrDefault();
            item.Interval = interval;
            item.Update();
            UcitajTaskoveAsync();
            MessageBox.Show("Interval uspesno azuriran!");
        }

        private async void ukloniToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(taskovi_dgv.SelectedCells.Count == 0)
            {
                MessageBox.Show("Niste selektovali ni jedan task!");
                return;
            }

            if (MessageBox.Show("Da li sigurno zelite ukloniti ovaj task korisniku?", "Potvrdi", MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;

            int itemID = Convert.ToInt32(taskovi_dgv.Rows[taskovi_dgv.SelectedCells[0].RowIndex].Cells["ID"].Value);

            TDOffice.TDTaskItem.Delete(itemID);
            await UcitajTaskoveAsync(true);
            MessageBox.Show("Task uspesno uklonjen!");
        }

        private void grad_cmb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded)
                return;

            korisnik.Grad = Convert.ToInt32(grad_cmb.SelectedValue);
            komercijalnoNalogIDSacuvaj_btn.Visible = true;
        }

        private void pretragaPrava_txt_KeyDown(object sender, KeyEventArgs e)
        {
            string selectString = "";
            string input = pretragaPrava_txt.Text;
            string[] inputElemets = input.Split('+');

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                for (int i = 0; i < inputElemets.Length; i++)
                    selectString += "CONVERT(" + col.Name + ", System.String) LIKE '%" + inputElemets[i] + "%' AND ";

                selectString = selectString.Remove(selectString.Length - 4);
                selectString += " OR ";
            }

            selectString = selectString.Remove(selectString.Length - 4);

            DataRow[] rows = _baseDataTable.Copy().Select(selectString);
            dataGridView1.DataSource = rows == null || rows.Count() == 0 ? null : rows.CopyToDataTable();
        }

        private void dataGridView1_Sorted(object sender, EventArgs e)
        {
            PopulateDGV();
        }

        private void resetujSifru_txt_Click(object sender, EventArgs e)
        {
            using(InputBox ib = new InputBox("Nova Sifra", "Unesite novu sifru"))
            {
                ib.ShowDialog();
                if(string.IsNullOrWhiteSpace(ib.returnData))
                {
                    MessageBox.Show("Neispravna sifra");
                    return;
                }
                korisnik.Password = ib.returnData;
                korisnik.Update();
                MessageBox.Show("Sifra korisnika uspesno azurirana!");
            }
        }

        private void help_btn_Click(object sender, EventArgs e)
        {
            _helpForm.Result.ShowDialog();
        }
    }
}