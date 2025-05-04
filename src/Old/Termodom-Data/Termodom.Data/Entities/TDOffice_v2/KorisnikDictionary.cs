using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.TDOffice_v2
{
	public class KorisnikDictionary : ReadOnlyDictionary<string, Korisnik>
	{
		public KorisnikDictionary(IDictionary<string, Korisnik> dictionary)
			: base(dictionary) { }
	}
}
