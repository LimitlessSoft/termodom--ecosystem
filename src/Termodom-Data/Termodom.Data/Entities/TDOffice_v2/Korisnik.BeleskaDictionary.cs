using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.TDOffice_v2
{
    public partial class Korisnik
    {
        public class BeleskaDictionary : ReadOnlyDictionary<int, Beleska>
        {
            public BeleskaDictionary(IDictionary<int, Beleska> dictionary) : base(dictionary)
            {
            }
        }
    }
}
