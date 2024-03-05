using LSCore.Domain;
using LSCore.Framework.Extensions;
using Microsoft.Extensions.Configuration;
using TD.Web.Common.DbMigrations.Helper;

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