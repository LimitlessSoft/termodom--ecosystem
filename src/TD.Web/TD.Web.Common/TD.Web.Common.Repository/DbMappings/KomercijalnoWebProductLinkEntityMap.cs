using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class KomercijalnoWebProductLinkEntityMap : LSCoreEntityMap<KomercijalnoWebProductLinkEntity>
    {
        public override EntityTypeBuilder<KomercijalnoWebProductLinkEntity> Map(EntityTypeBuilder<KomercijalnoWebProductLinkEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => new { x.RobaId, x.WebId })
                .IsUnique();

            return entityTypeBuilder;
        }
    }
}
