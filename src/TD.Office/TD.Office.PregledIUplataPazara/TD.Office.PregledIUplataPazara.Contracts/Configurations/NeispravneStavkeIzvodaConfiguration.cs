namespace TD.Office.PregledIUplataPazara.Contracts.Configurations;

public class NeispravneStavkeIzvodaConfiguration
{
	public Dictionary<int, Item> Items { get; set; } = [];

	public class Item
	{
		public int PPID { get; set; }
		public int PozivNaBrojPoslednjeTriCifre { get; set; }
		public int KontoPoslednjeDveCifre { get; set; }
		public int KontoPrveTriCifre { get; set; }
	}
}
