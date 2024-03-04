using TD.Web.Common.ImportTool.Helper;

namespace TD.Web.Common.DbMigrations
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TableHelper.ImportUsersStructure();
            TableHelper.ImportUsersData();
            Console.WriteLine("Hello World");
        }
    }
}