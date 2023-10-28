namespace TD.Web.Common.Contracts
{
    public static class Constants
    {
        public static readonly string DbName = "Web_Main";

        public static class DbMigrations
        {
            public static readonly string DbSeedsRoot = Path.Combine(Environment.CurrentDirectory, "DbSeeds");
            public static readonly string DbSeedsDownRoot = Path.Combine(Environment.CurrentDirectory, "DbSeeds", "Down");
        }
    }
}
