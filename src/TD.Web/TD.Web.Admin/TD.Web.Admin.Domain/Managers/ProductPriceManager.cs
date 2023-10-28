using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Extensions;
using TD.Core.Domain.Managers;
using TD.Web.Admin.Contracts.Dtos.ProductPrices;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductsPrices;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class ProductPriceManager : BaseManager<ProductPriceManager, ProductPriceEntity>, IProductPriceManager
    {
        public ProductPriceManager(ILogger<ProductPriceManager> logger, WebDbContext dbContext)
           : base(logger, dbContext)
        {
        }

        public ListResponse<ProductsPricesGetDto> GetMultiple() =>
            new ListResponse<ProductsPricesGetDto>(
                Queryable()
                .ToDtoList<ProductsPricesGetDto, ProductPriceEntity>());

        public Response Delete(IdRequest request) =>
            HardDelete(request.Id);

        public Response<long> Save(SaveProductPriceRequest request) =>
            Save(request, (entity) => new Response<long>(entity.Id));
    }
}
