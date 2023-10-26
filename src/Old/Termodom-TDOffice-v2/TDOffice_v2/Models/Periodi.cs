using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.Models
{
    public class Periodi
    {
        public class Period
        {
            public DateTime OdDatuma { get; set; }
            public DateTime DoDatuma { get; set; }
            public Period()
            {

            }
            public Period(DateTime odDatuma, DateTime doDatuma)
            {
                this.OdDatuma = odDatuma;
                this.DoDatuma = doDatuma;
            }
        }

        [JsonProperty]
        private Dictionary<int, Period> _list = new Dictionary<int, Period>();

        public int Count => _list.Count;
        public int[] Godine => _list.Keys.Select(x => x).ToArray();

        public Period this[int godina] { get => _list[godina]; set => _list[godina] = value; }

        public Periodi()
        {

        }
        public void Add(int godina, Period item)
        {
            _list[godina] = item;
        }
        public void Clear()
        {
            _list.Clear();
        }
        public bool Remove(int godina)
        {
            return _list.Remove(godina);
        }
    }
}
