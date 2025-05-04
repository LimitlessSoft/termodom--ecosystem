using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
	public class ProductPriceGroupEntityMap : LSCoreEntityMap<ProductPriceGroupEntity>
	{
		public override Action<EntityTypeBuilder<ProductPriceGroupEntity>> Mapper { get; } =
			entityTypeBuilder =>
			{
				entityTypeBuilder.HasIndex(x => x.Name).IsUnique();

				entityTypeBuilder.Property(x => x.TrackUserLevel).HasDefaultValue(false);
			};
	}
}
