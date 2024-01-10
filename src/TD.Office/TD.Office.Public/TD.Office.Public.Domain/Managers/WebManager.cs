using LSCore.Contracts.Extensions;
using LSCore.Contracts.Responses;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Office.Public.Contracts.Dtos.Web;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Web;

namespace TD.Office.Public.Domain.Managers
{
    public class WebManager : LSCoreBaseManager<WebManager>, IWebManager
    {
        private readonly ITDWebAdminApiManager _webAdminApimanager;
        public WebManager(ILogger<WebManager> logger, ITDWebAdminApiManager webAdminApimanager)
            : base(logger)
        {
            _webAdminApimanager = webAdminApimanager;
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

            var komercijalnoPrices = await _webAdminApimanager.KomercijalnoPricesGetMultipleAsync();
            response.Merge(komercijalnoPrices);
            if (response.NotOk)
                return response;

            response.Payload = new List<WebAzuriranjeCenaDto>();
            webProducts.Payload!.ForEach(x =>
            {
                var link = komercijalnoWebLinks.Payload!.FirstOrDefault(y => y.WebId == x.Id);
                if (link == null)
                    return;

                var komercijalnoPrice = komercijalnoPrices.Payload!.FirstOrDefault(y => y.RobaId == link.RobaId);
                if(komercijalnoPrice == null)
                    return;

                response.Payload.Add(new WebAzuriranjeCenaDto()
                {
                    Naziv = x.Name,
                    MinWebOsnova = x.MinWebBase,
                    MaxWebOsnova = x.MaxWebBase,
                    NabavnaCenaKomercijalno = komercijalnoPrice.NabavnaCenaBezPDV,
                    ProdajnaCenaKomercijalno = komercijalnoPrice.ProdajnaCenaBezPDV,
                    IronCena = 0,
                    SilverCena = 0,
                    GoldCena = 0,
                    PlatinumCena = 0
                });
            });

            return response;
        }
    }
}
