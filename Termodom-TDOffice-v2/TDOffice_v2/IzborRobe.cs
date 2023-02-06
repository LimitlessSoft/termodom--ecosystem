using FirebirdSql.Data.FirebirdClient;
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
    public partial class IzborRobe : Form
    {
        #region Buffer
        private Task<List<Komercijalno.Roba>> _roba { get; set; }
        private Task<List<Komercijalno.RobaUMagacinu>> _robaUMagacinu { get; set; }
        #endregion

        private IzborRobe_DodatniFilteri _dodatniFilterForm { get; set; } = null;

        public delegate void OnRobaClick(Komercijalno.RobaUMagacinu[] robaUMagacinu);

        public OnRobaClick OnRobaClickHandler;
        public bool PrikaziKolonuCena = true;
        public bool DozvoliMultiSelect {
            get
            {
                return _dozvoliMultiselect;
            }
            set
            {
                _dozvoliMultiselect = value;
                dataGridView1.MultiSelect = value;
            }
        }
        private bool _dozvoliMultiselect = false;

        /// <summary>
        /// Kada se pritisne X da li da dispozuje formu ili ga samo sakrije
        /// </summary>
        public bool DisposeOnClose { get => _disposeOnClose; set => _disposeOnClose = value; }
        private bool _disposeOnClose = true;

        private bool _dozvoliMenjanjeMagacina = false;
        public bool DozvoliMenjanjeMagacina
        {
            get
            {
                return _dozvoliMenjanjeMagacina;
            }
            set
            {
                _dozvoliMenjanjeMagacina = value;

                cmb_Magacini.Enabled = value;
            }
        }

        private int? _magacinID = null;
        public int? MagacinID
        {
            get
            {
                return _magacinID;
            }
            set
            {
                if (value > 0)
                {
                    karticaRobeToolStripMenuItem.Enabled = true;
                    _magacinID = value;
                    _robaUMagacinu = Komercijalno.RobaUMagacinu.ListByMagacinIDAsync((int)value);
                }
                else
                {
                    karticaRobeToolStripMenuItem.Enabled = false;
                    _magacinID = null;
                    _robaUMagacinu = Komercijalno.RobaUMagacinu.ListAsync();
                }

                cmb_Magacini.SelectedValue = _magacinID == null ? -1 : _magacinID;

                InitializeBaseData();
                UpdateDGV();
            }
        }
        private DataTable baseDataTable { get; set; } = null;
        private DataTable dataGridViewDataTable { get; set; } = null;

        private Task _UISetup { get; set; }

        private bool _loaded { get; set; } = false;
        public IzborRobe()
        {
            InitializeComponent();

            _dodatniFilterForm = new IzborRobe_DodatniFilteri();
            _dodatniFilterForm.OnFilterChanged += OnDodatniFilterFilterChanged;

            LoadDataAsync();
            _UISetup = SetUIAsync().ContinueWith((prev) =>
            {
                this.Invoke((MethodInvoker)delegate
                {
                    InitializeBaseData();
                    UpdateDGV();
                });
            });
        }

        public IzborRobe(int magacinID)
        {
            InitializeComponent();

            _dodatniFilterForm = new IzborRobe_DodatniFilteri();
            _dodatniFilterForm.OnFilterChanged += OnDodatniFilterFilterChanged;

            LoadDataAsync();
            _UISetup = SetUIAsync().ContinueWith((prev) =>
            {
                this.Invoke((MethodInvoker) delegate
                {
                    MagacinID = magacinID;
                });
            });
        }

        private async Task SetUIAsync()
        {
            karticaRobeToolStripMenuItem.Enabled = false;
            DataTable tempData = new DataTable();
            tempData.Columns.Add("ID", typeof(int));
            tempData.Columns.Add("KatBr", typeof(string));
            tempData.Columns.Add("KatBrPro", typeof(string));
            tempData.Columns.Add("Naziv", typeof(string));
            tempData.Columns.Add("JM", typeof(string));
            tempData.Columns.Add("Cena", typeof(double));
            tempData.Columns.Add("GrupaID", typeof(string));
            tempData.Columns.Add("Podgrupa", typeof(int));
            tempData.Columns.Add("ProID", typeof(string));
            tempData.Columns.Add("DOB_PPID", typeof(int));

            dataGridView1.DataSource = tempData;

            foreach (DataColumn col in tempData.Columns)
                comboBox1.Items.Add(col.ColumnName);

            comboBox1.SelectedItem = "Naziv";

            List<Komercijalno.Magacin> magacini = await Komercijalno.Magacin.ListAsync();
            magacini.Add(new Komercijalno.Magacin() { ID = -1, Naziv = "Sifarnik Robe" });
            magacini.Sort((x, y) => x.ID.CompareTo(y.ID));
            cmb_Magacini.DataSource = magacini;
            cmb_Magacini.DisplayMember = "Naziv";
            cmb_Magacini.ValueMember = "ID";
            cmb_Magacini.Enabled = _dozvoliMenjanjeMagacina;

            dataGridViewDataTable = tempData.Clone();
        }
        private void LoadDataAsync()
        {
            _roba = Komercijalno.Roba.ListAsync(DateTime.Now.Year);
        }
        private void InitializeBaseData()
        {
            DataTable temp = dataGridViewDataTable.Clone();

            List<object[]> data = new List<object[]>();
            object dataLock = new object();

            List<Komercijalno.RobaUMagacinu> robaUMagacinuZaDatiMgacinID = MagacinID == null ? null : _robaUMagacinu.Result;
            List<int> robaIDeviDatogMagacina = MagacinID == null ? null : robaUMagacinuZaDatiMgacinID.Select(z => z.RobaID).ToList();
            List<Komercijalno.Roba> robaKojaPripadaDatomMagacinuID = MagacinID == null ? _roba.Result : _roba.Result.Where(x => robaIDeviDatogMagacina.Any(y => y == x.ID)).ToList();

            Parallel.ForEach(robaKojaPripadaDatomMagacinuID, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, r =>
            {
                Komercijalno.RobaUMagacinu rum = MagacinID == null ? null : robaUMagacinuZaDatiMgacinID.Where(x => x.RobaID == r.ID).FirstOrDefault();

                object[] o = new object[10];

                o[0] = r.ID;
                o[1] = r.KatBr;
                o[2] = r.KatBrPro;
                o[3] = r.Naziv;
                o[4] = r.JM;
                o[5] = rum == null ? -1 : rum.ProdajnaCena;
                o[6] = r.GrupaID;
                o[7] = r.Podgrupa == null ? -1 : r.Podgrupa;
                o[8] = r.ProID;
                o[9] = r.DOB_PPID;

                lock (dataLock)
                {
                    data.Add(o);
                }
            });
            
            foreach (object[] o in data)
                temp.Rows.Add(o);

            dataGridViewDataTable = temp;
            baseDataTable = temp;
        }
        private void UpdateDGV()
        {
            dataGridView1.DataSource = dataGridViewDataTable;

            if (dataGridView1.Rows.Count == 0)
                return;

            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["KatBr"].Width = 150;
            dataGridView1.Columns["KatBrPro"].Width = 150;
            dataGridView1.Columns["Naziv"].Width = 300;
            dataGridView1.Columns["JM"].Width = 50;
            dataGridView1.Columns["JM"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridView1.Columns["Cena"].Width = 80;
            dataGridView1.Columns["Cena"].Visible = MagacinID == null ? false : PrikaziKolonuCena;
            dataGridView1.Columns["Cena"].DefaultCellStyle.Format = "#,##0.00";
            dataGridView1.Columns["Cena"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            slogova_lbl.Text = "Slogova: " + dataGridView1.Rows.Count;

            this.dataGridView1.Sort(this.dataGridView1.Columns["Naziv"], ListSortDirection.Ascending);
        }


        private void IzborRobe_Load(object sender, EventArgs e)
        {
            _loaded = true;
        }
        private void IzborRobe_Shown(object sender, EventArgs e)
        {
            textBox1.Focus();
            textBox1.SelectAll();
        }

        private void FilterCtrlA()
        {
            string selectString = "";
            string input = textBox1.Text;
            string[] inputElemets = input.Split('+');

            foreach (object o in comboBox1.Items)
            {
                for (int i = 0; i < inputElemets.Length; i++)
                    selectString += "CONVERT(" + o.ToString() + ", System.String) LIKE '%" + inputElemets[i] + "%' AND ";

                selectString = selectString.Remove(selectString.Length - 4);
                selectString += " OR ";
            }

            selectString = selectString.Remove(selectString.Length - 4);

            DataRow[] rows = baseDataTable.Copy().Select(selectString);
            dataGridViewDataTable = rows == null || rows.Count() == 0 ? null : rows.CopyToDataTable();

            UpdateDGV();
        }
        private void FilterEnter()
        {
            dataGridView1.ClearSelection();
            string kolona = comboBox1.SelectedItem.ToString();
            string input = textBox1.Text;

            if (string.IsNullOrWhiteSpace(input))
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = 0;
                dataGridView1.Rows[0].Selected = true;
                dataGridView1.Focus();
                dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["Naziv"];
                return;
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string vrednostCelije = row.Cells[kolona].Value.ToString();
                if(vrednostCelije.ToLower().IndexOf(input.ToLower()) == 0)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = row.Index > 0 ? row.Index - 1 : 0;
                    dataGridView1.Rows[row.Index].Selected = true;
                    dataGridView1.Focus();
                    dataGridView1.CurrentCell = dataGridView1.Rows[row.Index].Cells["Naziv"];
                    return;
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            List<int> selectedRobaID = new List<int>();
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                if (row != null)
                    selectedRobaID.Add(Convert.ToInt32(row.Cells["ID"].Value));

            if (OnRobaClickHandler != null)
                OnRobaClickHandler(_robaUMagacinu.Result.Where(x => selectedRobaID.Contains(x.RobaID)).DistinctBy(z => z.RobaID).ToArray());

            dataGridView1.ClearSelection();

            textBox1.Focus();
            textBox1.SelectAll();
        }
        private void OnDodatniFilterFilterChanged(IzborRobe_DodatniFilteri.OnFilterChangedArgs args)
        {
            string gotovSelect = "";

            if(args.PodgrupaID != null)
                gotovSelect += $" Podgrupa = '{args.PodgrupaID}' AND";
            else
                if (args.GrupaID != null)
                    gotovSelect += $" GrupaID = '{args.GrupaID}' AND";

            if (args.ProID != null && args.ProID != "-1")
                gotovSelect += $" ProID = '{args.ProID}' AND";

            if (args.DobavljacID != null)
                gotovSelect += $" DOB_PPID = '{args.DobavljacID}' AND";


            List<int> robaInFilter = new List<int>();

            List<Komercijalno.Magacin> magacini = Komercijalno.Magacin.ListAsync().Result;
            List<Komercijalno.RobaUMagacinu> robaUMagacinu = Komercijalno.RobaUMagacinu.BufferedList(TimeSpan.FromMinutes(60))
                                                                    .Where(x => x.MagacinID == (magacini.Count(y => y.ID == MagacinID) == 0 ? 50 : MagacinID))
                                                                    .ToList();


            if (args.IzvorRobe != IzborRobe_DodatniFilteri.DodatniFilterIzvorRobe.NULL)
            {
                if(args.IzvorRobe == IzborRobe_DodatniFilteri.DodatniFilterIzvorRobe.NePopisanaRoba)
                {
                    List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.List(DateTime.Now.Year);
                    robaInFilter.AddRange(robaUMagacinu
                            .Where(x => 
                                !(stavke
                                    .Where(y => y.VrDok == 7 && y.MagacinID == MagacinID)
                                    .Select(y => y.RobaID))
                            .Contains(x.RobaID))
                            .Select(x => x.RobaID));
                }
                else if(args.IzvorRobe == IzborRobe_DodatniFilteri.DodatniFilterIzvorRobe.RobaSaPrometom)
                {
                    DateTime prometOd = (args.IzvorRobeTag as Tuple<DateTime, DateTime>).Item1;
                    DateTime prometDo = (args.IzvorRobeTag as Tuple<DateTime, DateTime>).Item2;
                    List<Komercijalno.Stavka> stavke = Komercijalno.Stavka.List(DateTime.Now.Year, "MAGACINID = " + MagacinID);

                    List<Komercijalno.Dokument> dokumenti = Komercijalno.Dokument.List()
                        .Where(x =>
                            new int[] { 13, 15 }.Contains(x.VrDok) &&
                            x.MagacinID == MagacinID &&
                            x.Datum >= prometOd &&
                            x.Datum <= prometDo)
                        .ToList();

                    robaInFilter.AddRange(
                            stavke
                                .Where(y => dokumenti.Any(z => z.VrDok == y.VrDok && z.BrDok == y.BrDok))
                                .Select(y => y.RobaID));
                }
                else
                {
                    List<int> robaIDDokumenta = TDOffice.StavkaPopis.List().Where(x => x.BrDok == (int)args.IzvorRobeTag).Select(x => x.RobaID).ToList();
                    foreach(Komercijalno.RobaUMagacinu rum in robaUMagacinu)
                    {
                        if (robaIDDokumenta.Contains(rum.RobaID))
                            continue;

                        robaInFilter.Add(rum.RobaID);
                        robaIDDokumenta.Remove(rum.RobaID);
                    }
                }
            }

            if (args.StanjeRobe != IzborRobe_DodatniFilteri.DodatniFilterTrenutnoStanjeRobe.NULL)
            {
                robaInFilter.AddRange(robaUMagacinu
                    .Where(x =>
                        args.StanjeRobe == IzborRobe_DodatniFilteri.DodatniFilterTrenutnoStanjeRobe.ImaNaStanju ?
                            x.Stanje > 0 :
                            x.Stanje == 0)
                    .Select(x => x.RobaID));
            }

            if (args.OptimalneKriticneZalihe != IzborRobe_DodatniFilteri.DodatniFilterKriticneOptimalneZalihe.NULL)
            {
                robaInFilter.AddRange(robaUMagacinu
                    .Where(x =>
                        args.OptimalneKriticneZalihe == IzborRobe_DodatniFilteri.DodatniFilterKriticneOptimalneZalihe.IspodKriticnihZaliha ?
                            x.Stanje < x.KriticneZalihe :
                            x.Stanje < x.OptimalneZalihe)
                    .Select(x => x.RobaID));
            }

            if (robaInFilter.Count > 0)
                gotovSelect += $" ID in ({string.Join(",", robaInFilter)}) AND";


            if (!string.IsNullOrEmpty(gotovSelect))
                gotovSelect = gotovSelect.Remove(gotovSelect.Length - 4);

            DataRow[] rows = dataGridViewDataTable.Copy().Select(gotovSelect);

            if (rows.Count() > 0)
                dataGridView1.DataSource = rows.CopyToDataTable();
            else
                dataGridView1.DataSource = null;

            slogova_lbl.Text = "Slogova: " + rows.Count().ToString();

            MessageBox.Show(this, "Dodatni filter primenjen u izboru robe. Izlistano " + rows.Count().ToString() + " stavki.");
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                FilterEnter();
                dataGridView1.Focus();
            }
            else if(e.Control && e.KeyCode == Keys.A)
            {
                FilterCtrlA();
                dataGridView1.Focus();
            }
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow r in dataGridView1.Rows)
                r.Visible = true;
        }
        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                List<int> selectedRobaID = new List<int>();
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    if (row != null)
                        selectedRobaID.Add(Convert.ToInt32(row.Cells["ID"].Value));

                if (OnRobaClickHandler != null)
                    OnRobaClickHandler(_robaUMagacinu.Result.Where(x => selectedRobaID.Contains(x.RobaID)).DistinctBy(z => z.RobaID).ToArray());

                dataGridView1.ClearSelection();

                textBox1.Focus();
                textBox1.SelectAll();
                e.Handled = true;
            }
        }
        private void jednostavnoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (InputBox ib = new InputBox("Filtriraj jednostavno", "Unesite broj stavki koji zelite nasumicno da prikazem."))
            {
                ib.ShowDialog();
                double vrednost = 0;
                try
                {
                    vrednost = Convert.ToInt32(ib.returnData);
                }
                catch (Exception)
                {
                    MessageBox.Show("Neispravan broj!");
                }

                DataTable tempTable = dataGridViewDataTable.Clone();
                List<int> storedIndex = new List<int>();
                int nTries = 10;

                for(int i = 0; i < Math.Min(vrednost, dataGridViewDataTable.Rows.Count); i++)
                {
                    int rowIndex = Random.Next(0, dataGridViewDataTable.Rows.Count);

                    if(storedIndex.Contains(rowIndex))
                    {
                        nTries--;
                        if (nTries == 0)
                            break;
                    }

                    tempTable.Rows.Add(dataGridViewDataTable.Rows[rowIndex].ItemArray);
                }

                dataGridView1.DataSource = tempTable;
                slogova_lbl.Text = "Slogova: " + tempTable.Rows.Count.ToString();
            }
        }
        private void grupisiPoPopisnimGrupamaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int vrednost = 0;
            using (InputBox ib = new InputBox("Filtriraj specijal", "Unesite broj stavki koji zelite nasumicno da prikazem."))
            {
                ib.ShowDialog();
                try
                {
                    vrednost = Convert.ToInt32(ib.returnData);
                }
                catch (Exception)
                {
                    MessageBox.Show("Neispravan broj!");
                }
            }

            string popisnaGrupa = null;
            while (string.IsNullOrWhiteSpace(popisnaGrupa))
            {
                List<Komercijalno.Roba> krl = Komercijalno.Roba.List(DateTime.Now.Year);
                Komercijalno.Roba kr = krl[Random.Next(0, krl.Count)];
                string[] els = kr.KatBrPro.Split('?');
                if (els != null && els.Count() > 1 && !string.IsNullOrWhiteSpace(els[1]))
                    popisnaGrupa = els[1]; // ovde uzimamo broj bez ?
            }

            DataRow[] rows = dataGridViewDataTable.Copy().Select("KatBrPro LIKE '%?" + popisnaGrupa + "%'").Take(vrednost).ToArray();
            if (rows.Count() > 0)
                dataGridView1.DataSource = rows.CopyToDataTable();
            else
                dataGridView1.DataSource = null;

            slogova_lbl.Text = "Slogova: " + rows.Count().ToString();
        }
        private void tsbtn_DodatniFilter_Click(object sender, EventArgs e)
        {
            _dodatniFilterForm.ShowDialog();
        }
        private async void cmb_Magacini_SelectedIndexChanged(object sender, EventArgs e)
        {
            await _UISetup;
            if (!_loaded)
                return;

            MagacinID = Convert.ToInt32(cmb_Magacini.SelectedValue);
        }
        private void btn_ctrlA_Click(object sender, EventArgs e)
        {
            FilterCtrlA();
        }

        private void poslednje4GodineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MagacinID == null || MagacinID <= 0)
            {
                MessageBox.Show("Morate selektovati magacin!");
                return;
            }

            if (MessageBox.Show("Izdvojicete robu koja ima neslaganja za magacin " + Komercijalno.Magacin.Get(DateTime.Now.Year, (int)MagacinID).Naziv + ". Nastaviti?", "Potvrdi", MessageBoxButtons.YesNo) == DialogResult.No)
                return;


            this.Enabled = false;
            Thread t1 = new Thread(() =>
            {
                slogova_lbl.Text = "Ucitavam podatke iz baze...";

                int year = DateTime.Now.Year;

                Dictionary<string, List<Komercijalno.Stavka>> stavke = new Dictionary<string, List<Komercijalno.Stavka>>();
                stavke[year.ToString()] = Komercijalno.Stavka.ListByMagacinID((int)MagacinID);

                for (int i = 1; i < 4; i++)
                {
                    using (FbConnection con = new FbConnection(Komercijalno.Komercijalno.CONNECTION_STRING[year - i]))
                    {
                        con.Open();
                        stavke[(year - i).ToString()] = Komercijalno.Stavka.ListByMagacinID(con, (int)MagacinID);
                    }
                }

                DataTable dt = baseDataTable.Clone();

                for (int i = 0; i < 4; i++)
                {
                    dt.Columns.Add("PS_" + (year - 3 + i), typeof(double));
                    dt.Columns.Add("KS_" + (year - 3 + i), typeof(double));
                }

                List<int> vrDokKojiPovecavajuStanje = new List<int>() {
                    0,
                    1,
                    2,
                    3,
                    11,
                    12,
                    16,
                    18,
                    22,
                    26,
                    30,
                    992
                };
                List<int> vrDokKojiSmanjujuStanje = new List<int>() {
                    13,
                    14,
                    15,
                    17,
                    19,
                    23,
                    28,
                    29,
                    35,
                    993
                };
                int index = 0;

                Form f = this;

                DataRow[] baseRows = baseDataTable.Copy().Select();
                List<Dictionary<string, object>> finalData = new List<Dictionary<string, object>>();

                object dataLock = new object();

                Parallel.ForEach(baseRows, new ParallelOptions() { MaxDegreeOfParallelism = 4 }, baseRow =>
                {
                    f.Invoke((MethodInvoker)delegate {
                        slogova_lbl.Text = "Racunam stanja (1/2): " + index + " / " + baseRows.Length;
                    });
                    int robaID = Convert.ToInt32(baseRow["ID"]);

                    Dictionary<string, object> dr = new Dictionary<string, object>();

                    for (int i = 0; i < 4; i++)
                    {
                        dr["PS_" + (year - i)] = stavke[(year - i).ToString()].Where(x => x.RobaID == robaID && x.VrDok == 0).Sum(x => x.Kolicina);
                        double ulazi = stavke[(year - i).ToString()].Where(x => x.RobaID == robaID && vrDokKojiPovecavajuStanje.Contains(x.VrDok)).Sum(x => x.Kolicina);
                        double izlazi = stavke[(year - i).ToString()].Where(x => x.RobaID == robaID && vrDokKojiSmanjujuStanje.Contains(x.VrDok)).Sum(x => x.Kolicina);
                        dr["KS_" + (year - i)] = ulazi - izlazi;
                    }

                    dr["ID"] = baseRow["ID"];
                    dr["KatBr"] = baseRow["KatBr"];
                    dr["KatBrPro"] = baseRow["KatBrPro"];
                    dr["Naziv"] = baseRow["Naziv"];
                    dr["JM"] = baseRow["JM"];
                    dr["Cena"] = baseRow["Cena"];
                    dr["GrupaID"] = baseRow["GrupaID"];
                    dr["Podgrupa"] = baseRow["Podgrupa"];
                    dr["Proid"] = baseRow["Proid"];
                    dr["DOB_PPID"] = baseRow["DOB_PPID"];

                    lock (dataLock)
                    {
                        finalData.Add(dr);
                    }
                    index++;
                });

                index = 0;
                foreach(Dictionary<string, object> o in finalData)
                {
                    f.Invoke((MethodInvoker)delegate {
                        slogova_lbl.Text = "Racunam stanja (2/2): " + index + " / " + baseRows.Length;
                    });
                    DataRow dr = dt.NewRow();

                    foreach (string key in o.Keys)
                        dr[key] = o[key];

                    dt.Rows.Add(dr);
                    index++;
                }

                index = 0;
                for(int r = dt.Rows.Count - 1; r >= 0; r--)
                { 
                    this.Invoke((MethodInvoker)delegate
                    {
                        slogova_lbl.Text = "Trazim nepravilnosti... " + index + " / " + baseDataTable.Rows.Count;
                    });
                    DataRow dr = dt.Rows[r];
                    for (int i = 0; i < 3; i++)
                    {

                        // Radim kao int da bi uklonio decimale. To je tolerancija
                        int pocetnoStanje = Convert.ToInt32(dr["PS_" + (year - i)]);
                        int krajnjeStanje = Convert.ToInt32(dr["KS_" + (year - i - 1)]);

                        if (pocetnoStanje == krajnjeStanje)
                        {
                            dt.Rows.RemoveAt(r);
                            break;
                        }
                    }

                    index++;
                }

                this.Invoke((MethodInvoker)delegate
                {
                    dataGridView1.DataSource = dt.Clone();
                    for (int i = 0; i < 4; i++)
                    {
                        dataGridView1.Columns["PS_" + (year - i)].DefaultCellStyle.Format = "#,##0.00";
                        dataGridView1.Columns["KS_" + (year - i)].DefaultCellStyle.Format = "#,##0.00";
                    }

                    dataGridView1.DataSource = dt;
                    slogova_lbl.Text = "Slogova: " + dt.Rows.Count;
                    this.Enabled = true;

                });
            });
            t1.IsBackground = true;
            t1.Start();
        }

        public void Reload()
        {
            LoadDataAsync();
            UpdateDGV();
        }

        private void IzborRobe_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_disposeOnClose && e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void istorijaNabavkeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int robaID = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);
            using(fm_IstorijaCene_Index ist = new fm_IstorijaCene_Index(robaID))
                if(!ist.IsDisposed)
                    ist.ShowDialog();
        }

        private void karticaRobeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int robaID = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);

            using (_7_fm_Komercijalno_Roba_Kartica ist = new _7_fm_Komercijalno_Roba_Kartica(robaID, (int)MagacinID))
                if(!ist.IsDisposed)
                    ist.ShowDialog();
        }

        private void detaljiORobiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int robaID = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);
            using (fm_Roba_Index fi = new fm_Roba_Index(robaID))
                if(!fi.IsDisposed)
                    fi.ShowDialog();
        }
    }
}
