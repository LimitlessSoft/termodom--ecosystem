using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;

namespace TD.Web.Common.Repository.DbMappings
{
    public class UserEntityMap : LSCoreEntityMap<UserEntity>
    {
        private readonly Int16 _usernameMaxCharacters = 32;
        private readonly Int16 _nicknameMaxCharacters = 32;
        private readonly Int16 _mobileMaxCharacters = 16;
        private readonly Int16 _addressMaxCharacters = 32;
        private readonly Int16 _mailMaxCharacters = 32;
        private readonly Int16 _commentMaxLength = 1024;

        public override EntityTypeBuilder<UserEntity> Map(EntityTypeBuilder<UserEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder
                .HasIndex(x => x.Username)
                .IsUnique();

            entityTypeBuilder
                .Property(x => x.Username)
                .HasMaxLength(_usernameMaxCharacters)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Nickname)
                .HasMaxLength(_nicknameMaxCharacters)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.DateOfBirth)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Mobile)
                .HasMaxLength(_mobileMaxCharacters)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Address)
                .HasMaxLength(_addressMaxCharacters)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.CityId)
                .IsRequired();

            entityTypeBuilder
                .HasOne(x => x.City)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.CityId);

            entityTypeBuilder
                .HasOne(x => x.FavoriteStore)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.FavoriteStoreId);

            entityTypeBuilder
                .HasOne(x => x.Profession)
                .WithMany(x => x.Users)
                .HasForeignKey(x => x.ProfessionId);

            entityTypeBuilder
                .Property(x => x.FavoriteStoreId)
                .IsRequired();

            entityTypeBuilder
                .HasOne(x => x.Referent)
                .WithMany()
                .HasForeignKey(x => x.ReferentId);

            entityTypeBuilder
                .Property(x => x.Mail)
                .HasMaxLength(_mailMaxCharacters);

            entityTypeBuilder
                .Property(x => x.Type)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.Comment)
                .HasMaxLength(_commentMaxLength);

            return entityTypeBuilder;
        }
    }
}
