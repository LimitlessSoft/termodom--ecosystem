using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
    public class BankaDictionary : ReadOnlyDictionary<int, Banka>
    {
        public BankaDictionary(IDictionary<int, Banka> dictionary) : base(dictionary)
        {
        }
    }
}
