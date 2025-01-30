using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.Proracuni;
using TD.Office.Public.Contracts.Dtos.SpecifikacijaNovca;

namespace TD.Office.Public.Contracts.DtosMappings.SpecifikacijaNovca;
public class GetSpecifikacijaNovcaDtoMappings : ILSCoreDtoMapper<SpecifikacijaNovcaEntity, GetSpecifikacijaNovcaDto>
{
    public GetSpecifikacijaNovcaDto ToDto(SpecifikacijaNovcaEntity sender)
    {
        var dto = new GetSpecifikacijaNovcaDto();
        dto.InjectFrom(sender);
        return dto;
    }
}
