using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class OrderItemEntityMap : LSCoreEntityMap<OrderItemEntity>
    {
        public override Action<EntityTypeBuilder<OrderItemEntity>> Mapper { get; } = entityTypeBuilder =>
        {
            entityTypeBuilder
                .HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.OrderId);

            entityTypeBuilder
                .HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId);

            entityTypeBuilder
                .Property(x => x.Price)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Quantity)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.PriceWithoutDiscount)
                .IsRequired();
        };
    }
}
