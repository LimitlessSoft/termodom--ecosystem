using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class StatisticsItemEntityMap : LSCoreEntityMap<StatisticsItemEntity>
    {
        public override Action<EntityTypeBuilder<StatisticsItemEntity>> Mapper { get; } = entityTypeBuilder =>
        {
            entityTypeBuilder.Property(x => x.Type)
                .IsRequired();
        };
    }
}