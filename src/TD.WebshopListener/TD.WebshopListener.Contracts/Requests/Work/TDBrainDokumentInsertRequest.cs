namespace TD.WebshopListener.Contracts.Requests.Work
{
    public class TDBrainDokumentInsertRequest
    {
        public int BazaId { get; set; }
        public int? GodinaBaze { get; set; }
        public int VrDok { get; set; }
        public int MagacinId { get; set; }
        public string? InterniBroj { get; set; }
        public int? PPID { get; set; }
        public int NuId { get; set; }
        public int? KomercijalnoKorisnikId { get; set; }
        public bool DozvoliDaljeIzmeneUKomercijalnom { get; set; } = true;
    }
}
