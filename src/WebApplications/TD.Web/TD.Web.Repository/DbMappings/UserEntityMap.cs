using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;

namespace TD.Web.Repository.DbMappings
{
    public class UserEntityMap : EntityMap<UserEntity>
    {
        private readonly int UsernameMaxCharacters = 32;
        private readonly int NicknameMaxCharacters = 32;

        public override EntityTypeBuilder<UserEntity> Map(EntityTypeBuilder<UserEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => x.Username)
                .IsUnique();

            entityTypeBuilder
                .Property(x => x.Username)
                .HasMaxLength(UsernameMaxCharacters);

            entityTypeBuilder
                .Property(x => x.Nickname)
                .HasMaxLength(NicknameMaxCharacters);

            return entityTypeBuilder;
        }
    }
}
