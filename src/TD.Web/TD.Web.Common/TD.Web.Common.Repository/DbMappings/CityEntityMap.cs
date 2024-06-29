using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class CityEntityMap : LSCoreEntityMap<CityEntity>
    {

        public override Action<EntityTypeBuilder<CityEntity>> Mapper { get; } = entityTypeBuilder =>
        {
            entityTypeBuilder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(32);
        };
    }
}
