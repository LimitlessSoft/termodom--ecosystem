using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
    public class SettingEntityMap : LSCoreEntityMap<SettingEntity>
    {
        public override Action<EntityTypeBuilder<SettingEntity>> Mapper { get; } = builder => { };
    }
}
