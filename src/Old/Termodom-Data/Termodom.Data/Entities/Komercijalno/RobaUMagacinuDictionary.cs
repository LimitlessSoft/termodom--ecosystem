using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
	/// <summary>
	/// Key1 je magacinid, key2 je robaid, value je RobaUMagacinu
	/// </summary>
	public class RobaUMagacinuDictionary
		: ReadOnlyDictionary<int, ReadOnlyDictionary<int, RobaUMagacinu>>
	{
		public RobaUMagacinuDictionary(
			IDictionary<int, ReadOnlyDictionary<int, RobaUMagacinu>> dictionary
		)
			: base(dictionary) { }
	}
}
