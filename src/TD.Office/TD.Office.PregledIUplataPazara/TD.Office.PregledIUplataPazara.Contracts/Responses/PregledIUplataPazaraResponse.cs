namespace TD.Office.PregledIUplataPazara.Contracts.Responses;

public class PregledIUplataPazaraResponse
{
    public double UkupnaRazlika { get; set; } = 0;
    public List<Item> Items { get; } = [];
    public class Item
    {
        public string Konto { get; set; }
        public string PozNaBroj { get; set; }
        public int MagacinId { get; set; }
        public DateTime Datum { get; set; }
        public double MPRacuni { get; set; }
        public double Povratnice { get; set; }
        public double ZaUplatu => MPRacuni - Povratnice;
        public double Potrazuje { get; set; }
        public double Razlika => Potrazuje - ZaUplatu;
    }
}