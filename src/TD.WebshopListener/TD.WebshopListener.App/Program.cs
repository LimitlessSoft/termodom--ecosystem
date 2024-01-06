
using LSCore.Framework.Extensions;

namespace TD.WebshopListener.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LSCoreStartupExtensions.InitializeLSCoreApplication<Startup>(args);
        }
    }
}