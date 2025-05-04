using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
	public class MagacinDictionary : ReadOnlyDictionary<int, Magacin>
	{
		public MagacinDictionary(IDictionary<int, Magacin> dictionary)
			: base(dictionary) { }
	}
}
