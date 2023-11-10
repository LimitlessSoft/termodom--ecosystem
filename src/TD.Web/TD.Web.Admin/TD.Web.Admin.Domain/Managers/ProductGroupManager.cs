using LSCore.Contracts.Http;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using LSCore.Domain.Validators;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class ProductGroupManager : LSCoreBaseManager<ProductGroupManager, ProductGroupEntity>, IProductGroupManager
    {
        public ProductGroupManager(ILogger<ProductGroupManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse<ProductsGroupsGetDto> Get(LSCoreIdRequest request) =>
            First<ProductGroupEntity, ProductsGroupsGetDto>(x => x.Id == request.Id && x.IsActive);

        public LSCoreListResponse<ProductsGroupsGetDto> GetMultiple() =>
            new LSCoreListResponse<ProductsGroupsGetDto>(
                Queryable(x => x.IsActive)
                .ToDtoList<ProductsGroupsGetDto, ProductGroupEntity>());

        public LSCoreResponse<long> Save(ProductsGroupsSaveRequest request) => 
            Save(request, (entity) => new LSCoreResponse<long>(entity.Id));

        public LSCoreResponse Delete(ProductsGroupsDeleteRequest request)
        {
            var response = new LSCoreResponse();

            if(request.IsRequestInvalid(response))
                return response;

            return HardDelete(request.Id);
        }
    }
}
