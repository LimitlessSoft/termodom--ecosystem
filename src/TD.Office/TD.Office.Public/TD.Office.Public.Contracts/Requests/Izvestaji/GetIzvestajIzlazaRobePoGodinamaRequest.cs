namespace TD.Office.Public.Contracts.Requests.Izvestaji;

public class GetIzvestajIzlazaRobePoGodinamaRequest
{
    public List<int> Godina { get; set; }
    public List<int> Magacin { get; set; }
    public List<int> VrDok { get; set; }
    public DateTime OdDatuma { get; set; }
    public DateTime DoDatuma { get; set; }
}
