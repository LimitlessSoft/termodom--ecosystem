using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;

namespace TD.TDOffice.Contracts.IManagers;

public interface IDokumentTagIzvodManager
{
    List<DokumentTagIzvodGetDto> GetMultiple(DokumentTagIzvodGetMultipleRequest request);
    DokumentTagIzvodGetDto Save(DokumentTagizvodPutRequest request);
}