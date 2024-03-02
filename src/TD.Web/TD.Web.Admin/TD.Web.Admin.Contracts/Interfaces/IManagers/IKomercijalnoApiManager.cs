using TD.Web.Admin.Contracts.Requests.KomercijalnoApi;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Requests.Komentari;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers
{
    public interface IKomercijalnoApiManager
    {
        Task<LSCoreResponse<DokumentDto>> DokumentiPostAsync(KomercijalnoApiDokumentiCreateRequest request);
        Task<LSCoreResponse<StavkaDto>> StavkePostAsync(StavkaCreateRequest request);
        Task<LSCoreResponse<KomentarDto>> DokumentiKomentariPostAsync(CreateKomentarRequest createKomentarRequest);
    }
}