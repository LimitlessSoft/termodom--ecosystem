using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class OrderOneTimeInformationEntityMap : LSCoreEntityMap<OrderOneTimeInformationEntity>
    {
        public override EntityTypeBuilder<OrderOneTimeInformationEntity> Map(EntityTypeBuilder<OrderOneTimeInformationEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasOne(x => x.Order)
                .WithOne(x => x.OrderOneTimeInformation);

            entityTypeBuilder.Property(x => x.Name)
                .IsRequired();

            entityTypeBuilder.Property(x => x.Mobile)
                .IsRequired();

            return entityTypeBuilder;
        }
    }
}
