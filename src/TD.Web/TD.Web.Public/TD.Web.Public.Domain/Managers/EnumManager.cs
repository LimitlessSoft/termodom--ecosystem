using LSCore.Contracts.Dtos;
using LSCore.Contracts.Extensions;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Public.Contracts.Interfaces.IManagers;

namespace TD.Web.Public.Domain.Managers;

public class EnumManager : IEnumManager
{
    public List<LSCoreIdNamePairDto> GetProductStockTypes() =>
        Enum.GetValues<ProductStockType>()
            .Select(stockType => new LSCoreIdNamePairDto
            {
                Id = (int)stockType,
                Name = stockType.GetDescription()
            })
            .ToList();
}