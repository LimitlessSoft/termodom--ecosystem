using Microsoft.Extensions.Logging;
using System.Formats.Tar;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Extensions;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.DtoMappings.ProductPricesGroup;
using TD.Web.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Contracts.Requests.ProductPriceGroup;
using TD.Web.Repository;

namespace TD.Web.Domain.Managers
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
