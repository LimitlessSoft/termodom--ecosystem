namespace TD.Office.PregledIUplataPazara.Contracts.Responses;

public class PregledIUplataPazaraNeispravneStavkeIzvodaResponse
{
	public List<Item> Items { get; set; } = [];

	public class Item
	{
		public string Firma { get; set; }
		public int BrojIzvoda { get; set; }
		public int PPID { get; set; }
		public string Opis { get; set; }

		public Item(string firma, int brojIzvoda, int ppid, string opis)
		{
			Firma = firma;
			BrojIzvoda = brojIzvoda;
			PPID = ppid;
			Opis = opis;
		}
	}
}
