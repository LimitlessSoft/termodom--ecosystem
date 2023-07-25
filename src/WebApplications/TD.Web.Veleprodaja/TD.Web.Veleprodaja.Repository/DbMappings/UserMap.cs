using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Veleprodaja.Contracts.Entities;

namespace TD.Web.Veleprodaja.Repository.DbMappings
{
    public class UserMap : IEntityMap<User>
    {
        public EntityTypeBuilder<User> Map(EntityTypeBuilder<User> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            entityTypeBuilder
                .HasIndex(x => x.Username)
                .IsUnique();

            entityTypeBuilder
                .Property(x => x.Username)
                .HasMaxLength(32);

            entityTypeBuilder
                .Property(x => x.Nickname)
                .HasMaxLength(32);

            return entityTypeBuilder;
        }
    }
}
