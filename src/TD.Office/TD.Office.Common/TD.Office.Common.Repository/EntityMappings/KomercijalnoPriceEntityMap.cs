using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Office.Common.Repository.EntityMappings
{
    public class KomercijalnoPriceEntityMap : LSCoreEntityMap<KomercijalnoPriceEntity>
    {

        public override Action<EntityTypeBuilder<KomercijalnoPriceEntity>> Mapper { get; } = builder =>
        {
            builder
                .HasIndex(x => x.RobaId)
                .IsUnique();
        };
    }
}
