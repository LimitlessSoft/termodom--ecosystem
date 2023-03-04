using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TDOffice_v2.Komercijalno;
using TDOffice_v2.TDOffice;
using Termodom.Data.Entities.DBSettings;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
    public partial class fm_ObracunIUplataPazara : Form
    {
        private Task<MagacinDictionary> _komercijalnoMagacini { get; set; } = Komercijalno.Magacin.DictionaryAsync();

        public fm_ObracunIUplataPazara()
        {
            InitializeComponent();
        }

        private void fm_ObracunIUplataPazara_Load(object sender, EventArgs e)
        {
            _komercijalnoMagacini.ContinueWith(async (prev) =>
            {
                List<Tuple<int, string>> list = new List<Tuple<int, string>>();

                foreach(Termodom.Data.Entities.Komercijalno.Magacin m in (await _komercijalnoMagacini).Values)
                    list.Add(new Tuple<int, string>(m.ID, m.Naziv));

                this.Invoke((MethodInvoker) delegate
                {
                    clb_Magacini.DataSource = list;
                    clb_Magacini.DisplayMember = "Item2";
                    clb_Magacini.ValueMember = "Item1";
                });
            });
        }

        private void btn_Prikazi_Click(object sender, EventArgs e)
        {

        }
    }
}
