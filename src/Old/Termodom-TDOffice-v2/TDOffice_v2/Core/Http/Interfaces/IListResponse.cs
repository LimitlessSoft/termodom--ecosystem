using System.Collections.Generic;

namespace TDOffice_v2.Core.Http.Interfaces
{
	public interface IListResponse<TEntity> : IResponse
	{
		List<TEntity> Payload { get; set; }
	}
}
