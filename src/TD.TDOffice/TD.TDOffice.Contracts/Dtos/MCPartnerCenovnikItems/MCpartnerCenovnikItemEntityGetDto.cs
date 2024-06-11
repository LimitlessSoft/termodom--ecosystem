namespace TD.TDOffice.Contracts.Dtos.MCPartnerCenovnikItems;

public class MCpartnerCenovnikItemEntityGetDto
{
    public long Id { get; set; }
    public string KatBr { get; set; }
    public double VpCenaBezRabata { get; set; }
    public double Rabat { get; set; }
    public int PPID { get; set; }
    public DateTime VaziOdDana { get; set; }
}