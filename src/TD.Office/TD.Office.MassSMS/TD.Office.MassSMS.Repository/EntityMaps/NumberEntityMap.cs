using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.MassSMS.Contracts.Entities;

namespace TD.Office.MassSMS.Repository.EntityMaps;

public class NumberEntityMap : LSCoreEntityMap<NumberEntity>
{
	public override Action<EntityTypeBuilder<NumberEntity>> Mapper =>
		(builder) =>
		{
			builder.HasIndex(x => x.Number).IsUnique();
			builder.Property(x => x.Number).IsRequired();
			builder.HasIndex(x => x.IsBlacklisted);
			builder.Property(x => x.IsBlacklisted).HasDefaultValue(false).IsRequired();
		};
}
