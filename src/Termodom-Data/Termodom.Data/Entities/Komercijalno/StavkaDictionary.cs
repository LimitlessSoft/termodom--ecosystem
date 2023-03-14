using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
    public class StavkaDictionary : ReadOnlyDictionary<int, Stavka>
    {
        public StavkaDictionary(IDictionary<int, Stavka> dictionary) : base(dictionary) { }
    }
}
