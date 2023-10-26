using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Extensions;
using TD.Core.Domain.Managers;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Entities;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;
using TD.Web.Admin.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class ProductPriceGroupManager : BaseManager<ProductPriceGroupManager, ProductPriceGroupEntity>, IProductPriceGroupManager
    {
        public ProductPriceGroupManager(ILogger<ProductPriceGroupManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public Response Delete(IdRequest request) =>
            HardDelete(request.Id);

        public ListResponse<ProductPriceGroupGetDto> GetMultiple() => new ListResponse<ProductPriceGroupGetDto>(
            Queryable(x => x.IsActive)
            .ToDtoList<ProductPriceGroupGetDto, ProductPriceGroupEntity>());

        public Response<long> Save(ProductPriceGroupSaveRequest request) =>
            Save(request, (entity) => new Response<long>(entity.Id));
    }
}
