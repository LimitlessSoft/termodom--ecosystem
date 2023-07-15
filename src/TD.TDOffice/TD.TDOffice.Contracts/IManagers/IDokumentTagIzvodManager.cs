using TD.Core.Contracts.Http;
using TD.TDOffice.Contracts.Entities;
using TD.TDOffice.Contracts.Requests.DokumentTagIzvod;

namespace TD.TDOffice.Contracts.IManagers
{
    public interface IDokumentTagIzvodManager
    {
        ListResponse<DokumentTagIzvod> GetMultiple(DokumentTagIzvodGetMultipleRequest request);
        Response<bool> Save(DokumentTagizvodPutRequest request);
    }
}
