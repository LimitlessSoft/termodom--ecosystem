using Microsoft.AspNetCore.Http;

namespace TD.Core.Contracts.IManagers
{
	public interface IBaseManager
	{
		void SetContextInfo(HttpContext httpContext);
	}
}
