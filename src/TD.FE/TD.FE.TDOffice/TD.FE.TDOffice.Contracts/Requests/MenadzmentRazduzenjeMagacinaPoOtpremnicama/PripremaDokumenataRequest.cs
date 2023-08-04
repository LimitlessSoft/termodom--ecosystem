namespace TD.FE.TDOffice.Contracts.Requests.MenadzmentRazduzenjeMagacinaPoOtpremnicama
{
    public class PripremaDokumenataRequest
    {
        public int MagacinId { get; set; }
        public int VrDok { get; set; }
        public DateTime OdDatuma { get; set; }
        public DateTime DoDatuma { get; set; }
        public int Namena { get; set; }
        public int NacinPlacanja { get; set; }
    }
}
