using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.TDOffice.Contracts.Entities;

namespace TD.TDOffice.Repository.EntityMappings
{
    public class UserMap : IEntityMap<User>
    {
        public EntityTypeBuilder<User> Map(EntityTypeBuilder<User> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            return entityTypeBuilder;
        }
    }
}
