using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TDOffice_v2
{
    public partial class InputBox : Form
    {
        public string returnData;

        public InputBox(string Title, string Text)
        {
            InitializeComponent();
            this.TopMost = true;
            this.Text = Title;
            this.textBox2.Text = Text;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public static string Show(string Title, string Text)
        {
            using (InputBox ib = new InputBox(Title, Text))
            {
                ib.ShowDialog();
                return ib.returnData;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            returnData = textBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
            {
                button1.PerformClick();
                e.Handled = true;
            }
        }
    }
}
