using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
	public class IzvodDictionary : ReadOnlyDictionary<int, Izvod>
	{
		public IzvodDictionary(IDictionary<int, Izvod> dictionary)
			: base(dictionary) { }
	}
}
