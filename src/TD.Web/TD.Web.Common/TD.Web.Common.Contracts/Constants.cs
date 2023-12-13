namespace TD.Web.Common.Contracts
{
    public static class Constants
    {
        public static readonly string DbName = "Web_Main";

        public static readonly string RegexValidateUsernamePattern = "^[0-9A-Za-z]+$";

        public static readonly string RegexValidatePasswordPattern = @"^(?=.*[0-9])(?=.*[A-Za-z])";

        public static readonly string AltTextTag = "alt";

        public static readonly string DefaultImageFolderPath = "images";

        public static readonly string UploadImageFileNameDateTimeFormatString = "dd-MM-yyyy HH:mm:ss.fff";

        public static class DbMigrations
        {
            public static readonly string DbSeedsRoot = Path.Combine(Environment.CurrentDirectory, "DbSeeds");
            public static readonly string DbSeedsDownRoot = Path.Combine(Environment.CurrentDirectory, "DbSeeds", "Down");
        }
    }
}
