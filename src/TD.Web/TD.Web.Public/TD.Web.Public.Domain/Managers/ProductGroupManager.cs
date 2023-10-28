using Microsoft.Extensions.Logging;
using TD.Core.Contracts.Dtos;
using TD.Core.Contracts.Http;
using TD.Core.Domain.Extensions;
using TD.Core.Domain.Managers;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;
using TD.Web.Public.Contrats.Interfaces.IManagers;
using TD.Web.Public.Contrats.Requests.ProductsGroups;

namespace TD.Web.Public.Domain.Managers
{
    public class ProductGroupManager : BaseManager<ProductGroupManager, ProductGroupEntity>, IProductGroupManager
    {
        public ProductGroupManager(ILogger<ProductGroupManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public ListResponse<IdNamePairDto> GetMultiple(ProductsGroupsGetRequest request) =>
            new ListResponse<IdNamePairDto>(
                Queryable(x => x.IsActive &&
                    (request.ParentId == null || x.ParentGroupId == request.ParentId.Value))
                .ToDtoList<IdNamePairDto, ProductGroupEntity>());
    }
}
