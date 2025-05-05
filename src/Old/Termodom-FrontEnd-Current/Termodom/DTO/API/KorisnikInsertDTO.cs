using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Termodom.DTO.API
{
	public class KorisnikInsertDTO
	{
		public string Ime { get; set; }
		public string PW { get; set; }
		public int Tip { get; set; }
		public string Nadimak { get; set; }
		public string Mobilni { get; set; }
		public string Komentar { get; set; }
		public string Mail { get; set; }
		public string AdresaStanovanja { get; set; }
		public string Opstina { get; set; }
		public int MagacinID { get; set; }
		public DateTime DatumRodjenja { get; set; }
		public string PIB { get; set; }
		public int Zanimanje { get; set; }
	}
}
