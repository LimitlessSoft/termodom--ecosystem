using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IManagers;
using TD.Web.Common.Repository;
using TD.Web.Public.Contracts;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Requests.ProductsGroups;

namespace TD.Web.Public.Domain.Managers;

public class ProductGroupManager(
    ILogger<ProductGroupManager> logger,
    WebDbContext dbContext,
    LSCoreContextUser contextUser,
    ICacheManager cacheManager
)
    : LSCoreManagerBase<ProductGroupManager, ProductGroupEntity>(logger, dbContext, contextUser),
        IProductGroupManager
{
    public ProductsGroupsGetDto Get(string src) =>
        Queryable()
            .Include(x => x.ParentGroup)
            .FirstOrDefault(x => x.IsActive && x.Src.ToLower() == src.ToLower())
            ?.ToDto<ProductGroupEntity, ProductsGroupsGetDto>()
        ?? throw new LSCoreNotFoundException();

    public List<ProductsGroupsGetDto> GetMultiple(ProductsGroupsGetRequest request)
    {
        return cacheManager.GetDataAsync(Constants.CacheKeys.ProductGroups(request),
            () =>
            {
                return Queryable()
                    .Include(x => x.ParentGroup)
                    .Where(x =>
                        (
                            !request.ParentId.HasValue && !string.IsNullOrWhiteSpace(request.ParentName)
                            || x.ParentGroupId == request.ParentId
                        )
                        && (
                            string.IsNullOrWhiteSpace(request.ParentName)
                            || x.ParentGroup != null
                            && x.ParentGroup.Name.ToLower() == request.ParentName.ToLower()
                        )
                    )
                    .ToDtoList<ProductGroupEntity, ProductsGroupsGetDto>();
            }, TimeSpan.FromDays(1)).GetAwaiter().GetResult();
    }
}
