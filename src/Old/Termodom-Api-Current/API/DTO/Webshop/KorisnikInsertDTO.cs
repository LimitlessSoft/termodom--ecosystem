using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTO.Webshop
{
	public class KorisnikInsertDTO
	{
		public string Ime { get; set; }
		public string PW { get; set; }
		public Models.KorisnikTip Tip { get; set; }
		public string Nadimak { get; set; }
		public string Mobilni { get; set; }
		public string Komentar { get; set; }
		public string Mail { get; set; }
		public string AdresaStanovanja { get; set; }
		public string Opstina { get; set; }
		public Int16 MagacinID { get; set; }
		public DateTime DatumRodjenja { get; set; }
		public string PIB { get; set; }
		public Int16 Zanimanje { get; set; }
		public int Poseta { get; set; }
		public bool PrimaObavestenja { get; set; }
		public Int16? Referent { get; set; }
		public DateTime? DatumOdobrenja { get; set; }
		public DateTime? PoslednjiPutVidjen { get; set; }
	}
}
