using Microsoft.AspNetCore.Http;
using TD.Core.Contracts.Http.Interfaces;

namespace TD.Core.Contracts.IManagers
{
    public interface IBaseManager
    {
        void SetContext(HttpContext httpContext);
        bool IsContextInvalid(IResponse response);
    }
}
