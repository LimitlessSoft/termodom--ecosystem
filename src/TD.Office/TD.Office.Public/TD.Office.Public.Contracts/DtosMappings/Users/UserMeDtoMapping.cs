using LSCore.Mapper.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Common.Contracts.Enums;
using TD.Office.Public.Contracts.Dtos.Users;

namespace TD.Office.Public.Contracts.DtosMappings.Users;

public class UserMeDtoMapping : ILSCoreMapper<UserEntity, UserMeDto>
{
	public UserMeDto ToMapped(UserEntity? sender) =>
		new UserMeDto
		{
			UserData =
				sender == null
					? null
					: new UserMeDataDto
					{
						Username = sender.Username,
						StoreId = sender.StoreId,
						VpStoreId = sender.VPMagacinId,
						IsAdmin = sender.Type == UserType.SuperAdministrator,
					},
		};
}
