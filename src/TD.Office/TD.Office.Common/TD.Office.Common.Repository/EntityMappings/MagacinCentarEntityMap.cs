using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;

public class MagacinCentarEntityMap : LSCoreEntityMap<MagacinCentarEntity>
{
	public override Action<EntityTypeBuilder<MagacinCentarEntity>> Mapper { get; } =
		builder =>
		{
			builder.HasIndex(x => x.Naziv).IsUnique();
		};
}
