using LSCore.Contracts.Interfaces;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Users;

namespace TD.Office.Public.Contracts.DtosMappings.Users
{
    public class UserDtoMapping : ILSCoreDtoMapper<UserDto, UserEntity>
    {
        public UserDto ToDto(UserEntity sender) =>
            new()
            {
                Id = sender.Id,
                Username = sender.Username,
                Nickname = sender.Nickname
            };
    }
}