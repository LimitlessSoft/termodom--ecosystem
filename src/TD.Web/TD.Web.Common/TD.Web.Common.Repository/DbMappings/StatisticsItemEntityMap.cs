using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class StatisticsItemEntityMap : LSCoreEntityMap<StatisticsItemEntity>
    {
        public override EntityTypeBuilder<StatisticsItemEntity> Map(EntityTypeBuilder<StatisticsItemEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);
                
            entityTypeBuilder.Property(x => x.Type)
                .IsRequired();
            
            return entityTypeBuilder;
        }
    }
}