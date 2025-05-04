using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;

public class LogEntityMap : LSCoreEntityMap<LogEntity>
{
	public override Action<EntityTypeBuilder<LogEntity>> Mapper { get; } = builder => { };
}
