using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
	public class VrstaDokDictionary : ReadOnlyDictionary<int, VrstaDok>
	{
		public VrstaDokDictionary(IDictionary<int, VrstaDok> dictionary)
			: base(dictionary) { }
	}
}
