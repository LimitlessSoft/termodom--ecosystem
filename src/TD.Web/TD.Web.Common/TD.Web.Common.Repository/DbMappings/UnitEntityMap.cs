using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class UnitEntityMap : LSCoreEntityMap<UnitEntity>
    {
        public override EntityTypeBuilder<UnitEntity> Map(EntityTypeBuilder<UnitEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => x.Name)
                .IsUnique();

            entityTypeBuilder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(32);

            return entityTypeBuilder;
        }
    }
}
