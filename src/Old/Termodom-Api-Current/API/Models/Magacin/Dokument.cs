using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Models.Magacin
{
	public class Dokument
	{
		public int VrDok { get; set; }
		public int BrDok { get; set; }
		public DateTime Datum { get; set; }
		public int KorisnikID { get; set; }
	}
}
