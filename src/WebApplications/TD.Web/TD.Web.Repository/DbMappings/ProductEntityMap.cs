using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;

namespace TD.Web.Repository.DbMappings
{
    public class ProductEntityMap : EntityMap<ProductEntity>
    {
        public override EntityTypeBuilder<ProductEntity> Map(EntityTypeBuilder<ProductEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => x.Name)
                .IsUnique();

            entityTypeBuilder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(64);

            entityTypeBuilder
                .Property(x => x.Src)
                .IsRequired()
                .HasMaxLength(32);

            entityTypeBuilder
                .Property(x => x.Image)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.CatalogId)
                .IsRequired(false);

            entityTypeBuilder
                .Property(x => x.UnitId)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Classification)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.VAT)
                .IsRequired();

            entityTypeBuilder
                .HasMany(x => x.Groups)
                .WithMany(x => x.Products);

            return entityTypeBuilder;
        }
    }
}
