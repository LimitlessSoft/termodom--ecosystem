namespace TD.Office.Public.Contracts.Requests.Izvestaji;

public class GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest
{
    public int VrDok { get; set; }
    public short NUID { get; set; }
    public DateTime DatumOd { get; set; }
    public DateTime DatumDo { get; set; }
}
