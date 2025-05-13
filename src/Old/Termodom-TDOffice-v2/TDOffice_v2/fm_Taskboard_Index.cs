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
	public partial class fm_Taskboard_Index : Form
	{
		private int _taskboardID { get; set; }

		private Task<List<TDOffice.Taskboard.Item>> _items { get; set; }

		public fm_Taskboard_Index(int taskboardID)
		{
			InitializeComponent();
			_taskboardID = taskboardID;
			_items = TDOffice.Taskboard.Item.ListByTaskboardAsync(taskboardID);
		}

		private void fm_Taskboard_Index_Load(object sender, EventArgs e) { }

		private void fm_Taskboard_Index_Shown(object sender, EventArgs e)
		{
			UcitajTaskove();
		}

		private void UcitajTaskove()
		{
			foreach (Button btn in flowLayoutPanel1.Controls.OfType<Button>())
				flowLayoutPanel1.Controls.Remove(btn);

			foreach (TDOffice.Taskboard.Item t in _items.Result)
			{
				Button btn = new Button();
				btn.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
				btn.Size = new System.Drawing.Size(flowLayoutPanel1.Width - 6, 50);
				btn.Text = t.Naslov;
				btn.BackColor = TDOffice.Taskboard.Item.COLORS_BY_ITEM_STATUS[t.Status];
				btn.Name = "task" + t.ID + "_btn";
				btn.Click += (obj, args) =>
				{
					using (fm_Taskboard_Item_Index ti = new fm_Taskboard_Item_Index(t))
						ti.ShowDialog();

					//UcitajTaskove();
				};
				flowLayoutPanel1.Controls.Add(btn);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (!Program.TrenutniKorisnik.ImaPravo(1845002))
			{
				TDOffice.Pravo.NematePravoObavestenje(1845002);
				return;
			}
			using (fm_Taskboard_Item_New itn = new fm_Taskboard_Item_New(_taskboardID))
				itn.ShowDialog();

			UcitajTaskove();
		}
	}
}
