using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.FE.TDOffice.Contracts.DtoMappings.NaciniPlacanja;
using TD.FE.TDOffice.Contracts.Dtos.NaciniPlacanja;
using TD.FE.TDOffice.Contracts.IManagers;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class NacinPlacanjaManager : BaseManager<NacinPlacanjaManager>, INacinPlacanjaManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        public NacinPlacanjaManager(ILogger<NacinPlacanjaManager> logger, IKomercijalnoApiManager komercijalnoApiManager)
            : base(logger)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        public ListResponse<NacinPlacanjaDto> GetMultiple()
        {
            var apiResponse = _komercijalnoApiManager.GetAsync<List<Komercijalno.Contracts.Dtos.NaciniPlacanja.NacinPlacanjaDto>>("/nacini-placanja")
                .GetAwaiter()
                .GetResult();

            if (apiResponse.Status != System.Net.HttpStatusCode.OK ||
                apiResponse.Payload == null)
                return ListResponse<NacinPlacanjaDto>.BadRequest();

            return new ListResponse<NacinPlacanjaDto>(apiResponse.Payload.ToNacinPlacanjaDtoList());
        }
    }
}
