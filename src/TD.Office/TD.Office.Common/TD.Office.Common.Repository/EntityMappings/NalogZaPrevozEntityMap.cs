using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Office.Common.Repository.EntityMappings
{
    public class NalogZaPrevozEntityMap : LSCoreEntityMap<NalogZaPrevozEntity>
    {
        public override Action<EntityTypeBuilder<NalogZaPrevozEntity>> Mapper { get; } = builder =>
        {
            builder.Property(x => x.CenaPrevozaBezPdv)
                .IsRequired();
            
            builder.Property(x => x.MiNaplatiliKupcuBezPdv)
                .IsRequired();
            
            builder.Property(x => x.Address)
                .IsRequired();
            
            builder.Property(x => x.Mobilni)
                .IsRequired();
        };
    }
}