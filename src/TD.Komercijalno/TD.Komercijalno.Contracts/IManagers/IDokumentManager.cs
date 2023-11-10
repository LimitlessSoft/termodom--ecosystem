using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Requests.Dokument;

namespace TD.Komercijalno.Contracts.IManagers
{
    public interface IDokumentManager
    {
        LSCoreResponse<DokumentDto> Get(DokumentGetRequest request);
        LSCoreListResponse<DokumentDto> GetMultiple(DokumentGetMultipleRequest request);
        LSCoreResponse<DokumentDto> Create(DokumentCreateRequest request);
        LSCoreResponse<string> NextLinked(DokumentNextLinkedRequest request);
        LSCoreResponse SetNacinPlacanja(DokumentSetNacinPlacanjaRequest request);
    }
}
