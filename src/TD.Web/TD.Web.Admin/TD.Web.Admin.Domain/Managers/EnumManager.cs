using LSCore.Contracts;
using LSCore.Contracts.Dtos;
using LSCore.Contracts.Extensions;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers;

public class EnumManager(
    ILogger<EnumManager> logger,
    WebDbContext dbContext,
    LSCoreContextUser contextUser
) : LSCoreManagerBase<EnumManager>(logger, dbContext, contextUser), IEnumManager
{
    public List<LSCoreIdNamePairDto> GetOrderStatuses() =>
        Enum.GetValues(typeof(OrderStatus))
            .Cast<OrderStatus>()
            .Select(classification => new LSCoreIdNamePairDto
            {
                Id = (int)classification,
                Name = classification.GetDescription()
            })
            .ToList();

    public List<LSCoreIdNamePairDto> GetUserTypes() =>
        Enum.GetValues(typeof(UserType))
            .Cast<UserType>()
            .Select(classification => new LSCoreIdNamePairDto
            {
                Id = (int)classification,
                Name = classification.GetDescription()
            })
            .ToList();

    public List<LSCoreIdNamePairDto> GetProductGroupTypes() =>
        Enum.GetValues(typeof(ProductGroupType))
            .Cast<ProductGroupType>()
            .Select(classification => new LSCoreIdNamePairDto
            {
                Id = (int)classification,
                Name = classification.GetDescription()
            })
            .ToList();

    public List<LSCoreIdNamePairDto> GetProductStockTypes() =>
        Enum.GetValues(typeof(ProductStockType))
            .Cast<ProductStockType>()
            .Select(stockType => new LSCoreIdNamePairDto
            {
                Id = (int)stockType,
                Name = stockType.GetDescription()
            })
            .ToList();

    public List<LSCoreIdNamePairDto> GetCalculatorTypes()
    {
        var calculatorTypes = Enum.GetValues(typeof(CalculatorType))
            .Cast<CalculatorType>()
            .Select(calculatorType => new LSCoreIdNamePairDto
            {
                Id = (int)calculatorType,
                Name = calculatorType.GetDescription()
            })
            .ToList();

        return calculatorTypes;
    }
}
