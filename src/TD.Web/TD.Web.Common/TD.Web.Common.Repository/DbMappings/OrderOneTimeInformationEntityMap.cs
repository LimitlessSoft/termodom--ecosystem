using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class OrderOneTimeInformationEntityMap : LSCoreEntityMap<OrderOneTimeInformationEntity>
    {
        public override Action<EntityTypeBuilder<OrderOneTimeInformationEntity>> Mapper { get; } = entityTypeBuilder =>
        {
            entityTypeBuilder
                .HasOne(x => x.Order)
                .WithOne(x => x.OrderOneTimeInformation);

            entityTypeBuilder.Property(x => x.Name)
                .IsRequired();

            entityTypeBuilder.Property(x => x.Mobile)
                .IsRequired();
        };
    }
}
