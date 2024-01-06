using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class OrderItemEntityMap : LSCoreEntityMap<OrderItemEntity>
    {
        public override EntityTypeBuilder<OrderItemEntity> Map(EntityTypeBuilder<OrderItemEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.OrderId);

            entityTypeBuilder
                .HasOne(x => x.Product);

            entityTypeBuilder
                .Property(x => x.Price)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Quantity)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.PriceWithoutDiscount)
                .IsRequired();

            return entityTypeBuilder;
        }
    }
}
