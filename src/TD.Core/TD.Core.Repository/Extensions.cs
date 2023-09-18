using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TD.Core.Repository
{
    public static class Extensions
    {
        public static EntityTypeBuilder<TEntity> AddMap<TEntity>(this EntityTypeBuilder<TEntity> entityTypeBuilder, IEntityMap<TEntity> map)
            where TEntity : class
        {
            return map.Map(entityTypeBuilder);
        }
    }
}
