using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class PaymentTypeEntityMap : LSCoreEntityMap<PaymentTypeEntity>
    {
        public override EntityTypeBuilder<PaymentTypeEntity> Map(EntityTypeBuilder<PaymentTypeEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .Property(x => x.Name)
                .IsRequired();

            return entityTypeBuilder;
        }
    }
}
