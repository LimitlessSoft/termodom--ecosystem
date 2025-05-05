namespace TD.Office.Public.Contracts.Requests.Izvestaji;

public class ExportIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest
	: GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest
{
	public int DestinationVrDok { get; set; }
	public int DestinationBrDok { get; set; }
}
