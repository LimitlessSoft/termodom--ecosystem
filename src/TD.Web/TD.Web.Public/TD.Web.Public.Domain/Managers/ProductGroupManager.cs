using TD.Web.Public.Contrats.Requests.ProductsGroups;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using TD.Web.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using LSCore.Contracts.Http;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductGroupManager : LSCoreBaseManager<ProductGroupManager, ProductGroupEntity>, IProductGroupManager
    {
        public ProductGroupManager(ILogger<ProductGroupManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreResponse<ProductsGroupsGetDto> Get(string name)
        {
            var response = new LSCoreResponse<ProductsGroupsGetDto>();

            var qResponse = Queryable();
            response.Merge(qResponse);
            if (response.NotOk)
                return response;

            response.Payload = qResponse.Payload!
                .Include(x => x.ParentGroup)
                .Where(x =>
                    x.IsActive &&
                    x.Name == name)
                .FirstOrDefault()?
                .ToDto<ProductsGroupsGetDto, ProductGroupEntity>(); ;

            return response;
        }

        public LSCoreListResponse<ProductsGroupsGetDto> GetMultiple(ProductsGroupsGetRequest request) =>
            Queryable()
                .LSCoreIncludes(x => x.ParentGroup)
                .LSCoreFilters(x =>
                    (!request.ParentId.HasValue && !string.IsNullOrWhiteSpace(request.ParentName) || x.ParentGroupId == request.ParentId) &&
                    (string.IsNullOrWhiteSpace(request.ParentName) || x.ParentGroup != null && x.ParentGroup.Name == request.ParentName))
            .ToLSCoreListResponse<ProductsGroupsGetDto, ProductGroupEntity>();
    }
}
