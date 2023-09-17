using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;

namespace TD.Web.Repository.DbMappings
{
    public class ProductGroupEntityMap : IEntityMap<ProductGroupEntity>
    {
        public EntityTypeBuilder<ProductGroupEntity> Map(EntityTypeBuilder<ProductGroupEntity> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            entityTypeBuilder
                .Property(x => x.created_at)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.is_active)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.updated_at)
                .IsRequired(false);

            entityTypeBuilder
                .Property(x => x.updated_by)
                .IsRequired(false);

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
