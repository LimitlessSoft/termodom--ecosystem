using Omu.ValueInjecter;
using TD.TDOffice.Contracts.Dtos.Users;
using TD.TDOffice.Contracts.Entities;

namespace TD.TDOffice.Contracts.DtoMappings.Users
{
	public static class UserDtoMappings
	{
		public static List<UserDto> ToUserDtoList(this List<User> users)
		{
			var list = new List<UserDto>();
			foreach (var user in users)
				list.Add(ToUserDto(user));
			return list;
		}

		public static UserDto ToUserDto(this User user)
		{
			var dto = new UserDto();
			dto.InjectFrom(user);
			return dto;
		}
	}
}
