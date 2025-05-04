using TD.Core.Contracts.Entities;
using TD.Core.Contracts.Interfaces;
using TD.Core.Domain;

namespace TD.Core.Framework.Extensions
{
	public static class EntityExtensions
	{
		public static TDto ToDto<TDto, TEntity>(this TEntity sender)
			where TEntity : class
		{
			var dtoMapper = Constants.Container?.TryGetInstance<IDtoMapper<TDto, TEntity>>();
			if (dtoMapper == null)
				throw new ArgumentNullException(nameof(dtoMapper));

			return dtoMapper.ToDto(sender);
		}
	}
}
