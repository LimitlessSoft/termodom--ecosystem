using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Termodom.Data.Entities.Komercijalno
{
    public class TarifaDictionary : ReadOnlyDictionary<string, Tarifa>
    {
        public TarifaDictionary(IDictionary<string, Tarifa> dictionary) : base(dictionary) { }
    }
}
