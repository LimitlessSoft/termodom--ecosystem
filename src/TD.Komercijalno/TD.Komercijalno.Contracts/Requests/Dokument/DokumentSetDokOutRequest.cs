namespace TD.Komercijalno.Contracts.Requests.Dokument;

public class DokumentSetDokOutRequest
{
    public int VrDok { get; set; }
    public int BrDok { get; set; }
    public short? VrDokOut { get; set; }
    public int? BrDokOut { get; set; }
}