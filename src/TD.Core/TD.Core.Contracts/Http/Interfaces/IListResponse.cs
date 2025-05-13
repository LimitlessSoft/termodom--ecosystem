namespace TD.Core.Contracts.Http.Interfaces
{
	public interface IListResponse<TEntity> : IResponse
	{
		List<TEntity> Payload { get; set; }
	}
}
