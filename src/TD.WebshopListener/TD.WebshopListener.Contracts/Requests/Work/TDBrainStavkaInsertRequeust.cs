namespace TD.WebshopListener.Contracts.Requests.Work
{
    public class TDBrainStavkaInsertRequeust
    {
        public int BazaId { get; set; }
        public int? GodinaBaze { get; set; }
        public int VrDok { get; set; }
        public int BrDok { get; set; }
        public int RobaId { get; set; }
        public double Kolicina { get; set; }
        public double? ProdajnaCenaBezPdv { get; set; }
        public double Rabat { get; set; }
    }
}
