using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
	public class ProductPriceEntityMap : LSCoreEntityMap<ProductPriceEntity>
	{
		public override Action<EntityTypeBuilder<ProductPriceEntity>> Mapper { get; } =
			entityTypeBuilder =>
			{
				entityTypeBuilder.HasIndex(x => x.ProductId).IsUnique();

				entityTypeBuilder.Property(x => x.Min).IsRequired();

				entityTypeBuilder.Property(x => x.Max).IsRequired();
			};
	}
}
