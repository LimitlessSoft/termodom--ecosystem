using LSCore.Contracts.Interfaces;
using TD.Web.Admin.Contracts.Dtos.Professions;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Professions
{
    public class ProfessionsGetMultipleDtoMappings : ILSCoreDtoMapper<ProfessionsGetMultipleDto, ProfessionEntity>
    {
        public ProfessionsGetMultipleDto ToDto(ProfessionEntity sender) =>
            new ProfessionsGetMultipleDto()
            {
                Id = sender.Id,
                Name = sender.Name
            };
    }
}
