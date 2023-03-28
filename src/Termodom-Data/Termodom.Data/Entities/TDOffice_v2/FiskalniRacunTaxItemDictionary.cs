using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.TDOffice_v2
{
    /// <summary>
    /// 
    /// </summary>
    public class FiskalniRacunTaxItemDictionary : ReadOnlyDictionary<string, FiskalniRacunTaxItem>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        public FiskalniRacunTaxItemDictionary(IDictionary<string, FiskalniRacunTaxItem> dictionary) : base(dictionary)
        {
        }
    }
}
