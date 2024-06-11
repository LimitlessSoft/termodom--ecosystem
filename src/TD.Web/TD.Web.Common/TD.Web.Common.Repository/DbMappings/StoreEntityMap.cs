using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class StoreEntityMap : LSCoreEntityMap<StoreEntity>
    {

        public override Action<EntityTypeBuilder<StoreEntity>> Mapper { get; } = builder =>
        {

        };
    }
}
