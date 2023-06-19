using TD.Core.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Dokument;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IDokumentManager
    {
        ListResponse<DokumentDto> GetMultiple(DokumentGetMultipleRequest request);
        Response<DokumentDto> Create(DokumentCreateRequest request);
        Response<string> NextLinked(DokumentNextLinkedRequest request);
    }
}
