using TD.Core.Framework.Extensions;

namespace TD.Web.Common.DbMigrations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LSCoreStartupExtensions.CreateTDBuilder<Startup>(args);
        }
    }
}