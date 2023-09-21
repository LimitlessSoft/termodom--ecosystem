using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;

namespace TD.Web.Repository.DbMappings
{
    public class UnitsEntityMap : EntityMap<UnitsEntity>
    {
        public override EntityTypeBuilder<UnitsEntity> Map(EntityTypeBuilder<UnitsEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(32);

            return entityTypeBuilder;
        }
    }
}
