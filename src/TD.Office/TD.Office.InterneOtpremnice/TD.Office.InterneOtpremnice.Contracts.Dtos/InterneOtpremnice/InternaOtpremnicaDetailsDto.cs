namespace TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;

public class InternaOtpremnicaDetailsDto
{
    public long Id { get; set; }
    public int MagacinId { get; set; }
    public int DestinacioniMagacinId { get; set; }
    public int State { get; set; }
    public DateTime CreatedAt { get ; set ; }
    public string Referent { get; set; }
    public string? KomercijalnoDokument { get ; set ; }
    public List<InternaOtpremnicaItemDto> Items { get; set; } = [];
}