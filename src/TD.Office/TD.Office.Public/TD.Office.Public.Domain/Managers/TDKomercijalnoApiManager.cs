using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using TD.Office.Public.Contracts;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.KomercijalnoApi;

namespace TD.Office.Public.Domain.Managers
{
    public class TDKomercijalnoApiManager : LSCoreBaseApiManager, ITDKomercijalnoApiManager
    {
        public TDKomercijalnoApiManager(ILogger<TDKomercijalnoApiManager> logger)
        {
        }

        public Task<LSCoreResponse<List<RobaUMagacinuGetDto>>> GetRobaUMagacinu(KomercijalnoApiGetRobaUMagacinuRequest request)
        {
            base.HttpClient.BaseAddress = new Uri(string.Format(Constants.KomercijalnoApiUrlFormat, request.Godina));
            return base.GetAsync<List<RobaUMagacinuGetDto>>($"/roba-u-magacinu?magacinId={request.MagacinId}");
        }
    }
}
