using LSCore.Contracts.Interfaces;
using Omu.ValueInjecter;
using TD.Office.Common.Contracts.Entities;
using TD.Office.Public.Contracts.Dtos.NalogZaPrevoz;

namespace TD.Office.Public.Contracts.DtosMappings.NalogZaPrevoz
{
    public class GetNalogZaPrevozDtoMapper : ILSCoreDtoMapper<NalogZaPrevozEntity, GetNalogZaPrevozDto>
    {
        public GetNalogZaPrevozDto ToDto(NalogZaPrevozEntity sender)
        {
            var dto = new GetNalogZaPrevozDto();
            dto.InjectFrom(sender);
            return dto;
        }
    }
}