using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.Magacini;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class MagacinManager : BaseManager<MagacinManager>, IMagacinManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        public MagacinManager(ILogger<MagacinManager> logger, IKomercijalnoApiManager komercijalnoApiManager)
            : base(logger)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        public ListResponse<MagacinDto> GetMultiple()
        {
            var komercijalnoApiResponse = _komercijalnoApiManager
                .GetAsync<List<MagacinDto>>("/magacini")
                .GetAwaiter()
                .GetResult();

            if (komercijalnoApiResponse.Status != System.Net.HttpStatusCode.OK ||
                komercijalnoApiResponse.Payload == null)
                return ListResponse<MagacinDto>.BadRequest();

            return new ListResponse<MagacinDto>(komercijalnoApiResponse.Payload);
        }
    }
}
