using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Dokument;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IDokumentManager
    {
        ListResponse<Dokument> GetMultiple(DokumentGetMultipleRequest request);
        Response<Dokument> Create(DokumentCreateRequest request);
        Response<string> NextLinked(DokumentNextLinkedRequest request);
    }
}
