using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class ProductPriceGroupLevelEntityMap : EntityMap<ProductPriceGroupLevelEntity>
    {
        public override EntityTypeBuilder<ProductPriceGroupLevelEntity> Map(EntityTypeBuilder<ProductPriceGroupLevelEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .Property(x => x.Level)
                .IsRequired();

            entityTypeBuilder
                .HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId);

            entityTypeBuilder
                .HasOne(x => x.ProductPriceGroup)
                .WithMany()
                .HasForeignKey(x => x.ProductPriceGroupId);

            return entityTypeBuilder;
        }
    }
}
