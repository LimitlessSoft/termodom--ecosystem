using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Managers;
using TD.FE.TDOffice.Contracts.Dtos.WebUredjivanjeProizvoda;
using TD.FE.TDOffice.Contracts.IManagers;
using TD.Komercijalno.Contracts.Dtos.Roba;
using TD.Web.Veleprodaja.Contracts.Dtos.Products;
using static TD.FE.TDOffice.Contracts.DtoMappings.WebUredjivanjeProizvodaProizvodiGetDtoMappings;

namespace TD.FE.TDOffice.Domain.Managers
{
    public class WebUredjivanjeProizvodaManager : BaseManager<WebUredjivanjeProizvodaManager>, IWebUredjivanjeProizvodaManager
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

        public async Task<ListResponse<WebUredjivanjeProizvodaProizvodiGetDto>> ProizvodiGet()
        {
            var webProizvodiTask = _webVeleprodajaApiManager.GetAsync<List<ProductsGetDto>>("/products");
            var komercijalnoRobaTask = _komercijalnoApiManager.GetAsync<List<RobaDto>>("/roba");

            var webProizvodi = await webProizvodiTask;
            var komercijalnoRoba = await komercijalnoRobaTask;

            if (webProizvodi.Status != System.Net.HttpStatusCode.OK ||
                komercijalnoRoba.Status != System.Net.HttpStatusCode.OK ||
                webProizvodi.Payload == null ||
                komercijalnoRoba.Payload == null)
                return ListResponse<WebUredjivanjeProizvodaProizvodiGetDto>.BadRequest();

            return new ListResponse<WebUredjivanjeProizvodaProizvodiGetDto>(ConvertToWebUredjivanjeProizvodaProizvodiGetDtoList(webProizvodi.Payload, komercijalnoRoba.Payload));
        }
    }
}
