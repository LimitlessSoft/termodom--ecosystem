using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;

public class PopisDokumentEntityMap : LSCoreEntityMap<PopisDokumentEntity>
{
	public override Action<EntityTypeBuilder<PopisDokumentEntity>> Mapper { get; } =
		(builder) =>
		{
			builder.Property(x => x.MagacinId).IsRequired();
			builder.Property(x => x.Status).IsRequired();
			builder.Property(x => x.Type).IsRequired();
			builder.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.PopisDokumentId);
			builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.CreatedBy);
		};
}
