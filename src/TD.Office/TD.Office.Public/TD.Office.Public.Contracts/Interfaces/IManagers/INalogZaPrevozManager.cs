using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using TD.Office.Public.Contracts.Dtos.NalogZaPrevoz;
using TD.Office.Public.Contracts.Requests.NalogZaPrevoz;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface INalogZaPrevozManager
    {
        LSCoreResponse SaveNalogZaPrevoz(SaveNalogZaPrevozRequest request);
        Task<LSCoreResponse<GetReferentniDokumentNalogZaPrevozDto>> GetReferentniDokument(
            GetReferentniDokumentNalogZaPrevozRequest request);

        LSCoreListResponse<GetNalogZaPrevozDto> GetMultiple(GetMultipleNalogZaPrevozRequest request);
        LSCoreResponse<GetNalogZaPrevozDto> GetSingle(LSCoreIdRequest request);
    }
}