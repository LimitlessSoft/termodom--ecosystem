using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class ProductGroupEntityMap : EntityMap<ProductGroupEntity>
    {
        public override EntityTypeBuilder<ProductGroupEntity> Map(EntityTypeBuilder<ProductGroupEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => x.Name)
                .IsUnique();

            entityTypeBuilder
                .HasMany(x => x.Products)
                .WithMany(x => x.Groups);

            return entityTypeBuilder;
        }
    }
}
