using LSCore.Framework.Extensions;

namespace TD.Office.Common.DbMigrations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LSCoreStartupExtensions.CreateTDBuilder<Startup>(args);
        }
    }
}