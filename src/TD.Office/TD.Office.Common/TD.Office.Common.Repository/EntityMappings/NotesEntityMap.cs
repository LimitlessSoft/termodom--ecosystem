using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings;
public class NotesEntityMap : LSCoreEntityMap<NotesEntity>
{
    public override Action<EntityTypeBuilder<NotesEntity>> Mapper { get; } =
        (builder) =>
        {
            builder.Property(x => x.Name).IsRequired();
        };
}
