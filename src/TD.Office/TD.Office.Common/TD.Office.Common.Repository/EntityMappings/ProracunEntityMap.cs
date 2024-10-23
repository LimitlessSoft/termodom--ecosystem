using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;

public class ProracunEntityMap : LSCoreEntityMap<ProracunEntity>
{
    public override Action<EntityTypeBuilder<ProracunEntity>> Mapper { get; } =
        (builder) =>
        {
            builder.Property(x => x.MagacinId).IsRequired();
            builder.Property(x => x.State).IsRequired();
            builder.Property(x => x.Type).IsRequired();
            builder.HasMany(x => x.Items).WithOne().HasForeignKey(x => x.ProracunId);
            builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.CreatedBy);
        };
}
