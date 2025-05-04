using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.TDOffice_v2
{
	public class PopisDictionary : ReadOnlyDictionary<int, Popis>
	{
		public PopisDictionary(IDictionary<int, Popis> dict)
			: base(dict) { }
	}
}
