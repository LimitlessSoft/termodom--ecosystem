using LSCore.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class OrderEntityMap : LSCoreEntityMap<OrderEntity>
    {
        private readonly Int16 _noteMaxLength = 512;
        public override EntityTypeBuilder<OrderEntity> Map(EntityTypeBuilder<OrderEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .Property(x => x.Status)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Note)
                .HasMaxLength(_noteMaxLength);


            return entityTypeBuilder;
        }
    }
}
