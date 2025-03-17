namespace TD.Web.Common.Contracts.Helpers
{
	public static class GeneralHelpers
	{
		public static string GenerateBucketName(string environment) =>
			String.Format(
				LegacyConstants.MinioBucketFormat,
				environment.ToLower(),
				LegacyConstants.ProjectName.ToLower()
			);
	}
}
