using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Komentari;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Dtos.Stavke;
using TD.Komercijalno.Contracts.Requests.Komentari;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Web.Admin.Contracts.Requests.KomercijalnoApi;

namespace TD.Web.Admin.Contracts.Interfaces.IManagers;

public interface IKomercijalnoApiManager
{
	Task<DokumentDto> DokumentiPostAsync(KomercijalnoApiDokumentiCreateRequest request);
	Task<StavkaDto> StavkePostAsync(StavkaCreateRequest request);
	Task StavkeDeleteAsync(StavkeDeleteRequest request);
	Task FlushCommentsAsync(FlushCommentsRequest request);
	Task<KomentarDto> DokumentiKomentariPostAsync(CreateKomentarRequest createKomentarRequest);
	Task<KomentarDto> DokumentiKomentariUpdateAsync(UpdateKomentarRequest createKomentarRequest);
	Task<List<MagacinDto>> GetMagaciniAsync();
	Task UpdateDokOut(int sourceVrDok, int? sourceBrDok, int vrDokOut, int brDokOut);
}
