using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;

public class PopisItemEntityMap : LSCoreEntityMap<PopisItemEntity>
{
	public override Action<EntityTypeBuilder<PopisItemEntity>> Mapper { get; } =
		(builder) =>
		{
			builder.Property(x => x.PopisDokumentId).IsRequired();
			builder.Property(x => x.RobaId).IsRequired();
			builder.HasOne(x => x.Dokument).WithMany().HasForeignKey(x => x.PopisDokumentId);
		};
}
