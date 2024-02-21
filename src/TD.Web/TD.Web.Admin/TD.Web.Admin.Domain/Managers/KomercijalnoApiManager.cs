using TD.Web.Admin.Contracts.Requests.KomercijalnoApi;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using LSCore.Domain.Managers;
using TD.Web.Admin.Contracts;
using LSCore.Contracts.Http;

namespace TD.Web.Admin.Domain.Managers
{
    public class KomercijalnoApiManager : LSCoreBaseApiManager, IKomercijalnoApiManager
    {
        public KomercijalnoApiManager()
        {
            base.HttpClient.BaseAddress = new Uri(string.Format(Constants.KomercijalnoApiUrlFormat, DateTime.Now.Year));
        }

        public Task<LSCoreResponse<DokumentDto>> DokumentiPostAsync(KomercijalnoApiDokumentiCreateRequest request) =>
            PostAsync<DokumentCreateRequest, DokumentDto>($"/dokumenti", request);

        public Task<LSCoreResponse<StavkaDto>> StavkePostAsync(StavkaCreateRequest request) =>
            PostAsync<StavkaCreateRequest, StavkaDto>($"/stavke", request);
    }
}