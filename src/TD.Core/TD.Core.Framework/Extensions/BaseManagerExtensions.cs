using Microsoft.AspNetCore.Http;
using TD.Core.Contracts.IManagers;

namespace TD.Core.Framework.Extensions
{
    public static class BaseManagerExtensions
    {
        public static TManager AttachCurrentContext<TManager>(this TManager manager, HttpContext context)
            where TManager : IBaseManager
        {
            manager.SetContext(context);
            return manager;
        }
    }
}
