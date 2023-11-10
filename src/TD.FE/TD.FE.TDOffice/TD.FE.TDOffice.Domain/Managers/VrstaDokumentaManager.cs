using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.FE.TDOffice.Contracts.DtoMappings.VrsteDokumenata;
using TD.FE.TDOffice.Contracts.Dtos.VrsteDokumenata;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.VrstaDok;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class VrstaDokumentaManager : LSCoreBaseManager<VrstaDokumentaManager>, IVrstaDokumentaManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        public VrstaDokumentaManager(ILogger<VrstaDokumentaManager> logger, IKomercijalnoApiManager komercijalnoApiManager)
            : base(logger)
        {
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        LSCoreListResponse<VrstaDokumentaDto> IVrstaDokumentaManager.GetMultiple()
        {
            var komercijalnoApiResponse = _komercijalnoApiManager
                .GetAsync<List<VrstaDokDto>>("/vrste-dokumenata")
                .GetAwaiter()
                .GetResult();

            if (komercijalnoApiResponse.Status != System.Net.HttpStatusCode.OK ||
                komercijalnoApiResponse.Payload == null)
                return LSCoreListResponse<VrstaDokumentaDto>.BadRequest();

            return new LSCoreListResponse<VrstaDokumentaDto>(komercijalnoApiResponse.Payload.ToVrstaDokumentaDtoList());
        }
    }
}
