using LSCore.Contracts;
using TD.Web.Public.Contracts.Requests.ProductsGroups;
using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Public.Contracts.Dtos.ProductsGroups;
using TD.Web.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using TD.Web.Common.Repository;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;

namespace TD.Web.Public.Domain.Managers;

public class ProductGroupManager (ILogger<ProductGroupManager> logger, WebDbContext dbContext, LSCoreContextUser contextUser)
    : LSCoreManagerBase<ProductGroupManager, ProductGroupEntity>(logger, dbContext, contextUser), IProductGroupManager
{
    public ProductsGroupsGetDto Get(string name) =>
        Queryable()
           .Include(x =>
               x.ParentGroup)
           .FirstOrDefault(x => x.IsActive &&
                                x.Name.ToLower() == name.ToLower())?
           .ToDto<ProductGroupEntity, ProductsGroupsGetDto>()
       ?? throw new LSCoreNotFoundException();

    public List<ProductsGroupsGetDto> GetMultiple(ProductsGroupsGetRequest request) =>
        Queryable()
            .Include(x => x.ParentGroup)
            .Where(x =>
                (!request.ParentId.HasValue && !string.IsNullOrWhiteSpace(request.ParentName) || x.ParentGroupId == request.ParentId) &&
                (string.IsNullOrWhiteSpace(request.ParentName) || x.ParentGroup != null && x.ParentGroup.Name.ToLower() == request.ParentName.ToLower()))
            .ToDtoList<ProductGroupEntity, ProductsGroupsGetDto>();
}