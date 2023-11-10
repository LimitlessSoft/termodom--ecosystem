using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.FE.TDOffice.Contracts.DtoMappings.NaciniPlacanja;
using TD.FE.TDOffice.Contracts.Dtos.NaciniPlacanja;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class NacinPlacanjaManager : LSCoreBaseManager<NacinPlacanjaManager>, INacinPlacanjaManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        public NacinPlacanjaManager(ILogger<NacinPlacanjaManager> logger, IKomercijalnoApiManager komercijalnoApiManager)
            : base(logger)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        public LSCoreListResponse<NacinPlacanjaDto> GetMultiple()
        {
            var apiResponse = _komercijalnoApiManager.GetAsync<List<Komercijalno.Contracts.Dtos.NaciniPlacanja.NacinPlacanjaDto>>("/nacini-placanja")
                .GetAwaiter()
                .GetResult();

            if (apiResponse.Status != System.Net.HttpStatusCode.OK ||
                apiResponse.Payload == null)
                return LSCoreListResponse<NacinPlacanjaDto>.BadRequest();

            return new LSCoreListResponse<NacinPlacanjaDto>(apiResponse.Payload.ToNacinPlacanjaDtoList());
        }
    }
}
