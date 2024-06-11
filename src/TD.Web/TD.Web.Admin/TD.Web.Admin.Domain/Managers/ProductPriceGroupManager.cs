using TD.Web.Admin.Contracts.Requests.ProductPriceGroup;
using TD.Web.Admin.Contracts.Dtos.ProductsPricesGroup;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;

namespace TD.Web.Admin.Domain.Managers;

public class ProductPriceGroupManager (ILogger<ProductPriceGroupManager> logger, WebDbContext dbContext)
    : LSCoreManagerBase<ProductPriceGroupManager, ProductPriceGroupEntity>(logger, dbContext),
        IProductPriceGroupManager
{
    public void Delete(LSCoreIdRequest request) =>
        HardDelete(request.Id);

    public List<ProductPriceGroupGetDto> GetMultiple() =>
        Queryable().Where(x => x.IsActive)
            .ToDtoList<ProductPriceGroupEntity, ProductPriceGroupGetDto>();

    public long Save(ProductPriceGroupSaveRequest request) =>
        Save(request, (entity) => entity.Id);
}