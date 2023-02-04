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
    public partial class Input_CheckedListBox : Form
    {
        public List<Tuple<int, string>> DataSource
        {
            get
            {
                return this.checkedListBox1.DataSource as List<Tuple<int, string>>;
            }
            set
            {
                this.checkedListBox1.DataSource = value;
                this.checkedListBox1.ValueMember = "Item1";
                this.checkedListBox1.DisplayMember = "Item2";
            }
        }
        public bool CheckOnClick
        {
            get
            {
                return this.checkedListBox1.CheckOnClick;
            }
            set
            {
                this.checkedListBox1.CheckOnClick = value;
            }
        }

        public List<int> CheckedValues { get; set; } = new List<int>();

        public Input_CheckedListBox()
        {
            InitializeComponent();
        }

        private void Input_CheckedListBox_Load(object sender, EventArgs e)
        {

        }

        private void potvrdi_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Input_CheckedListBox_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
                CheckedValues.Add((checkedListBox1.Items[e.Index] as Tuple<int, string>).Item1);
            else
                CheckedValues.RemoveAll(x => x == (checkedListBox1.Items[e.Index] as Tuple<int, string>).Item1);
        }

        private void cekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, true);
        }

        private void decekirajSveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                checkedListBox1.SetItemChecked(i, false);
        }
    }
}
