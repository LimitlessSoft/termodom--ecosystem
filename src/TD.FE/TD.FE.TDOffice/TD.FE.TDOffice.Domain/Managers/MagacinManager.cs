using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.Magacini;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class MagacinManager : LSCoreBaseManager<MagacinManager>, IMagacinManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        public MagacinManager(ILogger<MagacinManager> logger, IKomercijalnoApiManager komercijalnoApiManager)
            : base(logger)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        public LSCoreListResponse<MagacinDto> GetMultiple()
        {
            var komercijalnoApiResponse = _komercijalnoApiManager
                .GetAsync<List<MagacinDto>>("/magacini")
                .GetAwaiter()
                .GetResult();

            if (komercijalnoApiResponse.Status != System.Net.HttpStatusCode.OK ||
                komercijalnoApiResponse.Payload == null)
                return LSCoreListResponse<MagacinDto>.BadRequest();

            return new LSCoreListResponse<MagacinDto>(komercijalnoApiResponse.Payload);
        }
    }
}
