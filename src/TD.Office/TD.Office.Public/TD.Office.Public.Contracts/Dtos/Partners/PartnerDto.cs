namespace TD.Office.Public.Contracts.Dtos.Partners;

public class PartnerDto
{
    public int Ppid { get; set; }
    public string Naziv { get; set; }
    public string? Adresa { get; set; }
    public string? Posta { get; set; }
    public string? Pib { get; set; }
    public string? Mobilni { get; set; }
    public DateTime CreatedAt { get; set; }
}
