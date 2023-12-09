using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class StoreEntityMap : LSCoreEntityMap<StoreEntity>
    {
        public override EntityTypeBuilder<StoreEntity> Map(EntityTypeBuilder<StoreEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            return entityTypeBuilder;
        }
    }
}
