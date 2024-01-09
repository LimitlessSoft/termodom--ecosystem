using Omu.ValueInjecter;
using LSCore.Contracts.Interfaces;
using TD.Komercijalno.Contracts.Dtos.RobaUMagacinu;

namespace TD.Komercijalno.Contracts.DtoMappings.RobaUMagacinu
{
    public class RobaUMagacinuGetDtoMapping : ILSCoreDtoMapper<RobaUMagacinuGetDto, Entities.RobaUMagacinu>
    {
        public RobaUMagacinuGetDto ToDto(Entities.RobaUMagacinu sender)
        {
            var dto = new RobaUMagacinuGetDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}
