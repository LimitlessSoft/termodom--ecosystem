using LSCore.Contracts.Exceptions;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using TD.Web.Admin.Contracts.Dtos.ProductsGroups;
using TD.Web.Admin.Contracts.Helpers.ProductGroups;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductsGroups;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;

namespace TD.Web.Admin.Domain.Managers;

public class ProductGroupManager(
    IProductGroupRepository repository
) : IProductGroupManager
{
    public ProductsGroupsGetDto Get(LSCoreIdRequest request) =>
        repository.GetMultiple()
            .FirstOrDefault(x => x.Id == request.Id)
            ?.ToDto<ProductGroupEntity, ProductsGroupsGetDto>()
        ?? throw new LSCoreNotFoundException();

    public List<ProductsGroupsGetDto> GetMultiple() =>
        repository.GetMultiple().ToDtoList<ProductGroupEntity, ProductsGroupsGetDto>();

    public long Save(ProductsGroupsSaveRequest request)
    {
        var entity =
            request.Id == 0
                ? new ProductGroupEntity()
                : repository.Get(request.Id!.Value);

        if (entity == null)
            throw new LSCoreNotFoundException();

        entity.Name = request.Name;
        entity.ParentGroupId = request.ParentGroupId;
        entity.WelcomeMessage = request.WelcomeMessage;
        entity.Src = ProductGroupsHelpers.FormatName(request.Name);

        repository.Insert(entity);
        return entity.Id;
    }

    public void Delete(ProductsGroupsDeleteRequest request)
    {
        request.Validate();
        repository.HardDelete(request.Id);
    }

    public void UpdateType(ProductsGroupUpdateTypeRequest request)
    {
        var entity = repository.Get(request.Id!.Value);
        entity.Type = request.Type;
        repository.Update(entity);
    }
}
