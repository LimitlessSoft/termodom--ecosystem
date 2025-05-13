using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.TDOffice.Contracts.Entities;

namespace TD.TDOffice.Repository.EntityMappings;

public class UserMap : LSCoreEntityMap<User>
{
	public override Action<EntityTypeBuilder<User>> Mapper { get; } =
		entityTypeBuilder =>
		{
			entityTypeBuilder.HasKey(x => x.Id);
		};
}
