using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Dtos.Procedure;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Office.Public.Contracts;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.KomercijalnoApi;

namespace TD.Office.Public.Domain.Managers
{
    public class TDKomercijalnoApiManager : LSCoreBaseApiManager, ITDKomercijalnoApiManager
    {
        public TDKomercijalnoApiManager(ILogger<TDKomercijalnoApiManager> logger)
        {
            base.HttpClient.BaseAddress = new Uri(string.Format(Constants.KomercijalnoApiUrlFormat, DateTime.Now.Year));
        }

        public Task<LSCoreResponse<List<RobaUMagacinuGetDto>>> GetRobaUMagacinu(KomercijalnoApiGetRobaUMagacinuRequest request) =>
            base.GetAsync<List<RobaUMagacinuGetDto>>($"/roba-u-magacinu?magacinId={request.MagacinId}");
        
        public Task<LSCoreResponse<List<NabavnaCenaNaDanDto>>> GetNabavnaCenaNaDan(ProceduraGetNabavnaCenaNaDanRequest request) =>
            base.GetAsync<List<NabavnaCenaNaDanDto>>($"/nabavna-cena-na-dan?robaId={request.RobaId}&datum={request.Datum:yyyy-MM-dd}");
    }
}
