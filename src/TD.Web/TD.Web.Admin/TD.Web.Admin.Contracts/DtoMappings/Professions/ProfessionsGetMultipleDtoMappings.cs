using TD.Web.Admin.Contracts.Dtos.Professions;
using TD.Web.Common.Contracts.Entities;
using LSCore.Contracts.Interfaces;

namespace TD.Web.Admin.Contracts.DtoMappings.Professions
{
    public class ProfessionsGetMultipleDtoMappings : ILSCoreDtoMapper<ProfessionEntity, ProfessionsGetMultipleDto>
    {
        public ProfessionsGetMultipleDto ToDto(ProfessionEntity sender) =>
            new ()
            {
                Id = sender.Id,
                Name = sender.Name
            };
    }
}
