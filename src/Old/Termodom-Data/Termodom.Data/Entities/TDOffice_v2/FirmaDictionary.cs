using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.TDOffice_v2
{
	public class FirmaDictionary : ReadOnlyDictionary<int, Firma>
	{
		public FirmaDictionary(IDictionary<int, Firma> dictionary)
			: base(dictionary) { }
	}
}
