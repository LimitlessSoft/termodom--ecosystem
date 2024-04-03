using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class ProductEntityMap : LSCoreEntityMap<ProductEntity>
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
                .HasMaxLength(64);

            entityTypeBuilder
                .HasOne(x => x.Unit)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.UnitId);

            entityTypeBuilder
                .HasOne(x => x.AlternateUnit)
                .WithMany(x => x.AlternateProducts)
                .HasForeignKey(x => x.AlternateUnitId);

            entityTypeBuilder
                .Property(x => x.Image)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.CatalogId)
                .IsRequired(false);

            entityTypeBuilder
                .Property(x => x.Classification)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.VAT)
                .IsRequired();

            entityTypeBuilder
                .HasMany(x => x.Groups)
                .WithMany(x => x.Products);

            entityTypeBuilder
                .HasOne(x => x.Price)
                .WithOne(x => x.Product)
                .HasForeignKey<ProductPriceEntity>(x => x.ProductId);

            entityTypeBuilder
                .HasOne(x => x.ProductPriceGroup)
                .WithMany(x => x.Products)
                .HasForeignKey(x => x.ProductPriceGroupId);

            entityTypeBuilder
                .Property(x => x.PriorityIndex)
                .IsRequired()
                .HasDefaultValue(0);
            
            return entityTypeBuilder;
        }
    }
}
