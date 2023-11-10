using LSCore.Contracts.Dtos;
using LSCore.Contracts.Http;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using TD.Web.Public.Contrats.Requests.ProductsGroups;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductGroupManager : LSCoreBaseManager<ProductGroupManager, ProductGroupEntity>, IProductGroupManager
    {
        public ProductGroupManager(ILogger<ProductGroupManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<LSCoreIdNamePairDto> GetMultiple(ProductsGroupsGetRequest request) =>
            new LSCoreListResponse<LSCoreIdNamePairDto>(
                Queryable(x => x.IsActive &&
                    (request.ParentId == null || x.ParentGroupId == request.ParentId.Value))
                .ToDtoList<LSCoreIdNamePairDto, ProductGroupEntity>());
    }
}
