namespace TD.Web.Common.Contracts.Helpers
{
    public static class GeneralHelpers
    {
        public static string GenerateBucketName(string environment) =>
            String.Format(Constants.MinioBucketFormat, environment.ToLower(), Constants.ProjectName.ToLower());
    }
}
