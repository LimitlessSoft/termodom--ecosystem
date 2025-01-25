using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.InterneOtpremnice.Contracts.Entities;
using TD.Office.InterneOtpremnice.Contracts.Enums;

namespace TD.Office.InterneOtpremnice.Repository.EntityMaps;

public class InternaOtpremnicaEntityMap : LSCoreEntityMap<InternaOtpremnicaEntity>
{
    public override Action<EntityTypeBuilder<InternaOtpremnicaEntity>> Mapper { get; } =
        (builder) =>
        {
            builder.ToTable(nameof(InterneOtpremniceDbContext.InterneOtpremnice));
            builder
                .Property(x => x.Status)
                .HasDefaultValue(InternaOtpremnicaStatus.Otkljucana)
                .IsRequired();

            builder.HasMany<InternaOtpremnicaItemEntity>()
                .WithOne()
                .HasForeignKey(x => x.InternaOtpremnicaId);
        };
}
