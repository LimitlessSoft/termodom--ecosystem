using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class UnitEntityMap : LSCoreEntityMap<UnitEntity>
    {
        public override Action<EntityTypeBuilder<UnitEntity>> Mapper { get; } = entityTypeBuilder =>
        {
            entityTypeBuilder
                .HasIndex(x => x.Name)
                .IsUnique();

            entityTypeBuilder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(32);
        };
    }
}
