using System.ComponentModel.DataAnnotations.Schema;

namespace TD.Komercijalno.Contracts.Dtos.Namene
{
	public class NamenaDto
	{
		public int Id { get; set; }
		public string Naziv { get; set; }
		public short Redosled { get; set; }
		public string Napomena { get; set; }
		public short PPO { get; set; }
		public short PDV { get; set; }
	}
}
