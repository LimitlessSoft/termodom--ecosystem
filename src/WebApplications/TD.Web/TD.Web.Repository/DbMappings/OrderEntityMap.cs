using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;

namespace TD.Web.Repository.DbMappings
{
    public class OrderEntityMap : EntityMap<OrderEntity>
    {
        public override EntityTypeBuilder<OrderEntity> Map(EntityTypeBuilder<OrderEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            //entityTypeBuilder
            //    .HasOne(x => x.UserEntity);

            entityTypeBuilder
                .Property(x => x.UserId)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Status)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Date)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.StoreId)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.PaymentType)
                .IsRequired();

            return entityTypeBuilder;
        }
    }
}
