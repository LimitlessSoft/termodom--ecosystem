using LSCore.Contracts.Requests;
using LSCore.Contracts.Responses;
using TD.Komercijalno.Contracts.Dtos.Dokumenti;
using TD.Komercijalno.Contracts.Dtos.Magacini;
using TD.Komercijalno.Contracts.Dtos.Mesto;
using TD.Komercijalno.Contracts.Dtos.NaciniPlacanja;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;
using TD.Komercijalno.Contracts.Entities;
using TD.Komercijalno.Contracts.Requests.Dokument;
using TD.Komercijalno.Contracts.Requests.Partneri;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Komercijalno.Contracts.Requests.Roba;
using TD.Komercijalno.Contracts.Requests.Stavke;
using TD.Office.Public.Contracts.Dtos.Partners;
using TD.Office.Public.Contracts.Requests.KomercijalnoApi;

namespace TD.Office.Public.Contracts.Interfaces.IManagers;

public interface ITDKomercijalnoApiManager
{
    Task<List<VrstaDokDto>> GetMultipleVrDokAsync();
    Task<List<RobaUMagacinuGetDto>> GetRobaUMagacinuAsync(
        KomercijalnoApiGetRobaUMagacinuRequest request
    );
    Task<List<NabavnaCenaNaDanDto>> GetNabavnaCenaNaDanAsync(
        ProceduraGetNabavnaCenaNaDanRequest request
    );
    Task<double> GetProdajnaCenaNaDanAsync(ProceduraGetProdajnaCenaNaDanRequest request);
    Task<List<ProdajnaCenaNaDanDto>> GetProdajnaCenaNaDanOptimizedAsync(
        ProceduraGetProdajnaCenaNaDanOptimizedRequest proceduraGetProdajnaCenaNaDanRequest
    );
    Task<List<MagacinDto>> GetMagaciniAsync();
    Task<DokumentDto> GetDokumentAsync(DokumentGetRequest request);
    Task<List<DokumentDto>> GetMultipleDokumentAsync(DokumentGetMultipleRequest request);
    Task<LSCoreSortedAndPagedResponse<PartnerDto>> GetPartnersAsync(
        PartneriGetMultipleRequest request
    );
    Task<PartnerDto> GetPartnerAsync(LSCoreIdRequest request);
    Task<int> CreatePartnerAsync(PartneriCreateRequest request);
    Task<List<MestoDto>> GetPartnersMestaAsync();
    Task<List<PPKategorija>> GetPartnersKategorijeAsync();
    Task<List<NacinPlacanjaDto>> GetMultipleNaciniPlacanjaAsync();
    Task<List<RobaDto>> GetMultipleRobaAsync(RobaGetMultipleRequest request);
    Task CreateStavkaAsync(StavkaCreateRequest request);
    Task SetDokumentNacinPlacanjaAsync(
        DokumentSetNacinPlacanjaRequest dokumentSetNacinPlacanjaRequest
    );
    Task<RobaDto> GetRobaAsync(LSCoreIdRequest lsCoreIdRequest);
}
