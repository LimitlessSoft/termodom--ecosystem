using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.TDOffice_v2
{
    public class FiskalniRacunDictionary : ReadOnlyDictionary<string, FiskalniRacun>
    {
        public FiskalniRacunDictionary(IDictionary<string, FiskalniRacun> dictionary) : base(dictionary)
        {
        }
    }
}
