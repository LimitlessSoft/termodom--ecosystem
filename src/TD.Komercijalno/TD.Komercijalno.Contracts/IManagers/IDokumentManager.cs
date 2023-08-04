using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IDokumentManager
    {
        Response<DokumentDto> Get(DokumentGetRequest request);
        ListResponse<DokumentDto> GetMultiple(DokumentGetMultipleRequest request);
        Response<DokumentDto> Create(DokumentCreateRequest request);
        Response<string> NextLinked(DokumentNextLinkedRequest request);
    }
}
