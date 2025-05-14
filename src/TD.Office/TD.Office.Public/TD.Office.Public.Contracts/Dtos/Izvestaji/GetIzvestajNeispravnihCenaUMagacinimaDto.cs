namespace TD.Office.Public.Contracts.Dtos.Izvestaji;

public class GetIzvestajNeispravnihCenaUMagacinimaDto
{
	public class Item
	{
		public string Baza { get; set; }
		public int MagacinId { get; set; }
		public long RobaId { get; set; }
		public string Opis { get; set; }
	}

	public List<Item> Items { get; set; }
}
