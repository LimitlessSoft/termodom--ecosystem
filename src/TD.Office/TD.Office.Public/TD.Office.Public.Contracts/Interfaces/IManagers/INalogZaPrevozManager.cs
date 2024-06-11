using TD.Office.Public.Contracts.Requests.NalogZaPrevoz;
using TD.Office.Public.Contracts.Dtos.NalogZaPrevoz;
using LSCore.Contracts.Requests;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface INalogZaPrevozManager
    {
        void SaveNalogZaPrevoz(SaveNalogZaPrevozRequest request);
        Task<GetReferentniDokumentNalogZaPrevozDto> GetReferentniDokumentAsync(
            GetReferentniDokumentNalogZaPrevozRequest request);

        List<GetNalogZaPrevozDto> GetMultiple(GetMultipleNalogZaPrevozRequest request);
        GetNalogZaPrevozDto GetSingle(LSCoreIdRequest request);
    }
}