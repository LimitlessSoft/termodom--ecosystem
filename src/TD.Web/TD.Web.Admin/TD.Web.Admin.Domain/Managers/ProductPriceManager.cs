using LSCore.Contracts.Extensions;
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

        public LSCoreListResponse<ProductsPricesGetDto> GetMultiple()
        {
            var response = new LSCoreListResponse<ProductsPricesGetDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!.ToDtoList<ProductsPricesGetDto, ProductPriceEntity>();
            return response;
        }

        public LSCoreResponse Delete(LSCoreIdRequest request) =>
            HardDelete(request.Id);

        public LSCoreResponse<long> Save(SaveProductPriceRequest request) =>
            Save(request, (entity) => new LSCoreResponse<long>(entity.Id));
    }
}
