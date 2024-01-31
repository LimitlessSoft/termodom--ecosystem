using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class OrderOneTimeInformationsEntityMap : LSCoreEntityMap<OrderOneTimeInformationsEntity>
    {
        public override EntityTypeBuilder<OrderOneTimeInformationsEntity> Map(EntityTypeBuilder<OrderOneTimeInformationsEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasOne(x => x.Order)
                .WithOne(x => x.OrderOneTimeInformations);

            entityTypeBuilder.Property(x => x.Name)
                .IsRequired();

            entityTypeBuilder.Property(x => x.Mobile)
                .IsRequired();

            return entityTypeBuilder;
        }
    }
}
