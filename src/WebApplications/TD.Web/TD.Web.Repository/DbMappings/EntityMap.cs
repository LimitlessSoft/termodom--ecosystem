using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Contracts.Entities;
using TD.Core.Repository;
using TD.Web.Contracts.Entities;

namespace TD.Web.Repository.DbMappings
{
    public abstract class EntityMap : IEntityMap<UserEntity>
    {
        public EntityMap(EntityTypeBuilder<Entity> entityTypeBuilder) 
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
        }

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

            return entityTypeBuilder;
        }
    }
}
