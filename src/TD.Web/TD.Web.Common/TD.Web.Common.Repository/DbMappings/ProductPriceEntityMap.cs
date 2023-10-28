using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Admin.Contracts.Entities;

namespace TD.Web.Admin.Repository.DbMappings
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
                .Property(x => x.Min)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Max)
                .IsRequired();

            return entityTypeBuilder;
        }
    }
}
