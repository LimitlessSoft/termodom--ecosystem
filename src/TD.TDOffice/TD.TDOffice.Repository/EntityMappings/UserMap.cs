using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.TDOffice.Contracts.Entities;
using LSCore.Repository;

namespace TD.TDOffice.Repository.EntityMappings;

public class UserMap : LSCoreEntityMap<User>
{
    public override Action<EntityTypeBuilder<User>> Mapper { get; } = entityTypeBuilder =>
    {
        entityTypeBuilder
            .HasKey(x => x.Id);
    };
}