namespace TD.Office.Public.Contracts.Requests.Izvestaji;

public class PromeniNacinUplateRequest
	: GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest
{
	public short DestinationNuid { get; set; }
}
