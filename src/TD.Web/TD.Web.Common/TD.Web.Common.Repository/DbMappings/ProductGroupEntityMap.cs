using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using TD.Web.Common.Contracts.Enums;

namespace TD.Web.Common.Repository.DbMappings
{
    public class ProductGroupEntityMap : LSCoreEntityMap<ProductGroupEntity>
    {
        public override EntityTypeBuilder<ProductGroupEntity> Map(EntityTypeBuilder<ProductGroupEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => x.Name)
                .IsUnique();

            entityTypeBuilder
                .HasOne(x => x.ParentGroup)
                .WithMany();

            entityTypeBuilder
                .HasMany(x => x.Products)
                .WithMany(x => x.Groups);

            entityTypeBuilder
                .Property(x => x.Type)
                .IsRequired()
                .HasDefaultValue(ProductGroupType.Standard);

            return entityTypeBuilder;
        }
    }
}
