using TD.Office.Public.Contracts.Dtos.Izvestaji;
using TD.Office.Public.Contracts.Requests.Izvestaji;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface IIzvestajManager
{
    Task<GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaDto> GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(
        GetIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest request
    );

    Task ExportIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaAsync(
        ExportIzvestajUkupnihKolicinaPoRobiUFiltriranimDokumentimaRequest request
    );

    Task PromeniNacinUplateAsync(PromeniNacinUplateRequest request);
}
