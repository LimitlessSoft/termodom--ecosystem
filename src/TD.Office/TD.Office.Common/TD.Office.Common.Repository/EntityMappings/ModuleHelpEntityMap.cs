using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;

public class ModuleHelpEntityMap : LSCoreEntityMap<ModuleHelpEntity>
{
	public override Action<EntityTypeBuilder<ModuleHelpEntity>> Mapper { get; } = (builder) => { };
}
