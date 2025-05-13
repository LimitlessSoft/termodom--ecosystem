using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
	public class NalogZaPrevozEntityMap : LSCoreEntityMap<NalogZaPrevozEntity>
	{
		public override Action<EntityTypeBuilder<NalogZaPrevozEntity>> Mapper { get; } =
			builder =>
			{
				builder.Property(x => x.CenaPrevozaBezPdv).IsRequired();

				builder.Property(x => x.MiNaplatiliKupcuBezPdv).IsRequired();

				builder.Property(x => x.Address).IsRequired();

				builder.Property(x => x.Mobilni).IsRequired();
			};
	}
}
