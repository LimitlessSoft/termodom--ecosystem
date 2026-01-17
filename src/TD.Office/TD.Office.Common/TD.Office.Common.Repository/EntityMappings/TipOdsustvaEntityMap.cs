using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
	public class TipOdsustvaEntityMap : LSCoreEntityMap<TipOdsustvaEntity>
	{
		public override Action<EntityTypeBuilder<TipOdsustvaEntity>> Mapper { get; } =
			builder =>
			{
				builder.Property(x => x.Naziv).IsRequired().HasMaxLength(100);

				builder.Property(x => x.Redosled).IsRequired();
			};
	}
}
