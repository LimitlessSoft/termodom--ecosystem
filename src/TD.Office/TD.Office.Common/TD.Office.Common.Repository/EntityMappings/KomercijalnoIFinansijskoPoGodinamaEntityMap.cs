using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;
public class KomercijalnoIFinansijskoPoGodinamaEntityMap : LSCoreEntityMap<KomercijalnoIFinansijskoPoGodinamaEntity>
{
    public static readonly int _commentMaximumLength = 512;
    public override Action<EntityTypeBuilder<KomercijalnoIFinansijskoPoGodinamaEntity>> Mapper { get; } =
        (builder) =>
        {
            builder.Property(x => x.PPID).IsRequired();
            builder.Property(x => x.Comment).HasMaxLength(_commentMaximumLength);
            builder.Property(x => x.StatusId)
            .IsRequired();
        };
}
