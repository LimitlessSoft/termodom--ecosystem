using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;

public class ProracunItemEntityMap : LSCoreEntityMap<ProracunItemEntity>
{
	public override Action<EntityTypeBuilder<ProracunItemEntity>> Mapper { get; } =
		(builder) =>
		{
			builder.Property(x => x.ProracunId).IsRequired();
			builder.Property(x => x.RobaId).IsRequired();
			builder.Property(x => x.Kolicina).IsRequired();
			builder.Property(x => x.CenaBezPdv).IsRequired();
			builder.Property(x => x.Rabat).IsRequired();
			builder.Property(x => x.Pdv).IsRequired();
		};
}
