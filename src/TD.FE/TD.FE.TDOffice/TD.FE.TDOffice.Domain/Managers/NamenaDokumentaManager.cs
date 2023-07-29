using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.FE.TDOffice.Contracts.DtoMappings.NaciniPlacanja;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.Namene;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class NamenaDokumentaManager : BaseManager<NamenaDokumentaManager>, INamenaDokumentaManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        public NamenaDokumentaManager(ILogger<NamenaDokumentaManager> logger, IKomercijalnoApiManager komercijalnoApiManager)
            : base(logger)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        public ListResponse<NamenaDto> GetMultiple()
        {
            var apiResponse = _komercijalnoApiManager.GetAsync<List<NamenaDto>>("/namene")
                .GetAwaiter()
                .GetResult();

            if (apiResponse.Status != System.Net.HttpStatusCode.OK ||
                apiResponse.Payload == null)
                return ListResponse<NamenaDto>.BadRequest();

            return new ListResponse<NamenaDto>(apiResponse.Payload);
        }
    }
}
