namespace TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;
public class GetSpecifikacijaNovcaDto
{
    public long Id { get; set; }
    public long MagacinId { get; set; }
    public DateTime Datum { get; set; }
    public string? Komentar { get; set; }
    public int Eur1Komada { get; set; }
    public double Eur1Kurs { get; set; }
    public int Eur2Komada { get; set; }
    public double Eur2Kurs { get; set; }
    public int Novcanica5000Komada { get; set; }
    public int Novcanica2000Komada { get; set; }
    public int Novcanica1000Komada { get; set; }
    public int Novcanica500Komada { get; set; }
    public int Novcanica200Komada { get; set; }
    public int Novcanica100Komada { get; set; }
    public int Novcanica50Komada { get; set; }
    public int Novcanica20Komada { get; set; }
    public int Novcanica10Komada { get; set; }
    public int Novcanica5Komada { get; set; }
    public int Novcanica2Komada { get; set; }
    public int Novcanica1Komada { get; set; }
    public double Kartice { get; set; }
    public string? KarticeKomentar { get; set; }
    public double Cekovi { get; set; }
    public string? CekoviKomentar { get; set; }
    public double Papiri { get; set; }
    public string? PapiriKomentar { get; set; }
    public double Troskovi { get; set; }
    public string? TroskoviKomentar { get; set; }
    public double Vozaci { get; set; }
    public string? VozaciKomentar { get; set; }
    public double Sasa { get; set; }
    public string? SasaKomentar { get; set; }
    public SpecifikacijaNovcaRacunarDto Racunar { get; set; }
    //public SpecifikacijaNovcaPaginationDto Pagination { get; set; }
}
