using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.Namene;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class NamenaDokumentaManager : LSCoreBaseManager<NamenaDokumentaManager>, INamenaDokumentaManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        public NamenaDokumentaManager(ILogger<NamenaDokumentaManager> logger, IKomercijalnoApiManager komercijalnoApiManager)
            : base(logger)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        public LSCoreListResponse<NamenaDto> GetMultiple()
        {
            var apiResponse = _komercijalnoApiManager.GetAsync<List<NamenaDto>>("/namene")
                .GetAwaiter()
                .GetResult();

            if (apiResponse.Status != System.Net.HttpStatusCode.OK ||
                apiResponse.Payload == null)
                return LSCoreListResponse<NamenaDto>.BadRequest();

            return new LSCoreListResponse<NamenaDto>(apiResponse.Payload);
        }
    }
}
