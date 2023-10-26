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
    public partial class fm_TaskBoard_List : Form
    {
        private Task<List<TDOffice.Taskboard>> _taskboards = TDOffice.Taskboard.ListAsync();
        private Task<List<TDOffice.Taskboard.Item>> _lstitems = TDOffice.Taskboard.Item.ListAsync();

        public fm_TaskBoard_List()
        {
            InitializeComponent();
        }
        private void fm_TaskBoard_List_Load(object sender, EventArgs e)
        {
            UcitajTaskboardove();
        }
        private void fm_TaskBoard_List_Shown(object sender, EventArgs e)
        {
            ResizeTaskboardItems();
        }
        private void flowLayoutPanel1_SizeChanged(object sender, EventArgs e)
        {
            ResizeTaskboardItems();
        }

        private void ResizeTaskboardItems()
        {
            foreach (Button btn in flowLayoutPanel1.Controls.OfType<Button>())
                btn.Width = flowLayoutPanel1.Width - 6;
        }
        private void UcitajTaskboardove()
        {
            foreach(Button btn in flowLayoutPanel1.Controls.OfType<Button>())
                flowLayoutPanel1.Controls.Remove(btn);

            foreach (TDOffice.Taskboard t in _taskboards.Result)
            {
                //int n = _lstitems.Result.Where(x => x.TaskboardID == t.ID).ToList().Count;

                Button btn = new Button();
                btn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
                btn.Size = new System.Drawing.Size(flowLayoutPanel1.Width - 6, 50);
                btn.Text = t.Naziv + "  [" + _lstitems.Result.Where(x => x.TaskboardID == t.ID && x.Status == TDOffice.Taskboard.ItemStatus.Requested).ToList().Count.ToString() +"]";
                btn.Name = "taskboard" + t.ID + "_btn";
                btn.Click += (obj, args) =>
                {
                    using (fm_Taskboard_Index ti = new fm_Taskboard_Index(t.ID))
                        ti.ShowDialog();
                };
                flowLayoutPanel1.Controls.Add(btn);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!Program.TrenutniKorisnik.ImaPravo(1845001))
            {
                TDOffice.Pravo.NematePravoObavestenje(1845001);
                return;
            }
            using(InputBox ib = new InputBox("Novi taskboard", "Unesite naziv novog taskboard-a"))
            {
                ib.ShowDialog();

                if (ib.DialogResult != DialogResult.OK)
                    return;

                string naziv = ib.returnData;

                TDOffice.Taskboard.Insert(naziv, Program.TrenutniKorisnik.ID);

                _taskboards = TDOffice.Taskboard.ListAsync();

                UcitajTaskboardove();

                MessageBox.Show("Novi taskboard uspesno dodat!");
            }
        }
    }
}
