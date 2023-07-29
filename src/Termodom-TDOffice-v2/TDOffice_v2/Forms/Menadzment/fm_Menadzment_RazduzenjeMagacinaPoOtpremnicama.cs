using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TDOffice_v2.Forms.Menadzment
{
    public partial class fm_Menadzment_RazduzenjeMagacinaPoOtpremnicama : Form
    {
        public fm_Menadzment_RazduzenjeMagacinaPoOtpremnicama()
        {
            InitializeComponent();
        }

        private void fm_Menadzment_RazduzenjeMagacinaPoOtpremnicama_Load(object sender, EventArgs e)
        {
            this.Enabled = false;
            UcitajUiAsync();
        }

        private async Task UcitajUiAsync()
        {
        }
    }
}
