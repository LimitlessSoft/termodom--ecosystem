using LSCore.Contracts.Extensions;
using LSCore.Contracts.Interfaces;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Contracts.DtoMappings.Users
{
    public class UsersGetDtoMappings : ILSCoreDtoMapper<UsersGetDto, UserEntity>
    {
        public UsersGetDto ToDto(UserEntity sender) =>
            new UsersGetDto()
            {
                UserTypeId = sender.Type,
                UserType = sender.Type.GetDescription(),
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
}
