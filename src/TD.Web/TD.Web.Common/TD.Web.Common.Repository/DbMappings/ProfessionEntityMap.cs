using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class ProfessionEntityMap : LSCoreEntityMap<ProfessionEntity>
    {
        private readonly Int16 _nameMaxLength = 32;
        public override EntityTypeBuilder<ProfessionEntity> Map(EntityTypeBuilder<ProfessionEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(_nameMaxLength);

            return entityTypeBuilder;
        }
    }
}
