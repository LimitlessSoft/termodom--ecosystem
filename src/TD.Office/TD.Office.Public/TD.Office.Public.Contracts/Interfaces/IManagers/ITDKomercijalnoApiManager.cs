using LSCore.Contracts.Responses;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Dtos.Mesto;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Office.Public.Contracts.Dtos.Partners;
using TD.Office.Public.Contracts.Requests.KomercijalnoApi;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface ITDKomercijalnoApiManager
{
    Task<List<RobaUMagacinuGetDto>> GetRobaUMagacinuAsync(
        KomercijalnoApiGetRobaUMagacinuRequest request
    );
    Task<List<NabavnaCenaNaDanDto>> GetNabavnaCenaNaDanAsync(
        ProceduraGetNabavnaCenaNaDanRequest request
    );
    Task<List<ProdajnaCenaNaDanDto>> GetProdajnaCenaNaDanAsync(
        ProceduraGetProdajnaCenaNaDanOptimizedRequest proceduraGetProdajnaCenaNaDanRequest
    );
    Task<List<MagacinDto>> GetMagaciniAsync();
    Task<DokumentDto> GetDokumentAsync(DokumentGetRequest request);
    Task<LSCoreSortedAndPagedResponse<PartnerDto>> GetPartnersAsync(
        PartneriGetMultipleRequest request
    );
    Task<int> CreatePartnerAsync(PartneriCreateRequest request);
    Task<List<MestoDto>> GetPartnersMestaAsync();
}
