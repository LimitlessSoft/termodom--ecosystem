using Newtonsoft.Json;
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
    public partial class fm_PodesavanjePromenjivih_List : Form
    {
        private List<TDOffice.ConfigParameter> _listaPromenjivih = new List<TDOffice.ConfigParameter>()
        {
            TDOffice.ConfigParameter.Undefined0,
            TDOffice.ConfigParameter.Undefined1,
            TDOffice.ConfigParameter.ConnectionStringsKomercijalno,
            TDOffice.ConfigParameter.ConnectionStringConfig,
            TDOffice.ConfigParameter.KoeficijentMinZalihe,
            TDOffice.ConfigParameter.KoeficijentPrekomernihZaliha,
            TDOffice.ConfigParameter.TekuciRacunZaCekove
        };
        private List<TDOffice.Config<string>> _listaConfiga { get; set; } = TDOffice.Config<string>.ListRaw();
        private bool _loaded { get; set; } = false;

        public fm_PodesavanjePromenjivih_List()
        {
            if (!Program.TrenutniKorisnik.ImaPravo(3))
            {
                TDOffice.Pravo.NematePravoObavestenje(3);
                this.Close();
                return;
            }
            InitializeComponent();
        }
        private void fm_PodesavanjePromenjivih_List_Load(object sender, EventArgs e)
        {
            UcitajPodatke();
            _loaded = true;
        }

        private void UcitajPodatke()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Opis", typeof(string));
            dt.Columns.Add("Vrednost", typeof(string));

            List<string> neuspesnePromenjive = new List<string>();

            foreach (TDOffice.ConfigParameter promenjiva in _listaPromenjivih)
            {
                TDOffice.Config<string> conf = _listaConfiga.FirstOrDefault(x => x.ID == (int)promenjiva);

                if(conf == null)
                {
                    neuspesnePromenjive.Add($"[{(int)promenjiva}] {promenjiva.ToString()}");
                    continue;
                }

                DataRow dr = dt.NewRow();
                dr["ID"] = (int)promenjiva;
                dr["Opis"] = promenjiva.ToString();
                dr["Vrednost"] = conf.Tag;
                dt.Rows.Add(dr);
            }
            dataGridView1.DataSource = dt;

            dataGridView1.Columns["ID"].ReadOnly = true;
            dataGridView1.Columns["ID"].Width = 50;

            dataGridView1.Columns["Opis"].ReadOnly = true;
            dataGridView1.Columns["Opis"].Width = 300;

            dataGridView1.Columns["Vrednost"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            if(neuspesnePromenjive.Count > 0)
                MessageBox.Show("Neuspesne Promenjive:" + string.Join(Environment.NewLine, neuspesnePromenjive));
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (!_loaded)
                return;

            if (e.ColumnIndex != dataGridView1.Columns["Vrednost"].Index)
                return;

            if (e.FormattedValue == dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["Vrednost"].Value)
                return;

            int id = Convert.ToInt32(dataGridView1.Rows[dataGridView1.SelectedCells[0].RowIndex].Cells["ID"].Value);

            TDOffice.Config<string> conf = TDOffice.Config<string>.GetRaw(id);
            conf.Tag = e.FormattedValue.ToString();
            conf.UpdateOrInsert(true);

            MessageBox.Show("Vrednost za parametar " + _listaPromenjivih.FirstOrDefault(x => (int)x == conf.ID).ToString() + " je uspesno azurirana!");
        }
    }
}
