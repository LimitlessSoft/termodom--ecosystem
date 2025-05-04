using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.TDOffice_v2
{
	public class GradDictionary : ReadOnlyDictionary<int, Grad>
	{
		public GradDictionary(IDictionary<int, Grad> dictionary)
			: base(dictionary) { }
	}
}
