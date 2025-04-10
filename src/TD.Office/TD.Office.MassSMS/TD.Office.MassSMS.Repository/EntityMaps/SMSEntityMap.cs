using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.MassSMS.Contracts.Entities;
using TD.Office.MassSMS.Contracts.Enums;

namespace TD.Office.MassSMS.Repository.EntityMaps;

public class SMSEntityMap : LSCoreEntityMap<SMSEntity>
{
	public override Action<EntityTypeBuilder<SMSEntity>> Mapper { get; } =
		(entity) =>
		{
			entity.Property(x => x.Text).IsRequired();
			entity.Property(x => x.Phone).IsRequired();
		};
}
