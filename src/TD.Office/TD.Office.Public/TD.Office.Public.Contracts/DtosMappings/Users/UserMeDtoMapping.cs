using TD.Office.Public.Contracts.Dtos.Users;
using TD.Office.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;

namespace TD.Office.Public.Contracts.DtosMappings.Users
{
    public class UserMeDtoMapping : ILSCoreDtoMapper<UserEntity, UserMeDto>
    {
        public UserMeDto ToDto(UserEntity? sender) => new UserMeDto
            {
                UserData = sender == null ? null :
                    new UserMeDataDto()
                    {
                        Username = sender.Username,
                        StoreId = sender.StoreId
                    }
            };
    }
}
