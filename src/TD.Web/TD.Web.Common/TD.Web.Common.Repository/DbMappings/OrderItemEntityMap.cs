using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class OrderItemEntityMap : LSCoreEntityMap<OrderItemEntity>
    {
        public override EntityTypeBuilder<OrderItemEntity> Map(EntityTypeBuilder<OrderItemEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            //


            return entityTypeBuilder;
        }
    }
}
