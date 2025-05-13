using TD.Core.Contracts.Interfaces;

namespace TD.Core.Domain.Extensions
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

		public static List<TDto> ToDtoList<TDto, TEntity>(this IEnumerable<TEntity> sender)
			where TEntity : class
		{
			var dtoList = new List<TDto>();

			foreach (var entity in sender)
				dtoList.Add(entity.ToDto<TDto, TEntity>());

			return dtoList;
		}
	}
}
