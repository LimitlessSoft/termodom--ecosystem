using TD.Office.Public.Contracts.Requests.KomercijalnoApi;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Dtos.Magacini;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface ITDKomercijalnoApiManager
{
    Task<List<RobaUMagacinuGetDto>> GetRobaUMagacinuAsync(KomercijalnoApiGetRobaUMagacinuRequest request);
    Task<List<NabavnaCenaNaDanDto>> GetNabavnaCenaNaDanAsync(ProceduraGetNabavnaCenaNaDanRequest request);
    Task<List<ProdajnaCenaNaDanDto>> GetProdajnaCenaNaDanAsync(ProceduraGetProdajnaCenaNaDanOptimizedRequest proceduraGetProdajnaCenaNaDanRequest);
    Task<List<MagacinDto>> GetMagaciniAsync();
    Task<DokumentDto> GetDokumentAsync(DokumentGetRequest request);
}