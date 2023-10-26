using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Http;
using TD.Core.Contracts.Requests;
using TD.Core.Domain.Extensions;
using TD.Core.Domain.Managers;
using TD.Core.Domain.Validators;
using TD.Web.Contracts.Dtos.ProductsGroups;
using TD.Web.Contracts.Entities;
using TD.Web.Contracts.Interfaces.IManagers;
using TD.Web.Contracts.Requests.ProductsGroups;
using TD.Web.Repository;

namespace TD.Web.Domain.Managers
{
    public class ProductGroupManager : BaseManager<ProductGroupManager, ProductGroupEntity>, IProductGroupManager
    {
        public ProductGroupManager(ILogger<ProductGroupManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public Response<ProductsGroupsGetDto> Get(IdRequest request) =>
            First<ProductGroupEntity, ProductsGroupsGetDto>(x => x.Id == request.Id && x.IsActive);

        public ListResponse<ProductsGroupsGetDto> GetMultiple() =>
            new ListResponse<ProductsGroupsGetDto>(
                Queryable(x => x.IsActive)
                .ToDtoList<ProductsGroupsGetDto, ProductGroupEntity>());

        public Response<long> Save(ProductsGroupsSaveRequest request) => 
            Save(request, (entity) => new Response<long>(entity.Id));
    }
}
