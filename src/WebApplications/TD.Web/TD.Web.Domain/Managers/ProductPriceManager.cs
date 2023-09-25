using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Managers;
using TD.Web.Contracts.DtoMappings.ProductsPrices;
using TD.Web.Contracts.Dtos.ProductPrices;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Contracts.Interfaces.Managers;
using TD.Web.Repository;

namespace TD.Web.Domain.Managers
{
    public class ProductPriceManager : BaseManager<ProductPriceManager, ProductPriceEntity>, IProductPriceManager
    {
        public ProductPriceManager(ILogger<ProductPriceManager> logger, WebDbContext dbContext)

           : base(logger, dbContext)
        {
        }

        public ListResponse<ProductsPricesGetDto> GetMultiple() => new ListResponse<ProductsPricesGetDto>(
            Queryable().
            ToList().
            ToListDto());

        public Response<bool> Delete(IdRequest request)
        {
            var response = new Response<bool>();
            var entityResponse = First(x => x.Id == request.Id);

            response.Merge(entityResponse);
            if (response.NotOk)
                return response;

            HardDelete(entityResponse.Payload);
            return response;
        }
    }
}
