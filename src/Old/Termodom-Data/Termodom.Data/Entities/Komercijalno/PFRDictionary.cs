using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
	/// <summary>
	///
	/// </summary>
	public class PFRDictionary : ReadOnlyDictionary<int, PFR>
	{
		/// <summary>
		///
		/// </summary>
		/// <param name="dictionary"></param>
		public PFRDictionary(IDictionary<int, PFR> dictionary)
			: base(dictionary) { }
	}
}
