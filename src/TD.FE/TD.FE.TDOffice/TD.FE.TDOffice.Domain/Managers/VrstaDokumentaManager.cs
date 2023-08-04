using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.FE.TDOffice.Contracts.DtoMappings.VrsteDokumenata;
using TD.FE.TDOffice.Contracts.Dtos.VrsteDokumenata;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class VrstaDokumentaManager : BaseManager<VrstaDokumentaManager>, IVrstaDokumentaManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        public VrstaDokumentaManager(ILogger<VrstaDokumentaManager> logger, IKomercijalnoApiManager komercijalnoApiManager)
            : base(logger)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        ListResponse<VrstaDokumentaDto> IVrstaDokumentaManager.GetMultiple()
        {
            var komercijalnoApiResponse = _komercijalnoApiManager
                .GetAsync<List<VrstaDokDto>>("/vrste-dokumenata")
                .GetAwaiter()
                .GetResult();

            if (komercijalnoApiResponse.Status != System.Net.HttpStatusCode.OK ||
                komercijalnoApiResponse.Payload == null)
                return ListResponse<VrstaDokumentaDto>.BadRequest();

            return new ListResponse<VrstaDokumentaDto>(komercijalnoApiResponse.Payload.ToVrstaDokumentaDtoList());
        }
    }
}
