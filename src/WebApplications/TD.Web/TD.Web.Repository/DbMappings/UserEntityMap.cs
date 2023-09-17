using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;

namespace TD.Web.Repository.DbMappings
{
    public class UserEntityMap : IEntityMap<UserEntity>
    {
        public EntityTypeBuilder<UserEntity> Map(EntityTypeBuilder<UserEntity> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            entityTypeBuilder
                .Property(x => x.created_at)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.is_active)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.updated_at)
                .IsRequired(false);

            entityTypeBuilder
                .Property(x => x.updated_by)
                .IsRequired(false);

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
