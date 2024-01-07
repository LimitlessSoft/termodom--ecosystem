using LSCore.Repository;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Office.Common.Contracts.Entities;

namespace TD.Office.Common.Repository.EntityMappings
{
    public class UserEntityMap : LSCoreEntityMap<UserEntity>
    {
        public override EntityTypeBuilder<UserEntity> Map(EntityTypeBuilder<UserEntity> entityTypeBuilder)
        {
            base.Map(entityTypeBuilder);

            entityTypeBuilder.Property(x => x.Username)
                .IsRequired()
                .HasMaxLength(64);

            entityTypeBuilder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(64);

            return entityTypeBuilder;
        }
    }
}
