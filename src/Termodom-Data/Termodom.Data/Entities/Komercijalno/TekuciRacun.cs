namespace Termodom.Data.Entities.Komercijalno
{
    public class TekuciRacun
    {
        public string Racun { get; set; }
        public int PPID { get; set; }
        public int BankaID { get; set; }
        public string Valuta { get; set; }
        public double Stanje { get; set; }
        public int? MagacinID { get; set; }
    }
}
