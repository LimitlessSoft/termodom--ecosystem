using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class OrderEntityMap : LSCoreEntityMap<OrderEntity>
    {
        public override EntityTypeBuilder<OrderEntity> Map(EntityTypeBuilder<OrderEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasOne(x => x.UserEntity)
                .WithMany(x => x.Orders) 
                .HasForeignKey(x => x.UserId);

            entityTypeBuilder
                .Property(x => x.UserId)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Status)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Date)
                .IsRequired();

            return entityTypeBuilder;
        }
    }
}
