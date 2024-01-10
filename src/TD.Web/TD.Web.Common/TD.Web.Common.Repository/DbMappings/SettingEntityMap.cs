using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class SettingEntityMap : LSCoreEntityMap<SettingEntity>
    {
        public override EntityTypeBuilder<SettingEntity> Map(EntityTypeBuilder<SettingEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => x.Key)
                .IsUnique();

            return entityTypeBuilder;
        }
    }
}
