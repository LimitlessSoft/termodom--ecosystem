using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Dtos.ProductPrices;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductsPrices;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class ProductPriceManager : LSCoreBaseManager<ProductPriceManager, ProductPriceEntity>, IProductPriceManager
    {
        public ProductPriceManager(ILogger<ProductPriceManager> logger, WebDbContext dbContext)
           : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<ProductsPricesGetDto> GetMultiple() =>
            new LSCoreListResponse<ProductsPricesGetDto>(
                Queryable()
                .ToDtoList<ProductsPricesGetDto, ProductPriceEntity>());

        public LSCoreResponse Delete(LSCoreIdRequest request) =>
            HardDelete(request.Id);

        public LSCoreResponse<long> Save(SaveProductPriceRequest request) =>
            Save(request, (entity) => new LSCoreResponse<long>(entity.Id));
    }
}
