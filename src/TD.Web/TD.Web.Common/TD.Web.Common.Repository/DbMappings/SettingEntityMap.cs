using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class SettingEntityMap : LSCoreEntityMap<SettingEntity>
    {
        public override Action<EntityTypeBuilder<SettingEntity>> Mapper { get; } = entityTypeBuilder =>
        {
            entityTypeBuilder
                .HasIndex(x => x.Key)
                .IsUnique();
        };
    }
}
