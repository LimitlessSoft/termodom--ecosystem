
using LSCore.Framework.Extensions;

namespace TD.TDOffice.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LSCoreStartupExtensions.InitializeLSCoreApplication<Startup>(args);
        }
    }
}