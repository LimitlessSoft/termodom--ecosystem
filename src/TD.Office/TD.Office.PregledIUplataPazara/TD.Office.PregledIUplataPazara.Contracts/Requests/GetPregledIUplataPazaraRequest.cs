namespace TD.Office.PregledIUplataPazara.Contracts.Requests;

public class GetPregledIUplataPazaraRequest
{
    public DateTime OdDatumaInclusive { get; set; }
    public DateTime DoDatumaInclusive { get; set; }
    public double Tolerancija { get; set; }
    public List<int> Magacin { get; set; }
}