using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;

namespace TDOffice_v2
{
    public partial class fm_ListaRobe : Form
    {
        public List<int> robaUListi { get; set; } = new List<int>();
        public string nazivListe { get; set; } = "<<< jednokratna lista >>>";
        public int MagacinID { get; set; } = 50;

        #region IzborRobe
        private IzborRobe _izborRobeBuffer { get; set; } = null;
        private IzborRobe _izborRobe
        {
            get
            {
                // Ovde je pitanje da li treba stajati magacin 50
                if (_izborRobeBuffer == null)
                {
                    _izborRobeBuffer = new IzborRobe();
                    _izborRobeBuffer.MagacinID = MagacinID;
                    _izborRobeBuffer.OnRobaClickHandler += OnClickIzborRobe;
                }

                return _izborRobeBuffer;
            }
        }
        #endregion

        public fm_ListaRobe()
        {
            InitializeComponent();
            SetUI();
        }

        private void fm_ListaRobe_Load(object sender, EventArgs e)
        {
            NapuniDGV();
        }

        private void SetUI()
        {
            DataTable tempDataTable = new DataTable();
            tempDataTable.Columns.Add("RobaID", typeof(int));
            tempDataTable.Columns.Add("KatBr", typeof(string));
            tempDataTable.Columns.Add("KatBrPro", typeof(string));
            tempDataTable.Columns.Add("Naziv", typeof(string));
            tempDataTable.Columns.Add("GrupaID", typeof(string));
            tempDataTable.Columns.Add("Podgrupa", typeof(string));
            dataGridView1.DataSource = tempDataTable;

            dataGridView1.Columns["KatBr"].Width = 150;
            dataGridView1.Columns["KatBr"].HeaderText = "Kataloski broj";
            dataGridView1.Columns["KatBrPro"].HeaderText = "Kataloski broj proizvodjaca";
            dataGridView1.Columns["KatBrPro"].Width = 150;
            dataGridView1.Columns["Naziv"].Width = 300;
        }
        private void NapuniDGV()
        {
            DataTable dt = (dataGridView1.DataSource as DataTable).Clone();

            foreach(int rid in robaUListi)
            {
                DataRow dr = dt.NewRow();
                Komercijalno.Roba r = Komercijalno.Roba.Get(DateTime.Now.Year, rid);
                if(r == null)
                {
                    MessageBox.Show("Doslo je do greske. U listi postoji roba koje nema u sifarniku!");
                    return;
                }

                dr["RobaID"] = rid;
                dr["KatBr"] = r.KatBr;
                dr["KatBrPro"] = r.KatBrPro;
                dr["Naziv"] = r.Naziv;
                dr["GrupaID"] = r.GrupaID;
                dr["Podgrupa"] = r.Podgrupa;

                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;
        }

        private void OnClickIzborRobe(Komercijalno.RobaUMagacinu[] izabranaRoba)
        {
            bool duplikati = false;
            foreach (Komercijalno.RobaUMagacinu r in izabranaRoba)
            {
                if (robaUListi.Contains(r.RobaID))
                {
                    duplikati = true;
                    continue;
                }

                robaUListi.Add(r.RobaID);
            }
            nazivListe = "<<< jednokratna lista >>>";
            NapuniDGV();

            if (duplikati)
                MessageBox.Show("Pokusali ste dodati duplikati. Ta roba je zaobidjena!");
        }

        private void tsbtn_NovaStavka_Click(object sender, EventArgs e)
        {
        }
        private void obrisiToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach(DataGridViewRow row in dataGridView1.SelectedRows)
            {
                int robaID = Convert.ToInt32(row.Cells["RobaID"].Value);

                robaUListi.Remove(robaID);
            }

            NapuniDGV();
            nazivListe = "<<< jednokratna lista >>>";
        }
        private void obrisiSveStavkeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Da li zelite da obrisete sve stavke iz liste?", "Lista robe brisanje stavki", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                robaUListi.Clear();
                NapuniDGV();
            }
            nazivListe = "<<< jednokratna lista >>>";
        }
        private void tsbtn_SacuvajListu_Click(object sender, EventArgs e)
        {
            string destinationPath = "\\\\4monitor\\Liste\\";

            string listName = null;

            using (InputBox box = new InputBox("Ime liste", "Unesite ime liste"))
            {
                box.ShowDialog();
                listName = box.returnData.ToString();

                if(string.IsNullOrWhiteSpace(listName))
                {
                    MessageBox.Show("Ime liste nije ispravno!");
                    return;
                }

                if (File.Exists(destinationPath + listName + ".rlt"))
                    if (MessageBox.Show("Lista sa ovim imenom vec postoji. Da li ipak zelite sacuvati? Stara lista ce biti obrisana!") != DialogResult.Yes)
                        return;

                File.WriteAllLines(destinationPath + listName + ".rlt", Komercijalno.File.GenerisiRLT(robaUListi));
            }
        }
        private void tsbtn_OtvoriListu_Click(object sender, EventArgs e)
        {
            string filePath = null;

            openFileDialog1.InitialDirectory = "\\\\4monitor\\Liste\\";

            System.Windows.Forms.Application.OpenForms.OfType<Main>().FirstOrDefault().Invoke((MethodInvoker)delegate
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                    filePath = openFileDialog1.FileName;
            });

            if (filePath == null || !File.Exists(filePath))
                return;

            List<int> robaIDIzListe = new List<int>();

            if (Path.GetExtension(filePath) == ".rlt")
                robaIDIzListe = Komercijalno.File.ProcitajRLT(filePath);
            else
            {
                MessageBox.Show("Nepodrzan fajl " + Path.GetExtension(filePath));
                return;
            }

            if (this.dataGridView1.RowCount > 0)
            {
                if (MessageBox.Show("Da li zelite ukloniti postojece stavke pre otvaranja liste?", "Ukloniti stavke?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    robaUListi.Clear();
                    robaUListi.AddRange(robaIDIzListe);
                }
                else
                {
                    robaUListi.AddRange(robaIDIzListe);
                    robaUListi.Distinct().ToList();
                }
            }
            else
            {
                robaUListi.AddRange(robaIDIzListe);
                robaUListi.Distinct().ToList();
            }
            
            NapuniDGV();
            nazivListe = Path.GetFileName(openFileDialog1.FileName);
        }
        private void btn_DodajListu_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            _izborRobe.ShowDialog();
            NapuniDGV();
        }
    }
}
