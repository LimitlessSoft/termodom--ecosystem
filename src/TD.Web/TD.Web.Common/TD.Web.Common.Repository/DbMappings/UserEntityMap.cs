using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Repository;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class UserEntityMap : EntityMap<UserEntity>
    {
        private readonly int UsernameMaxCharacters = 32;
        private readonly int NicknameMaxCharacters = 32;
        private readonly int MobileMaxCharacters = 16;
        private readonly int AddressMaxCharacters = 32;
        private readonly int MailMaxCharacters = 32;

        public override EntityTypeBuilder<UserEntity> Map(EntityTypeBuilder<UserEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => x.Username)
                .IsUnique();

            entityTypeBuilder
                .Property(x => x.Username)
                .HasMaxLength(UsernameMaxCharacters)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Nickname)
                .HasMaxLength(NicknameMaxCharacters)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.DateOfBirth)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Mobile)
                .HasMaxLength(MobileMaxCharacters)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Address)
                .HasMaxLength(AddressMaxCharacters)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.CityId)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.FavoriteStoreId)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Mail)
                .HasMaxLength(MailMaxCharacters);

            entityTypeBuilder
                .Property(x => x.Type)
                .IsRequired();

            return entityTypeBuilder;
        }
    }
}
