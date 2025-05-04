using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TD.Core.Repository
{
	public interface IEntityMap<TEntity>
		where TEntity : class
	{
		EntityTypeBuilder<TEntity> Map(EntityTypeBuilder<TEntity> entityTypeBuilder);
	}
}
