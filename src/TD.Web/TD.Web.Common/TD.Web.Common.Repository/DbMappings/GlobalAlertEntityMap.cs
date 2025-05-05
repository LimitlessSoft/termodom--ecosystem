using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
	public class GlobalAlertEntityMap : LSCoreEntityMap<GlobalAlertEntity>
	{
		public override Action<EntityTypeBuilder<GlobalAlertEntity>> Mapper { get; } =
			entityTypeBuilder =>
			{
				entityTypeBuilder.Property(x => x.Text).IsRequired().HasMaxLength(256);

				entityTypeBuilder.Property(x => x.Application).IsRequired();
			};
	}
}
