using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Termodom.Data.Entities.Komercijalno
{
	public class TekuciRacunList : ReadOnlyCollection<TekuciRacun>
	{
		public TekuciRacunList(IList<TekuciRacun> list)
			: base(list) { }
	}
}
