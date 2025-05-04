namespace TD.Core.Contracts.Interfaces
{
	public interface IDtoMapper<TDto, TEntity>
	{
		public TDto ToDto(TEntity sender);
	}
}
