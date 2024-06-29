using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Komercijalno.Contracts.Entities;
using LSCore.Repository;

namespace TD.Komercijalno.Repository.DbMappings;

public class NamenaMap() : LSCoreEntityMap<Namena>(true)
{
    public override Action<EntityTypeBuilder<Namena>> Mapper { get; } = builder =>
    {
        builder
            .HasKey(x => x.Id);
    };
}