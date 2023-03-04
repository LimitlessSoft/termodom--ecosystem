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
using Termodom.Data.Entities.DBSettings;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
    public partial class fm_ObracunIUplataPazara : Form
    {
        private Task<List<DistinctConnectionInfo>> _distinctPutanjeDoBaza { get; set; }

        private Task<List<Komercijalno.Magacin>> _komercijalnoMagacini { get; set; } = Task.Run(() =>
        {
            return Komercijalno.Magacin.ListAsync().Result.Where(x => new int[] { 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28 }.Contains(x.ID)).ToList();
        });
        public fm_ObracunIUplataPazara()
        {
            InitializeComponent();
        }

        private void fm_ObracunIUplataPazara_Load(object sender, EventArgs e)
        {
            _distinctPutanjeDoBaza = BazaManager.DistinctConnectionInfoListAsync();
            _distinctPutanjeDoBaza.ContinueWith(async (prev) =>
            {
                List<Tuple<string, string>> list = new List<Tuple<string, string>>();

                foreach (DistinctConnectionInfo csi in await _distinctPutanjeDoBaza)
                {
                    string[] putanjaParts = csi.PutanjaDoBaze.Split("/");
                    list.Add(new Tuple<string, string>(csi.PutanjaDoBaze, $"{csi.Godina} - {putanjaParts[putanjaParts.Length - 1]}"));
                }

                this.Invoke((MethodInvoker)delegate
                {
                    cmb_Baza.DataSource = new List<Tuple<string, string>>(list);
                    cmb_Baza.DisplayMember = "Item2";
                    cmb_Baza.ValueMember = "Item1";
                });
            });
        }
    }
}
