using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using TD.Web.Common.Contracts.Enums;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class ProductGroupEntityMap : LSCoreEntityMap<ProductGroupEntity>
    {
        public override Action<EntityTypeBuilder<ProductGroupEntity>> Mapper { get; } = entityTypeBuilder =>
        {
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
        };
    }
}
