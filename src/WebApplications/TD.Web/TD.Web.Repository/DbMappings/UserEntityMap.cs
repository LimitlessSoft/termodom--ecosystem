using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;

namespace TD.Web.Repository.DbMappings
{
    public class UserEntityMap : EntityMap<UserEntity>
    {
        public override EntityTypeBuilder<UserEntity> Map(EntityTypeBuilder<UserEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

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
