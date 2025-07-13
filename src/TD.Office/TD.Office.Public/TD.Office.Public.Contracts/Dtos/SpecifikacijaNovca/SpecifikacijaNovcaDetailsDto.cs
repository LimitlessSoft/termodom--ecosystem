namespace TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;

public class SpecifikacijaNovcaDetailsDto
{
    public EurDto Eur1 { get; set; } = new ();
    public EurDto Eur2 { get; set; } = new ();
    public List<NovcanicaDto> Novcanice { get; set; } = [];
    public List<OstaloDto> Ostalo { get; set; } = [];

    public class EurDto
    {
        public int Komada { get; set; }
        public double Kurs { get; set; }
    }

    public class NovcanicaDto
    {
        public int Key { get; set; }
        public int Value { get; set; }
    }

    public class OstaloDto
    {
        public string Key { get; set; }
        public double Vrednost { get; set; }
        public string? Komentar { get; set; }
    }
 
}