using TD.Core.Contracts.Http;
using TD.TDOffice.Contracts.Dtos.DokumentTagizvod;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.TDOffice.Contracts.IManagers
{
    public interface IDokumentTagIzvodManager
    {
        ListResponse<DokumentTagIzvodGetDto> GetMultiple(DokumentTagIzvodGetMultipleRequest request);
        Response<DokumentTagIzvodGetDto> Save(DokumentTagizvodPutRequest request);
    }
}
