using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Managers;
using TD.Web.Contracts.DtoMappings.ProductsGroups;
using TD.Web.Contracts.Dtos.ProductsGroups;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Repository;

namespace TD.Web.Domain.Managers
{
    public class ProductGroupManager : BaseManager<ProductGroupManager, ProductGroupEntity>, IProductGroupManager
    {
        public ProductGroupManager(ILogger<ProductGroupManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public Response<ProductsGroupsGetDto> Get(IdRequest request)
        {
            var response = new Response<ProductsGroupsGetDto>();
            var productGroupResponse = First(x => x.Id == request.Id && x.IsActive);

            response.Merge(productGroupResponse);
            if (response.NotOk)
                return response;

            response.Payload = productGroupResponse.Payload.ToDto();
            return response;
        }

        public ListResponse<ProductsGroupsGetDto> GetMultiple() =>
            new ListResponse<ProductsGroupsGetDto>(
                Queryable()
                .Where(x => x.IsActive)
                .ToList()
                .ToDtoList());
    }
}
