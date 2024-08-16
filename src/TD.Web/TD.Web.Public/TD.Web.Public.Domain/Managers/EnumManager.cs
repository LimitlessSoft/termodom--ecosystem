using TD.Web.Public.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Enums;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;
using LSCore.Contracts.Dtos;
using LSCore.Contracts;

namespace TD.Web.Public.Domain.Managers;

public class EnumManager (ILogger<EnumManager> logger, WebDbContext dbContext, LSCoreContextUser contextUser)
    : LSCoreManagerBase<EnumManager>(logger, dbContext, contextUser), IEnumManager
{
    public List<LSCoreIdNamePairDto> GetProductStockTypes() =>
        Enum.GetValues(typeof(ProductStockType))
            .Cast<ProductStockType>()
            .Select(stockType => new LSCoreIdNamePairDto
            {
                Id = (int)stockType,
                Name = stockType.GetDescription()
            })
            .ToList();
}