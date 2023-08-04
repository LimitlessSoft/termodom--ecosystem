namespace TD.FE.TDOffice.Contracts.Requests.MenadzmentRazduzenjeMagacinaPoOtpremnicama
{
    public class RazduziMagacinRequest
    {
        public PripremaDokumenataRequest Izvor { get; set; }
        public bool NoviDokument { get; set; }
        public int DestinacijaVrDok { get; set; }
        public int? DestinacijaBrDok { get; set; }
        public int? DestinacijaMagacinId { get; set; }
        public int? DestinacijaNacinPlacanja { get; set; }
        public int? DestinacijaNamena { get; set; }
        public int? DestinacijaReferent { get; set; }
        public int? DestinacijaZaposleni { get; set; }
        public int NakonAkcijePostaviIzvornimNacinPlacanjaNa { get; set; }
    }
}
