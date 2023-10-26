using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Extensions;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.DtoMappings.ProductsPrices;
using TD.Web.Contracts.Dtos.ProductPrices;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Contracts.Interfaces.Managers;
using TD.Web.Contracts.Requests.ProductsPrices;
using TD.Web.Repository;

namespace TD.Web.Domain.Managers
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
