using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
    public class StanjeRacunaDictionary : ReadOnlyDictionary<string, StanjeRacuna>
    {
        public StanjeRacunaDictionary(IDictionary<string, StanjeRacuna> dictionary) : base(dictionary)
        {
        }
    }
}
