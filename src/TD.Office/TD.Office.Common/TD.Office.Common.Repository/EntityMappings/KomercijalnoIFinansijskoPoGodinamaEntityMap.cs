using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;
public class KomercijalnoIFinansijskoPoGodinamaEntityMap : LSCoreEntityMap<KomercijalnoIFinansijskoPoGodinamaEntity>
{
    public override Action<EntityTypeBuilder<KomercijalnoIFinansijskoPoGodinamaEntity>> Mapper { get; } =
        (builder) =>
        {
            builder.Property(x => x.PPID).IsRequired();
            builder.Property(x => x.StatusId)
            .IsRequired();
        };
}
