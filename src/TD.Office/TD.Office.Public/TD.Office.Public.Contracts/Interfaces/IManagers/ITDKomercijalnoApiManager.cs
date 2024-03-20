using LSCore.Contracts.Http;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Office.Public.Contracts.Requests.KomercijalnoApi;

namespace TD.Office.Public.Contracts.Interfaces.IManagers
{
    public interface ITDKomercijalnoApiManager
    {
        Task<LSCoreResponse<List<RobaUMagacinuGetDto>>> GetRobaUMagacinu(KomercijalnoApiGetRobaUMagacinuRequest request);
        Task<LSCoreResponse<List<NabavnaCenaNaDanDto>>> GetNabavnaCenaNaDan(ProceduraGetNabavnaCenaNaDanRequest request);
    }
}
