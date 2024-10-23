namespace TD.Office.Public.Contracts.Dtos.Proracuni;

public class ProracunItemDto
{
    public long Id { get; set; }
    public int RobaId { get; set; }
    public string Naziv { get; set; }
    public decimal Kolicina { get; set; }
    public decimal CenaBezPdv { get; set; }
    public decimal Pdv { get; set; }
    public decimal Rabat { get; set; }
}
