using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TD.Core.Contracts;

namespace TD.Core.Repository
{
    /// <summary>
    /// Used to map entity fields for the database.
    /// By default Id, CreatedAt, IsActive, UpdatedAt and UpdatedBy are mapped.
    /// To add custom mapping, override Map(EntityTypeBuilder&lt;<typeparamref name="TEntity"/>&gt; entityTypeBuilder)
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityMap<TEntity> : IEntityMap<TEntity>
        where TEntity : class, IEntity
    {
        public virtual EntityTypeBuilder<TEntity> Map(EntityTypeBuilder<TEntity> entityTypeBuilder)
        {
            entityTypeBuilder
                .HasKey(x => x.Id);

            entityTypeBuilder
                .Property(x => x.created_at)
                .IsRequired();

            entityTypeBuilder
                .Property(x => x.is_active)
                .HasDefaultValue(true)
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
