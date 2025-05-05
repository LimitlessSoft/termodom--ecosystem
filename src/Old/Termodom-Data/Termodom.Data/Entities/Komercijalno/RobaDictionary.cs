using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
	public class RobaDictionary : ReadOnlyDictionary<int, Roba>
	{
		/// <summary>
		/// Kreira kolekciju robe
		/// </summary>
		/// <param name="dict"></param>
		public RobaDictionary(IDictionary<int, Roba> dict)
			: base(dict) { }
	}
}
