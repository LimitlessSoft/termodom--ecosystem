
using TD.Core.Framework.Extensions;

namespace TD.WebshopListener.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            LSCoreStartupExtensions.CreateTDBuilder<Startup>(args);
        }
    }
}