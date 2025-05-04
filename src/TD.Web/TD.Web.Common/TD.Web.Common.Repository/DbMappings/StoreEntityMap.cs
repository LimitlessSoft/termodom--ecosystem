using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
	public class StoreEntityMap : LSCoreEntityMap<StoreEntity>
	{
		public override Action<EntityTypeBuilder<StoreEntity>> Mapper { get; } = builder => { };
	}
}
