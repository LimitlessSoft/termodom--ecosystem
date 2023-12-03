using LSCore.Framework.Extensions;

namespace TD.Web.Common.DbMigrations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LSCoreStartupExtensions.InitializeLSCoreApplication<Startup>(args);
        }
    }
}