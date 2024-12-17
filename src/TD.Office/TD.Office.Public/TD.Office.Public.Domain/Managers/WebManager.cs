using LSCore.Domain.Managers;
using LSCore.Contracts.Exceptions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using TD.Komercijalno.Contracts.Requests.Procedure;
using TD.Office.Common.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Common.Repository;
using TD.Office.Public.Contracts.Dtos.Web;
using TD.Office.Public.Contracts.Interfaces.IManagers;
using TD.Office.Public.Contracts.Requests.Web;
using TD.Web.Admin.Contracts.Dtos.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Dtos.Products;
using TD.Web.Admin.Contracts.Requests.KomercijalnoWebProductLinks;
using TD.Web.Admin.Contracts.Requests.Products;
using TD.Web.Common.Contracts.Helpers;
using TD.Office.Common.Contracts.Enums.ValidationCodes;
using LSCore.Contracts.Extensions;

namespace TD.Office.Public.Domain.Managers
{
    public class WebManager(
        ILogger<WebManager> logger,
        OfficeDbContext dbContext,
        IDistributedCache distributedCache,
        ITDWebAdminApiManager webAdminApimanager,
        ITDKomercijalnoApiManager komercijalnoApiManager
    ) : LSCoreManagerBase<WebManager>(logger, dbContext), IWebManager
    {
        private readonly ILogger<WebManager> _logger = logger;

        public async Task<List<WebAzuriranjeCenaDto>> AzuriranjeCenaAsync(
            WebAzuiranjeCenaRequest request
        )
        {
            var responseList = new List<WebAzuriranjeCenaDto>();

            // TODO: Implement as sortable pageable
            var webProducts = await webAdminApimanager.ProductsGetMultipleAsync(
                new ProductsGetMultipleRequest()
            );
            var komercijalnoWebLinks =
                await webAdminApimanager.KomercijalnoKomercijalnoWebProductsLinksGetMultipleAsync();
            var komercijalnoPrices = Queryable<KomercijalnoPriceEntity>();

            webProducts!
                .Where(x => request.Id == null || x.Id == request.Id)
                .ToList()
                .ForEach(x =>
                {
                    var link = komercijalnoWebLinks?.FirstOrDefault(y => y.WebId == x.Id);
                    var komercijalnoPrice =
                        link == null
                            ? null
                            : komercijalnoPrices.FirstOrDefault(y => y.RobaId == link.RobaId);

                    var uslov = Queryable<UslovFormiranjaWebCeneEntity>()
                        .FirstOrDefault(z => z.WebProductId == x.Id);
                    if (uslov == null)
                    {
                        var savedUslov = Save<
                            UslovFormiranjaWebCeneEntity,
                            WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest
                        >(
                            new WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest()
                            {
                                WebProductId = x.Id,
                                Modifikator = 0,
                                Type = UslovFormiranjaWebCeneType
                                    .ProdajnaCenaPlusProcenat
                            }
                        );
                        uslov = savedUslov;
                    }

                    responseList.Add(
                        new WebAzuriranjeCenaDto()
                        {
                            Id = x.Id,
                            Naziv = x.Name,
                            MinWebOsnova = x.MinWebBase,
                            MaxWebOsnova = x.MaxWebBase,
                            NabavnaCenaKomercijalno = komercijalnoPrice?.NabavnaCenaBezPDV ?? 0,
                            ProdajnaCenaKomercijalno = komercijalnoPrice?.ProdajnaCenaBezPDV ?? 0,
                            IronCena = PricesHelpers.CalculateProductPriceByLevel(
                                x.MinWebBase,
                                x.MaxWebBase,
                                0
                            ),
                            SilverCena = PricesHelpers.CalculateProductPriceByLevel(
                                x.MinWebBase,
                                x.MaxWebBase,
                                1
                            ),
                            GoldCena = PricesHelpers.CalculateProductPriceByLevel(
                                x.MinWebBase,
                                x.MaxWebBase,
                                2
                            ),
                            PlatinumCena = PricesHelpers.CalculateProductPriceByLevel(
                                x.MinWebBase,
                                x.MaxWebBase,
                                3
                            ),
                            LinkRobaId = link?.RobaId,
                            LinkId = link?.Id,
                            UslovFormiranjaWebCeneId = uslov.Id,
                            UslovFormiranjaWebCeneModifikator = uslov.Modifikator,
                            UslovFormiranjaWebCeneType = uslov.Type
                        }
                    );
                });

            return responseList;
        }

        public async Task AzurirajCeneKomercijalnoPoslovanje()
        {
            #region Check if task is in progress
            var cachedData = await distributedCache.GetStringAsync(
                Constants.AzurirajCeneKomercijalnoPoslovanjeInprogressKey
            );

            if (!string.IsNullOrEmpty(cachedData))
                throw new LSCoreBadRequestException(WebValidationCodes.WVC_001.GetDescription()!);
            else
                await distributedCache.SetStringAsync(
                    Constants.AzurirajCeneKomercijalnoPoslovanjeInprogressKey,
                    "In progress",
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    }
                );

            #endregion

            var robaUMagacinu = await komercijalnoApiManager.GetRobaUMagacinuAsync(
                new Contracts.Requests.KomercijalnoApi.KomercijalnoApiGetRobaUMagacinuRequest()
                {
                    MagacinId = 150
                }
            );

            var nabavneCeneNaDan = await komercijalnoApiManager.GetNabavnaCenaNaDanAsync(
                new ProceduraGetNabavnaCenaNaDanRequest() { Datum = DateTime.UtcNow }
            );

            var prodajneCeneNaDan = await komercijalnoApiManager.GetProdajnaCenaNaDanOptimizedAsync(
                new ProceduraGetProdajnaCenaNaDanOptimizedRequest()
                {
                    Datum = DateTime.UtcNow,
                    MagacinId = 150,
                }
            );

            foreach (var rum in robaUMagacinu)
            {
                var nabavnaCenaNaDan = nabavneCeneNaDan.FirstOrDefault(x => x.RobaId == rum.RobaId);
                var prodajnaCenaNaDan = prodajneCeneNaDan.FirstOrDefault(x =>
                    x.RobaId == rum.RobaId
                );

                if (nabavnaCenaNaDan != null)
                    rum.NabavnaCena = nabavnaCenaNaDan.NabavnaCenaBezPDV;

                if (prodajnaCenaNaDan != null)
                    rum.ProdajnaCena = prodajnaCenaNaDan.ProdajnaCenaBezPDV;
            }

            var komercijalnoPrices = Queryable<KomercijalnoPriceEntity>().AsEnumerable();
            HardDelete(komercijalnoPrices);

            var list = new List<KomercijalnoPriceEntity>();
            robaUMagacinu.ForEach(roba =>
            {
                list.Add(
                    new KomercijalnoPriceEntity()
                    {
                        RobaId = roba.RobaId,
                        NabavnaCenaBezPDV = (decimal)roba.NabavnaCena,
                        ProdajnaCenaBezPDV = (decimal)roba.ProdajnaCena
                    }
                );
            });
            Insert(list);
        }

