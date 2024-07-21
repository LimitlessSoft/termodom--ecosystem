using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Web.Common.Contracts.Entities;
using LSCore.Repository;

namespace TD.Web.Common.Repository.DbMappings
{
    public class UserEntityMap : LSCoreEntityMap<UserEntity>
    {
        private const int _usernameMaxCharacters = 32;
        private const int _nicknameMaxCharacters = 32;
        private const int _mobileMaxCharacters = 16;
        private const int _addressMaxCharacters = 32;
        private const int _mailMaxCharacters = 256;
        private const int _commentMaxLength = 1024;

        public override Action<EntityTypeBuilder<UserEntity>> Mapper { get; } = entityTypeBuilder =>
        {
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
            
            entityTypeBuilder
                .HasMany(x => x.Permissions)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.UserId);
        };
    }
}
