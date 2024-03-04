using LSCore.Contracts.Dtos;
using LSCore.Contracts.Extensions;
using LSCore.Contracts.Http;
using LSCore.Domain.Managers;
using Microsoft.Extensions.Logging;
using TD.Web.Admin.Contracts.Interfaces.IManagers;
using TD.Web.Common.Contracts.Enums;
using TD.Web.Common.Repository;

namespace TD.Web.Admin.Domain.Managers
{
    public class EnumManager : LSCoreBaseManager<EnumManager>, IEnumManager
    {
        public EnumManager(ILogger<EnumManager> logger, WebDbContext dbContext)
            : base(logger, dbContext)
        {
        }

        public LSCoreListResponse<LSCoreIdNamePairDto> GetOrderStatuses() =>
            new LSCoreListResponse<LSCoreIdNamePairDto>(
            Enum.GetValues(typeof(OrderStatus))
                .Cast<OrderStatus>()
                .Select(classification => new LSCoreIdNamePairDto
                {
                    Id = (int)classification,
                    Name = classification.GetDescription()
                })
                .ToList());

        public LSCoreListResponse<LSCoreIdNamePairDto> GetUserTypes() =>
        new LSCoreListResponse<LSCoreIdNamePairDto>(
        Enum.GetValues(typeof(UserType))
        .Cast<UserType>()
        .Select(classification => new LSCoreIdNamePairDto
        {
            Id = (int)classification,
            Name = classification.GetDescription()
        })
        .ToList());
    }
}
