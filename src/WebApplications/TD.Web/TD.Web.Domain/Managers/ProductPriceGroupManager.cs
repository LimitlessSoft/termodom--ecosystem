using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
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

        public ListResponse<ProductPriceGroupGetDto> GetMultiple() => new ListResponse<ProductPriceGroupGetDto>(
            Queryable()
            .Where(x => x.IsActive)
            .ToList()
            .ToListDto());
            

        public Response<long> Save(ProductPriceGroupSaveRequest request)
        {
            var response = new Response<long>();

            if (request.IsRequestInvalid(response))
                return response;

            var productEntityResponse = base.Save(request);
            response.Merge(productEntityResponse);
            if (response.NotOk || productEntityResponse.Payload == null)
                return response;

            response.Payload = productEntityResponse.Payload.Id;

            Update(productEntityResponse.Payload);

            return response;
        }
    }
}
