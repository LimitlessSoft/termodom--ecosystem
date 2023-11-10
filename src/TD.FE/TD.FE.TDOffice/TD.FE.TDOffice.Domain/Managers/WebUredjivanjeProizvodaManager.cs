using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.FE.TDOffice.Contracts.Dtos.WebUredjivanjeProizvoda;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.WebshopListener.Contracts.Dtos;
using static TD.FE.TDOffice.Contracts.DtoMappings.WebUredjivanjeProizvodaProizvodiGetDtoMappings;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class WebUredjivanjeProizvodaManager : LSCoreBaseManager<WebUredjivanjeProizvodaManager>, IWebUredjivanjeProizvodaManager
    {
        private readonly IKomercijalnoApiManager _komercijalnoApiManager;
        private readonly ITDWebVeleprodajaApiManager _webVeleprodajaApiManager;

        public WebUredjivanjeProizvodaManager(ILogger<WebUredjivanjeProizvodaManager> logger,
            ITDWebVeleprodajaApiManager webVeleprodajaApiManager, IKomercijalnoApiManager komercijalnoApiManager)
            : base(logger)
        {
            _webVeleprodajaApiManager = webVeleprodajaApiManager;
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        public async Task<LSCoreListResponse<WebUredjivanjeProizvodaProizvodiGetDto>> ProizvodiGet()
        {
            var webProizvodiTask = _webVeleprodajaApiManager.GetAsync<List<ProductsGetDto>>("/products");
            var komercijalnoRobaTask = _komercijalnoApiManager.GetAsync<List<RobaDto>>("/roba");

            var webProizvodi = await webProizvodiTask;
            var komercijalnoRoba = await komercijalnoRobaTask;

            if (webProizvodi.Status != System.Net.HttpStatusCode.OK ||
                komercijalnoRoba.Status != System.Net.HttpStatusCode.OK ||
                webProizvodi.Payload == null ||
                komercijalnoRoba.Payload == null)
                return LSCoreListResponse<WebUredjivanjeProizvodaProizvodiGetDto>.BadRequest();

            return new LSCoreListResponse<WebUredjivanjeProizvodaProizvodiGetDto>(ConvertToWebUredjivanjeProizvodaProizvodiGetDtoList(webProizvodi.Payload, komercijalnoRoba.Payload));
        }
    }
}
