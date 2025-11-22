namespace TD.Office.Public.Contracts.Dtos.Web;

public class KomercijalnoPriceKoeficijentiDto
{
	public class Item
	{
		public long Id { get; set; }
		public string Naziv { get; set; } = null!;
		public decimal Vrednost { get; set; }
	}

	public List<Item> Items { get; set; } = [];
}
