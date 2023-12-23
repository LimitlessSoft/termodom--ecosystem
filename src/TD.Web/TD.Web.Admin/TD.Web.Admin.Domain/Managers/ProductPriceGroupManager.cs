using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class ProductPriceGroupManager : LSCoreBaseManager<ProductPriceGroupManager, ProductPriceGroupEntity>, IProductPriceGroupManager
    {
        public ProductPriceGroupManager(ILogger<ProductPriceGroupManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse Delete(LSCoreIdRequest request) =>
            HardDelete(request.Id);

        public LSCoreListResponse<ProductPriceGroupGetDto> GetMultiple()
        {
            var response = new LSCoreListResponse<ProductPriceGroupGetDto>();

            var qResponse = Queryable(x => x.IsActive);
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!.ToDtoList<ProductPriceGroupGetDto, ProductPriceGroupEntity>();
            return response;
        }

        public LSCoreResponse<long> Save(ProductPriceGroupSaveRequest request) =>
            Save(request, (entity) => new LSCoreResponse<long>(entity.Id));
    }
}
