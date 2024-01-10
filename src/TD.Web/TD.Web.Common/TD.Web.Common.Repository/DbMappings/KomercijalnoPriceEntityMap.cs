using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
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
