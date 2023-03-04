using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Termodom.Data.Entities.DBSettings;
using Termodom.Data.Entities.Komercijalno;

namespace TDOffice_v2
{
    public partial class fm_StanjeRacuna_Index : Form
    {
        private Task<Dictionary<int, IzvodDictionary>> _izvodi { get; set; }
        private Task<List<ConnectionInfo>> _bazaConnections { get; set; }
        public fm_StanjeRacuna_Index()
        {
            InitializeComponent();
        }

        private void fm_StanjeRacuna_Index_Load(object sender, EventArgs e)
        {
            _bazaConnections = Komercijalno.BazaManager.ListAsync();

            _izvodi = Task.Run(async () =>
            {
                Dictionary<int, IzvodDictionary> dict = new Dictionary<int, IzvodDictionary>();
                foreach (ConnectionInfo dci in (await _bazaConnections).Where(x => x.Godina == DateTime.Now.Year).DistinctBy(x => x.PutanjaDoBaze))
                    dict.Add(dci.MagacinID, await UcitajIzvodeAsync(dci.MagacinID, dci.Godina));

                return dict;
            });

            _izvodi.ContinueWith(async (prev) =>
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Banka", typeof(string));
                dt.Columns.Add("Racun", typeof(string));
                dt.Columns.Add("Valuta", typeof(string));
                dt.Columns.Add("Poc. stanje Duguje", typeof(double));
                dt.Columns.Add("Poc. stanje Potrazuje", typeof(double));
                dt.Columns.Add("Duguje", typeof(double));
                dt.Columns.Add("Potrzuje", typeof(double));
                dt.Columns.Add("Stanje", typeof(double));

                // Stanje_Racuna > PPID
                Dictionary<int, IzvodDictionary> izvodi = await _izvodi;
                foreach(int magacinId in izvodi.Keys)
                {
                    foreach(string racun in izvodi[magacinId].Values.Select(x => x.ZiroRacun).Distinct())
                    {
                        //double duguje = izvodi
                        DataRow dr = dt.NewRow();
                        dr["Banka"] = $"{magacinId} - uvuci";
                        dr["Racun"] = $"{racun}";
                        dr["Valuta"] = $"Uvuci";
                        //dr["Poc. stanje Duguje"] = 
                        dt.Rows.Add(dr);
                    }
                }
            });
        }

        private async Task<IzvodDictionary> UcitajIzvodeAsync(int bazaId, int godinaBaze)
        {
            var response = await TDBrain_v3.GetAsync($"/komrecijalno/izvod/dictionary?bazaId={bazaId}&godinaBaze={godinaBaze}");

            switch((int)response.StatusCode)
            {
                case 200:
                    return JsonConvert.DeserializeObject<IzvodDictionary>(await response.Content.ReadAsStringAsync());
                case 500:
                    throw new Termodom.Data.Exceptions.APIServerException();
                default:
                    throw new Termodom.Data.Exceptions.APIUnhandledStatusException(response.StatusCode);
            }
        }
    }
}
