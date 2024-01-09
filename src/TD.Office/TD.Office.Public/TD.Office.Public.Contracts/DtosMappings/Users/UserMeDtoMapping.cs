using LSCore.Contracts.Interfaces;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Users;

namespace TD.Office.Public.Contracts.DtosMappings.Users
{
    public class UserMeDtoMapping : ILSCoreDtoMapper<UserMeDto, UserEntity>
    {
        public UserMeDto ToDto(UserEntity sender) => new UserMeDto
            {
                UserData = sender == null ? null :
                    new UserMeDataDto()
                    {
                        Username = sender.Username,
                    }
            };
    }
}
