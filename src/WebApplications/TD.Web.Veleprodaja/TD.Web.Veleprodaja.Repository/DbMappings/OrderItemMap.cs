using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Veleprodaja.Contracts.Entities;

namespace TD.Web.Veleprodaja.Repository.DbMappings
{
    public class OrderItemMap : IEntityMap<OrderItem>
    {
        public EntityTypeBuilder<OrderItem> Map(EntityTypeBuilder<OrderItem> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            entityTypeBuilder
                .Property(x => x.Quantity)
                .HasPrecision(4);

            entityTypeBuilder
                .Property(x => x.PriceWithoutVat)
                .HasPrecision(4);

            entityTypeBuilder
                .Property(x => x.Vat)
                .HasPrecision(4);

            entityTypeBuilder
                .HasOne(x => x.Order)
                .WithMany(x => x.Items)
                .HasForeignKey(x => x.OrderId);

            return entityTypeBuilder;
        }
    }
}
