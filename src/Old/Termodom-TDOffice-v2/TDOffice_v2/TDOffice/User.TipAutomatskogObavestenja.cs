using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TDOffice_v2.TDOffice
{
	public partial class User
	{
		public enum TipAutomatskogObavestenja
		{
			NakonRazduzenjaRobe = 0,
			NakonZameneRobe = 1,
			NakonZakljucavanjaPopisa = 2,
			NakonKreiranjaNovogPartnera = 3,
			PravoPristupaModulu = 4,
			PrimaObavestenjaOObnoviBonusaZakljucavanja = 5
		}
	}
}
