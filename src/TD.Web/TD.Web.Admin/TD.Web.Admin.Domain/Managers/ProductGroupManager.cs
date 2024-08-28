using LSCore.Contracts;
using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Admin.Contracts.Helpers.ProductGroups;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers;

public class ProductGroupManager(
    ILogger<ProductGroupManager> logger,
    WebDbContext dbContext,
    LSCoreContextUser contextUser
)
    : LSCoreManagerBase<ProductGroupManager, ProductGroupEntity>(logger, dbContext, contextUser),
        IProductGroupManager
{
    public ProductsGroupsGetDto Get(LSCoreIdRequest request) =>
        Queryable()
            .FirstOrDefault(x => x.Id == request.Id && x.IsActive)
            ?.ToDto<ProductGroupEntity, ProductsGroupsGetDto>()
        ?? throw new LSCoreNotFoundException();

    public List<ProductsGroupsGetDto> GetMultiple() =>
        Queryable().Where(x => x.IsActive).ToDtoList<ProductGroupEntity, ProductsGroupsGetDto>();

    public long Save(ProductsGroupsSaveRequest request)
    {
        var entity =
            request.Id == 0
                ? new ProductGroupEntity()
                : Queryable().FirstOrDefault(x => x.Id == request.Id);

        if (entity == null)
            throw new LSCoreNotFoundException();

        entity.Name = request.Name;
        entity.ParentGroupId = request.ParentGroupId;
        entity.WelcomeMessage = request.WelcomeMessage;
        entity.Src = ProductGroupsHelpers.FormatName(request.Name);

        Insert(entity);
        return entity.Id;
    }

    public void Delete(ProductsGroupsDeleteRequest request)
    {
        request.Validate();
        HardDelete(request.Id);
    }

    public void UpdateType(ProductsGroupUpdateTypeRequest request) => Save(request);
}
