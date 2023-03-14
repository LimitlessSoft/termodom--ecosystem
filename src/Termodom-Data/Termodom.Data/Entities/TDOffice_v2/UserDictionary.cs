using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.TDOffice_v2
{
    public class UserDictionary : ReadOnlyDictionary<int, User>
    {
        public UserDictionary(IDictionary<int, User> dictionary) : base(dictionary)
        {
        }
    }
}
