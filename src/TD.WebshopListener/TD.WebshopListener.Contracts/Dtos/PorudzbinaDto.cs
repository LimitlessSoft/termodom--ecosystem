namespace TD.WebshopListener.Contracts.Dtos
{
    public class PorudzbinaDto
    {
        public int ID { get; set; }
        public int KorisnikID { get; set; }
        public int BrDokKomercijalno { get; set; }
        public DateTime Datum { get; set; }
        public int Status { get; set; }
        public int MagacinID { get; set; }
        public int? PPID { get; set; }
        public string? InterniKomentar { get; set; }
        public int? ReferentObrade { get; set; }
        public int NacinPlacanja { get; set; }
        public string? Hash { get; set; }
        public double K { get; set; }
        public double UstedaKorisnik { get; set; }
        public double UstedaKlijent { get; set; }
        public int Dostava { get; set; }
        public string? KomercijalnoInterniKomentar { get; set; }
        public string? KomercijalnoKomentar { get; set; }
        public string? KomercijalnoNapomena { get; set; }
        public string? Napomena { get; set; }
        public string? AdresaIsporuke { get; set; }
        public string? KontaktMobilni { get; set; }
        public string? ImeIPrezime { get; set; }
    }
}
