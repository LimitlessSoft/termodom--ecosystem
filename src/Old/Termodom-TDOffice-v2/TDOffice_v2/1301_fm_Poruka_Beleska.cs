using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TDOffice_v2.TDOffice;

namespace TDOffice_v2
{
    public partial class _1301_fm_Poruka_Beleska : Form
    {
        public Poruka poruka = null;
        public _1301_fm_Poruka_Beleska(Poruka poruka)
        {
            if (poruka == null)
                throw new NullReferenceException();

            InitializeComponent();
            this.poruka = poruka;
            richTextBox1.Text = this.poruka.Tag.Beleska;
        }

        private void sacuvaj_btn_Click(object sender, EventArgs e)
        {
            this.poruka.Tag.Beleska = richTextBox1.Text;
            this.poruka.Update();

            MessageBox.Show("Poruka uspesno sacuvana");
        }

        private void _1301_fm_Poruka_Beleska_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.poruka.Tag.Beleska != richTextBox1.Text)
            {
                if (MessageBox.Show("Izmenili ste belesku. Da li je zelite sacuvati pre izlaska?", "Sacuvati?", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.poruka.Tag.Beleska = richTextBox1.Text;
                    this.poruka.Update();
                }
            }
        }

        private void zatvori_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
