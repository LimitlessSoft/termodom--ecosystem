using LSCore.Contracts;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using Microsoft.Extensions.Logging;
using Omu.ValueInjecter;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Interfaces.IRepositories;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers;

public class ProductPriceGroupManager (
    ILogger<ProductPriceGroupManager> logger,
    IProductPriceGroupRepository repository,
    WebDbContext dbContext,
    LSCoreContextUser contextUser)
    : IProductPriceGroupManager
{
    public void Delete(LSCoreIdRequest request) =>
        repository.HardDelete(request.Id);

    public List<ProductPriceGroupGetDto> GetMultiple() =>
        repository.GetMultiple().ToDtoList<ProductPriceGroupEntity, ProductPriceGroupGetDto>();

    public long Save(ProductPriceGroupSaveRequest request)
    {
        var entity = request.Id == 0
                ? new ProductPriceGroupEntity()
                : repository.Get(request.Id!.Value);
        entity.InjectFrom(request);
        repository.UpdateOrInsert(entity);
        return entity.Id;
    }
}