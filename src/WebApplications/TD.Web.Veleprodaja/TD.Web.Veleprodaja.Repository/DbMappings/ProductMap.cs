using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Contracts;
using TD.Core.Repository;
using TD.Web.Veleprodaja.Contracts.Entities;
using TD.Web.Veleprodaja.Contracts.Requests.Products;

namespace TD.Web.Veleprodaja.Repository.DbMappings
{
    public class ProductMap : IEntityMap<Product>
    {
        public EntityTypeBuilder<Product> Map(EntityTypeBuilder<Product> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            entityTypeBuilder
                .Property(x => x.Name)
                .HasMaxLength(32);

            entityTypeBuilder
                .Property(x => x.Unit)
                .HasMaxLength(8);

            entityTypeBuilder
                .Property(x => x.ThumbnailImagePath)
                .HasMaxLength(256);

            entityTypeBuilder
                .Property(x => x.FullSizedImagePath)
                .HasMaxLength(256);

            entityTypeBuilder
                .Property(x => x.SKU)
                .HasMaxLength(32);

            entityTypeBuilder
                .Property(x => x.IsActive)
                .IsRequired(true)
                .HasDefaultValue(true);

            return entityTypeBuilder;
        }
    }
}
