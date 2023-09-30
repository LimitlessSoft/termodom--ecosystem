using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;

namespace TD.Web.Repository.DbMappings
{
    public class UnitEntityMap : EntityMap<UnitEntity>
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
