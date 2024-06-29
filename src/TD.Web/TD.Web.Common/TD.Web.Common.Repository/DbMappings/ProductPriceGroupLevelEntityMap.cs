using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class ProductPriceGroupLevelEntityMap : LSCoreEntityMap<ProductPriceGroupLevelEntity>
    {
        public override Action<EntityTypeBuilder<ProductPriceGroupLevelEntity>> Mapper { get; } = entityTypeBuilder =>
        {
            entityTypeBuilder
                .Property(x => x.Level)
                .IsRequired();

            entityTypeBuilder
                .HasOne(x => x.User)
                .WithMany(x => x.ProductPriceGroupLevels)
                .HasForeignKey(x => x.UserId);

            entityTypeBuilder
                .HasOne(x => x.ProductPriceGroup)
                .WithMany(x => x.ProductPriceGroupLevels)
                .HasForeignKey(x => x.ProductPriceGroupId);
        };
    }
}
