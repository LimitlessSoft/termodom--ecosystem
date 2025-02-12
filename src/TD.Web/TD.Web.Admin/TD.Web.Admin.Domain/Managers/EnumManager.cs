using LSCore.Contracts.Dtos;
using LSCore.Contracts.Extensions;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Admin.Domain.Managers;

public class EnumManager : IEnumManager
{
    public List<LSCoreIdNamePairDto> GetOrderStatuses() =>
        Enum.GetValues<OrderStatus>()
            .Select(classification => new LSCoreIdNamePairDto
            {
                Id = (int)classification,
                Name = classification.GetDescription()
            })
            .ToList();

    public List<LSCoreIdNamePairDto> GetUserTypes() =>
        Enum.GetValues<UserType>()
            .Select(classification => new LSCoreIdNamePairDto
            {
                Id = (int)classification,
                Name = classification.GetDescription()
            })
            .ToList();

    public List<LSCoreIdNamePairDto> GetProductGroupTypes() =>
        Enum.GetValues<ProductGroupType>()
            .Select(classification => new LSCoreIdNamePairDto
            {
                Id = (int)classification,
                Name = classification.GetDescription()
            })
            .ToList();

    public List<LSCoreIdNamePairDto> GetProductStockTypes() =>
        Enum.GetValues<ProductStockType>()
            .Select(stockType => new LSCoreIdNamePairDto
            {
                Id = (int)stockType,
                Name = stockType.GetDescription()
            })
            .ToList();

    public List<LSCoreIdNamePairDto> GetCalculatorTypes() =>
        Enum.GetValues<CalculatorType>()
            .Select(calculatorType => new LSCoreIdNamePairDto
            {
                Id = (int)calculatorType,
                Name = calculatorType.GetDescription()
            })
            .ToList();
}
