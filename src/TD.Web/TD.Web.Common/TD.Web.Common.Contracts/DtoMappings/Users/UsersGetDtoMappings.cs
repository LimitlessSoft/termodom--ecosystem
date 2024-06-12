using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;
using LSCore.Contracts.Extensions;

namespace TD.Web.Common.Contracts.DtoMappings.Users;

public class UsersGetDtoMappings : ILSCoreDtoMapper<UserEntity, UsersGetDto>
{
    public UsersGetDto ToDto(UserEntity sender) =>
        new ()
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
            CityId = sender.CityId
        };
}