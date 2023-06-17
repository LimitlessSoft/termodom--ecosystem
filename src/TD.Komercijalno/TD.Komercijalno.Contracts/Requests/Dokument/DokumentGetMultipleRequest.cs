namespace TD.Komercijalno.Contracts.Requests.Dokument
{
    public class DokumentGetMultipleRequest
    {
        public int? VrDok { get; set; }
        public string? IntBroj { get; set; }
        public short? KodDok { get; set; }
        public short? Flag { get; set; }
        public DateTime? DatumOd { get; set; }
        public DateTime? DatumDo { get; set; }
        public string? Linked { get; set; }
        public short? MagacinId { get; set; }
        public int? PPID { get; set; }
    }
}
