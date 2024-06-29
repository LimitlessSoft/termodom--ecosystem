using LSCore.Contracts;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Enums;
using Microsoft.Extensions.Logging;
using LSCore.Contracts.Extensions;
using TD.Web.Common.Repository;
using LSCore.Domain.Managers;
using LSCore.Contracts.Dtos;

namespace TD.Web.Admin.Domain.Managers;

public class EnumManager (ILogger<EnumManager> logger, WebDbContext dbContext, LSCoreContextUser contextUser)
    : LSCoreManagerBase<EnumManager>(logger, dbContext, contextUser), IEnumManager
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
}