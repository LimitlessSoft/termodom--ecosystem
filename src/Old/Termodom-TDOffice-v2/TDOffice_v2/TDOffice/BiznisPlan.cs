using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
    public class BiznisPlan
    {
        public int MagacinID { get; set; }
        public double PlaniraniPromet { get; set; } = 0;
        public Dictionary<string, double> PlaniraniTroskovi { get; set; } = new Dictionary<string, double>();

        public BiznisPlan()
        {

        }

        public void UpdateOrInsert()
        {
            Config<Dictionary<int, BiznisPlan>> c = Config<Dictionary<int, BiznisPlan>>.Get(ConfigParameter.BiznisPlan);
            if (c == null)
            {
                c = new Config<Dictionary<int, BiznisPlan>>();
                c.Tag = new Dictionary<int, BiznisPlan>();
                c.UpdateOrInsert();
            }

            if (c.Tag == null)
            {
                c.Tag = new Dictionary<int, BiznisPlan>();
                c.UpdateOrInsert();
            }
            c.Tag[MagacinID] = this;
            c.UpdateOrInsert();
        }

        public static BiznisPlan Get(int magacinID)
        {
            Config<Dictionary<int, BiznisPlan>> c = Config<Dictionary<int, BiznisPlan>>.Get(ConfigParameter.BiznisPlan);
            if(c == null)
            {
                c = new Config<Dictionary<int, BiznisPlan>>();
                c.Tag = new Dictionary<int, BiznisPlan>();
                c.UpdateOrInsert();
            }

            if(c.Tag == null)
            {
                c.Tag = new Dictionary<int, BiznisPlan>();
                c.UpdateOrInsert();
            }

            if (c.Tag.ContainsKey(magacinID))
                return c.Tag[magacinID];

            return new BiznisPlan() { MagacinID = magacinID };
        }
    }
}
