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
    public partial class fm_MagacinUPopisu_Index : Form
    {
        private Task<List<Komercijalno.Magacin>> _magacini { get; set; } = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);
        private Task<List<TDOffice.MagacinClan>> _clanoviMagacina { get; set; } = TDOffice.MagacinClan.ListAsync();
        private Task<TDOffice.Config<List<int>>> _config { get; set; } = TDOffice.Config<List<int>>.GetAsync(TDOffice.ConfigParameter.MagacinUPopisu);
        private Task<fm_Help> _helpFrom { get; set; }

        public fm_MagacinUPopisu_Index()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.MagacinUPopisu_Index);
        }
        private void fm_MagacinUPopisu_Index_Load(object sender, EventArgs e)
        {
            LoadMagacinUPopisu();
        }

        private void LoadMagacinUPopisu()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("MAGACINID", typeof(int));
            dt.Columns.Add("Magacin", typeof(string));
            dt.Columns.Add("UPopisu", typeof(int));
            dt.Columns.Add("PovezanihNaloga", typeof(int));

            foreach (Komercijalno.Magacin m in _magacini.Result)
            {
                DataRow dr = dt.NewRow();
                dr["MAGACINID"] = m.ID;
                dr["Magacin"] = m == null ? "Unknown" : m.Naziv;
                dr["UPopisu"] = _config.Result.Tag.Contains(m.ID) ? 1 : 0;
                dr["PovezanihNaloga"] = _clanoviMagacina.Result.Count(x => x.MagacinID == m.ID);
                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["MAGACINID"].ReadOnly = true;
            dataGridView1.Columns["MAGACINID"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

            dataGridView1.Columns["Magacin"].ReadOnly = true;
            dataGridView1.Columns["Magacin"].Width = 200;

            dataGridView1.Columns["UPopisu"].Width = 65;
            dataGridView1.Columns["UPopisu"].HeaderText = "U popisu";

            dataGridView1.Columns["PovezanihNaloga"].ReadOnly = true;
            dataGridView1.Columns["PovezanihNaloga"].Width = 65;
            dataGridView1.Columns["PovezanihNaloga"].HeaderText = "Povezanih Naloga";
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "UPopisu")
            {
                int formattedValue = Convert.ToInt32(e.FormattedValue);

                if (formattedValue != 0 && formattedValue != 1)
                    e.Cancel = true;
            }
        }

        private void dataGridView1_CellValidated(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "UPopisu")
            {
                int up = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["UPopisu"].Value);
                int mid = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["MAGACINID"].Value);

                TDOffice.Config<List<int>> config = TDOffice.Config<List<int>>.Get(TDOffice.ConfigParameter.MagacinUPopisu);

                if (!config.Tag.Contains(mid) && up == 1)
                    config.Tag.Add(mid);
                else if (config.Tag.Contains(mid) && up == 0)
                    config.Tag.Remove(mid);

                config.UpdateOrInsert();

                _config = Task.Run(() =>
                {
                    return config;
                });
            }
        }

        private void btnPrimeni_Click(object sender, EventArgs e)
        {
            TDOffice.Config<List<int>> config = TDOffice.Config<List<int>>.Get(TDOffice.ConfigParameter.MagacinUPopisu);
            List<TDOffice.MagacinClan> clanoviMagacina = TDOffice.MagacinClan.List();
            foreach (Komercijalno.Magacin m in _magacini.Result)
            {
                foreach(TDOffice.MagacinClan clan in clanoviMagacina.Where(x => x.MagacinID == m.ID))
                {
                    Komercijalno.PravaZap pzap = Komercijalno.PravaZap.Get(clan.KorisnikID, 1, 10017);
                    if (pzap == null)
                        continue;

                    pzap.V = config.Tag.Contains(m.ID) ? (Int16)0 : (Int16)1;
                    pzap.Update();
                }
            }
            LoadMagacinUPopisu();
        }
    }
}
