
using TD.Core.Framework.Extensions;

namespace TD.Web.Public.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LSCoreStartupExtensions.CreateTDBuilder<Startup>(args);
        }
    }
}