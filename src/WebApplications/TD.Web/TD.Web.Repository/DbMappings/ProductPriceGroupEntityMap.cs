using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;

namespace TD.Web.Repository.DbMappings
{
    public class ProductPriceGroupEntityMap : EntityMap<ProductPriceGroupEntity>
    {
        public override EntityTypeBuilder<ProductPriceGroupEntity> Map(EntityTypeBuilder<ProductPriceGroupEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => x.Name)
                .IsUnique();

            return entityTypeBuilder;
        }
    }
}
