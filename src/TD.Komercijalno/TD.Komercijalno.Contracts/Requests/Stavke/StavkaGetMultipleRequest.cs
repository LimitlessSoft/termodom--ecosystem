using TD.Komercijalno.Contracts.Requests.Dokument;

namespace TD.Komercijalno.Contracts.Requests.Stavke
{
    public class StavkaGetMultipleRequest
    {
        public long[]? VrDok { get; set; }
        public long[]? MagacinId { get; set; }
        public DokumentGetMultipleRequest? DokumentFilter { get; set; }
    }
}
