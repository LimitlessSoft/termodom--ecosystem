using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;
public class KomercijalnoIFinansijskoPoGodinamaStatusEntityMap : LSCoreEntityMap<KomercijalnoIFinansijskoPoGodinamaStatusEntity>
{
    public override Action<EntityTypeBuilder<KomercijalnoIFinansijskoPoGodinamaStatusEntity>> Mapper { get; } =
        (builder) =>
        {
            builder.Property(x => x.Naziv).IsRequired();
            builder.Property(x => x.IsDefault)
                .IsRequired()
                .HasDefaultValue(false);
        };
}
