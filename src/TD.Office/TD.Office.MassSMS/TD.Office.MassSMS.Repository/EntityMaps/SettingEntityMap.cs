using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.MassSMS.Contracts.Entities;
using TD.Office.MassSMS.Contracts.Enums;

namespace TD.Office.MassSMS.Repository.EntityMaps;

public class SettingEntityMap : LSCoreEntityMap<SettingEntity>
{
	public override Action<EntityTypeBuilder<SettingEntity>> Mapper { get; } =
		(entityBuilder) =>
		{
			entityBuilder.Property(x => x.Setting).IsRequired();
			entityBuilder.Property(x => x.Value).IsRequired();
		};
}
