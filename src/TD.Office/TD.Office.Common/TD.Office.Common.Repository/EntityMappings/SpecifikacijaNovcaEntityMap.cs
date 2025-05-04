using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;

public class SpecifikacijaNovcaEntityMap : LSCoreEntityMap<SpecifikacijaNovcaEntity>
{
	public override Action<EntityTypeBuilder<SpecifikacijaNovcaEntity>> Mapper { get; } =
		(builder) =>
		{
			builder.ToTable("SpecifikacijaNovca");
		};
}
