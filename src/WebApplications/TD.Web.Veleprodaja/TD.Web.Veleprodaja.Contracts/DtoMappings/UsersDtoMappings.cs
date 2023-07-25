using Omu.ValueInjecter;
using TD.Web.Veleprodaja.Contracts.Dtos.Users;
using TD.Web.Veleprodaja.Contracts.Entities;

namespace TD.Web.Veleprodaja.Contracts.DtoMappings
{
    public static class UsersDtoMappings
    {
        public static UsersMeDto ToUsersMeDto(this User user)
        {
            var dto = new UsersMeDto();
            dto.InjectFrom(user);
            return dto;
        }
    }
}
