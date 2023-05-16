using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
    public class ProizvodjacDictionary : ReadOnlyDictionary<string, Proizvodjac>
    {
        public ProizvodjacDictionary(IDictionary<string, Proizvodjac> dictionary) : base(dictionary)
        {
        }
    }
}
