using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Komercijalno.Contracts.Entities;
using LSCore.Repository;

namespace TD.Komercijalno.Repository.DbMappings;

public class NacinPlacanjaMap () : LSCoreEntityMap<NacinPlacanja>(true)
{
    public override Action<EntityTypeBuilder<NacinPlacanja>> Mapper { get; } = builder =>
    {
        builder.HasKey(x => x.Id);
    };
}
