using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
	public class MestoDictionary : ReadOnlyDictionary<string, Mesto>
	{
		/// <summary>
		/// Kreira kolekciju mesta
		/// </summary>
		/// <param name="dict"></param>
		public MestoDictionary(IDictionary<string, Mesto> dict)
			: base(dict) { }
	}
}