        public async Task<KomercijalnoWebProductLinksGetDto?> AzurirajCeneKomercijalnoPoslovanjePoveziProizvode(
            KomercijalnoWebProductLinksSaveRequest request
        ) => await webAdminApimanager.KomercijalnoWebProductLinksControllerPutAsync(request);

        public void AzurirajCeneUsloviFormiranjaMinWebOsnova(
            WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest request
        ) =>
            Save<
                UslovFormiranjaWebCeneEntity,
                WebAzuriranjeCenaUsloviFormiranjaMinWebOsnovaRequest
            >(request);

        public async Task AzurirajCeneMaxWebOsnove(ProductsUpdateMaxWebOsnoveRequest request) =>
            await webAdminApimanager.ProductsUpdateMaxWebOsnove(request);

        public async Task AzurirajCeneMinWebOsnove()
        {
            var request = new ProductsUpdateMinWebOsnoveRequest()
            {
                Items = new List<ProductsUpdateMinWebOsnoveRequest.MinItem>()
            };

            var azuriranjeCenaAsyncResponse = await AzuriranjeCenaAsync(
                new WebAzuiranjeCenaRequest()
            );

            azuriranjeCenaAsyncResponse.ForEach(x =>
            {
                request.Items.Add(
                    new ProductsUpdateMinWebOsnoveRequest.MinItem()
                    {
                        ProductId = x.Id,
                        MinWebOsnova = CalculateMinWebOsnova(x)
                    }
                );
            });

            await webAdminApimanager.UpdateMinWebOsnove(request);

            return;

            decimal CalculateMinWebOsnova(WebAzuriranjeCenaDto x)
            {
                switch (x.UslovFormiranjaWebCeneType)
                {
                    case UslovFormiranjaWebCeneType.NabavnaCenaPlusProcenat:
                        return x.NabavnaCenaKomercijalno
                            + (
                                x.NabavnaCenaKomercijalno
                                * x.UslovFormiranjaWebCeneModifikator
                                / 100
                            );
                    case UslovFormiranjaWebCeneType.ProdajnaCenaPlusProcenat:
                        return x.ProdajnaCenaKomercijalno
                            - (
                                x.ProdajnaCenaKomercijalno
                                * x.UslovFormiranjaWebCeneModifikator
                                / 100
                            );
                    case UslovFormiranjaWebCeneType.CenaNaUpit:
                        return 0;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public async Task<
            List<KeyValuePair<long, string>>
        > AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestion(
            AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestionRequest request
        )
        {
            var response = new List<KeyValuePair<long, string>>(
                new List<KeyValuePair<long, string>>()
            );

            if (
                string.IsNullOrWhiteSpace(request.SearchText)
                || request.SearchText.Length
                    < Contracts.Constants.AzurirajCeneUslovFormiranjaMinWebOsnovaProductSuggestionSearchTextMinimumLength
            )
                return response;

            var filteredWebProducts = await webAdminApimanager.ProductsGetMultipleAsync(
                new ProductsGetMultipleRequest() { SearchFilter = request.SearchText }
            );

            foreach (var productEntity in filteredWebProducts)
                response.Add(new KeyValuePair<long, string>(productEntity.Id, productEntity.Name));

            return response;
        }

        public async Task<List<ProductsGetDto>?> GetProducts(ProductsGetMultipleRequest request) =>
            await webAdminApimanager.ProductsGetMultipleAsync(request);
    }
}
