using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;

public class KomercijalnoMagacinFirmaEntityMap : LSCoreEntityMap<KomercijalnoMagacinFirmaEntity>
{
	public override Action<EntityTypeBuilder<KomercijalnoMagacinFirmaEntity>> Mapper { get; } =
		(builder) =>
		{
			builder.HasIndex(x => x.MagacinId).IsUnique();
			builder.Property(x => x.MagacinId).IsRequired();
			builder.Property(x => x.ApiFirma).IsRequired();
		};
}
