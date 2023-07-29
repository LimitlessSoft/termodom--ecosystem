using TD.Komercijalno.Contracts.Requests.Dokument;

namespace TD.Komercijalno.Contracts.Requests.Stavke
{
    public class StavkaGetMultipleRequest
    {
        public int[]? VrDok { get; set; }
        public int[]? MagacinId { get; set; }
        public DokumentGetMultipleRequest? DokumentFilter { get; set; }
    }
}
