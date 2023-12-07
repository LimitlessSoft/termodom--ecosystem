
using Omu.ValueInjecter;
using TD.Web.Common.Contracts.Dtos.Users;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Public.Contrats.Dtos.Users;

namespace TD.Web.Common.Contracts.DtoMappings
{
    public static class UsersDtoMappings
    {
        public static UserDataDto ToUserInformationDto(this UserEntity userEntity)
        {
            var dto = new UserDataDto();
            dto.InjectFrom(userEntity);
            return dto;
        }
    }
}
