using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Contracts.DtoMappings.Users;

public static class UserInformationDtoMappings
{
	public static UserInformationDto ToUserInformationDto(this UserEntity userEntity)
	{
		var dto = new UserInformationDto();
		if (userEntity == null)
			return dto;

		dto.UserData = new UserDataDto()
		{
			IsAdmin = userEntity.Type == UserType.Admin | userEntity.Type == UserType.SuperAdmin,
			Nickname = userEntity.Nickname
		};
		return dto;
	}
}
