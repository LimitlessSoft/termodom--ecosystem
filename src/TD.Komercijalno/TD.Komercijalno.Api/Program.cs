
using LSCore.Framework.Extensions;

namespace TD.Komercijalno.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LSCoreStartupExtensions.InitializeLSCoreApplication<Startup>(args);
        }
    }
}