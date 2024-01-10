using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
    public class KomercijalnoPriceEntityMap : LSCoreEntityMap<KomercijalnoPriceEntity>
    {
        public override EntityTypeBuilder<KomercijalnoPriceEntity> Map(EntityTypeBuilder<KomercijalnoPriceEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => x.RobaId)
                .IsUnique();

            return entityTypeBuilder;
        }
    }
}
