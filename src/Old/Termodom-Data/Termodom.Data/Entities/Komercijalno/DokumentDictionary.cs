using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
	/// <summary>
	/// Dictionary koji sadrzi dokumente.
	/// Prvi key je vrsta dokumenta, drugi key je broj dokumenta, value je objekat dokumenta
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DokumentDictionary : ReadOnlyDictionary<int, Dictionary<int, Dokument>>
	{
		public DokumentDictionary(IDictionary<int, Dictionary<int, Dokument>> dictionary)
			: base(dictionary) { }
	}
}
