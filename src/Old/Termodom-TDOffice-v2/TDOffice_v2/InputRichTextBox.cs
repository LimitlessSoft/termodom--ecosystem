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
    public partial class InputRichTextBox : Form
    {
        public string ReturnData { get; set; }
        public InputRichTextBox(string title, string text)
        {
            InitializeComponent();
            this.TopMost = true;
            this.Text = title;
            this.textBox2.Text = text;
        }

        private void InputRichTextBox_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReturnData = richTextBox1.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
