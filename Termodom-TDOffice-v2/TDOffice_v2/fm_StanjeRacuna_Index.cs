using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
    public partial class fm_StanjeRacuna_Index : Form
    {
        private Task<IzvodDictionary> _izvodi { get; set; }
        public fm_StanjeRacuna_Index()
        {
            InitializeComponent();
        }

        private void fm_StanjeRacuna_Index_Load(object sender, EventArgs e)
        {
            _izvodi = UcitajIzvode();
        }

        private void UcitajIzvode()
        {

        }
    }
}
