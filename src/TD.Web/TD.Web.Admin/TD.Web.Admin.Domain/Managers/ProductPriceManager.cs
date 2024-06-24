using LSCore.Contracts;
using TD.Web.Admin.Contracts.Requests.ProductsPrices;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Admin.Contracts.Dtos.ProductPrices;
using TD.Web.Common.Contracts.Entities;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Requests;
using LSCore.Domain.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;

namespace TD.Web.Admin.Domain.Managers;

public class ProductPriceManager (ILogger<ProductPriceManager> logger, WebDbContext dbContext, LSCoreContextUser contextUser)
    : LSCoreManagerBase<ProductPriceManager, ProductPriceEntity>(logger, dbContext, contextUser), IProductPriceManager
{
    public List<ProductsPricesGetDto> GetMultiple() =>
        Queryable()
            .Where(x => x.IsActive)
            .ToDtoList<ProductPriceEntity, ProductsPricesGetDto>();

    public void Delete(LSCoreIdRequest request) =>
        HardDelete(request.Id);

    public long Save(SaveProductPriceRequest request) =>
        Save(request, (entity) => entity.Id);
}