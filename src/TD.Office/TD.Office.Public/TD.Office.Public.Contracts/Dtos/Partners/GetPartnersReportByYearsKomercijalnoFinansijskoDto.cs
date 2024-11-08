namespace TD.Office.Public.Contracts.Dtos.Partners;

public class GetPartnersReportByYearsKomercijalnoFinansijskoDto
{
    public int PPID { get; set; }
    public string Naziv { get; set; }
    public List<YearStartEndDto> Komercijalno { get; set; }
    public List<YearStartEndDto> FinansijskoKupac { get; set; }
    public List<YearStartEndDto> FinansijskoDobavljac { get; set; }

}
