using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
	public class TipKorisnikaEntityMap : LSCoreEntityMap<TipKorisnikaEntity>
	{
		public override Action<EntityTypeBuilder<TipKorisnikaEntity>> Mapper { get; } =
			builder =>
			{
				builder.Property(x => x.Naziv).IsRequired().HasMaxLength(100);
				builder.Property(x => x.Boja).IsRequired().HasMaxLength(7);
			};
	}
}
