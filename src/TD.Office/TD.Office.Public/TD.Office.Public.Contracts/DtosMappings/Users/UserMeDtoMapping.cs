using LSCore.Contracts.Interfaces;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Users;

namespace TD.Office.Public.Contracts.DtosMappings.Users;

public class UserMeDtoMapping : ILSCoreDtoMapper<UserEntity, UserMeDto>
{
    public UserMeDto ToDto(UserEntity? sender) =>
        new UserMeDto
        {
            UserData =
                sender == null
                    ? null
                    : new UserMeDataDto
                    {
                        Username = sender.Username,
                        StoreId = sender.StoreId,
                        VpStoreId = sender.VPMagacinId
                    }
        };
}
