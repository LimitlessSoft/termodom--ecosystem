using TD.Core.Framework.Extensions;

namespace TD.Office.Common.DbMigrations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            StartupExtensions.CreateTDBuilder<Startup>(args);
        }
    }
}