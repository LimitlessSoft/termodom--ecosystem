using LSCore.Contracts.Http;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.TDOffice.Contracts.IManagers
{
    public interface IDokumentTagIzvodManager
    {
        LSCoreListResponse<DokumentTagIzvodGetDto> GetMultiple(DokumentTagIzvodGetMultipleRequest request);
        LSCoreResponse<DokumentTagIzvodGetDto> Save(DokumentTagizvodPutRequest request);
    }
}
