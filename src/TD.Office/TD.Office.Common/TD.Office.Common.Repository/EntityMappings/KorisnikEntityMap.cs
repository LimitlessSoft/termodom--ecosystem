using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
    public class KorisnikEntityMap : LSCoreEntityMap<KorisnikEntity>
    {
        public override EntityTypeBuilder<KorisnikEntity> Map(EntityTypeBuilder<KorisnikEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder.Property(x => x.Nadimak)
                .IsRequired()
                .HasMaxLength(64);

            entityTypeBuilder
                .HasIndex(x => x.Ime)
                .IsUnique();

            entityTypeBuilder.Property(x => x.Ime)
                .IsRequired()
                .HasMaxLength(64);

            return entityTypeBuilder;
        }
    }
}
