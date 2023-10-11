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
