using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;

namespace TD.Web.Repository.DbMappings
{
    public class ProductPriceEntityMap : EntityMap<ProductPriceEntity>
    {
        public override EntityTypeBuilder<ProductPriceEntity> Map(EntityTypeBuilder<ProductPriceEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => x.ProductId)
                .IsUnique();

            entityTypeBuilder
                .Property(x => x.ProductId)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Min)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Max)
                .IsRequired();

            return entityTypeBuilder;
        }
    }
}
