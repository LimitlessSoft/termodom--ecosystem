using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Termodom.Models
{
	public partial class Porudzbina
	{
		public class DodatniInfo
		{
			public double K { get; set; }
			public double UstedaKorisnik { get; set; }
			public double UstedaKlijent { get; set; }
			public bool Dostava { get; set; }
			public string KomercijalnoInterniKomentar { get; set; }
			public string KomercijalnoKomentar { get; set; }
			public string KomercijalnoNapomena { get; set; }
			public string Napomena { get; set; }
			public string AdresaIsporuke { get; set; }
			public string KontaktMobilni { get; set; }
			public string ImeIPrezime { get; set; }
		}
	}
}
