using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TD.TDOffice.Contracts.Dtos.DokumentTagizvod
{
	public class DokumentTagIzvodGetDto
	{
		public int Id { get; set; }
		public int BrojDokumentaIzvoda { get; set; }
		public decimal UnosPocetnoStanje { get; set; }
		public decimal UnosPotrazuje { get; set; }
		public decimal UnosDuguje { get; set; }
		public int Korisnik { get; set; }
	}
}
