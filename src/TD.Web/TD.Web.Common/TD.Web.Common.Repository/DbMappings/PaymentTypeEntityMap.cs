using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class PaymentTypeEntityMap : LSCoreEntityMap<PaymentTypeEntity>
    {
        public override Action<EntityTypeBuilder<PaymentTypeEntity>> Mapper { get; } = entityTypeBuilder =>
        {
            entityTypeBuilder
                .Property(x => x.Name)
                .IsRequired();
        };
    }
}
