namespace TD.Office.InterneOtpremnice.Contracts.Dtos.InterneOtpremnice;

public class InternaOtpremnicaItemDto
{
    public long Id { get; set; }
    public int RobaId { get; set; }
    public string Proizvod { get; set; }
    public string JM { get; set; }
    public decimal Kolicina { get; set; }
    public long InternaOtpremnicaId { get; set; }
}