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
    public partial class fm_Magacin_List : Form
    {
        private Task<List<Komercijalno.Magacin>> _magacini = Komercijalno.Magacin.ListAsync(DateTime.Now.Year);
        private Task<fm_Help> _helpFrom { get; set; }

        public fm_Magacin_List()
        {
            InitializeComponent();
            _helpFrom = this.InitializeHelpModulAsync(Modul.Magacin_List);
        }

        private void fm_Magacin_List_Load(object sender, EventArgs e)
        {
            Task.Run(() =>
            {
                _magacini.Wait();

                DataTable dt = new DataTable();
                dt.Columns.Add("ID", typeof(int));
                dt.Columns.Add("Naziv", typeof(string));

                foreach (Komercijalno.Magacin m in _magacini.Result)
                {
                    DataRow dr = dt.NewRow();
                    dr["ID"] = m.ID;
                    dr["Naziv"] = m.Naziv;
                    dt.Rows.Add(dr);
                }

                this.Invoke((MethodInvoker)delegate
                {
                    dataGridView1.DataSource = dt;

                    dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                });
            });
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int magacinID = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ID"].Value);

            using(fm_Magacin_Index mi = new fm_Magacin_Index(magacinID))
            {
                mi.ShowDialog();
            }
        }
    }
}
