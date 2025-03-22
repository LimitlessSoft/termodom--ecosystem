using LSCore.Mapper.Contracts;
using TD.Web.Admin.Contracts.Dtos.Professions;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Admin.Contracts.DtoMappings.Professions;

public class ProfessionsGetMultipleDtoMappings
	: ILSCoreMapper<ProfessionEntity, ProfessionsGetMultipleDto>
{
	public ProfessionsGetMultipleDto ToMapped(ProfessionEntity sender) =>
		new() { Id = sender.Id, Name = sender.Name };
}
