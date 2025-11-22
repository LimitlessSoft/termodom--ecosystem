using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;

public class KomercijalnoPriceKoeficijentEntityMap
	: LSCoreEntityMap<KomercijalnoPriceKoeficijentEntity>
{
	public override Action<EntityTypeBuilder<KomercijalnoPriceKoeficijentEntity>> Mapper { get; } =
		builder =>
		{
			builder.HasIndex(x => x.Naziv).IsUnique();
		};
}
