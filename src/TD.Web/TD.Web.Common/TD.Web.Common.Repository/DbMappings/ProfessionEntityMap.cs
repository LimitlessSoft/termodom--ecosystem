using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class ProfessionEntityMap : LSCoreEntityMap<ProfessionEntity>
    {
        private const int _nameMaxLength = 32;

        public override Action<EntityTypeBuilder<ProfessionEntity>> Mapper { get; } = entityTypeBuilder =>
        {
            entityTypeBuilder
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(_nameMaxLength);
        };
    }
}
