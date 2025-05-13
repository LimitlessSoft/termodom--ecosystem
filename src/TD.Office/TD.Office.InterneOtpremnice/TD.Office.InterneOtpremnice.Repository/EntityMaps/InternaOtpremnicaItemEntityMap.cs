using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.InterneOtpremnice.Contracts.Entities;

namespace TD.Office.InterneOtpremnice.Repository.EntityMaps;

public class InternaOtpremnicaItemEntityMap : LSCoreEntityMap<InternaOtpremnicaItemEntity>
{
	public override Action<EntityTypeBuilder<InternaOtpremnicaItemEntity>> Mapper { get; } =
		(builder) =>
		{
			builder.HasOne<InternaOtpremnicaEntity>().WithMany();
		};
}
