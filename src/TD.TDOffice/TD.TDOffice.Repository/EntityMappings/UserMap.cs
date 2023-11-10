using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.TDOffice.Contracts.Entities;

namespace TD.TDOffice.Repository.EntityMappings
{
    public class UserMap : ILSCoreEntityMap<User>
    {
        public EntityTypeBuilder<User> Map(EntityTypeBuilder<User> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            return entityTypeBuilder;
        }
    }
}
