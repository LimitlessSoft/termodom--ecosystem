using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using Microsoft.EntityFrameworkCore;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class ProductPriceGroupEntityMap : LSCoreEntityMap<ProductPriceGroupEntity>
    {
        public override Action<EntityTypeBuilder<ProductPriceGroupEntity>> Mapper { get; } = entityTypeBuilder =>
        {
            entityTypeBuilder
                .HasIndex(x => x.Name)
                .IsUnique();

            entityTypeBuilder
                .Property(x => x.TrackUserLevel)
                .HasDefaultValue(false);
        };
    }
}
