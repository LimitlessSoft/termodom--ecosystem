using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contrats.Dtos.Users;

namespace TD.Web.Common.Contracts.DtoMappings.Users
{
    public static class UserInformationDtoMappings
    {
        public static UserInformationDto ToUserInformationDto(this UserEntity? userEntity)
        {
            var dto = new UserInformationDto();
            if (userEntity != null)
            {
                dto.UserData = new UserDataDto();
                dto.UserData.InjectFrom(userEntity);
            }
            return dto;
        }
    }
}
