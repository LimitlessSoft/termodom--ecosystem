using LSCore.Mapper.Contracts;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Users;

namespace TD.Office.Public.Contracts.DtosMappings.Users;

public class UserDtoMapping : ILSCoreMapper<UserEntity, UserDto>
{
	public UserDto ToMapped(UserEntity sender) =>
		new()
		{
			Id = sender.Id,
			Username = sender.Username,
			Nickname = sender.Nickname,
			MaxRabatMPDokumenti = sender.MaxRabatMPDokumenti,
			MaxRabatVPDokumenti = sender.MaxRabatVPDokumenti,
			StoreId = sender.StoreId,
			VpMagacinId = sender.VPMagacinId
		};
}
