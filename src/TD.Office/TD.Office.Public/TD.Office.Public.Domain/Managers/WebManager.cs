using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Contracts.Responses;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Web;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Web;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Common.Contracts.Helpers;

namespace TD.Office.Public.Domain.Managers
{
    public class WebManager : LSCoreBaseManager<WebManager>, IWebManager
    {
        private readonly ITDWebAdminApiManager _webAdminApimanager;
        private readonly ITDKomercijalnoApiManager _komercijalnoApiManager;
        private readonly ILogger<WebManager> _logger;

        public WebManager(ILogger<WebManager> logger, OfficeDbContext dbContext, ITDWebAdminApiManager webAdminApimanager, ITDKomercijalnoApiManager komercijalnoApiManager)
            : base(logger, dbContext)
        {
            _logger = logger;
            _webAdminApimanager = webAdminApimanager;
            _komercijalnoApiManager = komercijalnoApiManager;
        }

        public async Task<LSCoreSortedPagedResponse<WebAzuriranjeCenaDto>> AzuriranjeCenaAsync(WebAzuiranjeCenaRequest request)
        {
            var response = new LSCoreSortedPagedResponse<WebAzuriranjeCenaDto>();

            var webProducts = await _webAdminApimanager.ProductsGetMultipleAsync();
            response.Merge(webProducts);
            if (response.NotOk)
                return response;

            var komercijalnoWebLinks = await _webAdminApimanager.KomercijalnoKomercijalnoWebProductsLinksGetMultipleAsync();
            response.Merge(komercijalnoWebLinks);
            if (response.NotOk)
                return response;

            var komercijalnoPrices = ExecuteCustomQuery<IQueryable<KomercijalnoPriceEntity>>();
            response.Merge(komercijalnoPrices);
            if (response.NotOk)
                return response;

            response.Payload = new List<WebAzuriranjeCenaDto>();
            webProducts.Payload!.ForEach(x =>
            {
                var link = komercijalnoWebLinks.Payload!.FirstOrDefault(y => y.WebId == x.Id);
                var komercijalnoPrice = link == null ? null : komercijalnoPrices.Payload!.FirstOrDefault(y => y.RobaId == link.RobaId);
                var rUslov = First<UslovFormiranjaWebCeneEntity>(x => x.WebProductId == x.Id);
                if(rUslov.Status == System.Net.HttpStatusCode.NotFound || rUslov.Payload == null)
                {
                    var rSave = Save<UslovFormiranjaWebCeneEntity, WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest>(new WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest()
                    {
                        WebProductId = x.Id,
                        Modifikator = 0,
                        Type = Common.Contracts.Enums.UslovFormiranjaWebCeneType.ProdajnaCenaPlusProcenat
                    });
                    response.Merge(rSave);
                    if (response.NotOk)
                        return;

                    rUslov.Payload = rSave.Payload;
                }

                response.Payload.Add(new WebAzuriranjeCenaDto()
                {
                    Id = x.Id,
                    Naziv = x.Name,
                    MinWebOsnova = x.MinWebBase,
                    MaxWebOsnova = x.MaxWebBase,
                    NabavnaCenaKomercijalno = komercijalnoPrice?.NabavnaCenaBezPDV ?? 0,
                    ProdajnaCenaKomercijalno = komercijalnoPrice?.ProdajnaCenaBezPDV ?? 0,
                    IronCena = PricesHelpers.CalculateProductPriceByLevel(x.MinWebBase, x.MaxWebBase, 0),
                    SilverCena = PricesHelpers.CalculateProductPriceByLevel(x.MinWebBase, x.MaxWebBase, 1),
                    GoldCena = PricesHelpers.CalculateProductPriceByLevel(x.MinWebBase, x.MaxWebBase, 2),
                    PlatinumCena = PricesHelpers.CalculateProductPriceByLevel(x.MinWebBase, x.MaxWebBase, 3),
                    LinkRobaId = link?.RobaId,
                    LinkId = link?.Id,
                    UslovFormiranjaWebCeneId = rUslov.Payload!.Id,
                    UslovFormiranjaWebCeneModifikator = rUslov.Payload!.Modifikator,
                    UslovFormiranjaWebCeneType = rUslov.Payload!.Type
                });
            });

            if (response.NotOk)
                response.Payload = null;

            return response;
        }
        public async Task<LSCoreResponse> AzurirajCeneKomercijalnoPoslovajne()
        {
            try
            {
                var robaUMagacinu = await _komercijalnoApiManager.GetRobaUMagacinu(new Contracts.Requests.KomercijalnoApi.KomercijalnoApiGetRobaUMagacinuRequest()
                {
                    MagacinId = 150
                });
                if (robaUMagacinu.NotOk)
                {
                    robaUMagacinu.LogError(_logger);
                    return LSCoreResponse.BadRequest();
                }

                var cResponse = ExecuteCustomCommand(robaUMagacinu.Payload!);
                if (cResponse.NotOk)
                {
                    robaUMagacinu.LogError(_logger);
                    return LSCoreResponse.BadRequest();
                }

                return new LSCoreResponse();
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return LSCoreResponse.BadRequest();
            }
        }
        public async Task<LSCoreResponse<KomercijalnoWebProductLinksGetDto>> AzurirajCeneKomercijalnoPoslovajnePoveziProizvode(KomercijalnoWebProductLinksSaveRequest request) =>
            await _webAdminApimanager.KomercijalnoWebProductLinksControllerPutAsync(request);

        public LSCoreResponse AzurirajCeneUsloviFormiranjaMinWebOsnova(WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest request)
        {
            var response = new LSCoreResponse();
            response.Merge(Save<UslovFormiranjaWebCeneEntity, WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest>(request));
            return response;
        }

        public async Task<LSCoreResponse> AzurirajCeneMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request) =>
            await _webAdminApimanager.ProductsUpdateMaxWebOsnove(request);
    }
}
