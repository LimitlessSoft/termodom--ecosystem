using TD.Core.Framework.Extensions;

namespace TD.DbMigrations.App
{
    public class Program
    {
        public static void Main(string[] args)
        {
            StartupExtensions.CreateTDBuilder<Startup>(args);
        }
    }
}