
using LSCore.Framework.Extensions;

namespace TD.Office.Public.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LSCoreStartupExtensions.InitializeLSCoreApplication<Startup>(args);
        }
    }
}