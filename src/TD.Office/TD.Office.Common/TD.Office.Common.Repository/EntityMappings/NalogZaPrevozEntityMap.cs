using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
    public class NalogZaPrevozEntityMap : LSCoreEntityMap<NalogZaPrevozEntity>
    {
        public override EntityTypeBuilder<NalogZaPrevozEntity> Map(EntityTypeBuilder<NalogZaPrevozEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder.Property(x => x.CenaPrevozaBezPdv)
                .IsRequired();
            
            entityTypeBuilder.Property(x => x.MiNaplatiliKupcuBezPdv)
                .IsRequired();
            
            entityTypeBuilder.Property(x => x.Address)
                .IsRequired();
            
            entityTypeBuilder.Property(x => x.Mobilni)
                .IsRequired();

            return entityTypeBuilder;
        }
    }
}