using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
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

        public ListResponse<ProductsPricesGetDto> GetMultiple() => new ListResponse<ProductsPricesGetDto>(
            Queryable()
            .ToList()
            .ToListDto());

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

        public Response<long> Save(SaveProductPriceRequest request)
        {
            var response = new Response<long>();

            if (request.IsRequestInvalid(response))
                return response;

            var saveResponse = base.Save(request);
            response.Merge(saveResponse);
            if (response.NotOk)
                return response;

            response.Payload = saveResponse.Payload.Id;
            return response;
        }
    }
}
