using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;
using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;

namespace TD.Komercijalno.Contracts.DtoMappings.RobaUMagacinu
{
    public class RobaUMagacinuGetDtoMapping : ILSCoreDtoMapper<Entities.RobaUMagacinu, RobaUMagacinuGetDto>
    {
        public RobaUMagacinuGetDto ToDto(Entities.RobaUMagacinu sender)
        {
            var dto = new RobaUMagacinuGetDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
