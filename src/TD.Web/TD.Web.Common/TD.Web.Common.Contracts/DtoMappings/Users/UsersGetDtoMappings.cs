using LSCore.Contracts.Extensions;
using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.DtoMappings.Users;

public class UsersGetDtoMappings : ILSCoreDtoMapper<UserEntity, UsersGetDto>
{
    public UsersGetDto ToDto(UserEntity sender) =>
        new()
        {
            UserTypeId = sender.Type,
            UserType = sender.Type.GetDescription()!,
            Id = sender.Id,
            Nickname = sender.Nickname,
            Username = sender.Username,
            Mobile = sender.Mobile,
            IsActive = sender.IsActive,
            FavoriteStoreId = sender.FavoriteStoreId,
            ProfessionId = sender.ProfessionId,
            CityId = sender.CityId,
            NumberOfOrdersLastThreeMonths = sender.Orders.Count(x =>
                x.CheckedOutAt >= DateTime.Now.AddMonths(-3)
                && x is { IsActive: true, Status: OrderStatus.Collected }
            ),
            NeverOrdered = sender.Orders.Count == 0
        };
}
