using LSCore.Contracts;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;

namespace TD.Web.Admin.Domain.Managers;

public class ProductGroupManager (ILogger<ProductGroupManager> logger, WebDbContext dbContext, LSCoreContextUser contextUser)
    : LSCoreManagerBase<ProductGroupManager, ProductGroupEntity>(logger, dbContext, contextUser), IProductGroupManager
{
    public ProductsGroupsGetDto Get(LSCoreIdRequest request) =>
        Queryable()
            .FirstOrDefault(x => x.Id == request.Id && x.IsActive)?
            .ToDto<ProductGroupEntity, ProductsGroupsGetDto>()
        ?? throw new LSCoreNotFoundException();

    public List<ProductsGroupsGetDto> GetMultiple() =>
        Queryable()
            .Where(x => x.IsActive)
            .ToDtoList<ProductGroupEntity, ProductsGroupsGetDto>();

    public long Save(ProductsGroupsSaveRequest request) => 
        Save(request, (entity) => entity.Id);

    public void Delete(ProductsGroupsDeleteRequest request)
    {
        request.Validate();
        HardDelete(request.Id);
    }

    public void UpdateType(ProductsGroupUpdateTypeRequest request) =>
        Save(request);
}