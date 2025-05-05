using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
	public class UserPermissionEntityMap : LSCoreEntityMap<UserPermissionEntity>
	{
		public override Action<EntityTypeBuilder<UserPermissionEntity>> Mapper { get; } =
			builder =>
			{
				builder.Property(x => x.Permission).IsRequired();

				builder.Property(x => x.UserId).IsRequired();
			};
	}
}
