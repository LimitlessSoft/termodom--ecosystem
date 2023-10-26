using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
    public class PartnerDictionary : ReadOnlyDictionary<int, Partner>
    {
        public PartnerDictionary(IDictionary<int, Partner> dictionary) : base(dictionary)
        {
        }
    }
}
